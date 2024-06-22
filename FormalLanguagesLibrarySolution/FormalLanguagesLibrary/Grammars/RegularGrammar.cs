using System.Numerics;

namespace FormalLanguagesLibrary.Grammars
{
    public class RegularGrammar<T> : ContextFreeGrammar<T> where T : IComparable<T>, IIncrementOperators<T>
    {
        //Else is leftRegular
        public bool isRightRegular { get; private set; }

        private bool _isSetRegularity = false;

        public RegularGrammar() : base() { }
        public RegularGrammar(ContextFreeGrammar<T> grammar) : base(grammar) { }
        public RegularGrammar(IEnumerable<Symbol<T>> nonTerminals, IEnumerable<Symbol<T>> terminals, Symbol<T>? starTSymbolValue, IEnumerable<ProductionRule<T>> productionRules) : base(nonTerminals, terminals, starTSymbolValue, productionRules) { }
        public RegularGrammar(T[] nonTerminals, T[] terminals, T? starTSymbolValue, Tuple<T[], T[]>[] productionRules) : base(nonTerminals, terminals, starTSymbolValue, productionRules) { }

        protected override void _checkFormatOfProductionRule(ProductionRule<T> rule)
        {

            if(rule.RightHandSide.Length != 1 && rule.RightHandSide.Length != 2)
            {
                throw new RegularGrammarException($"Right-hand side of a rule {rule} must be 1 or 2 characters long");
            }

            if(rule.RightHandSide.Length == 1 && rule.RightHandSide[0].Type == SymbolType.NonTerminal)
            {
                throw new RegularGrammarException($"Right-hand side of rule {rule} must be in format \"A->a\", where 'A' is a non-terminal and 'a' is a terminal");
            }

            if(rule.RightHandSide.Length == 2)
            {
                bool isRuleRightRegular = false;

                if (rule.RightHandSide[0].Type == SymbolType.Terminal && rule.RightHandSide[1].Type == SymbolType.NonTerminal)
                {
                    isRuleRightRegular = true;
                }
                else if (rule.RightHandSide[0].Type == SymbolType.NonTerminal && rule.RightHandSide[1].Type == SymbolType.Terminal)
                {
                    isRuleRightRegular = false;
                }
                else
                {
                    throw new RegularGrammarException($"Right-hand side of rule {rule} must be in format \"A->aB\" if the grammar is righ-regular or \"A->Ba\" if it is left-regular, where 'A' and 'B' are non-terminals and 'a' is a terminal");
                }

                if (!_isSetRegularity)
                {
                    _isSetRegularity = true;
                    isRightRegular = isRuleRightRegular;
                }
                else
                {
                    if(isRuleRightRegular && !isRightRegular)
                    {
                        throw new RegularGrammarException($"Right-hand side of rule {rule} must be in format \"A->aB\" becuase the grammar is righ-regular, where 'A' and 'B' are non-terminals and 'a' is a terminal");
                    }
                    else if(!isRuleRightRegular && isRightRegular)
                    {
                        throw new RegularGrammarException($"Right-hand side of rule {rule} must be in format \"A->Ba\" becuase the grammar is left-regular, where 'A' and 'B' are non-terminals and 'a' is a terminal");
                    }
                }
            }
        }
    }

    public class RegularGrammarException : ContextFreeGrammarException
    {
        public RegularGrammarException(string message) : base(message) { }
    }
}
