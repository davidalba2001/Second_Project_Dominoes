using DominoEngine.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;


namespace DominoEngine
{
    public class ClassicRules<TValue, T> : IRules<TValue, T> where TValue : IValue<T>
    {
        public IWinCondition<TValue, T> WinnerByChips = new WinnerByChips<TValue, T>();

        private IWinCondition<TValue, T> WinAllChip = new PlayAllChips<TValue, T>();
        private IEndCondition<TValue, T> LockedCondition = new IsLocked<TValue, T>();
        private IEndCondition<TValue, T> PlayAllChips = new PlayAllChips<TValue, T>();

        public bool PlayIsValid(Chip<TValue, T> chip, TValue value)
        {
            return chip.LinkL.Equals(value) || chip.LinkR.Equals(value) || value == null;
        }
        public bool IsTurnToPlay(int turn, int NumPlayers, int playerOrder)
        {
            return turn % NumPlayers == playerOrder;
        }
        public List<Chip<TValue, T>> GenerateChips(int cant, TValue[] values)
        {
            List<Chip<TValue, T>> Chips = new List<Chip<TValue, T>>();
            for (int i = 0; i < cant; i++)
            {
                for (int j = 1; j < cant; j++)
                {
                    Chips.Add(new Chip<TValue, T>(values[i], values[j]));
                }
            }
            return Chips;
        }

        public bool IsFinal(List<Player<TValue, T>> players)
        {
            return PlayAllChips.IsFinal(players) || LockedCondition.IsFinal(players);
        }
        public bool IsTie(List<Player<TValue, T>> players, out List<Player<TValue, T>>? winners)
        {
            List<Player<TValue, T>> playersWinners = new();
            if (IsFinal(players))
            {
                foreach (var player in players)
                {

                    if (WinnerByChips.IsWinner(player, players) || WinAllChip.IsWinner(player, players))
                    {
                        playersWinners.Add(player);
                    }

                }
                winners = playersWinners;
                if (playersWinners.Count == 1)
                {
                    return false;
                }
                return true;
            }
            winners = default(List<Player<TValue, T>>);
            return false;
        }
        public bool IsWinner(List<Player<TValue, T>> players, out Player<TValue, T>? player)
        {
            List<Player<TValue, T>>? playersWinners = new List<Player<TValue, T>>();

            if (IsTie(players, out playersWinners))
            {
                player = default(Player<TValue, T>);
                return false;
            }
            if (playersWinners == default(List<Player<TValue, T>>))
            {
                player = default(Player<TValue, T>);
                return false;
            }
            else if (playersWinners.Count == 1)
            {
                player = playersWinners[0];
                return true;
            }
            player = default(Player<TValue, T>);
            return false;
        }

    }
}