using FormalLanguagesLibrary.Grammars;
using System.Collections.Immutable;
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



            //var S = new Symbol<char>('S', SymbolType.NonTerminal);
            //var A = new Symbol<char>('A', SymbolType.NonTerminal);
            //var B = new Symbol<char>('B', SymbolType.NonTerminal);
            //var C = new Symbol<char>('C', SymbolType.NonTerminal);
            //var I = new Symbol<char>('T', SymbolType.NonTerminal);

            //var t0 = new Symbol<char>('0', SymbolType.Terminal);
            //var t1 = new Symbol<char>('1', SymbolType.Terminal);

            //HashSet<ProductionRule<char>> productionRules = new() {
            //    new([S],[A,B]),
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


            GrammarSymbol A = new('A', GrammarSymbolType.NonTerminal);
            GrammarSymbol B = new('B', GrammarSymbolType.NonTerminal);
            GrammarSymbol C = new('C', GrammarSymbolType.NonTerminal);

            GrammarSymbol a = new('a', GrammarSymbolType.Terminal);
            GrammarSymbol b = new('b', GrammarSymbolType.Terminal);



            HashSet<GrammarSymbol> nonTerminals = new() { A, B, C };
            HashSet<GrammarSymbol> terminals = new() { a, b };
            GrammarSymbol startSymbol = A;

            HashSet<ProductionRule<char>> productionRules = [
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

            AutomataState stateA = new("A");
            AutomataState stateB = new("B");
            AutomataState stateC = new("C");


            AutomataSymbol a2 = new('a');
            AutomataSymbol b2 = new('b');


            HashSet<AutomataSymbol> expectedInputAlphabet = [a2, b2];
            HashSet<AutomataState> expectedStates = [stateA, stateB, stateC];

            HashSet<AutomataState> expectedFinalStates = [stateA, stateC];
            AutomataState expectedStartingState = stateA;


        }
    }
    
}
