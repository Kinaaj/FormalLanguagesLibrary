using System.Numerics;
using System.Text;

namespace FormalLanguagesLibrary.Grammars
{
    // Abstract base class representing a formal grammar
    public abstract class Grammar<T> where T : IComparable<T>, IIncrementOperators<T>
    {
        // Sets of terminal and non-terminal symbols, the start symbol, and the production rules
        protected HashSet<Symbol<T>> _terminals = new HashSet<Symbol<T>>();
        protected HashSet<Symbol<T>> _nonTerminals = new HashSet<Symbol<T>>();
        protected Symbol<T>? _startSymbol = null;
        protected HashSet<ProductionRule<T>> _productionRules = new HashSet<ProductionRule<T>>();

        // Properties to access the terminal and non-terminal symbols, start symbol, and production rules
        public IReadOnlyCollection<Symbol<T>> Terminals => _terminals;
        public IReadOnlyCollection<Symbol<T>> NonTerminals => _nonTerminals;
        public Symbol<T>? StartSymbol => _startSymbol;
        public IReadOnlyCollection<ProductionRule<T>> ProductionRules => _productionRules;

        // Default constructor
        public Grammar()
        {
            _checkInvariants();
        }

        // Copy constructor
        public Grammar(Grammar<T> grammar)
        {
            if (grammar == null) throw new ArgumentNullException(nameof(grammar));
            foreach (var nonTerminal in grammar.NonTerminals)
            {
                _nonTerminals.Add(nonTerminal);
            }
            foreach (var terminal in grammar.Terminals)
            {
                _terminals.Add(terminal);
            }

            _startSymbol = grammar.StartSymbol;

            foreach (var rule in grammar.ProductionRules)
            {
                _productionRules.Add(rule);
            }

            _checkInvariants();
        }

        // Constructor with explicit sets of terminals, non-terminals, start symbol, and production rules
        public Grammar(HashSet<Symbol<T>> nonTerminals, HashSet<Symbol<T>> terminals, Symbol<T>? startTSymbolValue, HashSet<ProductionRule<T>> productionRules)
        {
            foreach (var nonTerminal in nonTerminals)
            {
                _nonTerminals.Add(nonTerminal);
            }
            foreach (var terminal in terminals)
            {
                _terminals.Add(terminal);
            }
            _startSymbol = startTSymbolValue;

            foreach (var rule in productionRules)
            {
                _productionRules.Add(rule);
            }

            _checkInvariants();
        }

        // Constructor with enumerable sets of terminals, non-terminals, start symbol, and production rules
        public Grammar(IEnumerable<T> nonTerminals, IEnumerable<T> terminals, T? startTSymbolValue, IEnumerable<Tuple<IEnumerable<T>, IEnumerable<T>>> productionRules)
        {
            foreach (T symbol in nonTerminals)
            {
                _nonTerminals.Add(new Symbol<T>(symbol, SymbolType.NonTerminal));
            }

            foreach (T symbol in terminals)
            {
                _terminals.Add(new Symbol<T>(symbol, SymbolType.Terminal));
            }

            if (startTSymbolValue is null)
            {
                _startSymbol = null;
            }
            else
            {
                _startSymbol = new Symbol<T>(startTSymbolValue, SymbolType.NonTerminal);
            }

            foreach (var rule in productionRules)
            {
                List<Symbol<T>> leftHandSide = new List<Symbol<T>>();

                foreach (T value in rule.Item1)
                {
                    if (nonTerminals.Contains(value))
                    {
                        leftHandSide.Add(new Symbol<T>(value, SymbolType.NonTerminal));
                    }
                    else if (terminals.Contains(value))
                    {
                        leftHandSide.Add(new Symbol<T>(value, SymbolType.Terminal));
                    }
                    else
                    {
                        throw new GrammarException($"Symbol {value} not in non-terminals and either in terminals.");
                    }
                }

                List<Symbol<T>> rightHandSide = new List<Symbol<T>>();

                if (!rule.Item2.Any())
                {
                    rightHandSide.Add(Symbol<T>.Epsilon);
                }

                foreach (T value in rule.Item2)
                {
                    if (nonTerminals.Contains(value))
                    {
                        rightHandSide.Add(new Symbol<T>(value, SymbolType.NonTerminal));
                    }
                    else if (terminals.Contains(value))
                    {
                        rightHandSide.Add(new Symbol<T>(value, SymbolType.Terminal));
                    }
                    else
                    {
                        throw new GrammarException($"Symbol {value} not in non-terminals and either in terminals.");
                    }
                }

                _productionRules.Add(new ProductionRule<T>(leftHandSide, rightHandSide));
            }
            _checkInvariants();
        }

