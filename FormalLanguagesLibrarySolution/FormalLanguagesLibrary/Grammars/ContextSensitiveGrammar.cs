using System.Numerics;



namespace FormalLanguagesLibrary.Grammars
{
    public class ContextSensitiveGrammar<T> : RecursivelyEnumerableGrammar<T> where T : IComparable<T>, IIncrementOperators<T>
    {

        private bool _isStarTSymbolValueOnRightSide = false;
        private bool _isStartingSymbolGeneratingEpsilon = false;



        public ContextSensitiveGrammar() : base() { }
        public ContextSensitiveGrammar(ContextSensitiveGrammar<T> grammar) : base(grammar) { }
        public ContextSensitiveGrammar(IEnumerable<Symbol<T>> nonTerminals, IEnumerable<Symbol<T>> terminals, Symbol<T>? starTSymbolValue, IEnumerable<ProductionRule<T>> productionRules) : base(nonTerminals, terminals, starTSymbolValue, productionRules) { }
        public ContextSensitiveGrammar(T[] nonTerminals, T[] terminals, T? starTSymbolValue, Tuple<T[], T[]>[] productionRules) : base(nonTerminals, terminals, starTSymbolValue, productionRules) { }



        protected override void _checkFormatOfProductionRule(ProductionRule<T> rule)
        {
            // LHS always contains at least one non-terminal, so we don't need to check it



            // Check if the LHS is at least as long as the RHS
            if (rule.LeftHandSide.Length > rule.RightHandSide.Length)
            {
                throw new ContextSensitiveGrammarException($"The left-hand side must be at least as long as the right-hand side for production rule: {rule}");
            }
            
            // Check if S is on the right side
            foreach (Symbol<T> symbol in rule.RightHandSide)
            {
                if (symbol == _startSymbol)
                {
                    _isStarTSymbolValueOnRightSide = true;
                }
            }

            // Check if S is generating epsilon
            if (rule.LeftHandSide[0] == _startSymbol && rule.IsEpsilonRule())
            {
                _isStartingSymbolGeneratingEpsilon = true;
            }


            // Check if the RHS is not empty
            if (rule.RightHandSide.Length == 0)
            {
                throw new ContextSensitiveGrammarException($"The right-hand side must be non-empty for production rule: {rule}");
            }

            // Ensure that S -> ε and S does not appear on the right side of any rule simoutaniously
            if (_isStartingSymbolGeneratingEpsilon && _isStarTSymbolValueOnRightSide)
            {
                throw new ContextSensitiveGrammarException($"The start symbol S cannot appear on the right-hand side of any rule if S -> ε for production rule: {rule}");
            }

            if (rule.RightHandSide.Length != 1)
            {
                _checkAlphaAndBetaContext(rule);
            }

        }

        // Left side length is >= 2

        // Check that rules are in format aXb -> ayb, where X is non-terminal and y is anything non-empty, a and b could be empty
        private void _checkAlphaAndBetaContext(ProductionRule<T> rule)
        {

            for (int i = 0; i < rule.LeftHandSide.Length; i++)
            {
                if (rule.LeftHandSide[i].Type == SymbolType.NonTerminal)
                {
                    Symbol<T>[] alphaLHS = rule.LeftHandSide.Take(i).ToArray();
                    //Symbol<T> nonTerminal = rule.LeftHandSide[i];
                    Symbol<T>[] betaLHS = rule.LeftHandSide.Skip(i + 1).ToArray();


                    Symbol<T>[] alphaRHS = rule.RightHandSide.Take(i).ToArray();
                    Symbol<T>[] betaRHS = rule.RightHandSide.TakeLast(betaLHS.Length).ToArray();

                    //Compare alpha
                    for (int k = 0; k < alphaLHS.Length; k++)
                    {
                        if (alphaLHS[k] != alphaRHS[k])
                        {
                            goto skipToAnotherBetaAndAlpha;
                        }
                    }

                    //Compare beta
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

    public class ContextSensitiveGrammarException : RecursivelyEnumerableGrammarException
    {
        public ContextSensitiveGrammarException(string message) : base(message) { }
    }
}
