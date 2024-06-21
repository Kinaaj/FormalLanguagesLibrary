using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FormalLanguagesLibrary.Grammars
{
    public class ContextFreeGrammar<T> : ContextSensitiveGrammar<T>
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


    }

    public class ContextFreeGrammarException : ContextSensitiveGrammarException
    {
        public ContextFreeGrammarException(string message) : base(message) { }
    }
}
