using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormalLanguagesLibrary.Automata
{
    internal class DeterministicFiniteAutomaton<TSymbol, TState> : FiniteStateAutomaton<TSymbol,TState>
    {


        public DeterministicFiniteAutomaton(HashSet<TSymbol> inputAlphabet, HashSet<TState> states, TState initialState, TransitionFunction<TState, TSymbol> transitionFunction) : base(inputAlphabet, states, initialState, transitionFunction) { }

        public DeterministicFiniteAutomaton(DeterministicFiniteAutomaton<TSymbol, TState> other) : base(other) { }


        protected override void _checkInvariants()
        {

        }

    }
}
