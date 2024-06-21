using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalLanguagesLibrary.Automata
{
    public class DeterministicFiniteAutomaton<TSymbolValue, TStateValue> : NonDeterministicFiniteAutomaton<TSymbolValue, TStateValue>
    {

        public DeterministicFiniteAutomaton(DeterministicFiniteAutomaton<TSymbolValue, TStateValue> other) : base(other) { }

        public DeterministicFiniteAutomaton(IEnumerable<TSymbolValue> inputAlphabetValues, IEnumerable<TStateValue> statesValues, TStateValue initialStateValue, TransitionFunction<TStateValue, TSymbolValue> transitionFunction, IEnumerable<TStateValue> finalStatesValues) : base(inputAlphabetValues, statesValues, initialStateValue, transitionFunction, finalStatesValues)
        {

        }

        public DeterministicFiniteAutomaton(HashSet<Symbol<TSymbolValue>> inputAlphabet, HashSet<State<TStateValue>> states, State<TStateValue> initialState, TransitionFunction<TStateValue, TSymbolValue> transitionFunction, HashSet<State<TStateValue>> finalStates) : base(inputAlphabet, states, initialState, transitionFunction, finalStates) { }


        protected override void _checkInvariants()
        {
            base._checkInvariants();

            // Ensure each state-symbol pair maps to exactly one state
            foreach (var transition in _transitionFunction.Transitions)
            {
                var outputStates = transition.Value;

                // For DFA, outputStates should contain exactly one state
                if (outputStates.Count != 1)
                {
                    throw new FiniteAutomatonException("In a DFA, each state-symbol pair must map to exactly one state.");
                }
            }


            //There has to be defined transition for each (state x symbol) pair
            foreach(var state in _states)
            {
                foreach(var symbol in _inputAlphabet)
                {
                    Tuple<State<TStateValue>, Symbol<TSymbolValue>> inputPair = new(state, symbol);
                    
                    if(!_transitionFunction.Contains(inputPair))
                    {
                        throw new FiniteAutomatonException($"Transition for ({state} x {symbol}) is not defined.");
                    }
                }
            }


        }

    }
}
