# Programmer Documentation:

## FormalLanguagesLibrary.Automata

### `Automaton`

Abstract base class representing an automaton.

### `FiniteStateAutomaton<TSymbolValue, TStateValue>`

Abstract base class representing a finite state automaton (NFA or DFA). Inherits from an abstract Automaton class.

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

---

### `Symbol<T>`

Represents a symbol in an automaton, parameterized by a type `T` for the symbol's value.

#### Overview

The `Symbol<T>` struct is used to define symbols within automata, providing methods for equality comparison, hashing, and string representation. It supports both regular and epsilon symbols.

#### Fields

- `Value`

  - Type: `T?`
  - Description: The value associated with the symbol. This value is used to identify and compare symbols within the automaton.

- `Type`

  - Type: `SymbolType`
  - Description: Indicates whether the symbol is a regular symbol or an epsilon symbol.

- `Epsilon`

  - Type: `Symbol<T>`
  - Description: A constant representing an epsilon symbol.

#### Constructors

- `Symbol<T>(T? value, SymbolType type = SymbolType.NonEpsilon)`

  Initializes a new symbol with the specified value and type. By default, the type is `NonEpsilon`.

#### Methods

- `Equals(object? obj): bool`

  Determines whether the specified object is equal to the current symbol by comparing their values and types.

- `Equals(Symbol<T> other): bool`

  Determines whether the specified `Symbol<T>` is equal to the current symbol. For epsilon symbols, equality is based on the type only.

- `Equals(T other): bool`

  Determines whether the specified value is equal to the value of the current symbol using the default equality comparer for type `T`.

- `GetHashCode(): int`

  Returns a hash code for the symbol. For epsilon symbols, it returns the hash code of the `SymbolType.Epsilon`.

- `ToString(): string`

  Returns a string that represents the symbol. For epsilon symbols, it returns `"$"`.

#### Operators

- `operator ==(Symbol<T> symbol1, Symbol<T> symbol2): bool`

  Determines whether two symbols are equal.

- `operator !=(Symbol<T> symbol1, Symbol<T> symbol2): bool`

  Determines whether two symbols are not equal.

#### Enums

- `SymbolType`

  Enum representing the type of a symbol.

  - `NonEpsilon`
    - Represents a regular symbol.

  - `Epsilon`
    - Represents an epsilon symbol.

---

### `TransitionFunction<TSymbolValue, TStateValue>`

Represents a transition function in an automaton, parameterized by types `TSymbolValue` for the symbol's value and `TStateValue` for the state's value.

#### Overview

The `TransitionFunction<TSymbolValue, TStateValue>` class is used to define the transitions within automata. It maintains a dictionary to store transitions and provides methods to add, remove, and query transitions. It also supports the computation of closures, including epsilon closures.

#### Fields

- `_transitions`

  - Type: `Dictionary<Tuple<State<TStateValue>, Symbol<TSymbolValue>>, HashSet<State<TStateValue>>>`
  - Description: Stores transitions where the key is a tuple of input state and input symbol, and the value is a set of output states.

#### Properties

- `Transitions`

  - Type: `IReadOnlyDictionary<Tuple<State<TStateValue>, Symbol<TSymbolValue>>, HashSet<State<TStateValue>>>`
  - Description: Exposes the transitions dictionary as a read-only dictionary.

#### Constructors

- `TransitionFunction()`

  Initializes a new instance of the `TransitionFunction` class.

- `TransitionFunction(Dictionary<Tuple<State<TStateValue>, Symbol<TSymbolValue>>, HashSet<State<TStateValue>>> transitions)`

  Initializes a new instance of the `TransitionFunction` class with the specified transitions.

- `TransitionFunction(TransitionFunction<TSymbolValue, TStateValue> transitionFunction)`

  Initializes a new instance of the `TransitionFunction` class by copying the specified transition function.

- `TransitionFunction(Dictionary<Tuple<TStateValue, TSymbolValue>, IEnumerable<TStateValue>> transitions)`

  Initializes a new instance of the `TransitionFunction` class from a dictionary with simpler types.

#### Methods

- `bool AddTransition(TStateValue inputStateValue, TSymbolValue inputSymbolValue, TStateValue outputStateValue)`

  Adds a transition to the function using state and symbol values.

- `bool AddTransition(State<TStateValue> inputState, Symbol<TSymbolValue> inputSymbol, State<TStateValue> outputState)`

  Adds a transition to the function using state and symbol objects.

