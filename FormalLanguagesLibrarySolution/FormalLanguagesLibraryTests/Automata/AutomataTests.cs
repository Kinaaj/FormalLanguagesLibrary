using FormalLanguagesLibrary.Automata;

namespace FormalLanguageLibraryTests.Automata
{
    public class AutomataTests
    {
        [Fact]
        public void DFA1Test()
        {
            //Design a DFA with ∑ = {0, 1} accepts those string which starts with 1 and ends with 0.

            //Arrange
            char[] inputAlphabet = { '0', '1' };
            string[] states = { "q1", "q2", "q3", "trash" };
            string initialState = "q1";
            string[] finalStates = { "q3" };

            TransitionFunction<char, string> transitionFunction = new();
            transitionFunction.AddTransition("q1", '1', "q2");
            transitionFunction.AddTransition("q1", '0', "trash");

            transitionFunction.AddTransition("trash", '0', "trash");
            transitionFunction.AddTransition("trash", '1', "trash");

            transitionFunction.AddTransition("q2", '0', "q3");
            transitionFunction.AddTransition("q2", '1', "q2");

            transitionFunction.AddTransition("q3", '0', "q3");
            transitionFunction.AddTransition("q3", '1', "q2");


            DeterministicFiniteAutomaton<char, string> automaton = new(inputAlphabet, states, initialState, transitionFunction, finalStates);

            //Testing inputs
            (string, bool)[] inputs = { ("0010011", false), ("10", true), ("", false), ("1", false), ("0", false), ("11111", false), ("101011", false), ("01111", false), ("1101010", true) };

            foreach (var (inputString, accepted) in inputs)
            {
                Assert.Equal(automaton.Accepts(inputString), accepted);
            }


        }

        [Fact]
        public void DFA2Test()
        {
            //Design a DFA with ∑ = {0} accepts strings 0^*

            //Testing second automaton
            TransitionFunction<char, string> transitionFunction2 = new();
            transitionFunction2.AddTransition("q1", '0', "q1");

            DeterministicFiniteAutomaton<char, string> automaton2 = new(new char[] { '0' }, new string[] { "q1" }, "q1", transitionFunction2, new string[] { "q1" });

            (string, bool)[] inputs2 = { ("", true), ("0000", true), ("0", true) };

            foreach (var (inputString, accepted) in inputs2)
            {
                Assert.Equal(automaton2.Accepts(inputString), accepted);
            }
        }

        [Fact]
        public void WrongFATest()
        {
            //Arrange
            char[] inputAlphabet = { '0', '1' };
            string[] states = { "q1", "q2", "q3", "trash" };
            string initialState = "q1";
            string[] finalStates = { "q3" };

            TransitionFunction<char, string> transitionFunction = new();
            transitionFunction.AddTransition("q1", '1', "q2");
            transitionFunction.AddTransition("q1", '0', "trash");

            transitionFunction.AddTransition("trash", '0', "trash");
            transitionFunction.AddTransition("trash", '1', "trash");

            transitionFunction.AddTransition("q2", '0', "q3");
            transitionFunction.AddTransition("q2", '1', "q2");

            transitionFunction.AddTransition("q3", '0', "q3");
            transitionFunction.AddTransition("q3", '1', "q2");

            //Adding transition with non-existing terminal:
            transitionFunction.AddTransition("q2", '3', "q2");

            Assert.Throws<FiniteAutomatonException>(() => new DeterministicFiniteAutomaton<char, string>(inputAlphabet, states, initialState, transitionFunction, finalStates));

        }

        [Fact]
        public void MissingTransitionInDFATest()
        {
            //Arrange
            char[] inputAlphabet = { '0', '1' };
            string[] states = { "q1", "q2", "q3", "trash" };
            string initialState = "q1";
            string[] finalStates = { "q3" };

            TransitionFunction<char, string> transitionFunction = new();
            transitionFunction.AddTransition("q1", '1', "q2");
            transitionFunction.AddTransition("q1", '0', "trash");

            transitionFunction.AddTransition("trash", '0', "trash");
            transitionFunction.AddTransition("trash", '1', "trash");

            //Missing transition:
            // transitionFunction.AddTransition("q2", '0', "q3");
            transitionFunction.AddTransition("q2", '1', "q2");

            transitionFunction.AddTransition("q3", '0', "q3");
            transitionFunction.AddTransition("q3", '1', "q2");


            Assert.Throws<DeterministicFiniteAutomatonException>(() => new DeterministicFiniteAutomaton<char, string>(inputAlphabet, states, initialState, transitionFunction, finalStates));

        }


        [Fact]
        public void eNFATest()
        {
            //Arrange
            char[] inputAlphabet = { '0', '1' };
            string[] states = { "q0", "q1", "q2" };
            string initialState = "q0";
            string[] finalStates = { "q1" };
            TransitionFunction<char, string> transitionFunction = new();
            transitionFunction.AddTransition("q0", '0', "q0");
            transitionFunction.AddTransition("q0", '0', "q2");
            transitionFunction.AddTransition(new State<string>("q0"), Symbol<char>.Epsilon, new State<string>("q2"));

            transitionFunction.AddTransition("q2", '1', "q2");
            transitionFunction.AddTransition("q2", '1', "q1");


            var epsilonNFA = new NonDeterministicEpsilonFiniteAutomaton<char, string>(inputAlphabet, states, initialState, transitionFunction, finalStates);



            //Testing inputs
            (string, bool)[] inputs = { ("00", false), ("10", true), ("", false), ("1", false), ("0", false), ("11111", false), ("101011", false), ("01111", false), ("1101010", true) };

            foreach (var (inputString, accepted) in inputs)
            {
                Console.WriteLine($"{inputString}: {epsilonNFA.Accepts(inputString)}");
                // Assert.Equal(automaton.Accepts(inputString), accepted);
            }


        }
    }
}