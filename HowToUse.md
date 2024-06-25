# How To Use FormalLanguagesLibrary
## Introduction

Welcome to the FormalLanguagesLibrary documentation. This library provides a set of tools and classes for working with formal languages and automata, including grammars, finite automata, and related operations.
## Grammars

Grammars are essential in formal language theory, representing rules for generating strings in a language. This library supports all grammars of the Chomsky Hierarchy.
Classes for creating such grammars are RecursivelyEnumerableGrammar, ContextSensitiveGrammar, ContextFreeGrammar, and RegularGrammar.

Each grammar consists of these main properties:

    NonTerminals: A set of non-terminal symbols (variables)
    Terminals: A set of terminal symbols (alphabet)
    StartSymbol: A starting symbol
    ProductionRules: A set of production rules
    
To define (for a example) a Context-Free Grammar with the library, we have to define all properties of the grammar.

```csharp
using FormalLanguagesLibrary.Grammars

  // First of all we will create a non-terminals:
  var S = new Symbol<char>('S', SymbolType.NonTerminal); // Will be starting symbol
  var A = new Symbol<char>('A', SymbolType.NonTerminal);
  var B = new Symbol<char>('B', SymbolType.NonTerminal);
  var C = new Symbol<char>('C', SymbolType.NonTerminal);
  var nonTerminals = new HashSet<Symbol<char>>() {S, A, B, C};

  // Then terminals:
  var t0 = new Symbol<char>('0', SymbolType.Terminal);
  var t1 = new Symbol<char>('1', SymbolType.Terminal);
  var terminals = new HashSet<Symbol<char>>() {t0, t1};

  // Then we will create a set of production rules:
  var productionRules = new HashSet<ProductionRule<char>>() {
    new ProductionRule<char>([S], [A,B]),
    // We continue in a shorter syntax
    new([C],[t0]),
    // For creating rule C->Epsilon, we can use the read-only static Epsilon field of the Symbol class. 
    new([C],[Symbol<char>.Epsilon]),
    new([A],[C, B]),
    new([A],[C, t0]),
    new([A],[t0, t0, t1]),
    new([B],[C]),
    new([B],[S]),
  };

  // Finally we will create a grammar:
  
  ContextFreeGrammar<char> grammar = new ContextFreeGrammar<char>(nonTerminals, terminals, S, productionRules);

  // We can then for-example find all non-terminals generating epsilon
  HashSet<Symbol<char>> symbolGeneratingE = grammar.GetNonTerminalsGeneratingEpsilon();
  // symbolGeneratingE = {C, B, A, S};

  // Remove all epsilon rules
  grammar.RemoveEpsilonRules();

  // And printing the final grammar:
  Console.WriteLine(grammar.ToString());

  // Non-Terminals: {S, A, B, C, T}
  // Terminals: {0, 1}
  // Starting Symbol: T
  // Production Rules: {
  // S->AB,
  // S->A,
  // S->B,
  // C->0,
  // A->CB,
  // T->$,
  // A->C,
  // A->B,
  // A->C0,
  // A->0,
  // A->001,
  // B->C,
  // T->S,
  // B->S
  // }
  
  

```

## Automata

Automata are fundamental in formal language theory, representing abstract machines that recognize or generate languages. This library provides several types of automata, each building on the abstract Automaton class.
### Finite Automata
This library provides several types of finite automata that are equivalent to Regular Grammars, such as NonDeterministicEpsilonFiniteAutomaton, NonDeterministicFiniteAutomaton, DeterministicFiniteAutomaton.

Each automaton consists of these main properties:

    InputAlphabet: A set of symbols on which the automaton operates
    States: A set of all states of the automaton
    InitialState: An initial state from States
    TransitionFunction: An object of the TransitionFunction class, which defines the transitions of the automaton
    FinalStates: A set of final states (must be a subset of States)

To define an automaton we can create a 

```csharp
using FormalLanguagesLibrary.Automata

  //Define input alphabet:
  var s0 = new Symbol<char>('0');
  var s1 = new Symbol<char>('1');
  var epsilon = Symbol<char>.Epsilon;

  //Define states:
  var A = new State<string>("A");
  var B = new State<string>("B");
  var C = new State<string>("C");

  //Define properties:
  var inputAlphabet = new HashSet<Symbol<char>> { s0, s1 };
  var states = new HashSet<State<string>> { A, B, C };
  var initialState = A;
  var finalStates = new HashSet<State<string>> { C };

  //Create a transition function
  var transitionFunction = new TransitionFunction<char, string>();
  transitionFunction.AddTransition(A, s0, A);
  transitionFunction.AddTransition(A, epsilon, B);

  transitionFunction.AddTransition(B, s1, B);
  transitionFunction.AddTransition(B, epsilon, C);

  transitionFunction.AddTransition(C, s0, C);
  transitionFunction.AddTransition(C, s1, C);

  // And then create an eNFA
  var eNFA = new NonDeterministicEpsilonFiniteAutomaton<char, string>(inputAlphabet, states, initialState, transitionFunction, finalStates);

  // We can ask if the corresponding sequence of Symbols<T> or T is accepted:
  char[] inputString = "00101".ToCharArray();

  eNFA.Accepts(inputString);
  // returns True

  eNFA.Accepts("");
  // returns True (because the automaton accepts empty string)

  // And then we can forexample convert it to the NFA or DFA
  NonDeterministicAutomaton NFA = eNFA.ToNFA();
  
```


## Converter
There is a Converter class, which consist of methods for converting specific Automata and Grammars.

For example converting a RegularGrammar to a NonDeterministicFiniteAutomaton:

```csharp
using GrammarSymbol = FormalLanguagesLibrary.Grammars.Symbol<char>;
using GrammarSymbolType = FormalLanguagesLibrary.Grammars.SymbolType;

using AutomataSymbol = FormalLanguagesLibrary.Automata.Symbol<char>;
using AutomataState = FormalLanguagesLibrary.Automata.State<string>;

    GrammarSymbol A = new('A', GrammarSymbolType.NonTerminal);
    GrammarSymbol B = new('B', GrammarSymbolType.NonTerminal);
    GrammarSymbol C = new('C', GrammarSymbolType.NonTerminal);

    GrammarSymbol a = new('a', GrammarSymbolType.Terminal);
    GrammarSymbol b = new('b', GrammarSymbolType.Terminal);


    HashSet<GrammarSymbol> nonTerminals = new() { A, B, C };
    HashSet<GrammarSymbol> terminals = new() { a, b };
    GrammarSymbol startSymbol = A;

    HashSet<ProductionRule<char>> productionRules = [
    new([A], [GrammarSymbol.Epsilon]),
    new([A], [a, B]),
    new([A], [b, A]),
    new([A], [b]),

    new([B], [a, C]),
    new([B], [b, B]),

    new([C], [a, A]),
    new([C], [b, C]),
    new([C], [a]),
    ];

    RegularGrammar<char> regularGrammar = new(nonTerminals, terminals, startSymbol, productionRules);

    var automaton = FormalLanguagesLibrary.Converter.Convert.RGtoNFA(regularGrammar);
    Console.WriteLine(automaton)

// InputAlphabet: {a, b}
// States: {A, B, C, final}
// InitialState: ATransition Funciton:
// (A, a) -> {B}
// (A, b) -> {A, final}
// (B, a) -> {C}
// (B, b) -> {B}
// (C, a) -> {A, final}
// (C, b) -> {C}
// Final States: {final, A}
```


