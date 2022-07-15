using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;


namespace DominoEngine
{
    public class IsLocked<TValue, T> : IEndCondition<TValue, T> where TValue : IValue<T>
    {
        public bool IsFinal(List<Player<TValue, T>> players)
        {
            foreach (var item in players)
            {
                if (!item.Pass) return false;
            }
            return true;
        }
    }
}