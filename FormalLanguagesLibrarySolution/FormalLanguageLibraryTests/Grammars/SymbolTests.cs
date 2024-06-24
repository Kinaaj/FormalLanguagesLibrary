using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FormalLanguagesLibrary.Grammars;

namespace FormalLanguageLibraryTests.Grammars
{
    public class SymbolTests
    {
        [Fact]
        public void SymbolTest()
        {
            Symbol<char> s = new('3', SymbolType.NonTerminal);
            Symbol<char> s2 = new('3', SymbolType.Terminal);
            Symbol<char> s3 = new('4', SymbolType.NonTerminal);


            Assert.True(s.Equals(s2));
            Assert.False(s == s3);
            Assert.False(s == Symbol<char>.Epsilon);
        }
        [Fact]
        public void EpsilonSymbolTest()
        {
            Symbol<char> s = new('3', SymbolType.NonTerminal);
            Symbol<char> e1 = Symbol<char>.Epsilon;
            Symbol<char> e2 = new('3', SymbolType.Epsilon);
            Symbol<char> e3 = new('b', SymbolType.Epsilon);


            Assert.False(s == e2);
            Assert.True(e1 == e2);
            Assert.True(e2 == e3);
        }

    }
}
