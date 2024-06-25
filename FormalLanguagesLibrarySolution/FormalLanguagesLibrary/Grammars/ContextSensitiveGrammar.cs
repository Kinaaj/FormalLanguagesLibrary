using System.Numerics;

namespace FormalLanguagesLibrary.Grammars
{
    // Class representing a context-sensitive grammar
    public class ContextSensitiveGrammar<T> : RecursivelyEnumerableGrammar<T> where T : IComparable<T>, IIncrementOperators<T>
    {
        private bool _isStarTSymbolValueOnRightSide = false;
        private bool _isStartingSymbolGeneratingEpsilon = false;

        // Default constructor
        public ContextSensitiveGrammar() : base() { }

        // Copy constructor
        public ContextSensitiveGrammar(ContextSensitiveGrammar<T> grammar) : base(grammar) { }

        public ContextSensitiveGrammar(HashSet<Symbol<T>> nonTerminals, HashSet<Symbol<T>> terminals, Symbol<T>? startTSymbolValue, HashSet<ProductionRule<T>> productionRules) : base(nonTerminals, terminals, startTSymbolValue, productionRules) { }

        public ContextSensitiveGrammar(IEnumerable<T> nonTerminals, IEnumerable<T> terminals, T? startTSymbolValue, IEnumerable<Tuple<IEnumerable<T>, IEnumerable<T>>> productionRules) : base(nonTerminals, terminals, startTSymbolValue, productionRules) { }

        // Check the format of production rules specific to context-sensitive grammars
        protected override void _checkFormatOfProductionRule(ProductionRule<T> rule)
        {
            // Check if the LHS is at least as long as the RHS
            if (rule.LeftHandSide.Length > rule.RightHandSide.Length)
            {
                throw new ContextSensitiveGrammarException($"The left-hand side must be at least as long as the right-hand side for production rule: {rule}");
            }

            // Check if the start symbol appears on the right-hand side of the rule
            foreach (Symbol<T> symbol in rule.RightHandSide)
            {
                if (symbol == _startSymbol)
                {
                    _isStarTSymbolValueOnRightSide = true;
                }
            }

            // Check if the start symbol generates epsilon
            if (rule.LeftHandSide[0] == _startSymbol && rule.IsEpsilonRule())
            {
                _isStartingSymbolGeneratingEpsilon = true;
            }

            // Check if the RHS is not empty
            if (rule.RightHandSide.Length == 0)
            {
                throw new ContextSensitiveGrammarException($"The right-hand side must be non-empty for production rule: {rule}");
            }

            // Ensure that the start symbol does not appear on the RHS if it generates epsilon
            if (_isStartingSymbolGeneratingEpsilon && _isStarTSymbolValueOnRightSide)
            {
                throw new ContextSensitiveGrammarException($"The start symbol S cannot appear on the right-hand side of any rule if S -> ε for production rule: {rule}");
            }

            // Check the format of rules where the RHS has more than one symbol
            if (rule.RightHandSide.Length != 1)
            {
                _checkAlphaAndBetaContext(rule);
            }
        }

        // Check that rules are in the format aXb -> ayb, where X is a non-terminal and y is any non-empty string
        private void _checkAlphaAndBetaContext(ProductionRule<T> rule)
        {
            for (int i = 0; i < rule.LeftHandSide.Length; i++)
            {
                if (rule.LeftHandSide[i].Type == SymbolType.NonTerminal)
                {
                    Symbol<T>[] alphaLHS = rule.LeftHandSide.Take(i).ToArray();
                    Symbol<T>[] betaLHS = rule.LeftHandSide.Skip(i + 1).ToArray();

                    Symbol<T>[] alphaRHS = rule.RightHandSide.Take(i).ToArray();
                    Symbol<T>[] betaRHS = rule.RightHandSide.TakeLast(betaLHS.Length).ToArray();

                    // Compare alpha segments
                    for (int k = 0; k < alphaLHS.Length; k++)
                    {
                        if (alphaLHS[k] != alphaRHS[k])
                        {
                            goto skipToAnotherBetaAndAlpha;
                        }
                    }

                    // Compare beta segments
                    for (int k = 0; k < betaLHS.Length; k++)
                    {
                        if (betaLHS[k] != betaRHS[k])
                        {
                            goto skipToAnotherBetaAndAlpha;
                        }
                    }
                    return;
                }

            skipToAnotherBetaAndAlpha:;
            }

            throw new ContextSensitiveGrammarException($"The rule {rule} is not in format \"aXb -> ayb\".");
        }
    }

    // Exception class specific to context-sensitive grammar-related errors
    public class ContextSensitiveGrammarException : RecursivelyEnumerableGrammarException
    {
        public ContextSensitiveGrammarException(string message) : base(message) { }
    }
}
