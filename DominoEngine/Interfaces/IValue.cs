using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine.Interfaces
{
    public interface IValue<T> : IEquatable<IValue<T>>
    {
        public T? Value { get; }
    }
    
}