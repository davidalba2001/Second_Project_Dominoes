using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public class Rules<TValue> where TValue:IEquatable<TValue>
    {
        public int NumPlayers {get;}
        public int NumChips {get;}
        public bool PlayIsValid(Chip<TValue> chip,TValue value)
        {
            return chip.LinkL.Equals(value)|| chip.LinkR.Equals(value);
        }
        public bool IsTurnToPlay(int turn,int playerOrder)
        {
            return turn% NumPlayers == playerOrder;
        }
        public bool IsFinal()
        {
            return true;
        }
    }
}