using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FormalLanguagesLibrary.Grammars;

namespace FormalLanguageLibraryTests.Grammars
{
    public class ProductionRuleTests
    {
        [Fact]
        public void PrductionRuleTest()
        {
            Symbol<char>[] l = { new Symbol<char>('a', SymbolType.Terminal), new Symbol<char>('B', SymbolType.NonTerminal) };
            Symbol<char>[] r = { new Symbol<char>('a', SymbolType.Terminal), new Symbol<char>('b', SymbolType.Terminal) };

            var p = new ProductionRule<char>(l, r);

            Assert.Equal("aB->ab", p.ToString());

        }


        [Fact]
        public void EpsilonOnLeftProductionRuleTest()
        {
            Symbol<char>[] l = { Symbol<char>.Epsilon };
            Symbol<char>[] r = { new Symbol<char>('a', SymbolType.Terminal), new Symbol<char>('b', SymbolType.Terminal) };

            Assert.Throws<ProductionRuleException>(() => new ProductionRule<char>(l, r));
        }

        [Fact]
        public void OnlyTerminalsOnLeftProductionRuleTest()
        {
            Symbol<char>[] l = { new Symbol<char>('a', SymbolType.Terminal), new Symbol<char>('B', SymbolType.Terminal) };
            Symbol<char>[] r = { new Symbol<char>('a', SymbolType.Terminal), new Symbol<char>('b', SymbolType.Terminal) };

            Assert.Throws<ProductionRuleException>(() => new ProductionRule<char>(l, r));
        }
    }
}
