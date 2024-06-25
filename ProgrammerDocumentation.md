# Programmer Documentation:

## FormalLanguagesLibrary.Automata

### `FiniteStateAutomaton<TSymbolValue, TStateValue>`

Abstract base class representing a finite state automaton (NFA or DFA).

#### Overview

The `FiniteStateAutomaton` class forms the foundation for implementing both deterministic and non-deterministic finite state automata within the Formal Languages Library. It encapsulates the structure and behavior common to these automata types, including states, transitions, and acceptance criteria.

#### Properties

- `InputAlphabet`: Represents the set of symbols that the automaton can read.
- `States`: Represents the set of states that the automaton can be in.
- `InitialState`: Specifies the initial state from which the automaton begins its computation.
- `TransitionFunction`: Defines the rules governing state transitions based on input symbols.
- `FinalStates`: Identifies the states that, if reached after processing input, signify the automaton accepts the input.

#### Constructors

1. `FiniteStateAutomaton(IEnumerable<TSymbolValue> inputAlphabetValues, IEnumerable<TStateValue> statesValues,
   TStateValue initialStateValue, TransitionFunction<TSymbolValue, TStateValue> transitionFunction,
   IEnumerable<TStateValue> finalStatesValues)`

   Initializes a new finite state automaton with specified input alphabet, states, initial state, transition function, and final states.

2. `FiniteStateAutomaton(HashSet<Symbol<TSymbolValue>> inputAlphabet, HashSet<State<TStateValue>> states,
   State<TStateValue> initialState, TransitionFunction<TSymbolValue, TStateValue> transitionFunction,
   HashSet<State<TStateValue>> finalStates)`

   Initializes a new finite state automaton with existing sets of input alphabet, states, initial state, transition function, and final states.

3. `FiniteStateAutomaton(FiniteStateAutomaton<TSymbolValue, TStateValue> other)`

   Creates a deep copy of another finite state automaton instance.

#### Methods

- `Accepts(IEnumerable<Symbol<TSymbolValue>> symbols): bool`

  Determines if the automaton accepts a sequence of symbols by simulating its computation.

- `Accepts(IEnumerable<TSymbolValue> symbols): bool`

  Overloaded method that accepts an IEnumerable of symbol values and checks if the automaton accepts the corresponding sequence of symbols.

- `Accepts(TSymbolValue[] symbols): bool`

  Overloaded method that accepts an array of symbol values and checks if the automaton accepts the corresponding sequence of symbols.

- `ToDFA(): DeterministicFiniteAutomaton<TSymbolValue, TStateValue>`

  Abstract method to convert the non-deterministic automaton to a deterministic finite automaton (DFA).

- `GetReachableStates(): HashSet<State<TStateValue>>`

  Retrieves all states reachable from the initial state using epsilon transitions.

- `RemoveUnreachableStates(): void`

  Removes states that are not reachable from the initial state, optimizing the automaton.

- `ToString(): string`

  Provides a string representation of the automaton, detailing its input alphabet, states, initial state, transition function, and final states.

#### Exceptions

- `FiniteAutomatonException`

  Custom exception class used to handle errors specific to finite state automata, such as invalid states or symbols in transition functions.

---

### `NonDeterministicEpsilonFiniteAutomaton<TSymbolValue, TStateValue>`

Represents a non-deterministic epsilon finite automaton (NFA-epsilon), which extends the `FiniteStateAutomaton` class.

#### Overview

The `NonDeterministicEpsilonFiniteAutomaton` class extends the capabilities of `FiniteStateAutomaton` to handle non-deterministic automata with epsilon transitions. It allows for the representation and manipulation of automata that can transition on input symbols or epsilon transitions.

#### Constructors

1. `NonDeterministicEpsilonFiniteAutomaton(NonDeterministicFiniteAutomaton<TSymbolValue, TStateValue> other)`

   Initializes a new NFA-epsilon instance by copying another non-deterministic finite automaton.

2. `NonDeterministicEpsilonFiniteAutomaton(HashSet<Symbol<TSymbolValue>> inputAlphabet, HashSet<State<TStateValue>> states, State<TStateValue> initialState, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, HashSet<State<TStateValue>> finalStates)`

   Initializes a new NFA-epsilon instance with specified input alphabet, states, initial state, transition function, and final states.

3. `NonDeterministicEpsilonFiniteAutomaton(IEnumerable<TSymbolValue> inputAlphabetValues, IEnumerable<TStateValue> statesValues, TStateValue initialStateValue, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, IEnumerable<TStateValue> finalStatesValues)`

   Initializes a new NFA-epsilon instance with enumerable values for input alphabet, states, initial state, transition function, and final states.

#### Methods

- `ToNFA(): NonDeterministicFiniteAutomaton<TSymbolValue, TStateValue>`

  Converts the NFA-epsilon to a non-deterministic finite automaton (NFA) by resolving epsilon closures and transitions.

- `ToDFA(): DeterministicFiniteAutomaton<TSymbolValue, TStateValue>`

  Overrides the base class method to convert the NFA-epsilon to a deterministic finite automaton (DFA) by first converting it to an NFA.

#### Protected Methods

- `_checkInvariants()`

  Overrides the base class method to verify invariants specific to NFA-epsilon, although no additional invariants are currently checked.
    inputAlphabet, states, initialState, transitionFunction, finalStates);

---

### `NonDeterministicFiniteAutomaton<TSymbolValue, TStateValue>`

