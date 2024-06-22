using FormalLanguagesLibrary.Automata;
using FormalLanguagesLibrary.Grammars;

namespace FormalLanguagesLibrary.Converter
{

    public class RGtoDFA
    {
        public static NonDeterministicFiniteAutomaton<char, string> Convert(RegularGrammar<char> grammar)
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
            foreach (var nonTerminal in  grammar.NonTerminals)
            {
                var state = new State<string>($"{ nonTerminal.Value }");
                states.Add(state);
                if(grammar.StartSymbol == nonTerminal)
                {
                    initialState = state;
                }
            }

            //Establish final state
            var finalState = new State<string>("f");
            int i = 0;

            while (states.Contains(finalState))
            {
                finalState = new State<string>($"f{i}");
                i++;
            }

            finalStates.Add(finalState);


            //Establish transition function

            TransitionFunction<char, string> transitionFunction = new TransitionFunction<char, string>();

            foreach(var rule in grammar.ProductionRules)
            {
                if (rule.IsEpsilonRule())
                {
                    
                }

                string inputStateValue;
                char inputSymbolValue = default;
                string outputStateValue = "";

                inputStateValue = $"{rule.LeftHandSide[0].Value}";


                if(rule.RightHandSide.Length == 1)
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

            return new NonDeterministicFiniteAutomaton<char, string>(inputAlphabet, states, initialState, transitionFunction, finalStates);
        }
    }
}
