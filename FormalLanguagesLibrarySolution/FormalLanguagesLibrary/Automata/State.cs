using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