Represents a non-deterministic finite automaton (NFA), extending the `NonDeterministicEpsilonFiniteAutomaton` class.

#### Overview

The `NonDeterministicFiniteAutomaton` class extends the `NonDeterministicEpsilonFiniteAutomaton` to handle non-deterministic automata without epsilon transitions. This class ensures that transitions occur strictly on input symbols, providing methods for validation and conversion.

#### Constructors

1. `NonDeterministicFiniteAutomaton(NonDeterministicFiniteAutomaton<TSymbolValue, TStateValue> other)`

   Initializes a new NFA instance by copying another non-deterministic finite automaton.

2. `NonDeterministicFiniteAutomaton(HashSet<Symbol<TSymbolValue>> inputAlphabet, HashSet<State<TStateValue>> states, State<TStateValue> initialState, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, HashSet<State<TStateValue>> finalStates)`

   Initializes a new NFA instance with specified input alphabet, states, initial state, transition function, and final states.

3. `NonDeterministicFiniteAutomaton(IEnumerable<TSymbolValue> inputAlphabetValues, IEnumerable<TStateValue> statesValues, TStateValue initialStateValue, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, IEnumerable<TStateValue> finalStatesValues)`

   Initializes a new NFA instance with enumerable values for input alphabet, states, initial state, transition function, and final states.

#### Methods

- `ToDFA(): DeterministicFiniteAutomaton<TSymbolValue, TStateValue>`

  Overrides the base class method to convert the NFA to a deterministic finite automaton (DFA). This method is not yet implemented.

#### Protected Methods

- `_checkInvariants()`

  Overrides the base class method to verify invariants specific to NFA. It ensures there are no epsilon transitions in the automaton. If epsilon transitions are found, a `NonDeterministicFiniteAutomatonException` is thrown.

#### Exceptions

- `NonDeterministicFiniteAutomatonException`

  Custom exception class for handling errors specific to non-deterministic finite automata, such as the presence of epsilon transitions.
  
---

### `DeterministicFiniteAutomaton<TSymbolValue, TStateValue>`

Represents a deterministic finite automaton (DFA), extending the `NonDeterministicFiniteAutomaton` class.

#### Overview

The `DeterministicFiniteAutomaton` class extends the `NonDeterministicFiniteAutomaton` to handle deterministic automata. It ensures that each state-symbol pair maps to exactly one state and provides methods for DFA minimization.

#### Constructors

1. `DeterministicFiniteAutomaton(DeterministicFiniteAutomaton<TSymbolValue, TStateValue> other)`

   Initializes a new DFA instance by copying another deterministic finite automaton.

2. `DeterministicFiniteAutomaton(IEnumerable<TSymbolValue> inputAlphabetValues, IEnumerable<TStateValue> statesValues, TStateValue initialStateValue, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, IEnumerable<TStateValue> finalStatesValues)`

   Initializes a new DFA instance with enumerable values for input alphabet, states, initial state, transition function, and final states.

3. `DeterministicFiniteAutomaton(HashSet<Symbol<TSymbolValue>> inputAlphabet, HashSet<State<TStateValue>> states, State<TStateValue> initialState, TransitionFunction<TSymbolValue, TStateValue> transitionFunction, HashSet<State<TStateValue>> finalStates)`

   Initializes a new DFA instance with specified input alphabet, states, initial state, transition function, and final states.

#### Methods

- `Minimize()`

  Minimizes the DFA by removing unreachable states and merging equivalent states.

- `GetEquivalentPartitions(): List<HashSet<State<TStateValue>>>`

  Returns the equivalent partitions of states used in the minimization process.

- `AreStatesEquivalent(State<TStateValue> state1, State<TStateValue> state2, List<HashSet<State<TStateValue>>> partitionsOfEquivalence): bool`

  Checks if two states are equivalent in the context of DFA minimization.

- `RecalculatePartition(HashSet<State<TStateValue>> partition, List<HashSet<State<TStateValue>>> partitionsOfEquivalence): List<HashSet<State<TStateValue>>>`

  Refines a partition of states based on their equivalence.

#### Protected Methods

- `_checkInvariants()`

  Overrides the base class method to verify invariants specific to DFA. It ensures that each state-symbol pair maps to exactly one state and that there is a defined transition for each state-symbol pair. Throws `DeterministicFiniteAutomatonException` if invariants are violated.

#### Exceptions

- `DeterministicFiniteAutomatonException`

  Custom exception class for handling errors specific to deterministic finite automata, such as invalid transitions or violations of DFA invariants.

---

### `State<T>`

Represents a state in an automaton, parameterized by a type `T` for the state's value.

#### Overview

The `State<T>` struct is a fundamental component in defining states within various types of automata (e.g., DFA, NFA, NFA-epsilon). It provides methods for equality comparison, hashing, and string representation.

#### Fields

- `Value`

  - Type: `T`
  - Description: The value associated with the state. This value is used to identify and compare states within the automaton.

#### Constructors

- `State<T>(T value)`

  Initializes a new state with the specified value.

Methods

- `Equals(State<T> other): bool`

    Determines whether the specified State<T> is equal to the current state by comparing their values using the default equality comparer for type T.


- `GetHashCode(): int`

    Returns a hash code for the state, based on its value. If the value is null, it returns 0.


- `ToString(): string`

    Returns a string that represents the state. If the value is null, it returns an empty string.
