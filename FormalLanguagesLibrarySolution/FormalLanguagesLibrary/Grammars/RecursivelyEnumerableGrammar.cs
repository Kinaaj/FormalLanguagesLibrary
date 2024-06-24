using System.Numerics;



namespace FormalLanguagesLibrary.Grammars
{
    public class RecursivelyEnumerableGrammar<T> : Grammar<T> where T : IComparable<T>, IIncrementOperators<T>
    {

        public RecursivelyEnumerableGrammar() : base() { }
        public RecursivelyEnumerableGrammar(RecursivelyEnumerableGrammar<T> grammar) : base(grammar) { }
        public RecursivelyEnumerableGrammar(HashSet<Symbol<T>> nonTerminals, HashSet<Symbol<T>> terminals, Symbol<T>? startTSymbolValue, HashSet<ProductionRule<T>> productionRules) : base(nonTerminals, terminals, startTSymbolValue, productionRules) { }
        public RecursivelyEnumerableGrammar(IEnumerable<T> nonTerminals, IEnumerable<T> terminals, T? startTSymbolValue, IEnumerable<Tuple<IEnumerable<T>, IEnumerable<T>>> productionRules) : base(nonTerminals, terminals, startTSymbolValue, productionRules) { }



        //There are no contraints on the production rules
        protected override void _checkFormatOfProductionRule(ProductionRule<T> rule)
        {
            return;
        }
    }

    public class RecursivelyEnumerableGrammarException : GrammarException
    {
        public RecursivelyEnumerableGrammarException(string message) : base(message)
        {

        }
    }
}
