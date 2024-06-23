
using System;
using System.Text;


namespace FormalLanguagesLibrary.Automata
{
    //TODO TSymbolValue -> Symbol<TSymbolValue> and TState -> State<TState>
    public abstract class FiniteStateAutomaton<TSymbolValue, TStateValue> : Automaton
    {

        protected HashSet<Symbol<TSymbolValue>> _inputAlphabet = new();
        protected HashSet<State<TStateValue>> _states = new();
        protected State<TStateValue> _initialState;
        protected TransitionFunction<TSymbolValue, TStateValue> _transitionFunction;
        protected HashSet<State<TStateValue>> _finalStates = new();


        public IReadOnlyCollection<Symbol<TSymbolValue>> InputAlphabet => _inputAlphabet;
        public IReadOnlyCollection<State<TStateValue>> States => _states;
        public State<TStateValue> InitialState => _initialState;
        public TransitionFunction<TSymbolValue, TStateValue> TransitionFunction => _transitionFunction;
        public IReadOnlyCollection<State<TStateValue>> FinalStates => _finalStates;




        public FiniteStateAutomaton(IEnumerable<TSymbolValue> inputAlphabetValues, IEnumerable<TStateValue> statesValues, TStateValue initialStateValue, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, IEnumerable<TStateValue> finalStatesValues)
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

            _checkInvariants();
        }

        public FiniteStateAutomaton(HashSet<Symbol<TSymbolValue>> inputAlphabet, HashSet<State<TStateValue>> states, State<TStateValue> initialState, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, HashSet<State<TStateValue>> finalStates)
        {
            _inputAlphabet = inputAlphabet;
            _states = states;
            _initialState = initialState;
            _transitionFunction = transitionFunction;
            _finalStates = finalStates;

            _checkInvariants();
        }

        public FiniteStateAutomaton(FiniteStateAutomaton<TSymbolValue, TStateValue> other)
        {
            _inputAlphabet = new(other.InputAlphabet);
            _states = new(other.States);
            _initialState = other.InitialState;
            _transitionFunction = other.TransitionFunction;
            _finalStates = new(other.FinalStates);
        }


        public bool Accepts(IEnumerable<Symbol<TSymbolValue>> symbols)
        {
            // Start at the initial state
            HashSet<State<TStateValue>> reachedStates = new() { _initialState };

            foreach (var symbol in symbols)
            {
                if (!_inputAlphabet.Contains(symbol))
                {
                    throw new FiniteAutomatonException($"Symbol {symbol} in the input sequence {symbols.ToString()} is not included in the input alphabet.");
                }

                HashSet<State<TStateValue>> newReachedStates = _transitionFunction.GetClosure(reachedStates, symbol);


                reachedStates = newReachedStates;
                newReachedStates = new();
            }

            // Check if there is at least one final state in the reached states
            return _finalStates.Intersect(reachedStates).Any();
        }

        public bool Accepts(IEnumerable<TSymbolValue> symbols)
        {
            List<Symbol<TSymbolValue>> list = new();

            foreach (var symbolValue in symbols)
            {
                list.Add(new Symbol<TSymbolValue>(symbolValue));
            }

            return Accepts(list);
        }

        public bool Accepts(TSymbolValue[] symbols)
        {
            return Accepts((IEnumerable<TSymbolValue>)symbols);
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            // Input Alphabet
            sb.Append("Input Alphabet: {");
            sb.Append(string.Join(", ", _inputAlphabet.Select(symbol => symbol.ToString())));
            sb.AppendLine("}");

            // States
            sb.Append("States: {");
            sb.Append(string.Join(", ", _states.Select(state => state.ToString())));
            sb.AppendLine("}");

            // Initial State
            sb.AppendLine($"InitialState: {_initialState}");

            // Transition Function (delta)
            sb.Append($"delta:\n{_transitionFunction}");

            // Final States
            sb.Append("Final States: {");
            sb.Append(string.Join(", ", _finalStates.Select(state => state.ToString())));
            sb.AppendLine("}");

            return sb.ToString();
        }


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
                        throw new FiniteAutomatonException($"State {inputState} in the transition function must be part of the set of states.");
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

        public HashSet<State<TStateValue>> GetReachableStates()
        {
            var reachable = new HashSet<State<TStateValue>>();
            var toProcess = new Queue<State<TStateValue>>();
            toProcess.Enqueue(_initialState);

            while (toProcess.Count > 0)
            {
                var state = toProcess.Dequeue();
                if (!reachable.Contains(state))
                {
                    reachable.Add(state);


                    foreach (var symbol in _inputAlphabet)
                    {
                        var newReachableStates = _transitionFunction.GetClosure(state, symbol);
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

        public void RemoveUnreachableStates()
        {
            //Remove unreachable states
            var reachableStates = GetReachableStates();
            var unreachableStates = _states.Except(reachableStates);
            _states.IntersectWith(reachableStates);
            _finalStates.IntersectWith(reachableStates);

            foreach (var unreachableState in unreachableStates)
            {
                _transitionFunction.RemoveTransition(unreachableState);
            }
        }


    }



    public class FiniteAutomatonException : Exception
    {
        public FiniteAutomatonException(string message) : base(message)
        {

        }
    }

}
