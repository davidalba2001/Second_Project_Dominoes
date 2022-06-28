using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public class Number : IEquatable<Number>
    {
        public Number(int number)
        {
            this.number = number;
        }

        public int number{get;}

        public bool Equals(Number? other)
        {
            if(this.number==other.number) return true;
            else return false;
        }
    }
}