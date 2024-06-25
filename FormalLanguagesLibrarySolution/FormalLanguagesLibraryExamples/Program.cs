//using FormalLanguagesLibrary.Grammars;
using System.Collections.Immutable;
using FormalLanguagesLibrary.Automata;
using FormalLanguagesLibrary.Grammars;
//using FormalLanguagesLibrary.Automata;

using GrammarSymbol = FormalLanguagesLibrary.Grammars.Symbol<char>;
using GrammarSymbolType = FormalLanguagesLibrary.Grammars.SymbolType;

using AutomataSymbol = FormalLanguagesLibrary.Automata.Symbol<char>;
using AutomataState = FormalLanguagesLibrary.Automata.State<string>;

namespace FormalLanguagesLibraryExamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
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

            Console.WriteLine(automaton.ToString());


            ////Define input alphabet:
            //var s0 = new Symbol<char>('0');
            //var s1 = new Symbol<char>('1');
            //var epsilon = Symbol<char>.Epsilon;

            ////Define states:
            //var A = new State<string>("A");
            //var B = new State<string>("B");
            //var C = new State<string>("C");

            ////Define properties:
            //var inputAlphabet = new HashSet<Symbol<char>> { s0, s1 };
            //var states = new HashSet<State<string>> { A, B, C };
            //var initialState = A;
            //var finalStates = new HashSet<State<string>> { C };

            ////Create a transition function
            //var transitionFunction = new TransitionFunction<char, string>();
            //transitionFunction.AddTransition(A, s0, A);
            //transitionFunction.AddTransition(A, epsilon, B);

            //transitionFunction.AddTransition(B, s1, B);
            //transitionFunction.AddTransition(B, epsilon, C);

            //transitionFunction.AddTransition(C, s0, C);
            //transitionFunction.AddTransition(C, s1, C);

            //// And then create an eNFA
            //var eNFA = new NonDeterministicEpsilonFiniteAutomaton<char, string>(inputAlphabet, states, initialState, transitionFunction, finalStates);

            //// We can ask if the corresponding sequence of Symbols<T> or T is accepted:
            //char[] inputString = "00101".ToCharArray();

            //Console.WriteLine(eNFA.Accepts(inputString));
            //Console.WriteLine(eNFA.Accepts(""));
            //returns True


            //// First of all we will create a non-terminals:
            //var S = new Symbol<char>('S', SymbolType.NonTerminal); // Will be starting symbol
            //var A = new Symbol<char>('A', SymbolType.NonTerminal);
            //var B = new Symbol<char>('B', SymbolType.NonTerminal);
            //var C = new Symbol<char>('C', SymbolType.NonTerminal);
            //var nonTerminals = new HashSet<Symbol<char>>() { S, A, B, C };

            //// Then terminals:
            //var t0 = new Symbol<char>('0', SymbolType.Terminal);
            //var t1 = new Symbol<char>('1', SymbolType.Terminal);
            //var terminals = new HashSet<Symbol<char>>() { t0, t1 };

            //// Then we will create a set of production rules:
            //var productionRules = new HashSet<ProductionRule<char>>() {
            //new ProductionRule<char>([S], [A,B]),
            //// We continue in a shorter syntax
            //new([C],[t0]),
            //// For creating rule C->Epsilon, we can use the read-only static Epsilon field of the Symbol class. 
            //new([C],[Symbol<char>.Epsilon]),
            //new([A],[C, B]),
            //new([A],[C, t0]),
            //new([A],[t0, t0, t1]),
            //new([B],[C]),
            //new([B],[S]),
            //};

            //// Finally we will create a grammar:

            //ContextFreeGrammar<char> grammar = new ContextFreeGrammar<char>(nonTerminals, terminals, S, productionRules);

            //// We can then for-example find all non-terminals generating epsilon
            //HashSet<Symbol<char>> symbolGeneratingE = grammar.GetNonTerminalsGeneratingEpsilon();
            //// symbolGeneratingE = {C, B, A, S};

            //// Remove all epsilon rules
            //grammar.RemoveEpsilonRules();

            //// And printing the final grammar:
            //Console.WriteLine(grammar.ToString());

            //





            //var S = new Symbol<char>('S', SymbolType.NonTerminal);
            //var A = new Symbol<char>('A', SymbolType.NonTerminal);
            //var B = new Symbol<char>('B', SymbolType.NonTerminal);
            //var C = new Symbol<char>('C', SymbolType.NonTerminal);

            //var t0 = new Symbol<char>('0', SymbolType.Terminal);
            //var t1 = new Symbol<char>('1', SymbolType.Terminal);

            //HashSet<ProductionRule<char>> productionRules = new() {
            //    new ProductionRule<char>([S],[A,B]),
            //    new([C],[Symbol<char>.Epsilon]),
            //    new([C],[t0]),
            //    new([A],[C, B]),
            //    new([A],[C, t0]),
            //    new([A],[t0, t0, t1]),
            //    new([B],[C]),
            //    new([B],[S]),
            //};


            //ContextFreeGrammar<char> g = new ContextFreeGrammar<char>(
            //    [S, A, B, C, I],
            //    [t0, t1],
            //    S,
            //    productionRules
            //);


            //g.RemoveEpsilonRules();

            //Symbol<char> expectedStartingSymbol = new('U', SymbolType.NonTerminal);

            //HashSet<Symbol<char>> expectedNonTerminals = new() { S, A, B, C, I, expectedStartingSymbol };

            //HashSet<ProductionRule<char>> expectedProductionRules = new() {
            //    new([expectedStartingSymbol],[S]),
            //    new([expectedStartingSymbol],[Symbol<char>.Epsilon]),
            //    new([S],[A,B]),
            //    new([S],[A,B]),
            //    new([S],[A]),
            //    new([S],[B]),
            //    new([C],[t0]),
            //    new([A],[C, B]),
            //    new([A],[C]),
            //    new([A],[B]),
            //    new([A],[C, t0]),
            //    new([A],[t0, t0, t1]),
            //    new([A],[t0]),
            //    new([B],[C]),
            //    new([B],[S]),
            //};

            //Console.WriteLine(string.Join(',', expectedProductionRules));
            //Console.WriteLine(string.Join(',', g.ProductionRules));


            //GrammarSymbol A = new('A', GrammarSymbolType.NonTerminal);
            //GrammarSymbol B = new('B', GrammarSymbolType.NonTerminal);
            //GrammarSymbol C = new('C', GrammarSymbolType.NonTerminal);

            //GrammarSymbol a = new('a', GrammarSymbolType.Terminal);
            //GrammarSymbol b = new('b', GrammarSymbolType.Terminal);



            //HashSet<GrammarSymbol> nonTerminals = new() { A, B, C };
            //HashSet<GrammarSymbol> terminals = new() { a, b };
            //GrammarSymbol startSymbol = A;

            //HashSet<ProductionRule<char>> productionRules = [
            //    new([A], [a, B]),
            //    new([A], [b, A]),
            //    new([A], [b]),

            //    new([B], [a, C]),
            //    new([B], [b, B]),

            //    new([C], [a, A]),
            //    new([C], [b, C]),
            //    new([C], [a]),
            //    ];


            //RegularGrammar<char> regularGrammar = new(nonTerminals, terminals, startSymbol, productionRules);


            //var automaton = FormalLanguagesLibrary.Converter.Convert.RGtoNFA(regularGrammar);

            //AutomataState stateA = new("A");
            //AutomataState stateB = new("B");
            //AutomataState stateC = new("C");


            //AutomataSymbol a2 = new('a');
            //AutomataSymbol b2 = new('b');


            //HashSet<AutomataSymbol> expectedInputAlphabet = [a2, b2];
            //HashSet<AutomataState> expectedStates = [stateA, stateB, stateC];

            //HashSet<AutomataState> expectedFinalStates = [stateA, stateC];
            //AutomataState expectedStartingState = stateA;


        }
    }
    
}
