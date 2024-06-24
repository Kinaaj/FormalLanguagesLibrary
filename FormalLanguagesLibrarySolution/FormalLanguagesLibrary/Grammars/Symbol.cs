using System.Numerics;

namespace FormalLanguagesLibrary.Grammars
{

    public struct Symbol<T>: IEquatable<Symbol<T>>, IComparable<Symbol<T>> where T : IComparable<T>, IIncrementOperators<T>
    {

        public T? Value { get; }
        public SymbolType Type { get; }

        // Constant to create an Epsilon symbol
        public static readonly Symbol<T> Epsilon = new Symbol<T>(default, SymbolType.Epsilon);

        private static readonly string EpsilonString = "$";

        public override bool Equals(object? obj) => obj is Symbol<T> other && this.Equals(other);

        public Symbol(T? value, SymbolType type)
        {
            this.Value = value;
            this.Type = type;
        }

        //Creates an Epsilon
        public Symbol()
        {
            this.Value = default;
            this.Type = SymbolType.Epsilon;
        }

        //Compares only values!
        public bool Equals(Symbol<T> other)
        {
            // For epsilons it doesn't depend on the value of the symbol
            if (Type == SymbolType.Epsilon && other.Type == SymbolType.Epsilon)
                return true;

            else if((Type == SymbolType.Epsilon && other.Type != SymbolType.Epsilon) || (Type != SymbolType.Epsilon && other.Type == SymbolType.Epsilon))
                return false;

            else
                return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public bool Equals(T other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other);
        }

        public override int GetHashCode()
        {
            // HashCode of two Epsilon symbols must be always Equal
            if (Type == SymbolType.Epsilon) return SymbolType.Epsilon.GetHashCode();

            return Value?.GetHashCode() ?? 0;
        }

        public static bool operator ==(Symbol<T> symbol1, Symbol<T> symbol2) => symbol1.Equals(symbol2);
        public static bool operator !=(Symbol<T> symbol1, Symbol<T> symbol2) => !(symbol1 == symbol2);

        public Symbol<T> GetNextSymbol()
        {
            if (Value is not null)
            {
                T newValue = Value;
                newValue++;
                return new Symbol<T>(newValue, Type);
            }
            else return this;
        }

        public override string ToString()
        {
            if (!Type.Equals(SymbolType.Epsilon))
            {
                return Value?.ToString() ?? "";
            }
            else
            {
                return EpsilonString;
            }

        }

        public int CompareTo(Symbol<T> other)
        {
            return (Value?.CompareTo(other.Value) ?? -1);
        }
    }

    public enum SymbolType { NonTerminal, Terminal, Epsilon}
}

