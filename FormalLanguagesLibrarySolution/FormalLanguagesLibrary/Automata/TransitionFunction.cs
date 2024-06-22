using System;
using System.Text;


namespace FormalLanguagesLibrary.Automata
{

    public class TransitionFunction<TSymbolValue, TStateValue>
    {

        private Dictionary<Tuple<State<TStateValue>, Symbol<TSymbolValue>>, HashSet<State<TStateValue>>> _transitions = new();
        public IReadOnlyDictionary<Tuple<State<TStateValue>, Symbol<TSymbolValue>>, HashSet<State<TStateValue>>> Transitions => _transitions;

        public TransitionFunction()
        {

        }

        public TransitionFunction(Dictionary<Tuple<State<TStateValue>, Symbol<TSymbolValue>>, HashSet<State<TStateValue>>> transitions)
        {
            _transitions = transitions;
        }

        public TransitionFunction(TransitionFunction<TSymbolValue, TStateValue> transitionFunction)
        {
            _transitions = new(transitionFunction.Transitions);
        }

        public TransitionFunction(Dictionary<Tuple<TStateValue,TSymbolValue>,IEnumerable<TStateValue>> transitions)
        {

            foreach(var pair in transitions)
            {
                State<TStateValue> inputState = new(pair.Key.Item1);
                Symbol<TSymbolValue> inputSymbol = new(pair.Key.Item2);
                Tuple<State<TStateValue>, Symbol<TSymbolValue>> inputTuple = new(inputState, inputSymbol);

                HashSet<State<TStateValue>> outputStates = new();
                foreach(var stateValue in pair.Value)
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

        public bool AddTransitions(Tuple<State<TStateValue>, Symbol<TSymbolValue>> input, HashSet<State<TStateValue>> states)
        {

            if (!_transitions.ContainsKey(input))
            {
                _transitions[input] = new HashSet<State<TStateValue>>();
            }


            bool isAddedSuccessfuly = true;

            foreach (State<TStateValue> state in states)
            {
                isAddedSuccessfuly &= _transitions[input].Add(state);
            }
            return isAddedSuccessfuly;
        }

        public bool AddTransition(Tuple<State<TStateValue>, Symbol<TSymbolValue>> input, State<TStateValue> state)
        {
            if (!_transitions.ContainsKey(input))
            {
                _transitions[input] = new HashSet<State<TStateValue>>();
            }

            return _transitions[input].Add(state);
        }

        public bool AddTransition(Tuple<TStateValue, TSymbolValue> input, TStateValue stateValue)
        {

            State<TStateValue> inputState = new(input.Item1);
            Symbol<TSymbolValue> symbol = new(input.Item2);

            Tuple<State<TStateValue>, Symbol<TSymbolValue>> newInput = new(inputState, symbol);

            State<TStateValue> outputState = new(stateValue);

            return AddTransition(newInput, outputState);
        }

        public bool RemoveTransition(Tuple<State<TStateValue>, Symbol<TSymbolValue>> input)
        {
            return _transitions.Remove(input);
        }

        public bool RemoveTransition(Tuple<TStateValue,TSymbolValue> input)
        {
            State<TStateValue> inputState = new(input.Item1);
            Symbol<TSymbolValue> symbol = new(input.Item2);

            Tuple<State<TStateValue>, Symbol<TSymbolValue>> newInput = new(inputState, symbol);

            return _transitions.Remove(newInput);
        }

        public bool RemoveTransition(State<TStateValue> state)
        {
            bool removed = false;

            foreach(var ((inputState,inputSymbol),_) in _transitions)
            {
                if(inputState == state)
                {
                    _transitions.Remove(new(inputState, inputSymbol));
                    removed = true;
                }
            }

            return removed;
        }

        public bool RemoveTransition(State<TStateValue> state,Symbol<TSymbolValue> symbol)
        {
            return _transitions.Remove(new(state, symbol));
        }


        public bool HasEpsilonTransitions()
        {
            foreach(var transition in _transitions)
            {
                var inputSymbol = transition.Key.Item2;
                if (inputSymbol.Type.Equals(SymbolType.Epsilon))
                {
                    return true;
                }
            }
            return false;
        }


        public bool Contains(State<TStateValue> inputState, Symbol<TSymbolValue> inputSymbol)
        {
            return Contains(new(inputState,inputSymbol));
        }

        public bool Contains(Tuple<State<TStateValue>, Symbol<TSymbolValue>> inputPair)
        {
            return _transitions.ContainsKey(inputPair);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var transition in _transitions)
            {
                var inputState = transition.Key.Item1;
                var inputSymbol = transition.Key.Item2;
                var outputStates = transition.Value;

                sb.AppendLine($"({inputState.ToString()}, {inputSymbol.ToString()}) -> [{string.Join(", ", outputStates)}]");
            }

            return sb.ToString();
        }

        public HashSet<State<TStateValue>> this[State<TStateValue> inputState, Symbol<TSymbolValue> inputSymbol]
        {
            get
            {
               return GetTransition(inputState, inputSymbol);
            }
        }

        public HashSet<State<TStateValue>> GetTransition(State<TStateValue> inputState, Symbol<TSymbolValue> inputSymbol)
        {
            var key = Tuple.Create(inputState, inputSymbol);
            return _transitions.ContainsKey(key) ? _transitions[key] : new HashSet<State<TStateValue>>();
        }

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

            HashSet<State<TStateValue>> finalClosure = GetEpsilonClosure(closure);

            return finalClosure;
        }

        public HashSet<State<TStateValue>> GetClosure(HashSet<State<TStateValue>> currentStates, Symbol<TSymbolValue> transitionSymbol)
        {
            HashSet<State<TStateValue>> finalClosure = new();
            foreach(var currentState in currentStates)
            {
                finalClosure.UnionWith(GetClosure(currentState, transitionSymbol));
            }

            return finalClosure;
        }

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



        public HashSet<State<TStateValue>> GetEpsilonClosure(HashSet<State<TStateValue>> currentStates)
        {
            HashSet<State<TStateValue>> finalClosure = new();
            foreach(var currentState in currentStates)
            {
                finalClosure.UnionWith(GetEpsilonClosure(currentState));
            }
            return finalClosure;
        }




    }
}
