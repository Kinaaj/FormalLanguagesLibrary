using FormalLanguagesLibrary.Grammars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalLanguagesLibrary.Automata
{
    //TODO TSymbolValue -> Symbol<TSymbolValue> and TState -> State<TState>
    public abstract class FiniteStateAutomaton<TSymbolValue, TStateValue> : Automaton
    {

        protected HashSet<Symbol<TSymbolValue>> _inputAlphabet = new();
        protected HashSet<State<TStateValue>> _states = new();
        protected State<TStateValue> _initialState;
        protected TransitionFunction<TStateValue, TSymbolValue> _transitionFunction;
        protected HashSet<State<TStateValue>> _finalStates = new();


        public IReadOnlyCollection<Symbol<TSymbolValue>> InputAlphabet => _inputAlphabet;
        public IReadOnlyCollection<State<TStateValue>> States => _states;
        public State<TStateValue> InitialState => _initialState;
        public TransitionFunction<TStateValue, TSymbolValue> TransitionFunction => _transitionFunction;
        public IReadOnlyCollection<State<TStateValue>> FinalStates => _finalStates;




        public FiniteStateAutomaton(IEnumerable<TSymbolValue> inputAlphabetValues, IEnumerable<TStateValue> statesValues, TStateValue initialStateValue, TransitionFunction<TStateValue, TSymbolValue> transitionFunction, IEnumerable<TStateValue> finalStatesValues)
        {
            foreach(TSymbolValue symbolValue in inputAlphabetValues)
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

            foreach(TStateValue stateValue in finalStatesValues)
            {
                State<TStateValue> state = new(stateValue);
                _finalStates.Add(state);
            }

            _checkInvariants();
        }

        public FiniteStateAutomaton(HashSet<Symbol<TSymbolValue>> inputAlphabet, HashSet<State<TStateValue>> states, State<TStateValue> initialState, TransitionFunction<TStateValue, TSymbolValue> transitionFunction, HashSet<State<TStateValue>> finalStates)
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
                if(!_inputAlphabet.Contains(symbol))
                {
                    throw new FiniteAutomatonException($"Symbol {symbol} in the input sequence {symbols.ToString()} is not included in the input alphabet.");
                }

                HashSet<State<TStateValue>> newReachedStates = new();
                foreach(var state in reachedStates)
                {
                    var closure = _transitionFunction.GetClosure(state, symbol);
                    newReachedStates.UnionWith(closure);
                }
                reachedStates = newReachedStates;
                newReachedStates = new();
            }

            // Check if there is at least one final state in the reached states
            return _finalStates.Intersect(reachedStates).Any();
        }

        public bool Accepts(IEnumerable<TSymbolValue> symbols)
        {
            List<Symbol<TSymbolValue>> list = new();

            foreach(var symbolValue in symbols)
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
                if (!_inputAlphabet.Contains(inputSymbol))
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
                if(!_states.Contains(finalState))
                {
                    throw new FiniteAutomatonException($"Final state {finalState} must be defined in states.");
                }
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
