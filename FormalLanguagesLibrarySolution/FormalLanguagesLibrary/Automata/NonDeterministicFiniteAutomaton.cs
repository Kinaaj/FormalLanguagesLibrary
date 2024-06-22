using System;


namespace FormalLanguagesLibrary.Automata
{
    public class NonDeterministicFiniteAutomaton<TSymbolValue,TStateValue> : NonDeterministicEpsilonFiniteAutomaton<TSymbolValue, TStateValue>
    {
        
        public NonDeterministicFiniteAutomaton(NonDeterministicFiniteAutomaton<TSymbolValue, TStateValue> other) : base(other) { }

        public NonDeterministicFiniteAutomaton(HashSet<Symbol<TSymbolValue>> inputAlphabet, HashSet<State<TStateValue>> states, State<TStateValue> initialState, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, HashSet<State<TStateValue>> finalStates) : base(inputAlphabet, states, initialState, transitionFunction, finalStates) { }

        public NonDeterministicFiniteAutomaton(IEnumerable<TSymbolValue> inputAlphabetValues, IEnumerable<TStateValue> statesValues, TStateValue initialStateValue, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, IEnumerable<TStateValue> finalStatesValues) : base(inputAlphabetValues, statesValues, initialStateValue, transitionFunction, finalStatesValues)
        {

        }

        protected override void _checkInvariants()
        {
            base._checkInvariants();
            //It cannot include any epsilon transitions
            if (_transitionFunction.HasEpsilonTransitions())
            {
                throw new FiniteAutomatonException("In a NFA, there cannot be epsilon transitions");
            }
        }

        public override DeterministicFiniteAutomaton<TSymbolValue,TStateValue> ToDFA()
        {
            throw new NotImplementedException();
        }

    }
}
