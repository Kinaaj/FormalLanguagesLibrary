using System;

namespace FormalLanguagesLibrary.Automata
{
    public readonly record struct State<T>(T Value) : IEquatable<State<T>>
    {
        public bool Equals(State<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }
        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "";
        }

    }
}
