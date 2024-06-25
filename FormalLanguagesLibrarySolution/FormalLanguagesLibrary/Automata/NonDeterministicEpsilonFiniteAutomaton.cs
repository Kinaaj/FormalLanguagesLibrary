using System;
using System.Collections.Generic;

namespace FormalLanguagesLibrary.Automata
{
    // Non-deterministic epsilon finite automaton (NFA-epsilon)
    public class NonDeterministicEpsilonFiniteAutomaton<TSymbolValue, TStateValue> : FiniteStateAutomaton<TSymbolValue, TStateValue>
    {
        // Constructors to initialize the NFA-epsilon from various inputs
        public NonDeterministicEpsilonFiniteAutomaton(NonDeterministicFiniteAutomaton<TSymbolValue, TStateValue> other) : base(other) { }

        public NonDeterministicEpsilonFiniteAutomaton(HashSet<Symbol<TSymbolValue>> inputAlphabet, HashSet<State<TStateValue>> states, State<TStateValue> initialState, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, HashSet<State<TStateValue>> finalStates) : base(inputAlphabet, states, initialState, transitionFunction, finalStates) { }

        public NonDeterministicEpsilonFiniteAutomaton(IEnumerable<TSymbolValue> inputAlphabetValues, IEnumerable<TStateValue> statesValues, TStateValue initialStateValue, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, IEnumerable<TStateValue> finalStatesValues) : base(inputAlphabetValues, statesValues, initialStateValue, transitionFunction, finalStatesValues)
        {

        }

        // Convert the NFA-epsilon to a regular NFA
        public NonDeterministicFiniteAutomaton<TSymbolValue, TStateValue> ToNFA()
        {
            // Create a new transition function and dictionary to store epsilon closures
            TransitionFunction<TSymbolValue, TStateValue> newTransitionFunction = new();
            Dictionary<State<TStateValue>, HashSet<State<TStateValue>>> epsilonClosures = new();

            // Compute epsilon closures for each state
            foreach (var state in _states)
            {
                epsilonClosures[state] = _transitionFunction.GetEpsilonClosure(state);
            }

            // Build transitions for the new NFA from epsilon transitions
            foreach (var actualState in _states)
            {
                foreach (var ((inputState, inputSymbol), outputStates) in _transitionFunction.Transitions)
                {
                    // Skip epsilon transitions
                    if (inputSymbol.Type == SymbolType.Epsilon)
                    {
                        continue;
                    }

                    // Check if the actual state can reach the input state through epsilon transitions
                    if (epsilonClosures[actualState].Contains(inputState))
                    {
                        HashSet<State<TStateValue>> newOutputStates = new HashSet<State<TStateValue>>();

                        // Compute epsilon closures for output states
                        foreach (var outputState in outputStates)
                        {
                            newOutputStates.UnionWith(epsilonClosures[outputState]);
                        }

                        // Add new transition to the transition function
                        newTransitionFunction.AddTransition(actualState, inputSymbol, newOutputStates);
                    }
                }
            }

            // Determine new final states based on epsilon closures
            HashSet<State<TStateValue>> newFinalStates = new HashSet<State<TStateValue>>();
            foreach (var state in _states)
            {
                if (epsilonClosures[state].Intersect(_finalStates).Any())
                {
                    newFinalStates.Add(state);
                }
            }

            // Return a new NFA constructed from computed data
            return new NonDeterministicFiniteAutomaton<TSymbolValue, TStateValue>(new(_inputAlphabet), new(_states), _initialState, newTransitionFunction, newFinalStates);
        }

        // Convert the NFA-epsilon to a DFA
        public override DeterministicFiniteAutomaton<TSymbolValue, TStateValue> ToDFA()
        {
            // Convert to NFA and then to DFA
            return ToNFA().ToDFA();
        }

        // Method to check invariants (no additional invariants checked currently)
        protected override void _checkInvariants()
        {
            base._checkInvariants();
        }
    }
}
