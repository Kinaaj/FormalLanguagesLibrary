using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormalLanguagesLibrary.Automata
{
    // Abstract base class representing a finite state automaton (NFA or DFA)
    public abstract class FiniteStateAutomaton<TSymbolValue, TStateValue> : Automaton
    {
        // Protected fields to store input alphabet, states, initial state, transition function, and final states
        protected HashSet<Symbol<TSymbolValue>> _inputAlphabet = new();
        protected HashSet<State<TStateValue>> _states = new();
        protected State<TStateValue> _initialState;
        protected TransitionFunction<TSymbolValue, TStateValue> _transitionFunction;
        protected HashSet<State<TStateValue>> _finalStates = new();

        // Public properties to expose read-only collections of input alphabet, states, initial state, transition function, and final states
        public IReadOnlyCollection<Symbol<TSymbolValue>> InputAlphabet => _inputAlphabet;
        public IReadOnlyCollection<State<TStateValue>> States => _states;
        public State<TStateValue> InitialState => _initialState;
        public TransitionFunction<TSymbolValue, TStateValue> TransitionFunction => _transitionFunction;
        public IReadOnlyCollection<State<TStateValue>> FinalStates => _finalStates;

        public FiniteStateAutomaton(IEnumerable<TSymbolValue> inputAlphabetValues, IEnumerable<TStateValue> statesValues,
                                    TStateValue initialStateValue, TransitionFunction<TSymbolValue, TStateValue> transitionFunction,
                                    IEnumerable<TStateValue> finalStatesValues)
        {
            foreach (TSymbolValue symbolValue in inputAlphabetValues)
            {
                Symbol<TSymbolValue> symbol = new(symbolValue);
                _inputAlphabet.Add(symbol);
            }

            foreach (TStateValue stateValue in statesValues)
            {
                State<TStateValue> state = new(stateValue);
                _states.Add(state);
            }

            _initialState = new(initialStateValue);

            _transitionFunction = new(transitionFunction);

            foreach (TStateValue stateValue in finalStatesValues)
            {
                State<TStateValue> state = new(stateValue);
                _finalStates.Add(state);
            }

            // Check invariants to ensure consistency of the automaton
            _checkInvariants();
        }

        public FiniteStateAutomaton(HashSet<Symbol<TSymbolValue>> inputAlphabet, HashSet<State<TStateValue>> states,
                                    State<TStateValue> initialState, TransitionFunction<TSymbolValue, TStateValue> transitionFunction,
                                    HashSet<State<TStateValue>> finalStates)
        {
            _inputAlphabet = inputAlphabet;
            _states = states;
            _initialState = initialState;
            _transitionFunction = transitionFunction;
            _finalStates = finalStates;

            // Check invariants to ensure consistency of the automaton
            _checkInvariants();
        }

        // Copy constructor to create a deep copy of another finite state automaton
        public FiniteStateAutomaton(FiniteStateAutomaton<TSymbolValue, TStateValue> other)
        {
            _inputAlphabet = new(other.InputAlphabet);
            _states = new(other.States);
            _initialState = other.InitialState;
            _transitionFunction = other.TransitionFunction;
            _finalStates = new(other.FinalStates);
        }

        // Method to check if the automaton accepts a sequence of symbols
        public bool Accepts(IEnumerable<Symbol<TSymbolValue>> symbols)
        {
            // Start with the initial state
            HashSet<State<TStateValue>> reachedStates = new() { _initialState };

            foreach (var symbol in symbols)
            {
                // Check if the symbol is in the input alphabet
                if (!_inputAlphabet.Contains(symbol))
                {
                    throw new FiniteAutomatonException($"Symbol {symbol} in the input sequence {symbols.ToString()} is not included in the input alphabet.");
                }

                // Compute epsilon closure of the reached states
                reachedStates = _transitionFunction.GetEpsilonClosure(reachedStates);

                // Compute new reached states based on current symbol
                HashSet<State<TStateValue>> newReachedStates = _transitionFunction.GetClosure(reachedStates, symbol);

                // Update reached states with new reached states
                reachedStates = newReachedStates;
                newReachedStates = new();
            }

            // Compute epsilon closure of final reached states
            reachedStates = _transitionFunction.GetEpsilonClosure(reachedStates);

            // Check if there is at least one final state in the reached states
            return _finalStates.Intersect(reachedStates).Any();
        }

        // Overloaded Accepts method to accept an IEnumerable of symbol values
        public bool Accepts(IEnumerable<TSymbolValue> symbols)
        {
            // Convert symbol values to symbols and call the main Accepts method
            List<Symbol<TSymbolValue>> list = symbols.Select(symbolValue => new Symbol<TSymbolValue>(symbolValue)).ToList();
            return Accepts(list);
        }

        // Overloaded Accepts method to accept an array of symbol values
        public bool Accepts(TSymbolValue[] symbols)
        {
            // Call the Accepts method with IEnumerable of symbol values
            return Accepts((IEnumerable<TSymbolValue>)symbols);
        }

        // Method to convert the NFA to its corresponding DFA (to be implemented in derived classes)
        public abstract DeterministicFiniteAutomaton<TSymbolValue, TStateValue> ToDFA();

        protected virtual void _checkInvariants()
        {
            // Check if the initial state is in the set of states
            if (!_states.Contains(_initialState))
            {
                throw new FiniteAutomatonException($"The initial state {_initialState} must be part of the set of states.");
            }


            // Check if all states and symbols in the transition function are valid
            foreach (var transition in _transitionFunction.Transitions)
            {
                var inputState = transition.Key.Item1;
                var inputSymbol = transition.Key.Item2;
                var outputStates = transition.Value;

                // Check if the input state is in the set of states
                if (!_states.Contains(inputState))
                {
                    throw new FiniteAutomatonException($"State {inputState} in the transition function must be part of the set of states.");
                }

                // Check if the input symbol is in the input alphabet
                if (!_inputAlphabet.Contains(inputSymbol) && inputSymbol.Type != SymbolType.Epsilon)
                {
                    throw new FiniteAutomatonException($"Symbol {inputSymbol} in the transition function must be part of the input alphabet.");
                }

                // Check if all output states are in the set of states
                foreach (var outputState in outputStates)
                {
                    if (!_states.Contains(outputState))
                    {
                        throw new FiniteAutomatonException($"State {outputState} in the transition function must be part of the set of states.");
                    }
                }
            }

            foreach (var finalState in _finalStates)
            {
                if (!_states.Contains(finalState))
                {
                    throw new FiniteAutomatonException($"Final state {finalState} must be defined in states.");
                }
            }

        }


        // Method to get reachable states starting from the initial state
        public HashSet<State<TStateValue>> GetReachableStates()
        {
            // Initialize reachable states and queue to process states
            var reachable = new HashSet<State<TStateValue>>();
            var toProcess = new Queue<State<TStateValue>>();
            toProcess.Enqueue(_initialState);

            while (toProcess.Count > 0)
            {
                var state = toProcess.Dequeue();

                // Add state to reachable if not already added
                if (!reachable.Contains(state))
                {
                    reachable.Add(state);

                    // Iterate through input alphabet to find reachable states for each symbol
                    foreach (var symbol in _inputAlphabet)
                    {
                        var newReachableStates = _transitionFunction.GetClosure(state, symbol);

                        // Add epsilon closure of new state to queue for processing
                        foreach (var newState in newReachableStates)
                        {
                            var epsilonClosureOfNewState = _transitionFunction.GetEpsilonClosure(newState);
                            foreach (var reachableState in epsilonClosureOfNewState)
                            {
                                toProcess.Enqueue(reachableState);
                            }
                        }
                    }
                }
            }

            return reachable;
        }

        // Method to remove unreachable states from the automaton
        public void RemoveUnreachableStates()
        {
            // Get reachable and unreachable states
            var reachableStates = GetReachableStates();
            var unreachableStates = _states.Except(reachableStates);

            // Update states and final states to include only reachable states
            _states.IntersectWith(reachableStates);
            _finalStates.IntersectWith(reachableStates);

            // Remove transitions to/from unreachable states in transition function
            foreach (var unreachableState in unreachableStates)
            {
                _transitionFunction.RemoveTransition(unreachableState);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            //Input Alphabet
            sb.Append("InputAlphabet: {");
            sb.Append(string.Join(", ", _inputAlphabet));
            sb.AppendLine("}");

            //States
            sb.Append("States: {");
            sb.Append(string.Join(", ", _states));
            sb.AppendLine("}");

            //Initial State
            sb.Append($"InitialState: {_initialState}");

            //Transition Function
            sb.Append($"Transition Funciton:\n{_transitionFunction}");

            //Final States
            sb.Append("Final States: {");
            sb.Append(string.Join(", ", _finalStates));
            sb.AppendLine("}");

            return sb.ToString();

        }

    }

    // Exception class for finite state automaton related errors
    public class FiniteAutomatonException : Exception
    {
        public FiniteAutomatonException(string message) : base(message)
        {

        }
    }
}