- `bool AddTransition(State<TStateValue> inputState, Symbol<TSymbolValue> inputSymbol, HashSet<State<TStateValue>> outputStates)`

  Adds a transition to the function with multiple output states.

- `bool AddTransitions(Tuple<State<TStateValue>, Symbol<TSymbolValue>> input, HashSet<State<TStateValue>> states)`

  Adds multiple transitions to the function.

- `bool AddTransition(Tuple<State<TStateValue>, Symbol<TSymbolValue>> input, State<TStateValue> state)`

  Adds a transition to the function.

- `bool AddTransition(Tuple<TStateValue, TSymbolValue> input, TStateValue stateValue)`

  Adds a transition to the function using a tuple of state and symbol values.

- `bool RemoveTransition(Tuple<State<TStateValue>, Symbol<TSymbolValue>> input)`

  Removes a transition from the function.

- `bool RemoveTransition(Tuple<TStateValue, TSymbolValue> input)`

  Removes a transition from the function using a tuple of state and symbol values.

- `bool RemoveTransition(State<TStateValue> state)`

  Removes transitions associated with the specified state.

- `bool RemoveTransition(State<TStateValue> state, Symbol<TSymbolValue> symbol)`

  Removes transitions associated with the specified state and symbol.

- `bool RemoveTransition(State<TStateValue> state, Symbol<TSymbolValue> symbol, State<TStateValue> outputState)`

  Removes a specific transition.

- `bool RemoveTransition(TStateValue stateValue, TSymbolValue symbolValue, TStateValue outputStateValue)`

  Removes a specific transition using state and symbol values.

- `bool HasEpsilonTransitions()`

  Checks if there are any epsilon transitions in the function.

- `bool Contains(State<TStateValue> inputState, Symbol<TSymbolValue> inputSymbol)`

  Checks if a specific input state and symbol combination exists in the function.

- `bool Contains(Tuple<State<TStateValue>, Symbol<TSymbolValue>> inputPair)`

  Checks if a specific input pair exists in the function.

- `string ToString()`

  Converts the transition function to a string representation.

- `HashSet<State<TStateValue>> this[State<TStateValue> inputState, Symbol<TSymbolValue> inputSymbol]`

  Indexer to get output states for a specific input state and symbol.

- `HashSet<State<TStateValue>> this[TStateValue inputState, TSymbolValue inputSymbol]`

  Indexer to get output states for a specific input state and symbol values.

- `HashSet<State<TStateValue>> GetOutputStates(State<TStateValue> inputState, Symbol<TSymbolValue> inputSymbol)`

  Gets output states for a specific input state and symbol.

- `HashSet<State<TStateValue>> GetClosure(State<TStateValue> currentState, Symbol<TSymbolValue> transitionSymbol)`

  Gets closure states for a specific input state and symbol.

- `HashSet<State<TStateValue>> GetClosure(HashSet<State<TStateValue>> currentStates, Symbol<TSymbolValue> transitionSymbol)`

  Gets closure states for a set of input states and a specific symbol.

- `HashSet<State<TStateValue>> GetEpsilonClosure(State<TStateValue> currentState)`

  Gets epsilon closure states for a specific input state.

- `HashSet<State<TStateValue>> GetEpsilonClosure(HashSet<State<TStateValue>> currentStates)`

  Gets epsilon closure states for a set of input states.

- `bool Equals(TransitionFunction<TSymbolValue, TStateValue>? other)`

  Checks equality of this transition function with another transition function.

---

## FormalLanguagesLibrary.Grammars

### `Grammar<T>`

Represents an abstract base class for formal grammars, parameterized by type `T` that must implement `IComparable<T>` and `IIncrementOperators<T>`.

#### Overview

The `Grammar<T>` class defines the structure of a formal grammar, including sets of terminal and non-terminal symbols, the start symbol, and production rules. It provides methods to check invariants, add production rules, and ensure the validity of grammar components.

#### Fields

- `_terminals`

  - Type: `HashSet<Symbol<T>>`
  - Description: Set of terminal symbols in the grammar.

- `_nonTerminals`

  - Type: `HashSet<Symbol<T>>`
  - Description: Set of non-terminal symbols in the grammar.

- `_startSymbol`

  - Type: `Symbol<T>?`
  - Description: Start symbol of the grammar.

