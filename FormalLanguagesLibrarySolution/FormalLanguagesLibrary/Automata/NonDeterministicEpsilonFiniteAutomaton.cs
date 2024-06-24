using System;


namespace FormalLanguagesLibrary.Automata
{
    //TODO:
    public class NonDeterministicEpsilonFiniteAutomaton<TSymbolValue, TStateValue> : FiniteStateAutomaton<TSymbolValue, TStateValue>
    {

        public NonDeterministicEpsilonFiniteAutomaton(NonDeterministicFiniteAutomaton<TSymbolValue, TStateValue> other) : base(other) { }

        public NonDeterministicEpsilonFiniteAutomaton(HashSet<Symbol<TSymbolValue>> inputAlphabet, HashSet<State<TStateValue>> states, State<TStateValue> initialState, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, HashSet<State<TStateValue>> finalStates) : base(inputAlphabet, states, initialState, transitionFunction, finalStates) { }

        public NonDeterministicEpsilonFiniteAutomaton(IEnumerable<TSymbolValue> inputAlphabetValues, IEnumerable<TStateValue> statesValues, TStateValue initialStateValue, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, IEnumerable<TStateValue> finalStatesValues) : base(inputAlphabetValues, statesValues, initialStateValue, transitionFunction, finalStatesValues)
        {

        }


        public NonDeterministicFiniteAutomaton<TSymbolValue,TStateValue> ToNFA()
        {

            TransitionFunction<TSymbolValue,TStateValue> newTransitionFunction = new();
            Dictionary<State<TStateValue>, HashSet<State<TStateValue>>> epsilonClosures = new();

            foreach(var state in _states)
            {
                epsilonClosures[state] = _transitionFunction.GetEpsilonClosure(state);
            }


            foreach(var actualState in _states)
            {

                foreach (var ((inputState, inputSymbol), outputStates) in _transitionFunction.Transitions)
                {
                    if (inputSymbol.Type == SymbolType.Epsilon)
                    {
                        continue;
                    }

                    // if the state at the left side of transition is reachable from the actual state
                    if(epsilonClosures[actualState].Contains(inputState))
                    {
                        HashSet<State<TStateValue>> newOutputStates = new HashSet<State<TStateValue>>();

                        foreach (var outputState in outputStates)
                        {
                            newOutputStates.UnionWith(epsilonClosures[outputState]);
                        }
                        newTransitionFunction.AddTransition(actualState, inputSymbol, newOutputStates);
                    }
                }
            }



            HashSet<State<TStateValue>> newFinalStates = new();
            foreach(var state in _states)
            {
                if (epsilonClosures[state].Intersect(_finalStates).Any())
                {
                    newFinalStates.Add(state);
                }
            }
            return new NonDeterministicFiniteAutomaton<TSymbolValue, TStateValue>(new(_inputAlphabet), new(_states), _initialState, newTransitionFunction, newFinalStates);
        }


        public override DeterministicFiniteAutomaton<TSymbolValue,TStateValue> ToDFA()
        {
            return ToNFA().ToDFA();
        }



        protected override void _checkInvariants()
        {
            base._checkInvariants();
        }
    }
}
