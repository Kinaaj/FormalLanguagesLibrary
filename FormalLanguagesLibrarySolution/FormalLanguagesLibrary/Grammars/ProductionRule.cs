using System.Numerics;
using System.Text;





namespace FormalLanguagesLibrary.Grammars
{


    public sealed record class ProductionRule<T> : IEquatable<ProductionRule<T>> where T : IComparable<T>, IIncrementOperators<T>
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
            if (RightHandSide.Length == 1 && RightHandSide[0].Type == SymbolType.Epsilon)
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
            unchecked // Allow overflow
            {
                int hash = 17;

                foreach (var symbol in LeftHandSide)
                {
                    hash = hash * 23 + symbol.GetHashCode();
                }

                foreach (var symbol in RightHandSide)
                {
                    hash = hash * 23 + symbol.GetHashCode();
                }

                return hash;
            }
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

        public bool Equals(ProductionRule<T>? other)
        {
            if (other is null) return false;
            else
            {
                return (Enumerable.SequenceEqual(LeftHandSide, other.LeftHandSide) && Enumerable.SequenceEqual(RightHandSide, other.RightHandSide));
            }
        }
    }

    public class ProductionRuleException : Exception
    {
        public ProductionRuleException(string message) : base(message)
        {

        }
    }
}
