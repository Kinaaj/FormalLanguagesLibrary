using FormalLanguagesLibrary.Automata;
using FormalLanguagesLibrary.Grammars;

namespace FormalLanguagesLibrary.Converter
{
    public class Convert
    {
        // Method to convert a Regular Grammar (RG) to a Non-Deterministic Finite Automaton (NFA)
        public static NonDeterministicFiniteAutomaton<char, string> RGtoNFA(RegularGrammar<char> grammar)
        {
            var states = new HashSet<State<string>>();              // Set to store states
            var finalStates = new HashSet<State<string>>();         // Set to store final states
            var inputAlphabet = new HashSet<Automata.Symbol<char>>(); // Set to store input alphabet symbols
            State<string> initialState = default;                   // Variable to store the initial state

            // Establish inputAlphabet from the terminals of the grammar
            foreach (var terminal in grammar.Terminals)
            {
                inputAlphabet.Add(new Automata.Symbol<char>(terminal.Value));
            }

            // Establish states and initialState
            // Create one state for each non-terminal in the grammar
            foreach (var nonTerminal in grammar.NonTerminals)
            {
                var state = new State<string>($"{nonTerminal.Value}");
                states.Add(state);
                if (grammar.StartSymbol == nonTerminal)
                {
                    initialState = state;
                }
            }

            // Establish final state
            var finalState = new State<string>("final");
            int i = 0;
            while (states.Contains(finalState))
            {
                i++;
            }
            finalStates.Add(finalState);
            states.Add(finalState);

            // Establish transition function
            TransitionFunction<char, string> transitionFunction = new TransitionFunction<char, string>();

            // Process each production rule in the grammar
            foreach (var rule in grammar.ProductionRules)
            {
                string inputStateValue;
                char inputSymbolValue = default;
                string outputStateValue = "";

                inputStateValue = $"{rule.LeftHandSide[0].Value}";

                if (rule.IsEpsilonRule())
                {
                    // If the rule is an epsilon rule, add the input state to final states
                    finalStates.Add(new State<string>(inputStateValue));
                }
                else if (rule.RightHandSide.Length == 1)
                {
                    // If the rule has a single symbol on the right-hand side, transition to the final state
                    inputSymbolValue = rule.RightHandSide[0].Value;
                    outputStateValue = finalState.Value;
                    transitionFunction.AddTransition(inputStateValue, inputSymbolValue, outputStateValue);
                }
                else
                {
                    // If the rule has two symbols on the right-hand side, handle right-regular or left-regular formats
                    if (grammar.isRightRegular)
                    {
                        // Right-regular format: A -> aB
                        inputSymbolValue = rule.RightHandSide[0].Value;
                        outputStateValue = $"{rule.RightHandSide[1].Value}";
                    }
                    else
                    {
                        // Left-regular format: A -> Ba
                        outputStateValue = $"{rule.RightHandSide[0].Value}";
                        inputSymbolValue = rule.RightHandSide[1].Value;
                    }
                    transitionFunction.AddTransition(inputStateValue, inputSymbolValue, outputStateValue);
                }
            }

            // Return the constructed NFA
            return new NonDeterministicFiniteAutomaton<char, string>(inputAlphabet, states, initialState, transitionFunction, finalStates);
        }
    }
}
