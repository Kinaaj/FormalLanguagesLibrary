using FormalLanguagesLibrary.Automata;
using FormalLanguagesLibrary.Grammars;

namespace FormalLanguagesLibrary.Converter
{

    public class Convert
    {
        public static NonDeterministicFiniteAutomaton<char, string> RGtoNFA(RegularGrammar<char> grammar)
        {

            grammar.RemoveEpsilonRules();

            var states = new HashSet<State<string>>();
            var finalStates = new HashSet<State<string>>();
            var inputAlphabet = new HashSet<Automata.Symbol<char>>();
            State<string> initialState = default;

            //Establish inputAlphabet
            foreach(var terminal in grammar.Terminals)
            {
                inputAlphabet.Add(new Automata.Symbol<char>(terminal.Value));
            }

            //Establish states and initialState
            //Create one state for each nonTerminal
            foreach (var nonTerminal in grammar.NonTerminals)
            {
                var state = new State<string>($"{nonTerminal.Value}");
                states.Add(state);
                if(grammar.StartSymbol == nonTerminal)
                {
                    initialState = state;
                }
            }

            //Establish final state
            var finalState = new State<string>("final");
            int i = 0;

            while (states.Contains(finalState))
            {
                finalState = new State<string>($"final{i}");
                i++;
            }

            finalStates.Add(finalState);
            states.Add(finalState);


            //Establish transition function

            TransitionFunction<char, string> transitionFunction = new TransitionFunction<char, string>();

            foreach(var rule in grammar.ProductionRules)
            {


                string inputStateValue;
                char inputSymbolValue = default;
                string outputStateValue = "";

                inputStateValue = $"{rule.LeftHandSide[0].Value}";

                if (rule.IsEpsilonRule())
                {
                    finalStates.Add(new State<string>(inputStateValue));
                }

                else if (rule.RightHandSide.Length == 1)
                {
                    inputSymbolValue = rule.RightHandSide[0].Value;
                    outputStateValue = finalState.Value;
                }

                else
                {
                    // A -> aB
                    if (grammar.isRightRegular)
                    {
                        inputSymbolValue = rule.RightHandSide[0].Value;
                        outputStateValue = $"{rule.RightHandSide[1].Value}";
                    }

                    // A -> Ba
                    else
                    {
                        outputStateValue = $"{rule.RightHandSide[0].Value}";
                        inputSymbolValue = rule.RightHandSide[1].Value;
                    }
                }
                transitionFunction.AddTransition(inputStateValue, inputSymbolValue, outputStateValue);
            }

            Console.WriteLine($"Input alphabet: {string.Join(',', inputAlphabet)}");
            Console.WriteLine($"States: {string.Join(',', states)}");
            Console.WriteLine($"Initial state: {initialState}");
            Console.WriteLine($"Transition function: {string.Join(',', transitionFunction.Transitions)}");
            Console.WriteLine($"Final states: {string.Join(',', finalStates)}");

            return new NonDeterministicFiniteAutomaton<char, string>(inputAlphabet, states, initialState, transitionFunction, finalStates);
        }
    }
}
