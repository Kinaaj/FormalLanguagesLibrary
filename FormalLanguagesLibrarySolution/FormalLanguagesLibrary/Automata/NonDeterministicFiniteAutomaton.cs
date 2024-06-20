using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalLanguagesLibrary.Automata
{
    internal class NonDeterministicFiniteAutomaton<TSymbol,TState> : FiniteStateAutomaton<TSymbol, TState>
    {
        
        public NonDeterministicFiniteAutomaton(NonDeterministicFiniteAutomaton<TSymbol,TState> other) : base(other) { }

        public NonDeterministicFiniteAutomaton(HashSet<TSymbol> inputAlphabet, HashSet<TState> states, TState initialState, TransitionFunction<TState, TSymbol> transitionFunction) : base(inputAlphabet, states, initialState, transitionFunction) { }


        protected override void _checkInvariants()
        {
            throw new NotImplementedException();
        }
    }
}
