using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;
namespace DominoEngine
{
    public class Numeric : IValue<int>, IComparable<Numeric>
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


        public static bool operator <(Numeric value1, Numeric value2)
        {
            return value1.CompareTo(value2) == -1;
        }
        public static bool operator >(Numeric value1, Numeric value2)
        {
            return value1.CompareTo(value2) == 1;
        }
        public static bool operator ==(Numeric value1, Numeric value2)
        {
            return value1.CompareTo(value2) == 0;
        }
        public static bool operator !=(Numeric value1, Numeric value2)
        {
            return value1.CompareTo(value2) != 0;
        }
    }
}