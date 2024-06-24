using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormalLanguagesLibrary.Grammars;

namespace FormalLanguageLibraryTests.Grammars
{
    public class RegularGrammarTests
    {
        [Fact]
        public void RightRegularGrammarTest()
        {

            Symbol<char> A = new('A', SymbolType.NonTerminal);
            Symbol<char> B = new('B', SymbolType.NonTerminal);
            Symbol<char> C = new('C', SymbolType.NonTerminal);

            Symbol<char> t0 = new('0', SymbolType.Terminal);
            Symbol<char> t1 = new('1', SymbolType.Terminal);



            HashSet<Symbol<char>> nonTerminals = new() { A,B,C};
            HashSet<Symbol<char>> terminals = new() { t0,t1};
            Symbol<char> startSymbol = A;

            HashSet<ProductionRule<char>> productionRules = [
                new([A], [t0]), 
                new([B], [t0, B]), 
                new([A], [t1, C])
                ];


            RegularGrammar<char> regularGrammar = new(nonTerminals, terminals,startSymbol,productionRules);

            Assert.Equal([A, B, C], regularGrammar.NonTerminals);
            Assert.Equal([t0, t1], regularGrammar.Terminals);
            Assert.True(regularGrammar.isRightRegular);

        }

        [Fact]
        public void WrongRightAndLeftRegularGrammarTest()
        {

            Symbol<char> A = new('A', SymbolType.NonTerminal);
            Symbol<char> B = new('B', SymbolType.NonTerminal);
            Symbol<char> C = new('C', SymbolType.NonTerminal);

            Symbol<char> t0 = new('0', SymbolType.Terminal);
            Symbol<char> t1 = new('1', SymbolType.Terminal);



            HashSet<Symbol<char>> nonTerminals = new() { A, B, C };
            HashSet<Symbol<char>> terminals = new() { t0, t1 };
            Symbol<char> startSymbol = A;

            HashSet<ProductionRule<char>> productionRules = [
                new([A], [t0]),
                new([B], [t0, B]),
                new([A], [C, t1])
                ];

            Assert.Throws<RegularGrammarException>(() => new RegularGrammar<char>(nonTerminals, terminals, startSymbol, productionRules));

        }

        [Fact]
        public void TryAddLeftRegularRuleToRightRegularGrammarTest()
        {

            Symbol<char> A = new('A', SymbolType.NonTerminal);
            Symbol<char> B = new('B', SymbolType.NonTerminal);
            Symbol<char> C = new('C', SymbolType.NonTerminal);

            Symbol<char> t0 = new('0', SymbolType.Terminal);
            Symbol<char> t1 = new('1', SymbolType.Terminal);



            HashSet<Symbol<char>> nonTerminals = new() { A, B, C };
            HashSet<Symbol<char>> terminals = new() { t0, t1 };
            Symbol<char> startSymbol = A;

            HashSet<ProductionRule<char>> productionRules = [
                new([A], [t0]),
                new([B], [t0, B]),
                new([A], [t1, C])
                ];


            RegularGrammar<char> regularGrammar = new(nonTerminals, terminals, startSymbol, productionRules);


            ProductionRule<char> leftRegularProductionRule = new([A], [B, t1]);
            Assert.False(regularGrammar.TryAddRule(leftRegularProductionRule));

        }
    }
}
