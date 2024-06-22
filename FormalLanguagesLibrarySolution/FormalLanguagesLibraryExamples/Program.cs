using FormalLanguagesLibrary.Grammars;
using FormalLanguagesLibrary.Automata;



namespace FormalLanguagesLibraryExamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            char[] nonTerminals = { 'A', 'B', 'S','C' };
            char[] terminals = { '1', '2', '3','4'};
            char starTSymbolValue = 'S';

            var rule1 = new Tuple<char[], char[]>(
                "1B".ToCharArray(), "12".ToCharArray()
            );

            var rule2 = new Tuple<char[], char[]>(
                ['A', '1', 'B', '2', '3', 'B'],
                ['A', '1', 'B', '2','4', 'B', 'S']
            );

            var productionRules = new[] { rule1 };

            ContextSensitiveGrammar<char> g = new ContextSensitiveGrammar<char>(
                nonTerminals,
                terminals,
                starTSymbolValue,
                productionRules
            );


            FormalLanguagesLibrary.Grammars.Symbol<char> s = new('2', FormalLanguagesLibrary.Grammars.SymbolType.NonTerminal);
            Console.WriteLine(s.ToString());
            Console.WriteLine(s.Type);





            

        }
    }
}
