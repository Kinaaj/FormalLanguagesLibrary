using FormalLanguagesLibrary.Automata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalLanguageLibraryTests.Automata
{
    public class SymbolTests
    {
        [Fact]
        public void SymbolTest()
        {
            Symbol<char> s = new('3');
            Symbol<char> s2 = new('3');
            Symbol<char> s3 = new('4');


            Assert.True(s.Equals(s2));
            Assert.False(s == s3);
            Assert.False(s == Symbol<char>.Epsilon);
        }
        [Fact]
        public void EpsilonSymbolTest()
        {
            Symbol<char> s = new('3');
            Symbol<char> e1 = Symbol<char>.Epsilon;
            Symbol<char> e2 = new('3', SymbolType.Epsilon);
            Symbol<char> e3 = new('b', SymbolType.Epsilon);


            Assert.False(s==e2);
            Assert.True(e1 == e2);
            Assert.True(e2 == e3);
        }


    }
}
