using DominoEngine.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;


namespace DominoEngine
{


    public class Rules<T> : IRules<T>
    {

        private List<IWinCondition<T>> WinConditions;
        private List<IEndCondition<T>> EndConditions;

        public Rules(List<IWinCondition<T>> winConditions, List<IEndCondition<T>> endConditions, int numPlayers, int numChips)
        {
            WinConditions = winConditions;
            EndConditions = endConditions;
            NumPlayers = numPlayers;
            NumChips = numChips;
        }
        public int NumPlayers { get; private set; }
        public int NumChips { get; }
        public bool PlayIsValid(Chip<T> chip, IValue<T>? value)
        {
            return chip.LinkL.Equals(value) || chip.LinkR.Equals(value) || value == null;
        }
        public bool IsTurnToPlay(int turn, int playerOrder)
        {
            return turn % NumPlayers == playerOrder;
        }
        public bool IsFinal(List<Player<T>> players)
        {
            bool isFinal = false;
            foreach (var condition in EndConditions)
            {
                isFinal = isFinal || condition.IsFinal(players);
            }

            return isFinal;
        }
        public bool IsTie(List<Player<T>> players, out List<Player<T>>? winners)
        {
            List<Player<T>> playersWinners = new();
            if (IsFinal(players))
            {
                foreach (var player in players)
                {
                    foreach (var condition in WinConditions)
                    {
                        if (condition.IsWinner(player, players)) playersWinners.Add(player);
                    }
                }
                winners = playersWinners;
                if (playersWinners.Count == 1)
                {
                    return false;
                }
                return true;
            }
            winners = default(List<Player<T>>);
            return false;
        }
        public List<Chip<T>> GenerateChips(int cant, List<IValue<T>> values)
        {
            List<Chip<T>> Chips = new List<Chip<T>>();
            for (int i = 0; i < cant; i++)
            {
                for (int j = 1; j < cant; j++)
                {
                    Chips.Add(new Chip<T>(values[i], values[j]));
                }
            }
            return Chips;
        }
        public void SetNumPlayers(int n)
        {
            this.NumPlayers = n;
        }

        public bool IsWinner(List<Player<T>> players, out Player<T>? player)
        {
            List<Player<T>> playersWinners = new List<Player<T>>();

            if (IsTie(players, out playersWinners))
            {
                player = default(Player<T>);
                return false;
            }
            if (playersWinners == null)
            {
                player = default(Player<T>);
                return false;
            }
            else if (playersWinners.Count == 1)
            {
                player = playersWinners[0];
                return true;
            }
            player = default(Player<T>);
            return false;
        }
    }
}