using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalLanguagesLibrary.Automata
{
    internal class TransitionFunction<TState, TSymbol>
    {
        private Dictionary<Tuple<TState, TSymbol>, HashSet<TState>> _transitions = new();

        public IReadOnlyDictionary<Tuple<TState, TSymbol>, HashSet<TState>> Transitions => _transitions;

        public TransitionFunction()
        {

        }

        public TransitionFunction(Dictionary<Tuple<TState, TSymbol>, HashSet<TState>> transitions)
        {
            _transitions = transitions;
        }

        public TransitionFunction(TransitionFunction<TState, TSymbol> transitionFunction)
        {
            _transitions = new Dictionary<Tuple<TState, TSymbol>, HashSet<TState>>(transitionFunction.Transitions);
        }

        public bool AddTransitions(Tuple<TState, TSymbol> input, HashSet<TState> states)
        {

            if (!_transitions.ContainsKey(input))
            {
                _transitions[input] = new HashSet<TState>();
            }


            bool isAddedSuccessfuly = true;

            foreach (TState state in states)
            {
                isAddedSuccessfuly &= _transitions[input].Add(state);
            }
            return isAddedSuccessfuly;
        }

        public bool AddTransition(Tuple<TState, TSymbol> input, TState state)
        {
            if (!_transitions.ContainsKey(input))
            {
                _transitions[input] = new HashSet<TState>();
            }

            return _transitions[input].Add(state);
        }

        public bool RemoveTransition(Tuple<TState, TSymbol> input)
        {
            return _transitions.Remove(input);
        }
    }
}
