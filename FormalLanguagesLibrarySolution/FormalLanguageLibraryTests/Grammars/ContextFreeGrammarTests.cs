using FormalLanguagesLibrary.Grammars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalLanguageLibraryTests.Grammars
{
    public class ContextFreeGrammarTests
    {
        [Fact]
        public void ContextFreeGrammarTest()
        {
            char[] nonTerminals = { 'A', 'B', 'S', 'C' };
            char[] terminals = { '1', '2', '3', '4' };
            char starTSymbolValue = 'S';

            Tuple<IEnumerable<char>, IEnumerable<char>> rule1 = new(
                "B".ToCharArray(), "12".ToCharArray()
            );

            Tuple<IEnumerable<char>, IEnumerable<char>> rule2 = new("A".ToCharArray(), "A4C".ToCharArray());
            Tuple<IEnumerable<char>, IEnumerable<char>> rule3 = new("S".ToCharArray(), "A".ToCharArray());


            List<Tuple<IEnumerable<char>, IEnumerable<char>>> productionRules = new() { rule1, rule2, rule3};


            ContextFreeGrammar<char> g = new ContextFreeGrammar<char>(
                nonTerminals,
                terminals,
                starTSymbolValue,
                productionRules
            );

            HashSet < Symbol<char>> expectedNonTerminals = new() {
                new('A', SymbolType.NonTerminal),
                new('S', SymbolType.NonTerminal),
                new('B', SymbolType.NonTerminal),
                new('C', SymbolType.NonTerminal)
            };

            Assert.Equal(expectedNonTerminals, g.NonTerminals);

        }

        [Fact]
        public void GetNonTerminalsGeneratingEpsilonTest()
        {

            var S = new Symbol<char>('S', SymbolType.NonTerminal);
            var A = new Symbol<char>('A', SymbolType.NonTerminal);
            var B = new Symbol<char>('B', SymbolType.NonTerminal);
            var C = new Symbol<char>('C', SymbolType.NonTerminal);

            var t0 = new Symbol<char>('0', SymbolType.Terminal);
            var t1 = new Symbol<char>('1', SymbolType.Terminal);

            HashSet<ProductionRule<char>> productionRules = new() {
                new([S],[A,B]),
                new([C],[Symbol<char>.Epsilon]),
                new([A],[C, B]),
                new([A],[C, t0]),
                new([B],[C]),
            };


            ContextFreeGrammar<char> g = new ContextFreeGrammar<char>(
                [S,A,B,C],
                [t0, t1],
                S,
                productionRules
            );

            HashSet<Symbol<char>> expectedEpsilonNonTerminals = new() { S,A,B,C};

            Assert.Equal(expectedEpsilonNonTerminals, g.GetNonTerminalsGeneratingEpsilon());

        }

        [Fact]
        public void RemoveEpsilonRulesTest()
        {

            var S = new Symbol<char>('S', SymbolType.NonTerminal);
            var A = new Symbol<char>('A', SymbolType.NonTerminal);
            var B = new Symbol<char>('B', SymbolType.NonTerminal);
            var C = new Symbol<char>('C', SymbolType.NonTerminal);

            var t0 = new Symbol<char>('0', SymbolType.Terminal);
            var t1 = new Symbol<char>('1', SymbolType.Terminal);

            HashSet<ProductionRule<char>> productionRules = new() {
                new([S],[A,B]),
                new([C],[Symbol<char>.Epsilon]),
                new([C],[t0]),
                new([A],[C, B]),
                new([A],[C, t0]),
                new([A],[t0, t0, t1]),
                new([B],[C]),
            };


            ContextFreeGrammar<char> g = new ContextFreeGrammar<char>(
                [S, A, B, C],
                [t0, t1],
                S,
                productionRules
            );


            HashSet<ProductionRule<char>> expectedProductionRules = new() {
                new([S],[A,B]),
                new([S],[A]),
                new([S],[B]),
                new([S],[Symbol<char>.Epsilon]),
                new([C],[t0]),
                new([A],[C, B]),
                new([A],[C]),
                new([A],[B]),
                new([A],[C, t0]),
                new([A],[t0, t0, t1]),
                new([A],[t0]),
                new([B],[C]),
            };

            g.RemoveEpsilonRules();

            Assert.Equal(expectedProductionRules, g.ProductionRules);

        }

        [Fact]
        public void RemoveEpsilonRulesAndCreatingNewStartingSymbolTest()
        {

            var S = new Symbol<char>('S', SymbolType.NonTerminal);
            var A = new Symbol<char>('A', SymbolType.NonTerminal);
            var B = new Symbol<char>('B', SymbolType.NonTerminal);
            var C = new Symbol<char>('C', SymbolType.NonTerminal);
            var I = new Symbol<char>('T', SymbolType.NonTerminal);

            var t0 = new Symbol<char>('0', SymbolType.Terminal);
            var t1 = new Symbol<char>('1', SymbolType.Terminal);

            HashSet<ProductionRule<char>> productionRules = new() {
                new([S],[A,B]),
                new([C],[Symbol<char>.Epsilon]),
                new([C],[t0]),
                new([A],[C, B]),
                new([A],[C, t0]),
                new([A],[t0, t0, t1]),
                new([B],[C]),
                new([B],[S]),
            };


            ContextFreeGrammar<char> g = new ContextFreeGrammar<char>(
                [S, A, B, C, I],
                [t0, t1],
                S,
                productionRules
            );


            g.RemoveEpsilonRules();

            Symbol<char> expectedStartingSymbol = new('U', SymbolType.NonTerminal);

            HashSet<Symbol<char>> expectedNonTerminals = new(){S,A,B,C,I,expectedStartingSymbol};

            HashSet<ProductionRule<char>> expectedProductionRules = new() {
                new([expectedStartingSymbol],[S]),
                new([expectedStartingSymbol],[Symbol<char>.Epsilon]),
                new([S],[A,B]),
                new([S],[A,B]),
                new([S],[A]),
                new([S],[B]),
                new([C],[t0]),
                new([A],[C, B]),
                new([A],[C]),
                new([A],[B]),
                new([A],[C, t0]),
                new([A],[t0, t0, t1]),
                new([A],[t0]),
                new([B],[C]),
                new([B],[S]),
            };



            Assert.Equal(expectedStartingSymbol, g.StartSymbol);
            Assert.Equal(expectedNonTerminals, g.NonTerminals);
            Assert.Equal(expectedProductionRules, g.ProductionRules);

        }
    }
}
