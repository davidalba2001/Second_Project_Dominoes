using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;

namespace DominoEngine
{

    public class PlayAllChips<TValue, T> : IWinCondition<TValue, T>, IEndCondition<TValue, T> where TValue : IValue<T>
    {
        public bool IsFinal(List<Player<TValue, T>> players)
        {
            foreach (var player in players)
            {
                if (IsWinner(player, players)) return true;
            }
            return false;
        }
        public bool IsWinner(Player<TValue, T> player, List<Player<TValue, T>> players)
        {
            return (player.NumChips == 0);
        }
    }

    public class WinnerByChips<TValue, T> : IWinCondition<TValue, T> where TValue : IValue<T>
    {
        public bool IsWinner(Player<TValue, T> player, List<Player<TValue, T>> players)
        {
            return (player.NumChips == players.Min(x => x.NumChips));
        }
    }

    //TODO: Implementa el winner Por Puntos

    public class WinnerByPuntos<TValue, T> : IWinCondition<TValue, T> where TValue : IValue<T>, IComparable<TValue>
    {
        public bool IsWinner(Player<TValue,T> player, List<Player<TValue,T>> players)
        {
            throw new NotImplementedException();
        }
    }
}