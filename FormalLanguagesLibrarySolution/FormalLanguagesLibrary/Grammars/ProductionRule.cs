using System.Numerics;
using System.Text;





namespace FormalLanguagesLibrary.Grammars
{


    public record class ProductionRule<T> where T : IComparable<T>, IIncrementOperators<T>
    {
        static readonly string ProductionRuleStringSeparator = "->";

        public Symbol<T>[] LeftHandSide { get; }
        public Symbol<T>[] RightHandSide { get; }


        public ProductionRule(IEnumerable<Symbol<T>> leftHandSide, IEnumerable<Symbol<T>> rightHandSide)
        {
            LeftHandSide = leftHandSide.ToArray();
            RightHandSide = rightHandSide.ToArray();

            _checkInvariants();
        }

        public bool IsEpsilonRule()
        {
            if (LeftHandSide.Length == 1 && LeftHandSide[0].Type == SymbolType.Epsilon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (LeftHandSide,RightHandSide).GetHashCode();
        }


        public void _checkInvariants()
        {
            if (LeftHandSide.Length == 0)
            {
                throw new ProductionRuleException($"Left-hand side cannot be empty for production rule: {ToString()}");
            }
            if (RightHandSide.Length == 0)
            {
                throw new ProductionRuleException($"Right-hand side cannot be empty for production rule: {ToString()}");
            }
            
            // Left-hand side must include a non-terminal & cannot include an epsilon
            {
                bool nonTerminalIncluded = false;

                foreach (Symbol<T> symbol in LeftHandSide)
                {
                    if(symbol.Type == SymbolType.NonTerminal)
                    {
                        nonTerminalIncluded = true;
                    }
                    else if(symbol.Type == SymbolType.Epsilon)
                    {
                        throw new ProductionRuleException($"Left-hand side cannot include an epsilon for production rule: {ToString()}");
                    }
                }

                if(!nonTerminalIncluded)
                {
                    throw new ProductionRuleException($"Left-hand side must include a non-terminal symbol for production rule: {ToString()}");
                }
            }

            // Right-hand side cannot include an epsilon and other symbols at the same time

            {
                bool epsilonIncluded = false;
                
                foreach (Symbol<T> symbol in RightHandSide)
                {
                    if (symbol.Type == SymbolType.Epsilon)
                    {
                        epsilonIncluded = true;
                    }
                }

                if (RightHandSide.Length != 1 && epsilonIncluded)
                {
                    throw new ProductionRuleException($"Right-hand side cannot include Epsilon symbol and other symbols for production rule: {ToString()}");
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach(var symbol in LeftHandSide)
            {
                sb.Append(symbol.ToString());
            }
            sb.Append(ProductionRuleStringSeparator);

            foreach(var symbol in RightHandSide)
            {
                sb.Append(symbol.ToString());
            }

            return sb.ToString();
        }

    }

    public class ProductionRuleException : Exception
    {
        public ProductionRuleException(string message) : base(message)
        {

        }
    }
}
