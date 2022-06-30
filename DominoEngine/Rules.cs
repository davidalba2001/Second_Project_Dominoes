using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DominoEngine
{
    public class Rules<T>
    {

        private delegate bool WinCondition(List<Player<T>> players, out Player<T>? player);
        private delegate bool LockConditions(List<Player<T>> players);

        public int NumPlayers { get; private set; }
        public int NumChips { get; }
        public bool PlayIsValid(Chip<T> chip, IValue<T> value)
        {
            return chip.LinkL.Equals(value) || chip.LinkR.Equals(value)|| value == null;
        }
        public bool IsTurnToPlay(int turn, int playerOrder)
        {
            return turn % NumPlayers == playerOrder;
        }
        public bool IsFinal(List<Player<T>> players)
        {
            LockConditions locked = new LockConditions(EndConditions<T>.IsLocked);
            return locked(players);
        }
        public bool IsWinner(List<Player<T>> players, out Player<T>? player)
        {
            WinCondition playAllChips = new WinCondition(WinConditions<T>.PlayAllChips);
            WinCondition winForChips = new WinCondition(WinConditions<T>.WinnerForChips);
            if (playAllChips(players, out player) || IsFinal(players))
            {
                if (player != null) return true;
                else
                {
                    return winForChips(players, out player);
                }
            }
            else return false;
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
    }
}