- `_productionRules`

  - Type: `HashSet<ProductionRule<T>>`
  - Description: Set of production rules defining the grammar.

#### Properties

- `Terminals`

  - Type: `IReadOnlyCollection<Symbol<T>>`
  - Description: Read-only collection of terminal symbols.

- `NonTerminals`

  - Type: `IReadOnlyCollection<Symbol<T>>`
  - Description: Read-only collection of non-terminal symbols.

- `StartSymbol`

  - Type: `Symbol<T>?`
  - Description: Start symbol of the grammar.

- `ProductionRules`

  - Type: `IReadOnlyCollection<ProductionRule<T>>`
  - Description: Read-only collection of production rules.

#### Constructors

- `Grammar()`

  - Description: Initializes a new instance of the `Grammar<T>` class.

- `Grammar(Grammar<T> grammar)`

  - Parameters:
    - `grammar`: Another instance of `Grammar<T>` to copy.

  - Description: Initializes a new instance of the `Grammar<T>` class by copying another grammar instance.

- `Grammar(HashSet<Symbol<T>> nonTerminals, HashSet<Symbol<T>> terminals, Symbol<T>? startSymbol, HashSet<ProductionRule<T>> productionRules)`

  - Parameters:
    - `nonTerminals`: Set of non-terminal symbols.
    - `terminals`: Set of terminal symbols.
    - `startSymbol`: Start symbol of the grammar.
    - `productionRules`: Set of production rules.

  - Description: Initializes a new instance of the `Grammar<T>` class with specified sets of terminals, non-terminals, start symbol, and production rules.

- `Grammar(IEnumerable<T> nonTerminals, IEnumerable<T> terminals, T? startSymbol, IEnumerable<Tuple<IEnumerable<T>, IEnumerable<T>>> productionRules)`

  - Parameters:
    - `nonTerminals`: Enumerable collection of non-terminal symbols.
    - `terminals`: Enumerable collection of terminal symbols.
    - `startSymbol`: Start symbol of the grammar.
    - `productionRules`: Enumerable collection of production rules.

  - Description: Initializes a new instance of the `Grammar<T>` class with enumerable sets of terminals, non-terminals, start symbol, and production rules.

#### Methods

- `bool TryAddRule(ProductionRule<T> rule)`

  - Parameters:
    - `rule`: The production rule to attempt adding.

  - Returns: `true` if the rule was successfully added, otherwise `false`.

  - Description: Attempts to add a production rule to the grammar, performing necessary checks.

- `void ToString()`

  - Returns: A string representation of the grammar.

  - Description: Returns a string containing details about the non-terminals, terminals, start symbol, and production rules of the grammar.

#### Exceptions

- `GrammarException`

  - Description: Exception class specific to errors related to grammars.


---

### `RecursivelyEnumerableGrammar<T>`

Represents a class for recursively enumerable grammars, inheriting from `Grammar<T>`.

#### Overview

The `RecursivelyEnumerableGrammar<T>` class extends the functionality of `Grammar<T>` to handle recursively enumerable grammars. It provides constructors for initialization and overrides the production rule format check to accommodate the nature of recursively enumerable grammars.

#### Constructors

- `RecursivelyEnumerableGrammar()`

  - Description: Initializes a new instance of the `RecursivelyEnumerableGrammar<T>` class.

- `RecursivelyEnumerableGrammar(RecursivelyEnumerableGrammar<T> grammar)`

  - Parameters:
    - `grammar`: Another instance of `RecursivelyEnumerableGrammar<T>` to copy.

  - Description: Initializes a new instance of the `RecursivelyEnumerableGrammar<T>` class by copying another grammar instance.

- `RecursivelyEnumerableGrammar(HashSet<Symbol<T>> nonTerminals, HashSet<Symbol<T>> terminals, Symbol<T>? startSymbol, HashSet<ProductionRule<T>> productionRules)`

  - Parameters:
    - `nonTerminals`: Set of non-terminal symbols.
    - `terminals`: Set of terminal symbols.
    - `startSymbol`: Start symbol of the grammar.
    - `productionRules`: Set of production rules.

  - Description: Initializes a new instance of the `RecursivelyEnumerableGrammar<T>` class with specified sets of terminals, non-terminals, start symbol, and production rules.