        // Check the invariants of the grammar
        private void _checkInvariants()
        {
            _checkNonTerminals();
            _checkStarTSymbolValue();
            _checkTerminals();
            _checkTerminalsAndNonTerminals();
            _checkProductionRules();
        }

        // Abstract method to check the format of production rules, to be implemented by derived classes
        protected abstract void _checkFormatOfProductionRule(ProductionRule<T> rule);

        // Attempt to add a production rule, returning true if successful, false otherwise
        public bool TryAddRule(ProductionRule<T> rule)
        {
            try
            {
                _checkProductionRule(rule);
                _checkFormatOfProductionRule(rule);
                _productionRules.Add(rule);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Check all non-terminal symbols
        private void _checkNonTerminals()
        {
            foreach (var nonTerminal in _nonTerminals)
            {
                _checkNonTerminal(nonTerminal);
            }
        }

        // Check a single non-terminal symbol
        private void _checkNonTerminal(Symbol<T> nonTerminal)
        {
            if (nonTerminal.Type != SymbolType.NonTerminal)
            {
                throw new GrammarException($"Invalid symbol type '{nonTerminal.Type}' for {nonTerminal.Value} symbol. Expected 'NonTerminal'.");
            }
        }

        // Check all terminal symbols
        private void _checkTerminals()
        {
            foreach (var terminal in _terminals)
            {
                _checkTerminal(terminal);
            }
        }

        // Check a single terminal symbol
        private void _checkTerminal(Symbol<T> terminal)
        {
            if (terminal.Type != SymbolType.Terminal)
            {
                throw new GrammarException($"Invalid symbol type '{terminal.Type}' for {terminal.Value} symbol. Expected 'Terminal'.");
            }
        }

        // Check the start symbol
        private void _checkStarTSymbolValue()
        {
            if (_startSymbol is null)
            {
                return;
            }
            if (_startSymbol?.Type != SymbolType.NonTerminal)
            {
                throw new GrammarException($"Invalid symbol type '{_startSymbol?.Type}' for the start symbol. Expected 'NonTerminal'.");
            }
            if (!_nonTerminals.Contains((Symbol<T>)_startSymbol))
            {
                throw new GrammarException($"Start symbol must be included in non-terminals of the grammar.");
            }
        }

        // Check if the sets of terminals and non-terminals are disjoint
        private void _checkTerminalsAndNonTerminals()
        {
            var intersection = _nonTerminals.Intersect(_terminals);
            if (intersection.Any())
            {
                throw new GrammarException($"Terminals and NonTerminals should not intersect. Found common symbols {intersection}.");
            }
        }

        // Check all production rules
        private void _checkProductionRules()
        {
            foreach (var rule in ProductionRules)
            {
                _checkProductionRule(rule);
                _checkFormatOfProductionRule(rule);
            }
        }

        // Check a single production rule
        private void _checkProductionRule(ProductionRule<T> rule)
        {
            foreach (var symbol in rule.LeftHandSide)
            {
                if (!_nonTerminals.Contains(symbol) && !_terminals.Contains(symbol))
                {
                    throw new GrammarException($"Symbol {symbol} is not defined in terminals and either in non-terminals.");
                }
            }

            if (rule.IsEpsilonRule())
            {
                return;
            }

            foreach (var symbol in rule.RightHandSide)
            {
                if (!_nonTerminals.Contains(symbol) && !_terminals.Contains(symbol))
                {
                    throw new GrammarException($"Symbol {symbol} is not defined in terminals and either in non-terminals.");
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            // Non-terminals
            sb.Append("Non-Terminals: {");
            sb.Append(string.Join(", ", _nonTerminals));
            sb.AppendLine("}");

            // Terminals
            sb.Append("Terminals: {");
            sb.Append(string.Join(", ", _terminals));
            sb.AppendLine("}");

            // Start symbol
            sb.Append("Starting Symbol: ");
            sb.Append(_startSymbol?.ToString() ?? "None");
            sb.AppendLine();

            // Production rules
            sb.AppendLine("Production Rules: {");
            sb.Append(string.Join(",\n", _productionRules));
            sb.AppendLine("");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }

    // Exception class specific to grammar-related errors
    public class GrammarException : Exception
    {
        public GrammarException(string message) : base(message)
        {
        }
    }
}
