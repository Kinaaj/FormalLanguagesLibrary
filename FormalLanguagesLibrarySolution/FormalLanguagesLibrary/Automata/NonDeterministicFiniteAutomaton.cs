using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalLanguagesLibrary.Automata
{
    public class NonDeterministicFiniteAutomaton<TSymbolValue,TStateValue> : FiniteStateAutomaton<TSymbolValue, TStateValue>
    {
        
        public NonDeterministicFiniteAutomaton(NonDeterministicFiniteAutomaton<TSymbolValue, TStateValue> other) : base(other) { }

        public NonDeterministicFiniteAutomaton(HashSet<Symbol<TSymbolValue>> inputAlphabet, HashSet<State<TStateValue>> states, State<TStateValue> initialState, TransitionFunction<TStateValue, TSymbolValue> transitionFunction, HashSet<State<TStateValue>> finalStates) : base(inputAlphabet, states, initialState, transitionFunction, finalStates) { }

        public NonDeterministicFiniteAutomaton(IEnumerable<TSymbolValue> inputAlphabetValues, IEnumerable<TStateValue> statesValues, TStateValue initialStateValue, TransitionFunction<TStateValue, TSymbolValue> transitionFunction, IEnumerable<TStateValue> finalStatesValues) : base(inputAlphabetValues, statesValues, initialStateValue, transitionFunction, finalStatesValues)
        {

        }


        protected override void _checkInvariants()
        {
            base._checkInvariants();
            //It cannot include any epsilon transitions
            if (_transitionFunction.HasEpsilonTransitions())
            {
                throw new FiniteAutomatonException("In a DFA, there cannot be epsilon transitions");
            }
        }
    }
}