- `RecursivelyEnumerableGrammar(IEnumerable<T> nonTerminals, IEnumerable<T> terminals, T? startSymbol, IEnumerable<Tuple<IEnumerable<T>, IEnumerable<T>>> productionRules)`

  - Parameters:
    - `nonTerminals`: Enumerable collection of non-terminal symbols.
    - `terminals`: Enumerable collection of terminal symbols.
    - `startSymbol`: Start symbol of the grammar.
    - `productionRules`: Enumerable collection of production rules.

  - Description: Initializes a new instance of the `RecursivelyEnumerableGrammar<T>` class with enumerable sets of terminals, non-terminals, start symbol, and production rules.

#### Methods

- `protected override void _checkFormatOfProductionRule(ProductionRule<T> rule)`

  - Parameters:
    - `rule`: The production rule to check.

  - Description: Overrides the method to check the format of production rules for recursively enumerable grammars. For these grammars, no specific format constraints are enforced.

#### Exceptions

- `RecursivelyEnumerableGrammarException`

  - Description: Exception class specific to errors related to recursively enumerable grammars.

---

### `ContextSensitiveGrammar<T>`

Represents a class for context-sensitive grammars, inheriting from `RecursivelyEnumerableGrammar<T>`.

#### Overview

The `ContextSensitiveGrammar<T>` class extends the capabilities of recursively enumerable grammars (`RecursivelyEnumerableGrammar<T>`) to handle context-sensitive grammars. It introduces additional checks specific to context-sensitive grammar rules and manages flags to track properties like whether the start symbol appears on the right-hand side or generates epsilon.

#### Fields

- `private bool _isStarTSymbolValueOnRightSide = false`
  - Description: Indicates whether the start symbol (`_startSymbol`) appears on the right-hand side of any production rule.

- `private bool _isStartingSymbolGeneratingEpsilon = false`
  - Description: Indicates whether the start symbol (`_startSymbol`) generates epsilon (`ε`), i.e., an empty string.

#### Constructors

- `ContextSensitiveGrammar()`

  - Description: Initializes a new instance of the `ContextSensitiveGrammar<T>` class.

- `ContextSensitiveGrammar(ContextSensitiveGrammar<T> grammar)`

  - Parameters:
    - `grammar`: Another instance of `ContextSensitiveGrammar<T>` to copy.

  - Description: Initializes a new instance of the `ContextSensitiveGrammar<T>` class by copying another grammar instance.

- `ContextSensitiveGrammar(HashSet<Symbol<T>> nonTerminals, HashSet<Symbol<T>> terminals, Symbol<T>? startSymbol, HashSet<ProductionRule<T>> productionRules)`

  - Parameters:
    - `nonTerminals`: Set of non-terminal symbols.
    - `terminals`: Set of terminal symbols.
    - `startSymbol`: Start symbol of the grammar.
    - `productionRules`: Set of production rules.

  - Description: Initializes a new instance of the `ContextSensitiveGrammar<T>` class with specified sets of terminals, non-terminals, start symbol, and production rules.

- `ContextSensitiveGrammar(IEnumerable<T> nonTerminals, IEnumerable<T> terminals, T? startSymbol, IEnumerable<Tuple<IEnumerable<T>, IEnumerable<T>>> productionRules)`

  - Parameters:
    - `nonTerminals`: Enumerable collection of non-terminal symbols.
    - `terminals`: Enumerable collection of terminal symbols.
    - `startSymbol`: Start symbol of the grammar.
    - `productionRules`: Enumerable collection of production rules.

  - Description: Initializes a new instance of the `ContextSensitiveGrammar<T>` class with enumerable sets of terminals, non-terminals, start symbol, and production rules.

#### Methods

- `protected override void _checkFormatOfProductionRule(ProductionRule<T> rule)`

  - Parameters:
    - `rule`: The production rule to check.

  - Description: Overrides the method from `RecursivelyEnumerableGrammar<T>` to check the format of production rules specific to context-sensitive grammars. Checks include ensuring the left-hand side is at least as long as the right-hand side, verifying if the start symbol appears on the right-hand side, validating that the start symbol does not generate epsilon if it appears on the right-hand side, and enforcing the format `aXb -> ayb` for rules with multiple symbols on the right-hand side.

- `private void _checkAlphaAndBetaContext(ProductionRule<T> rule)`

  - Parameters:
    - `rule`: The production rule to check.

  - Description: Checks that rules adhere to the format `aXb -> ayb`, where X is a non-terminal symbol and a, b are any non-empty strings (alpha and beta segments). Raises a `ContextSensitiveGrammarException` if the rule does not match the expected format.

