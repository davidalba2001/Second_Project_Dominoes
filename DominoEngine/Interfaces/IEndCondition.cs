using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine.Interfaces
{
    public interface IEndCondition<T>
    {
        public bool IsFinal(List<Player<T>> players);
    }
}