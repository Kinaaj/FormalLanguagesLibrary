using System.Numerics;

namespace FormalLanguagesLibrary.Grammars
{
    // Class representing a regular grammar, which is a special type of context-free grammar
    public class RegularGrammar<T> : ContextFreeGrammar<T> where T : IComparable<T>, IIncrementOperators<T>
    {
        // Property indicating if the grammar is right-regular (true) or left-regular (false)
        public bool isRightRegular { get; private set; }

        // Private field to check if regularity is already set
        private bool _isSetRegularity = false;

        // Default constructor
        public RegularGrammar() : base() { }

        // Copy constructor from another context-free grammar
        public RegularGrammar(ContextFreeGrammar<T> grammar) : base(grammar) { }

        public RegularGrammar(HashSet<Symbol<T>> nonTerminals, HashSet<Symbol<T>> terminals, Symbol<T>? startTSymbolValue, HashSet<ProductionRule<T>> productionRules) : base(nonTerminals, terminals, startTSymbolValue, productionRules) { }

        public RegularGrammar(IEnumerable<T> nonTerminals, IEnumerable<T> terminals, T? startTSymbolValue, IEnumerable<Tuple<IEnumerable<T>, IEnumerable<T>>> productionRules) : base(nonTerminals, terminals, startTSymbolValue, productionRules) { }

        // Method to check the format of a production rule
        protected override void _checkFormatOfProductionRule(ProductionRule<T> rule)
        {
            // Ensure right-hand side is either 1 or 2 symbols long
            if (rule.RightHandSide.Length != 1 && rule.RightHandSide.Length != 2)
            {
                throw new RegularGrammarException($"Right-hand side of a rule {rule} must be 1 or 2 characters long");
            }

            // Ensure single symbol right-hand side is not a non-terminal
            if (rule.RightHandSide.Length == 1 && rule.RightHandSide[0].Type == SymbolType.NonTerminal)
            {
                throw new RegularGrammarException($"Right-hand side of rule {rule} must be in format \"A->a\", where 'A' is a non-terminal and 'a' is a terminal");
            }

            // Ensure two-symbol right-hand side follows the regular grammar format (only A->aB or only A->Ba)
            if (rule.RightHandSide.Length == 2)
            {
                bool isRuleRightRegular;

                // Check if the rule is in right-regular format
                if (rule.RightHandSide[0].Type == SymbolType.Terminal && rule.RightHandSide[1].Type == SymbolType.NonTerminal)
                {
                    isRuleRightRegular = true;
                }
                // Check if the rule is in left-regular format
                else if (rule.RightHandSide[0].Type == SymbolType.NonTerminal && rule.RightHandSide[1].Type == SymbolType.Terminal)
                {
                    isRuleRightRegular = false;
                }
                else
                {
                    throw new RegularGrammarException($"Right-hand side of rule {rule} must be in format \"A->aB\" if the grammar is right-regular or \"A->Ba\" if it is left-regular, where 'A' and 'B' are non-terminals and 'a' is a terminal");
                }

                // Set or validate the regularity of the grammar
                if (!_isSetRegularity)
                {
                    _isSetRegularity = true;
                    isRightRegular = isRuleRightRegular;
                }
                else
                {
                    if (isRuleRightRegular && !isRightRegular)
                    {
                        throw new RegularGrammarException($"Right-hand side of rule {rule} must be in format \"A->aB\" because the grammar is right-regular, where 'A' and 'B' are non-terminals and 'a' is a terminal");
                    }
                    else if (!isRuleRightRegular && isRightRegular)
                    {
                        throw new RegularGrammarException($"Right-hand side of rule {rule} must be in format \"A->Ba\" because the grammar is left-regular, where 'A' and 'B' are non-terminals and 'a' is a terminal");
                    }
                }
            }
        }
    }

    // Exception class specific to regular grammar-related errors
    public class RegularGrammarException : ContextFreeGrammarException
    {
        public RegularGrammarException(string message) : base(message) { }
    }
}
