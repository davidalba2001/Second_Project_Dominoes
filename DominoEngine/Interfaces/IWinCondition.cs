using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine.Interfaces
{
    public interface IWinCondition<T>
    {
        public bool IsWinner(Player<T> player,List<Player<T>> players);
    }
}