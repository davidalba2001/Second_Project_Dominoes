using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine.Interfaces
{
    public interface IStrategy<TValue, T> where TValue : IValue<T>
    {
        public bool ValidMove(Player<TValue, T> player, Board<TValue, T> board, IRules<TValue, T> rules, out (Chip<TValue, T>,TValue) move);
    }


}