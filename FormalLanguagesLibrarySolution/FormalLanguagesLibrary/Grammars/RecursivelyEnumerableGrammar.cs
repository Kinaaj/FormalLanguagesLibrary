using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FormalLanguagesLibrary.Grammars
{
    public class RecursivelyEnumerableGrammar<T> : Grammar<T>
    {

        public RecursivelyEnumerableGrammar() : base() { }
        public RecursivelyEnumerableGrammar(RecursivelyEnumerableGrammar<T> grammar) : base(grammar) { }
        public RecursivelyEnumerableGrammar(IEnumerable<Symbol<T>> nonTerminals, IEnumerable<Symbol<T>> terminals, Symbol<T>? startSymbol, IEnumerable<ProductionRule<T>> productionRules) : base(nonTerminals, terminals, startSymbol, productionRules) { }
        public RecursivelyEnumerableGrammar(T[] nonTerminals, T[] terminals, T? startSymbol, Tuple<T[], T[]>[] productionRules) : base(nonTerminals, terminals, startSymbol, productionRules) { }



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
