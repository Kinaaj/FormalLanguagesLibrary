using System;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace FormalLanguagesLibrary.Automata
{
    public class TransitionFunction<TSymbolValue, TStateValue> : IEquatable<TransitionFunction<TSymbolValue, TStateValue>>
    {
        // Dictionary to store transitions, where the key is a tuple of input state and input symbol,
        // and the value is a set of output states.
        private Dictionary<Tuple<State<TStateValue>, Symbol<TSymbolValue>>, HashSet<State<TStateValue>>> _transitions = new();

        // Property to expose the transitions dictionary as a read-only dictionary.
        public IReadOnlyDictionary<Tuple<State<TStateValue>, Symbol<TSymbolValue>>, HashSet<State<TStateValue>>> Transitions => _transitions;

        // Default constructor
        public TransitionFunction() { }

        public TransitionFunction(Dictionary<Tuple<State<TStateValue>, Symbol<TSymbolValue>>, HashSet<State<TStateValue>>> transitions)
        {
            _transitions = transitions;
        }

        public TransitionFunction(TransitionFunction<TSymbolValue, TStateValue> transitionFunction)
        {
            _transitions = new(transitionFunction.Transitions);
        }

        // Constructor to initialize the transition function from a dictionary with simpler types.
        public TransitionFunction(Dictionary<Tuple<TStateValue, TSymbolValue>, IEnumerable<TStateValue>> transitions)
        {
            foreach (var pair in transitions)
            {
                State<TStateValue> inputState = new(pair.Key.Item1);
                Symbol<TSymbolValue> inputSymbol = new(pair.Key.Item2);
                Tuple<State<TStateValue>, Symbol<TSymbolValue>> inputTuple = new(inputState, inputSymbol);

                HashSet<State<TStateValue>> outputStates = new();
                foreach (var stateValue in pair.Value)
                {
                    State<TStateValue> outputState = new(stateValue);
                    outputStates.Add(outputState);
                }
                _transitions[inputTuple] = outputStates;
            }
        }

        public bool AddTransition(TStateValue inputStateValue, TSymbolValue inputSymbolValue, TStateValue outputStateValue)
        {
            return AddTransition(new State<TStateValue>(inputStateValue), new Symbol<TSymbolValue>(inputSymbolValue), new State<TStateValue>(outputStateValue));
        }

        public bool AddTransition(State<TStateValue> inputState, Symbol<TSymbolValue> inputSymbol, State<TStateValue> outputState)
        {
            return AddTransition(new Tuple<State<TStateValue>, Symbol<TSymbolValue>>(inputState, inputSymbol), outputState);
        }

        public bool AddTransition(State<TStateValue> inputState, Symbol<TSymbolValue> inputSymbol, HashSet<State<TStateValue>> outputStates)
        {
            return AddTransitions(new Tuple<State<TStateValue>, Symbol<TSymbolValue>>(inputState, inputSymbol), outputStates);
        }

        // Method to add multiple transitions to the function, returns false if something wasn't added properly
        public bool AddTransitions(Tuple<State<TStateValue>, Symbol<TSymbolValue>> input, HashSet<State<TStateValue>> states)
        {
            if (!_transitions.ContainsKey(input))
            {
                _transitions[input] = new HashSet<State<TStateValue>>();
            }

            bool isAddedSuccessfully = true;

            foreach (State<TStateValue> state in states)
            {
                isAddedSuccessfully &= _transitions[input].Add(state);
            }
            return isAddedSuccessfully;
        }

        // Method to add multiple transitions to the function, returns false if the state wasn't added properly
        public bool AddTransition(Tuple<State<TStateValue>, Symbol<TSymbolValue>> input, State<TStateValue> state)
        {
            if (!_transitions.ContainsKey(input))
            {
                _transitions[input] = new HashSet<State<TStateValue>>();
            }

            return _transitions[input].Add(state);
        }

        // Method to add a transition using tuple of state and symbol values.
        public bool AddTransition(Tuple<TStateValue, TSymbolValue> input, TStateValue stateValue)
        {
            State<TStateValue> inputState = new(input.Item1);
            Symbol<TSymbolValue> symbol = new(input.Item2);

            Tuple<State<TStateValue>, Symbol<TSymbolValue>> newInput = new(inputState, symbol);
            State<TStateValue> outputState = new(stateValue);

            return AddTransition(newInput, outputState);
        }

        // Returns false if the transition wasn't there
        public bool RemoveTransition(Tuple<State<TStateValue>, Symbol<TSymbolValue>> input)
        {
            return _transitions.Remove(input);
        }

        // Returns false if the transition wasn't there
        public bool RemoveTransition(Tuple<TStateValue, TSymbolValue> input)
        {
            State<TStateValue> inputState = new(input.Item1);
            Symbol<TSymbolValue> symbol = new(input.Item2);

            Tuple<State<TStateValue>, Symbol<TSymbolValue>> newInput = new(inputState, symbol);

            return _transitions.Remove(newInput);
        }

        // Returns false if the transition wasn't there
        public bool RemoveTransition(State<TStateValue> state)
        {
            bool removed = false;

            foreach (var ((inputState, inputSymbol), _) in _transitions)
            {
                if (inputState == state)
                {
                    _transitions.Remove(new(inputState, inputSymbol));
                    removed = true;
                }
            }

            return removed;
        }

        public bool RemoveTransition(State<TStateValue> state, Symbol<TSymbolValue> symbol)
        {
            return _transitions.Remove(new(state, symbol));
        }

        public bool RemoveTransition(State<TStateValue> state, Symbol<TSymbolValue> symbol, State<TStateValue> outputState)
        {
            Tuple<State<TStateValue>, Symbol<TSymbolValue>> pair = new(state, symbol);
            bool successfullyRemoved = _transitions[pair]?.Remove(outputState) ?? false;

            if (successfullyRemoved && _transitions[pair].Count == 0)
            {
                _transitions.Remove(pair);
            }
            return successfullyRemoved;
        }

        public bool RemoveTransition(TStateValue stateValue, TSymbolValue symbolValue, TStateValue outputStateValue)
        {
            return RemoveTransition(new State<TStateValue>(stateValue), new Symbol<TSymbolValue>(symbolValue), new State<TStateValue>(outputStateValue));
        }

        // Method to check if there are any epsilon transitions in the function.
        public bool HasEpsilonTransitions()
        {
            foreach (var transition in _transitions)
            {
                var inputSymbol = transition.Key.Item2;
                if (inputSymbol.Type.Equals(SymbolType.Epsilon) && transition.Value.Count != 0)
                {
                    return true;
                }
            }
            return false;
        }

        // Method to check if a specific input state and symbol combination exists in the function.
        public bool Contains(State<TStateValue> inputState, Symbol<TSymbolValue> inputSymbol)
        {
            return Contains(new(inputState, inputSymbol));
        }

        // Method to check if a specific input pair exists in the function.
        public bool Contains(Tuple<State<TStateValue>, Symbol<TSymbolValue>> inputPair)
        {
            return (_transitions.ContainsKey(inputPair) && _transitions[inputPair].Count != 0);
        }

        // Method to convert the transition function to a string representation.
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var transition in _transitions)
            {
                var inputState = transition.Key.Item1;
                var inputSymbol = transition.Key.Item2;
                var outputStates = transition.Value;

                sb.AppendLine($"({inputState.ToString()}, {inputSymbol.ToString()}) -> {{{string.Join(", ", outputStates)}}}");
            }

            return sb.ToString();
        }

        // Indexer to get output states for a specific input state and symbol.
        public HashSet<State<TStateValue>> this[State<TStateValue> inputState, Symbol<TSymbolValue> inputSymbol]
        {
            get
            {
                return GetOutputStates(inputState, inputSymbol);
            }
        }

        // Indexer to get output states for a specific input state and symbol values.
        public HashSet<State<TStateValue>> this[TStateValue inputState, TSymbolValue inputSymbol]
        {
            get
            {
                return GetOutputStates(new(inputState), new(inputSymbol));
            }
        }

        // Method to get output states for a specific input state and symbol.
        public HashSet<State<TStateValue>> GetOutputStates(State<TStateValue> inputState, Symbol<TSymbolValue> inputSymbol)
        {
            var key = Tuple.Create(inputState, inputSymbol);
            return _transitions.ContainsKey(key) ? _transitions[key] : new HashSet<State<TStateValue>>();
        }

        // Method to get closure states for a specific input state and symbol.
        public HashSet<State<TStateValue>> GetClosure(State<TStateValue> currentState, Symbol<TSymbolValue> transitionSymbol)
        {
            HashSet<State<TStateValue>> closure = new();
            // Check transitions from currentState with the specific transitionSymbol
            foreach (var transition in _transitions)
            {
                var inputState = transition.Key.Item1;
                var inputSymbol = transition.Key.Item2;
                var outputStates = transition.Value;

                if (inputState == currentState && inputSymbol == transitionSymbol)
                {
                    closure.UnionWith(outputStates);
                }
            }
            return closure;
        }

        // Method to get closure states for a set of input states and a specific symbol.
        public HashSet<State<TStateValue>> GetClosure(HashSet<State<TStateValue>> currentStates, Symbol<TSymbolValue> transitionSymbol)
        {
            HashSet<State<TStateValue>> finalClosure = new();
            foreach (var currentState in currentStates)
            {
                finalClosure.UnionWith(GetClosure(currentState, transitionSymbol));
            }

            return finalClosure;
        }

        // Method to get epsilon closure states for a specific input state.
        public HashSet<State<TStateValue>> GetEpsilonClosure(State<TStateValue> currentState)
        {
            HashSet<State<TStateValue>> closure = new();
            Queue<State<TStateValue>> queue = new();
            HashSet<State<TStateValue>> visited = new();

            queue.Enqueue(currentState);
            visited.Add(currentState);
            closure.Add(currentState);

            while (queue.Count > 0)
            {
                var state = queue.Dequeue();

                // Check epsilon transitions from state
                foreach (var transition in _transitions)
                {
                    var inputState = transition.Key.Item1;
                    var inputSymbol = transition.Key.Item2;
                    var outputStates = transition.Value;

                    if (inputState == state && inputSymbol.Type == SymbolType.Epsilon)
                    {
                        foreach (var nextState in outputStates)
                        {
                            if (!visited.Contains(nextState))
                            {
                                visited.Add(nextState);
                                closure.Add(nextState);
                                queue.Enqueue(nextState);
                            }
                        }
                    }
                }
            }

            return closure;
        }

        // Method to get epsilon closure states for a set of input states.
        public HashSet<State<TStateValue>> GetEpsilonClosure(HashSet<State<TStateValue>> currentStates)
        {
            HashSet<State<TStateValue>> finalClosure = new();
            foreach (var currentState in currentStates)
            {
                finalClosure.UnionWith(GetEpsilonClosure(currentState));
            }
            return finalClosure;
        }

        // Method to check equality of this transition function with another transition function.
        public bool Equals(TransitionFunction<TSymbolValue, TStateValue>? other)
        {
            if (other == null)
            {
                return false;
            }

            foreach (var (inputPair, outputStates) in other.Transitions)
            {
                if (!_transitions.TryGetValue(inputPair, out _))
                {
                    return false;
                }
                else
                {
                    if (!_transitions[inputPair].SetEquals(outputStates))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
