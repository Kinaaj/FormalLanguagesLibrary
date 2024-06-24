using FormalLanguagesLibrary.Automata;


namespace FormalLanguageLibraryTests.Automata
{
    public class TransitionFunctionTests
    {
        [Fact]
        public void CreatingTransitionFunctionTest()
        {
            TransitionFunction<char, string> transitionFunction = new();
            transitionFunction.AddTransition("q0", '0', "q0");
            transitionFunction.AddTransition("q0", '1', "q2");
            transitionFunction.AddTransition(new State<string>("q0"), Symbol<char>.Epsilon, new State<string>("q1"));

            transitionFunction.AddTransition("q2", '1', "q2");
            transitionFunction.AddTransition("q2", '1', "q1");

            Assert.True(transitionFunction.HasEpsilonTransitions());
            transitionFunction.RemoveTransition(new State<string>("q0"), Symbol<char>.Epsilon, new State<string>("q1"));
            Assert.False(transitionFunction.HasEpsilonTransitions());


            Assert.Contains(new State<string>("q1"), transitionFunction["q2", '1'] );
            transitionFunction.RemoveTransition("q2", '1', "q1");
            Assert.DoesNotContain(new State<string>("q1"), transitionFunction["q2", '1']);

        }

        [Fact]
        public void ClosureTransitionFunctionTest()
        {
            var q0 = new State<string>("q0");
            var q1 = new State<string>("q1");
            var q2 = new State<string>("q2");
            var q3 = new State<string>("q3");
            var q4 = new State<string>("q4");

            var s0 = new Symbol<char>('0');
            var s1 = new Symbol<char>('1');


            TransitionFunction<char, string> transitionFunction = new();
            transitionFunction.AddTransition(q0, s0, q0);
            transitionFunction.AddTransition(q0, s1, q2);
            transitionFunction.AddTransition(q0, s1, q3);
            transitionFunction.AddTransition(q2, s1, q1);
            transitionFunction.AddTransition(q0, Symbol<char>.Epsilon, q1);
            transitionFunction.AddTransition(q1, Symbol<char>.Epsilon, q2);
            transitionFunction.AddTransition(q2, Symbol<char>.Epsilon, q1);
            transitionFunction.AddTransition(q2, Symbol<char>.Epsilon, q3);
            transitionFunction.AddTransition(q2, Symbol<char>.Epsilon, q4);


            HashSet<State<string>> expectedEpsilonClosure = new() { q0, q1, q2, q3, q4 };
            HashSet<State<string>> expectedClosure = new() {q2, q3};

            Assert.Equal(expectedEpsilonClosure, transitionFunction.GetEpsilonClosure(q0));
            Assert.Equal(expectedClosure, transitionFunction.GetClosure(q0, s1));

        }


    }
}