#### Exceptions

- `ContextSensitiveGrammarException`

  - Description: Exception class specific to errors related to context-sensitive grammars, inheriting from `RecursivelyEnumerableGrammarException`.

---

### `ContextFreeGrammar<T>`

Represents a class for context-free grammars, inheriting from `ContextSensitiveGrammar<T>`.

#### Overview

The `ContextFreeGrammar<T>` class extends the functionality of context-sensitive grammars (`ContextSensitiveGrammar<T>`) to handle context-free grammars. It introduces methods to remove epsilon rules and to retrieve non-terminals that generate epsilon.

#### Fields

None additional beyond inherited fields.

#### Constructors

- `ContextFreeGrammar()`

  - Description: Initializes a new instance of the `ContextFreeGrammar<T>` class.

- `ContextFreeGrammar(ContextFreeGrammar<T> grammar)`

  - Parameters:
    - `grammar`: Another instance of `ContextFreeGrammar<T>` to copy.

  - Description: Initializes a new instance of the `ContextFreeGrammar<T>` class by copying another grammar instance.

- `ContextFreeGrammar(HashSet<Symbol<T>> nonTerminals, HashSet<Symbol<T>> terminals, Symbol<T>? startTSymbolValue, HashSet<ProductionRule<T>> productionRules)`

  - Parameters:
    - `nonTerminals`: Set of non-terminal symbols.
    - `terminals`: Set of terminal symbols.
    - `startTSymbolValue`: Start symbol of the grammar.
    - `productionRules`: Set of production rules.

  - Description: Initializes a new instance of the `ContextFreeGrammar<T>` class with specified sets of terminals, non-terminals, start symbol, and production rules.

- `ContextFreeGrammar(IEnumerable<T> nonTerminals, IEnumerable<T> terminals, T? startTSymbolValue, IEnumerable<Tuple<IEnumerable<T>, IEnumerable<T>>> productionRules)`

  - Parameters:
    - `nonTerminals`: Enumerable collection of non-terminal symbols.
    - `terminals`: Enumerable collection of terminal symbols.
    - `startTSymbolValue`: Start symbol of the grammar.
    - `productionRules`: Enumerable collection of production rules.

  - Description: Initializes a new instance of the `ContextFreeGrammar<T>` class with enumerable sets of terminals, non-terminals, start symbol, and production rules.

#### Methods

- `protected override void _checkFormatOfProductionRule(ProductionRule<T> rule)`

  - Parameters:
    - `rule`: The production rule to check.

  - Description: Overrides the method from `ContextSensitiveGrammar<T>` to check the format of production rules specific to context-free grammars. Ensures that the left-hand side of each production rule has exactly one symbol.

- `public void RemoveEpsilonRules()`

  - Description: Removes epsilon (`ε`) rules from the grammar. This method modifies the set of production rules by creating new rules that do not contain nullable non-terminals (`nullable`).

- `public HashSet<Symbol<T>> GetNonTerminalsGeneratingEpsilon()`

  - Returns: A set of non-terminals that generate epsilon (`ε`).

  - Description: Retrieves a set of non-terminals that generate epsilon (`ε`) based on the current set of production rules.

#### Exceptions

- `ContextFreeGrammarException`

  - Description: Exception class specific to errors related to context-free grammars, inheriting from `ContextSensitiveGrammarException`.

---

### `RegularGrammar<T>`

Represents a class for regular grammars, which are a special type of context-free grammars (`ContextFreeGrammar<T>`).

#### Overview

The `RegularGrammar<T>` class extends the functionality of context-free grammars to enforce rules specific to regular grammars. Regular grammars are characterized by production rules where the right-hand side is either one terminal symbol or a non-terminal followed by a terminal symbol (`A -> a` or `A -> aB` for right-regular and `A -> Ba` for left-regular).

#### Fields

- `public bool isRightRegular { get; private set; }`

  - Description: Indicates if the grammar is right-regular (`true`) or left-regular (`false`).

- `private bool _isSetRegularity`

  - Description: Private field to track whether regularity has been set or validated.

#### Constructors

- `RegularGrammar()`

  - Description: Initializes a new instance of the `RegularGrammar<T>` class.

