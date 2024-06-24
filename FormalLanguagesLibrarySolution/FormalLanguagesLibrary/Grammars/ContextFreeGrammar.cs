using System.Numerics;



namespace FormalLanguagesLibrary.Grammars
{
    public class ContextFreeGrammar<T> : ContextSensitiveGrammar<T> where T : IComparable<T>, IIncrementOperators<T>
    {
        public ContextFreeGrammar() : base() { }
        public ContextFreeGrammar(ContextFreeGrammar<T> grammar) : base(grammar) { }
        public ContextFreeGrammar(HashSet<Symbol<T>> nonTerminals, HashSet<Symbol<T>> terminals, Symbol<T>? startTSymbolValue, HashSet<ProductionRule<T>> productionRules) : base(nonTerminals,terminals, startTSymbolValue, productionRules) { }
        public ContextFreeGrammar(IEnumerable<T> nonTerminals, IEnumerable<T> terminals, T? startTSymbolValue, IEnumerable<Tuple<IEnumerable<T>, IEnumerable<T>>> productionRules) : base(nonTerminals, terminals, startTSymbolValue, productionRules) { }

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

            //Create a new production rules without nullable non-terminals
            foreach(var rule in _productionRules)
            {
                newRules.UnionWith(CreateNewProductionRulesForNullableNonTerminals(rule, nullable));
            }

            //Remove epsilon rules
            foreach(var rule in newRules)
            {
                if (rule.IsEpsilonRule()) newRules.Remove(rule);
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

                    ProductionRule<T> newRule1 = new([newStartSymbol], [startSymbol]);
                    ProductionRule<T> newRule2 = new([newStartSymbol], [Symbol<T>.Epsilon]);

                    newRules.Add(newRule1);
                    newRules.Add(newRule2);

                    _startSymbol = newStartSymbol;
                    _nonTerminals.Add(newStartSymbol);
                }
                else if(!isStartingSymbolOnRightSide && nullable.Contains(startSymbol))
                {
                    newRules.Add(new ProductionRule<T>([startSymbol], [Symbol<T>.Epsilon]));
                }
            }

            _productionRules = newRules;
        }


        private HashSet<ProductionRule<T>> CreateNewProductionRulesForNullableNonTerminals(ProductionRule<T> rule, HashSet<Symbol<T>> nullable)
        {



            var newRules = new HashSet<ProductionRule<T>>() {rule};

            int numberOfNullableOnRightHandSide = 0;

            foreach(var symbol in rule.RightHandSide)
            {
                if(nullable.Contains(symbol))
                {
                    numberOfNullableOnRightHandSide++;
                }
            }
            
            int totalCombinations = 1 << numberOfNullableOnRightHandSide; // 2^length

            //Skipping the last combination, where we are going to get the original rule
            for (int i = 0; i < totalCombinations; i++)
            {
                var newRightSide = new List<Symbol<T>>();
                int indexOfNullable = 0;
                for (int j = 0; j < rule.RightHandSide.Length; j++)
                {
                    // We add original symbol
                    if (!nullable.Contains(rule.RightHandSide[j]))
                    {
                        newRightSide.Add(rule.RightHandSide[j]);
                    }
                    else
                    {
                        //We add nullable symbol based on the combination
                        if ((i & (1 << indexOfNullable)) != 0)
                        {
                            newRightSide.Add(rule.RightHandSide[j]);
                        }
                        indexOfNullable++;
                    }
                }


                if (newRightSide.Count != 0)
                {
                    ProductionRule < T > newRule = new(rule.LeftHandSide, newRightSide);
                    newRules.Add(newRule);
                }
                else
                {
                    ProductionRule<T> newRule = new(rule.LeftHandSide, [Symbol<T>.Epsilon]);
                    newRules.Add(newRule);
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

                    if(nullable.Contains(leftSymbol))
                    {
                        continue;
                    }

                    if (rule.IsEpsilonRule())
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
