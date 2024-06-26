﻿using System.Numerics;

namespace FormalLanguagesLibrary.Grammars
{
    // Class representing a recursively enumerable grammar
    public class RecursivelyEnumerableGrammar<T> : Grammar<T> where T : IComparable<T>, IIncrementOperators<T>
    {
        // Default constructor
        public RecursivelyEnumerableGrammar() : base() { }

        // Copy constructor
        public RecursivelyEnumerableGrammar(RecursivelyEnumerableGrammar<T> grammar) : base(grammar) { }

        public RecursivelyEnumerableGrammar(HashSet<Symbol<T>> nonTerminals, HashSet<Symbol<T>> terminals, Symbol<T>? startTSymbolValue, HashSet<ProductionRule<T>> productionRules) : base(nonTerminals, terminals, startTSymbolValue, productionRules) { }

        public RecursivelyEnumerableGrammar(IEnumerable<T> nonTerminals, IEnumerable<T> terminals, T? startTSymbolValue, IEnumerable<Tuple<IEnumerable<T>, IEnumerable<T>>> productionRules) : base(nonTerminals, terminals, startTSymbolValue, productionRules) { }

        // For recursively enumerable grammars, there are no constraints on the production rules
        protected override void _checkFormatOfProductionRule(ProductionRule<T> rule)
        {
            return;
        }
    }

    // Exception class specific to recursively enumerable grammar-related errors
    public class RecursivelyEnumerableGrammarException : GrammarException
    {
        public RecursivelyEnumerableGrammarException(string message) : base(message) { }
    }
}
