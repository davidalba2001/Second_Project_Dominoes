using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine.Interfaces
{
    public interface IWinCondition<TValue,T> where TValue : IValue<T>
    {
        public bool IsWinner(Player<TValue,T> player,List<Player<TValue,T>> players);
    }
}