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
            (string, bool)[] inputs = { ("00", false), ("10", false), ("", false), ("1", true), ("0", false), ("11111", true), ("101011", false), ("01111", true), ("1101010", false) };

            foreach (var (inputString, accepted) in inputs)
            {
                // Console.WriteLine($"{inputString}: {epsilonNFA.Accepts(inputString)}");
                Assert.Equal(epsilonNFA.Accepts(inputString), accepted);
            }


        }


        [Fact]
        public void eNFATest2()
        {
            //Arrange
            char[] inputAlphabet = { '0', '1' };
            string[] states = { "q0", "q1", "q2" };
            string initialState = "q0";
            string[] finalStates = { "q1" };
            TransitionFunction<char, string> transitionFunction = new();
            transitionFunction.AddTransition("q0", '0', "q0");
            transitionFunction.AddTransition("q0", '1', "q2");
            transitionFunction.AddTransition(new State<string>("q0"), Symbol<char>.Epsilon, new State<string>("q1"));

            transitionFunction.AddTransition("q2", '1', "q2");
            transitionFunction.AddTransition("q2", '1', "q1");


            var epsilonNFA = new NonDeterministicEpsilonFiniteAutomaton<char, string>(inputAlphabet, states, initialState, transitionFunction, finalStates);

            //Testing inputs
            (string, bool)[] inputs = { ("00", true), ("10", false), ("", true), ("1", false), ("0", true), ("11111", true), ("101011", false), ("01111", true), ("1101010", false) };

            foreach (var (inputString, accepted) in inputs)
            {
                // Console.WriteLine($"{inputString}: {epsilonNFA.Accepts(inputString)}");
                Assert.Equal(epsilonNFA.Accepts(inputString), accepted);
            }
        }

        [Fact]
        public void NFAReachableStatesTest()
        {
            //Arrange
            char[] inputAlphabet = { '0', '1' };
            string[] states = { "q0", "q1", "q2", "q3", "q4" };
            string initialState = "q0";
            string[] finalStates = { "q1" };
            TransitionFunction<char, string> transitionFunction = new();
            transitionFunction.AddTransition("q0", '0', "q0");
            transitionFunction.AddTransition("q0", '1', "q2");
            transitionFunction.AddTransition(new State<string>("q0"), Symbol<char>.Epsilon, new State<string>("q1"));

            transitionFunction.AddTransition("q2", '1', "q2");
            transitionFunction.AddTransition("q2", '1', "q2");
            transitionFunction.AddTransition("q3", '0', "q2");
            transitionFunction.AddTransition("q4", '1', "q1");
            transitionFunction.AddTransition("q4", '0', "q1");


            var NFA = new NonDeterministicEpsilonFiniteAutomaton<char, string>(inputAlphabet, states, initialState, transitionFunction, finalStates);

            Assert.Equal(new HashSet<State<string>> { new State<string>("q0"), new State<string>("q1"), new State<string>("q2") }, NFA.GetReachableStates());

            //Compare before removing reachable states
            Assert.Equal(new HashSet<State<string>> { new State<string>("q0"), new State<string>("q1"), new State<string>("q2"), new State<string>("q3"), new State<string>("q4") }, NFA.States);
            NFA.RemoveUnreachableStates();
            //Compare after removing unreachable states
            Assert.Equal(new HashSet<State<string>> { new State<string>("q0"), new State<string>("q1"), new State<string>("q2") }, NFA.States);
        }


        [Fact]
        public void eNFAtoNFATest()
        {


            var s0 = new Symbol<char>('0');
            var s1 = new Symbol<char>('1');
            var epsilon = Symbol<char>.Epsilon;

            var A = new State<string>("A");
            var B = new State<string>("B");
            var C = new State<string>("C");

            //Arrange
            var inputAlphabet = new HashSet<Symbol<char>> { s0, s1 };
            var states = new HashSet<State<string>> { A, B, C };
            var initialState = A;
            var finalStates = new HashSet<State<string>> { C };

            var transitionFunction = new TransitionFunction<char, string>();
            transitionFunction.AddTransition(A, s0, A);
            transitionFunction.AddTransition(A, epsilon, B);

            transitionFunction.AddTransition(B, s1, B);
            transitionFunction.AddTransition(B, epsilon, C);

            transitionFunction.AddTransition(C, s0, C);
            transitionFunction.AddTransition(C, s1, C);




            var eNFA = new NonDeterministicEpsilonFiniteAutomaton<char, string>(inputAlphabet, states, initialState, transitionFunction, finalStates);

            var actualNFA = eNFA.ToNFA();

            var expectedTransitionFunction = new TransitionFunction<char, string>();

            expectedTransitionFunction.AddTransition(A, s0, [A, B, C]);
            expectedTransitionFunction.AddTransition(A, s1, [B, C]);

            expectedTransitionFunction.AddTransition(B, s0, C);
            expectedTransitionFunction.AddTransition(B, s1, [B, C]);

            expectedTransitionFunction.AddTransition(C, s0, C);
            expectedTransitionFunction.AddTransition(C, s1, C);

            var expectedFinalStates = new HashSet<State<string>> { A, B, C };


            var expectedNFA = new NonDeterministicFiniteAutomaton<char, string>(inputAlphabet, states, initialState, expectedTransitionFunction, expectedFinalStates);


            Assert.Equal(expectedNFA.InputAlphabet, actualNFA.InputAlphabet);
            Assert.Equal(expectedNFA.States, actualNFA.States);
            Assert.Equal(expectedNFA.InitialState, actualNFA.InitialState);
            Assert.Equal(expectedNFA.TransitionFunction, actualNFA.TransitionFunction);
            Assert.Equal(expectedNFA.FinalStates, actualNFA.FinalStates);



        }
    }
}