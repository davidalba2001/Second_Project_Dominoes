using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;

namespace DominoEngine
{

    public class PlayAllChips<T> : IWinCondition<T>, IEndCondition<T>
    {
        public bool IsFinal(List<Player<T>> players)
        {
            foreach (var player in players)
            {
                if(IsWinner(player,players)) return true;
            }
            return false;
        }
        public bool IsWinner(Player<T> player,List<Player<T>> players)
        {
            return (player.NumChips == 0);
        }
    }

    public class WinnerByChips<T> : IWinCondition<T>
    {
        public bool IsWinner(Player<T> player,List<Player<T>> players)
        {
            return (player.NumChips == players.Min(x => x.NumChips));
        }
    }
}