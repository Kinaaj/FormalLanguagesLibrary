using FormalLanguagesLibrary.Grammars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalLanguagesLibrary.Automata
{
    //TODO TSymbol -> Symbol<TSymbol> and TState -> State<TState>
    internal abstract class FiniteStateAutomaton<TSymbol, TState> : Automaton
    {
        protected HashSet<Symbol<TSymbol>> _inputAlphabet = new();
        protected HashSet<State<TState>> _states = new();
        protected TState _initialState;
        protected TransitionFunction<TState, TSymbol> _transitionFunction;


        public IReadOnlyCollection<TSymbol> InputAlphabet => _inputAlphabet;
        public IReadOnlyCollection<TState> States => _states;
        public TState InitialState => _initialState;
        public TransitionFunction<TState, TSymbol> TransitionFunction => _transitionFunction;


        public FiniteStateAutomaton(HashSet<TSymbol> inputAlphabet, HashSet<TState> states, TState initialState, TransitionFunction<TState, TSymbol> transitionFunction)
        {
            _inputAlphabet = inputAlphabet;
            _states = states;
            _initialState = initialState;
            _transitionFunction = transitionFunction;

            _checkInvariants();
        }

        public FiniteStateAutomaton(FiniteStateAutomaton<TSymbol,TState> other)
        {
            _inputAlphabet = new(other.InputAlphabet);
            _states = new(other.States);
            _initialState = other.InitialState;
            _transitionFunction = other.TransitionFunction;
        }



        protected abstract void _checkInvariants();

    }


    public class FiniteAutomatonException : Exception
    {
        public FiniteAutomatonException(string message) : base(message)
        {

        }
    }

}