- `RegularGrammar(ContextFreeGrammar<T> grammar)`

  - Parameters:
    - `grammar`: Another instance of `ContextFreeGrammar<T>` to copy.

  - Description: Initializes a new instance of the `RegularGrammar<T>` class by copying another context-free grammar instance.

- `RegularGrammar(HashSet<Symbol<T>> nonTerminals, HashSet<Symbol<T>> terminals, Symbol<T>? startTSymbolValue, HashSet<ProductionRule<T>> productionRules)`

  - Parameters:
    - `nonTerminals`: Set of non-terminal symbols.
    - `terminals`: Set of terminal symbols.
    - `startTSymbolValue`: Start symbol of the grammar.
    - `productionRules`: Set of production rules.

  - Description: Initializes a new instance of the `RegularGrammar<T>` class with specified sets of terminals, non-terminals, start symbol, and production rules.

- `RegularGrammar(IEnumerable<T> nonTerminals, IEnumerable<T> terminals, T? startTSymbolValue, IEnumerable<Tuple<IEnumerable<T>, IEnumerable<T>>> productionRules)`

  - Parameters:
    - `nonTerminals`: Enumerable collection of non-terminal symbols.
    - `terminals`: Enumerable collection of terminal symbols.
    - `startTSymbolValue`: Start symbol of the grammar.
    - `productionRules`: Enumerable collection of production rules.

  - Description: Initializes a new instance of the `RegularGrammar<T>` class with enumerable sets of terminals, non-terminals, start symbol, and production rules.

#### Methods

- `protected override void _checkFormatOfProductionRule(ProductionRule<T> rule)`

  - Parameters:
    - `rule`: The production rule to check.

  - Description: Overrides the method from `ContextFreeGrammar<T>` to check the format of production rules specific to regular grammars. Ensures that the right-hand side of each production rule adheres to the regular grammar format.

#### Exceptions

- `RegularGrammarException`

  - Description: Exception class specific to errors related to regular grammars, inheriting from `ContextFreeGrammarException`.

---

### `ProductionRule<T>`

Represents a production rule in a grammar, defining a relationship between symbols.

#### Overview

The `ProductionRule<T>` class is a sealed record that encapsulates a production rule consisting of a left-hand side (LHS) and a right-hand side (RHS). It ensures that the rule adheres to specific invariants and provides methods for checking if it is an epsilon rule, computing hash codes, and ensuring equality with other production rules.

#### Fields

- `static readonly string ProductionRuleStringSeparator`

  - Description: Separator used in the string representation of the production rule (`->`).

#### Properties

- `public Symbol<T>[] LeftHandSide { get; }`

  - Description: Gets the array of symbols on the left-hand side of the production rule.

- `public Symbol<T>[] RightHandSide { get; }`

  - Description: Gets the array of symbols on the right-hand side of the production rule.

#### Constructors

- `public ProductionRule(IEnumerable<Symbol<T>> leftHandSide, IEnumerable<Symbol<T>> rightHandSide)`

  - Parameters:
    - `leftHandSide`: Enumerable collection of symbols on the left-hand side.
    - `rightHandSide`: Enumerable collection of symbols on the right-hand side.

  - Description: Initializes a new instance of the `ProductionRule<T>` class with specified left-hand side and right-hand side symbols. It checks invariants to ensure the validity of the production rule.

#### Methods

- `public bool IsEpsilonRule()`

  - Description: Checks if the production rule is an epsilon rule, which means the right-hand side consists of a single epsilon symbol.

- `public override int GetHashCode()`

  - Description: Computes a hash code for the production rule based on its left-hand side and right-hand side symbols.

- `public void _checkInvariants()`

  - Description: Checks invariants of the production rule:
    - Left-hand side cannot be empty.
    - Right-hand side cannot be empty.
    - Left-hand side must include at least one non-terminal symbol and cannot include an epsilon symbol.
    - Right-hand side cannot include both an epsilon symbol and other symbols simultaneously.

- `public override string ToString()`

  - Description: Returns a string representation of the production rule in the format `LHS -> RHS`.

- `public bool Equals(ProductionRule<T>? other)`

  - Parameters:
    - `other`: Another production rule to compare.

  - Description: Determines whether the current production rule object is equal to another production rule object based on their left-hand side and right-hand side symbols.

#### Exceptions

- `ProductionRuleException`

  - Description: Exception class specific to errors related to production rules, inheriting from `Exception`.

---

### `Symbol<T>`

