using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine.Interfaces
{
    public interface IStrategy<T>
    {
        public bool ValidMove(Player<T> player, Board<T> board, IRules<T> rules, out (Chip<T>, IValue<T>) move);
    }
}