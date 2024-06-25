using System;
using System.Collections.Generic;

namespace FormalLanguagesLibrary.Automata
{
    // State struct representing a state in an automaton, parameterized by a type T for the state's value.
    public readonly record struct State<T>(T Value) : IEquatable<State<T>>
    {
        public bool Equals(State<T> other)
        {
            // Uses the default equality comparer for type T to compare the values of the states.
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        // Hash code for the state based on its value.
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
