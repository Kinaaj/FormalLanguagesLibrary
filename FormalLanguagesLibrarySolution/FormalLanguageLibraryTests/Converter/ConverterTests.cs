using FormalLanguagesLibrary.Grammars;
using FormalLanguagesLibrary.Automata;

using GrammarSymbol = FormalLanguagesLibrary.Grammars.Symbol<char>;
using GrammarSymbolType = FormalLanguagesLibrary.Grammars.SymbolType;

using AutomataSymbol = FormalLanguagesLibrary.Automata.Symbol<char>;
using AutomataState = FormalLanguagesLibrary.Automata.State<string>;

namespace FormalLanguageLibraryTests.Converter
{
    public class ConverterTests
    {
        [Fact]
        public void Test1()
        {
            GrammarSymbol A = new('A', GrammarSymbolType.NonTerminal);
            GrammarSymbol B = new('B', GrammarSymbolType.NonTerminal);
            GrammarSymbol C = new('C', GrammarSymbolType.NonTerminal);

            GrammarSymbol a = new('a', GrammarSymbolType.Terminal);
            GrammarSymbol b = new('b', GrammarSymbolType.Terminal);



            HashSet<GrammarSymbol> nonTerminals = new() { A, B, C };
            HashSet<GrammarSymbol> terminals = new() { a, b };
            GrammarSymbol startSymbol = A;

            HashSet<ProductionRule<char>> productionRules = [
                new([A], [GrammarSymbol.Epsilon]),
                new([A], [a, B]),
                new([A], [b, A]),
                new([A], [b]),

                new([B], [a, C]),
                new([B], [b, B]),

                new([C], [a, A]),
                new([C], [b, C]),
                new([C], [a]),
                ];


            RegularGrammar<char> regularGrammar = new(nonTerminals, terminals, startSymbol, productionRules);


            var automaton = FormalLanguagesLibrary.Converter.Convert.RGtoNFA(regularGrammar);

            AutomataState stateA = new("A");
            AutomataState stateB = new("B");
            AutomataState stateC = new("C");
            AutomataState finalState = new("final");

            AutomataSymbol a2 = new('a');
            AutomataSymbol b2 = new('b');


            HashSet<AutomataSymbol> expectedInputAlphabet = [a2, b2];
            HashSet<AutomataState> expectedStates = [stateA, stateB, stateC, finalState];

            HashSet<AutomataState> expectedFinalStates = [stateA, finalState];
            AutomataState expectedStartingState = stateA;





            var expectedTransitionFunction = new TransitionFunction<char, string>();

            expectedTransitionFunction.AddTransition(stateA, b2, stateA);
            expectedTransitionFunction.AddTransition(stateA, a2, stateB);
            expectedTransitionFunction.AddTransition(stateA, b2, finalState);

            expectedTransitionFunction.AddTransition(stateB, b2, stateB);
            expectedTransitionFunction.AddTransition(stateB, a2, stateC);

            expectedTransitionFunction.AddTransition(stateC, b2, stateC);
            expectedTransitionFunction.AddTransition(stateC, a2, stateA);
            expectedTransitionFunction.AddTransition(stateC, a2, finalState);


            Assert.Equal(expectedInputAlphabet, automaton.InputAlphabet);
            Assert.Equal(expectedStates, automaton.States);
            Assert.Equal(expectedStartingState, automaton.InitialState);
            Assert.Equal(expectedTransitionFunction.Transitions, automaton.TransitionFunction.Transitions);
            Assert.Equal(expectedFinalStates, automaton.FinalStates);



        }
    }
}
