using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DominoEngine
{
    public class Rules<TValue> where TValue : IEquatable<TValue>
    {

        private delegate bool WinCondition(List<Player<TValue>> players, out Player<TValue> player);
        private delegate bool LockConditions(List<Player<TValue>> players);

        public int NumPlayers { get; private set;}
        public int NumChips { get; }
        public bool PlayIsValid(Chip<TValue> chip, TValue value)
        {
            return chip.LinkL.Equals(value) || chip.LinkR.Equals(value);
        }
        public bool IsTurnToPlay(int turn, int playerOrder)
        {
            return turn % NumPlayers == playerOrder;
        }
        public bool IsFinal(List<Player<TValue>> players)
        {
            LockConditions locked = new LockConditions(EndConditions<TValue>.IsLocked);
            return locked(players);
        }

        public bool IsWinner(List<Player<TValue>> players, out Player<TValue> player)
        {
            WinCondition playAllChips = new WinCondition(WinConditions<TValue>.PlayAllChips);
            WinCondition winForChips  = new WinCondition(WinConditions<TValue>.WinnerForChips);
            if (playAllChips(players,out player)||IsFinal(players))
            {
                if(player != null) return true;
                else
                {
                    return winForChips(players,out player);
                }
            }
            else return false;
        }
        public List<Chip<Number>> GetFichas()
        {
            List<Chip<Number>> Chips = new List<Chip<Number>>();
            for(int i = 0; i<10; i++)
            {
                for(int j=1; j<10; j++)
                {
                    Chips.Add(new Chip<Number>(new Number(i),new Number(j)));
                }
            }
            return Chips;
        }
        public void SetNumPlayers(int N)
        {
            this.NumPlayers = N;
        }
    }
}