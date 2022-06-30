using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public interface IValue<T> : IEquatable<T>
    {
        public T value { get; }
    }
    public class Number<T> : IValue<T>
    {
        private T item;
        public Number(T value)
        {
            this.item = value;
        }
        public T value => item;

        public bool Equals(T other)
        {
             if (this.value.Equals(other)) return true;
            else return false;
        }
    }

    public class Number2<T> : IValue<T>
    {
        private T item;
        public Number2(T value)
        {
            this.item = value;
        }
        public T value => item;

        public bool Equals(T other)
        {
             if (this.value.Equals(other)) return true;
            else return false;
        }
    }
}