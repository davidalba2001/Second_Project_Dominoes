using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine.Interfaces
{
    public interface IGameLogic<TValue,T> where TValue:IValue<T>
    {
        int Turn { get; }
        Board<TValue, T> board { get; }
        List<Player<TValue, T>> Players { get; }
        Rules<TValue, T> Rules { get; }
        List<Chip<TValue, T>> Chips { get; }
        Player<TValue, T>? CurrentPlayer { get; }

        void ChangeValidCurrentPlayer();
        void CurrentTurn();
        bool EndGame();
        void HandOutChips(int CountChip);
    }
}