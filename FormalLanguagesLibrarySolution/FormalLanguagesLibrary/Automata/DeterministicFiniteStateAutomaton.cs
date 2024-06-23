

namespace FormalLanguagesLibrary.Automata
{
    public class DeterministicFiniteAutomaton<TSymbolValue, TStateValue> : NonDeterministicFiniteAutomaton<TSymbolValue, TStateValue>
    {

        public DeterministicFiniteAutomaton(DeterministicFiniteAutomaton<TSymbolValue, TStateValue> other) : base(other) { }

        public DeterministicFiniteAutomaton(IEnumerable<TSymbolValue> inputAlphabetValues, IEnumerable<TStateValue> statesValues, TStateValue initialStateValue, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, IEnumerable<TStateValue> finalStatesValues) : base(inputAlphabetValues, statesValues, initialStateValue, transitionFunction, finalStatesValues)
        {

        }

        public DeterministicFiniteAutomaton(HashSet<Symbol<TSymbolValue>> inputAlphabet, HashSet<State<TStateValue>> states, State<TStateValue> initialState, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, HashSet<State<TStateValue>> finalStates) : base(inputAlphabet, states, initialState, transitionFunction, finalStates) { }


        protected override void _checkInvariants()
        {
            base._checkInvariants();

            // Ensure each state-symbol pair maps to exactly one state
            foreach (var transition in _transitionFunction.Transitions)
            {
                var outputStates = transition.Value;

                // For DFA, outputStates should contain exactly one state
                if (outputStates.Count != 1)
                {
                    throw new DeterministicFiniteAutomatonException("In a DFA, each state-symbol pair must map to exactly one state.");
                }
            }


            //There has to be defined transition for each (state x symbol) pair
            foreach (var state in _states)
            {
                foreach (var symbol in _inputAlphabet)
                {
                    Tuple<State<TStateValue>, Symbol<TSymbolValue>> inputPair = new(state, symbol);

                    if (!_transitionFunction.Contains(inputPair))
                    {
                        throw new DeterministicFiniteAutomatonException($"Transition for ({state} x {symbol}) is not defined.");
                    }
                }
            }
        }

        public void Minimize()
        {
            RemoveUnreachableStates();

            var partitions = GetEquivalentPartitions();

            var partitionsMapping = new Dictionary<State<TStateValue>, HashSet<State<TStateValue>>>();


            //Just initializing newInitialState to not throw it an error
            State<TStateValue> newInitialState = _initialState;
            var newFinalStates = new HashSet<State<TStateValue>>();

            foreach (var partition in partitions)
            {
                var symbolRepresentingPartition = partition.First();
                partitionsMapping[symbolRepresentingPartition] = partition;
                if (partition.Contains(_initialState))
                {
                    newInitialState = symbolRepresentingPartition;
                }
                if (partition.Intersect(_finalStates).Any())
                {
                    newFinalStates.Add(symbolRepresentingPartition);
                }
            }

            var newStates = new HashSet<State<TStateValue>>(partitionsMapping.Keys);


            var newTransitionFunction = new TransitionFunction<TSymbolValue, TStateValue>();


            //Create mapping for new states
            foreach (var newState in newStates)
            {
                foreach (var symbol in _inputAlphabet)
                {
                    var outputState = _transitionFunction[newState, symbol].First();
                    foreach (var (symbolRepresentingPartition, partition) in partitionsMapping)
                    {
                        if (partition.Contains(outputState))
                        {
                            newTransitionFunction.AddTransition(newState, symbol, symbolRepresentingPartition);
                        }
                    }
                }
            }

            _states = newStates;
            _finalStates = newStates;
            _initialState = newInitialState;
            _transitionFunction = newTransitionFunction;
            _checkInvariants();
        }

        private List<HashSet<State<TStateValue>>> GetEquivalentPartitions()
        {
            // Create one initial partition of final and non-final states
            var partitions = new List<HashSet<State<TStateValue>>>
            {
                new HashSet<State<TStateValue>>(_finalStates),
                new HashSet<State<TStateValue>>(_states.Except(_finalStates))
            };

            bool partitionChanged;

            do
            {
                partitionChanged = false;
                var newPartitions = new List<HashSet<State<TStateValue>>>();

                foreach (var partition in partitions)
                {
                    var recalculatedPartitionList = RecalculatePartition(partition, partitions);
                    if (recalculatedPartitionList.Count != 1)
                    {
                        partitionChanged = true;
                    }
                    foreach (var newPartition in recalculatedPartitionList)
                    {
                        newPartitions.Add(newPartition);
                    }
                }
                partitions = newPartitions;
            }
            while (partitionChanged == true);


            return partitions;
        }

        private bool AreStatesEquivalent(State<TStateValue> state1, State<TStateValue> state2, List<HashSet<State<TStateValue>>> partitionsOfEquivalence)
        {
            foreach (var symbol in _inputAlphabet)
            {
                var inputPair1 = new Tuple<State<TStateValue>, Symbol<TSymbolValue>>(state1, symbol);
                var inputPair2 = new Tuple<State<TStateValue>, Symbol<TSymbolValue>>(state2, symbol);

                var targetState1 = _transitionFunction.Transitions[inputPair1].First();
                var targetState2 = _transitionFunction.Transitions[inputPair2].First();

                var targetPartition1 = partitionsOfEquivalence.First(p => p.Contains(targetState1));
                var targetPartition2 = partitionsOfEquivalence.First(p => p.Contains(targetState2));

                if (!targetPartition1.SetEquals(targetPartition2))
                {
                    return false;
                }
            }
            return true;
        }

        private List<HashSet<State<TStateValue>>> RecalculatePartition(HashSet<State<TStateValue>> partition, List<HashSet<State<TStateValue>>> partitionsOfEquivalence)
        {

            var newPartitions = new List<HashSet<State<TStateValue>>>();

            foreach (var state in partition)
            {
                if (newPartitions.Count == 0)
                {
                    newPartitions.Add(new HashSet<State<TStateValue>> { state });
                    continue;
                }

                else
                {
                    foreach (var newPartition in newPartitions)
                    {
                        if (AreStatesEquivalent(state, newPartition.First(), partitionsOfEquivalence))
                        {
                            newPartition.Add(state);
                            continue;
                        }
                    }
                    //Else create a new partition with the state
                    newPartitions.Add(new HashSet<State<TStateValue>> { state });
                }
            }
            return newPartitions;
        }

    }
    public class DeterministicFiniteAutomatonException : FiniteAutomatonException
    {
        public DeterministicFiniteAutomatonException(string message) : base(message)
        {

        }
    }
}
