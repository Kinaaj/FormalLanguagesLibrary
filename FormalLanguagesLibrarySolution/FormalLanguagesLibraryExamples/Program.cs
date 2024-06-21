using FormalLanguagesLibrary.Grammars;
using FormalLanguagesLibrary.Automata;

namespace FormalLanguagesLibraryExamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            char[] nonTerminals = { 'A', 'B', 'S','C' };
            char[] terminals = { '1', '2', '3','4'};
            char starTSymbolValue = 'S';

            var rule1 = new Tuple<char[], char[]>(
                "1B".ToCharArray(), "12".ToCharArray()
            );

            var rule2 = new Tuple<char[], char[]>(
                ['A', '1', 'B', '2', '3', 'B'],
                ['A', '1', 'B', '2','4', 'B', 'S']
            );

            var productionRules = new[] { rule1 };

            ContextSensitiveGrammar<char> g = new ContextSensitiveGrammar<char>(
                nonTerminals,
                terminals,
                starTSymbolValue,
                productionRules
            );

            //TODO: Repair! Accept function is not working...


            //Design a DFA with ∑ = {0, 1} accepts those string which starts with 1 and ends with 0.

            char[] inputAlphabet = { '0', '1' };
            string[] states = { "q1", "q2", "q3", "trash"};
            string initialState = "q1";
            string[] finalStates = { "q2" };

            TransitionFunction<string, char> transitionFunction = new();
            transitionFunction.AddTransition("q1", '1', "q2");
            transitionFunction.AddTransition("q1", '0', "trash");

            transitionFunction.AddTransition("trash", '0', "trash");
            transitionFunction.AddTransition("trash", '1', "trash");

            transitionFunction.AddTransition("q2", '0', "q3");
            transitionFunction.AddTransition("q2", '1', "q2");

            transitionFunction.AddTransition("q3", '0', "q3");
            transitionFunction.AddTransition("q3", '1', "q2");


            DeterministicFiniteAutomaton<char, string> automaton = new(inputAlphabet, states, initialState, transitionFunction,finalStates);

            Console.WriteLine(automaton.ToString());
            string[] inputs = { "0010011", "10", "", "1", "0", "11111", "101011", "01111", "1101010" };

            foreach(var input in inputs)
            {
                Console.WriteLine($"{input}: {automaton.Accepts(input)}");
            }


        }
    }
}
