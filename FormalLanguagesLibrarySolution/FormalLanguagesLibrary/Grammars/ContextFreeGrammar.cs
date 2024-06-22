using System.Numerics;



namespace FormalLanguagesLibrary.Grammars
{
    public class ContextFreeGrammar<T> : ContextSensitiveGrammar<T> where T : IComparable<T>, IIncrementOperators<T>
    {
        public ContextFreeGrammar() : base() { }
        public ContextFreeGrammar(ContextFreeGrammar<T> grammar) : base(grammar) { }
        public ContextFreeGrammar(IEnumerable<Symbol<T>> nonTerminals, IEnumerable<Symbol<T>> terminals, Symbol<T>? starTSymbolValue, IEnumerable<ProductionRule<T>> productionRules) : base(nonTerminals,terminals,starTSymbolValue,productionRules) { }
        public ContextFreeGrammar(T[] nonTerminals, T[] terminals, T? starTSymbolValue, Tuple<T[], T[]>[] productionRules) : base(nonTerminals, terminals, starTSymbolValue, productionRules) { }

        protected override void _checkFormatOfProductionRule(ProductionRule<T> rule)
        {
            if(rule.LeftHandSide.Length != 1)
            {
                throw new ContextFreeGrammarException($"Left-hand side length is not equal to 1 for production rule: {rule}");
            }
        }
        public void RemoveEpsilonRules()
        {
            var nullable = GetNonTerminalsGeneratingEpsilon();
            var newRules = new HashSet<ProductionRule<T>>();

            foreach(var rule in _productionRules)
            {
                newRules.UnionWith(CreateNewProductionRules(rule, nullable));
            }

            //Check if the starting symbol is on the right side and is generating epsilon

            bool isStartingSymbolOnRightSide = false;

            foreach(var rule in _productionRules)
            {
                foreach(var symbol in rule.RightHandSide)
                {
                    if(symbol == _startSymbol)
                    {
                        isStartingSymbolOnRightSide = true;
                    }
                }
            }

            // If true then generate new starting symbol with epsilon rule and rule pointing to the old starting symbol
            if(_startSymbol is Symbol<T> startSymbol)
            {
                if (isStartingSymbolOnRightSide && nullable.Contains(startSymbol))
                {
                    var newStartSymbol = startSymbol.GetNextSymbol();
                    while(_nonTerminals.Contains(newStartSymbol))
                    {
                        newStartSymbol = newStartSymbol.GetNextSymbol();
                    }

                    Symbol<T>[] l = { newStartSymbol };
                    Symbol<T>[] r1 = { startSymbol };
                    Symbol<T>[] r2 = { Symbol<T>.Epsilon };
                    newRules.Add(new ProductionRule<T>(l, r1));
                    newRules.Add(new ProductionRule<T>(l, r2));
                    _startSymbol = newStartSymbol;
                    _nonTerminals.Add(newStartSymbol);
                }
            }

        }


        private HashSet<ProductionRule<T>> CreateNewProductionRules(ProductionRule<T> rule, HashSet<Symbol<T>> nullable)
        {
            var newRules = new HashSet<ProductionRule<T>>();

            int numberOfNullableOnRightHandSide = 0;

            foreach(var symbol in rule.RightHandSide)
            {
                if(nullable.Contains(symbol))
                {
                    numberOfNullableOnRightHandSide++;
                }
            }


            
            int totalCombinations = 1 << numberOfNullableOnRightHandSide; // 2^length

            for (int i = 0; i < totalCombinations; i++)
            {
                var newRightSide = new List<Symbol<T>>();
                int indexOfNextNullable = 0;
                for (int j = 0; j < numberOfNullableOnRightHandSide; j++)
                {
                    while (!nullable.Contains(rule.RightHandSide[indexOfNextNullable]))
                    {
                        newRightSide.Add(rule.RightHandSide[indexOfNextNullable]);
                        indexOfNextNullable++;
                    }

                    if ((i & (1 << j)) != 0)
                    {
                        newRightSide.Add(rule.RightHandSide[indexOfNextNullable]);
                    }

                    indexOfNextNullable++;
                }


                if (newRightSide.Count != 0)
                {
                    newRules.Add(new(rule.LeftHandSide, newRightSide));
                }
                else
                {
                    //Don't add the epsilon rules
                }
            }
            return newRules;
        }


        public HashSet<Symbol<T>> GetNonTerminalsGeneratingEpsilon()
        {
            var nullable = new HashSet<Symbol<T>>();

            while(true)
            {
                var newNullable = new HashSet<Symbol<T>>();

                foreach(var rule in _productionRules)
                {
                    var leftSymbol = rule.LeftHandSide[0];

                    if (rule.IsEpsilonRule() && !nullable.Contains(leftSymbol))
                    {
                        newNullable.Add(leftSymbol);
                    }
                    else
                    {
                        bool isGeneratingEpsilon = true;
                        foreach(var symbol in rule.RightHandSide)
                        {
                            if(!nullable.Contains(symbol))
                            {
                                isGeneratingEpsilon = false;
                            }
                        }

                        if(isGeneratingEpsilon)
                        {
                            newNullable.Add(leftSymbol);
                        }
                    }
                }

                if(newNullable.Count != 0)
                {
                    nullable.UnionWith(newNullable);
                }
                else
                {
                    break;
                }
            }

            return nullable;
        }



    }

    public class ContextFreeGrammarException : ContextSensitiveGrammarException
    {
        public ContextFreeGrammarException(string message) : base(message) { }
    }

}