Represents a symbol in a formal language grammar, parameterized by a type `T` that implements `IComparable<T>` and `IIncrementOperators<T>`.

#### Overview

The `Symbol<T>` struct encapsulates a symbol in a grammar, which can be a non-terminal, terminal, or epsilon (empty symbol). It provides methods for comparison, equality checks, hash code computation, and string representation.

#### Fields

- `public T? Value { get; }`
  - Description: Gets the value associated with the symbol, which could be `null` for epsilon symbols.
  
- `public SymbolType Type { get; }`
  - Description: Gets the type of the symbol, which could be `NonTerminal`, `Terminal`, or `Epsilon`.

- `public static readonly Symbol<T> Epsilon`
  - Description: Represents the epsilon symbol, a constant symbol in the grammar.

- `private static readonly string EpsilonString = "$"`
  - Description: String representation of the epsilon symbol.

#### Constructors

- `public Symbol(T? value, SymbolType type)`
  - Parameters:
    - `value`: The value associated with the symbol.
    - `type`: The type of the symbol (`NonTerminal`, `Terminal`, or `Epsilon`).

  - Description: Initializes a new instance of the `Symbol<T>` struct with the specified value and type.

- `public Symbol()`
  - Description: Initializes a new instance of the `Symbol<T>` struct representing an epsilon symbol (`Type` set to `SymbolType.Epsilon`).

#### Methods

- `public bool Equals(Symbol<T> other)`
  - Parameters:
    - `other`: The symbol to compare with the current symbol.

  - Description: Determines whether the current symbol is equal to another symbol based on their values and types.

- `public bool Equals(T other)`
  - Parameters:
    - `other`: The value to compare with the current symbol's value.

  - Description: Determines whether the current symbol's value is equal to the specified value.

- `public override int GetHashCode()`
  - Description: Computes a hash code for the symbol based on its value. For epsilon symbols, the hash code is consistent.

- `public Symbol<T> GetNextSymbol()`
  - Description: Returns the next symbol in a sequence, primarily used for iterating over symbols.

- `public override string ToString()`
  - Description: Returns a string representation of the symbol. If the symbol is an epsilon, it returns `EpsilonString`; otherwise, it returns the string representation of its value.

- `public int CompareTo(Symbol<T> other)`
  - Parameters:
    - `other`: The symbol to compare with the current symbol.

  - Description: Compares the current symbol with another symbol based on their values.

#### Operators

- `public static bool operator ==(Symbol<T> symbol1, Symbol<T> symbol2)`
- `public static bool operator !=(Symbol<T> symbol1, Symbol<T> symbol2)`
  - Description: Equality operators to compare two symbols based on their values and types.

#### Enum

- `public enum SymbolType { NonTerminal, Terminal, Epsilon }`
  - Description: Enumerates the different types of symbols: `NonTerminal`, `Terminal`, and `Epsilon`.



---

## FormalLanguagesLibrary.Converter

### `Convert`

The `Convert` class in the `FormalLanguagesLibrary.Converter` namespace provides methods to convert formal language constructs, specifically from Regular Grammar (RG) to Non-Deterministic Finite Automaton (NFA).

#### Overview

The `Convert` class contains static methods for converting a `RegularGrammar<char>` into a `NonDeterministicFiniteAutomaton<char, string>` using state, transition, and input alphabet mappings.

#### Methods

- `public static NonDeterministicFiniteAutomaton<char, string> RGtoNFA(RegularGrammar<char> grammar)`

  - **Parameters:**
    - `grammar`: The regular grammar to be converted into an NFA.

  - **Returns:**
    - Returns a `NonDeterministicFiniteAutomaton<char, string>` representing the converted NFA.

  - **Description:**
    - Converts a regular grammar (`RegularGrammar<char>`) into a Non-Deterministic Finite Automaton (`NonDeterministicFiniteAutomaton<char, string>`).
    - Constructs states, final states, input alphabet, initial state, and transition functions based on the grammar's rules and properties.
    - Handles epsilon rules, single symbol rules, and two-symbol rules in both right-regular and left-regular formats.

#### Usage

To use the `RGtoNFA` method:

```csharp
// Example usage:
RegularGrammar<char> regularGrammar = new RegularGrammar<char>();
// Initialize and populate the regular grammar...

// Convert regular grammar to NFA
NonDeterministicFiniteAutomaton<char, string> nfa = Convert.RGtoNFA(regularGrammar);
```


