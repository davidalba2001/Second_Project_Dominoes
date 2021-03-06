using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;
namespace DominoEngine
{
    public class Numeric : IValue<int>, IRankable
    {
        public Numeric(int value)
        {
            Value = value;
        }

        public int Value { get; }



        public int CompareTo(Numeric other)
        {
            if (Value == other.Value) return 0;
            if (Value < other.Value) return -1;
            return 1;
        }

        public bool Equals(IValue<int>? other)
        {
            return object.Equals(this, other);
        }

        public int Rank()
        {
            return Value;
        }

        public int CompareTo(IRankable? other)
        {
            if (this.Rank() == other.Rank()) return 0;
            if (this.Rank() < other.Rank()) return -1;
            return 1;
        }

    }

    public class Emojis : IValue<string>, IRankable
    {
        public string Value { get; }
        public Emojis(string value)
        {
            Value = value;
        }

        public bool Equals(IValue<string>? other)
        {
            return object.Equals(this, other);
        }

        public int Rank()
        {
            return ((short)Value[0]);
        }

        public int CompareTo(IRankable? other)
        {
            if (this.Rank() == other.Rank()) return 0;
            if (this.Rank() < other.Rank()) return -1;
            return 1;
        }
    }

}