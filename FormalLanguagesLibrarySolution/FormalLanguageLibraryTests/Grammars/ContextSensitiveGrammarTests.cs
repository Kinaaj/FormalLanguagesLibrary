using FormalLanguagesLibrary.Grammars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace FormalLanguageLibraryTests.Grammars
{
    public class ContextSensitiveGrammarTests
    {
        [Fact]
        public void ContextSensitiveGrammarTest()
        {
            char[] nonTerminals = { 'A', 'B', 'S', 'C' };
            char[] terminals = { '1', '2', '3', '4' };
            char starTSymbolValue = 'S';

            Tuple<IEnumerable<char>, IEnumerable<char>> rule1 = new(
                "1B".ToCharArray(), "12".ToCharArray()
            );

            Tuple<IEnumerable<char>, IEnumerable<char>> rule2 = new(
                ['A', '1', 'B', '2', '3', 'B'],
                ['A', '1', 'B', '2', '4', 'B', 'S']
            );

            List<Tuple<IEnumerable<char>, IEnumerable<char>>> productionRules = new() { rule1 };

            ContextSensitiveGrammar<char> g = new ContextSensitiveGrammar<char>(
                nonTerminals,
                terminals,
                starTSymbolValue,
                productionRules
            );

            HashSet<Symbol<char>> expectedNonTerminals = new() {
                new('A', SymbolType.NonTerminal),
                new('S', SymbolType.NonTerminal),
                new('B', SymbolType.NonTerminal),
                new('C', SymbolType.NonTerminal)
            };

            Assert.Equal(expectedNonTerminals, g.NonTerminals);



        }

        [Fact]
        public void WrongContextSensitiveGrammar()
        {
            char[] nonTerminals = { 'A', 'B', 'S', 'C' };
            char[] terminals = { '1', '2', '3', '4' };
            char starTSymbolValue = 'S';

            Tuple<IEnumerable<char>, IEnumerable<char>> rule1 = new(
                "1B".ToCharArray(), "12".ToCharArray()
            );

            Tuple<IEnumerable<char>, IEnumerable<char>> wrongRule = new(
                ['A', '1', 'B', '2', '3', 'B'],
                ['A', '1', 'B', '2', '4', 'B', 'S']
            );

            List<Tuple<IEnumerable<char>, IEnumerable<char>>> productionRules = new() { rule1, wrongRule };

            Assert.Throws<ContextSensitiveGrammarException>(() => new ContextSensitiveGrammar<char>(
                nonTerminals,
                terminals,
                starTSymbolValue,
                productionRules
            ));



        }
    }


}
