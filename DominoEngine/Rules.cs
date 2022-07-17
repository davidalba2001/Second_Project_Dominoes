using DominoEngine.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;


namespace DominoEngine
{
    public class Rules<TValue, T> where TValue : IValue<T>
    {
        ICollection<IWinCondition<TValue, T>> WinConditions;
        ICollection<IEndCondition<TValue, T>> FinalCondition;

        public Rules(ICollection<IWinCondition<TValue, T>> winConditions, ICollection<IEndCondition<TValue, T>> finalCondition)
        {
            WinConditions = winConditions;
            FinalCondition = finalCondition;
        }
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
                for (int j = i; j < cant; j++)
                {
                    Chips.Add(new Chip<TValue, T>(values[i], values[j]));
                }
            }
            return Chips;
        }
        public bool IsFinal(List<Player<TValue, T>> players)
        {
            bool isFinal = false;
            foreach (var finalCondition in FinalCondition)
            {
                isFinal = isFinal || finalCondition.IsFinal(players);
            }
            return isFinal;
        }
        public bool IsTie(List<Player<TValue, T>> players, out  List<Player<TValue, T>> winners)
        {
            List<Player<TValue, T>> playersWinners = new();
            foreach (var winCondition in WinConditions)
            {
                foreach (var player in players)
                {
                    if (winCondition.IsWinner(player, players))
                    {
                        if (!playersWinners.Contains(player))
                        {
                            playersWinners.Add(player);
                        }
                    }
                }
            }
            if(playersWinners.Count > 1)
            {
                winners = playersWinners;
                return true;
            }
            winners = playersWinners;
            return false;
        }
        public bool IsWinner(List<Player<TValue, T>> players, out Player<TValue, T> player)
        {
            List<Player<TValue, T>> playerWin;
            if(!IsTie(players,out playerWin))
            {
                if(playerWin.Count == 1)
                {
                    player = playerWin[0];
                    return true;
                }
            }
            player = default(Player<TValue, T>);
            return false;
        }
    }
}