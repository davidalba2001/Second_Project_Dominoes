using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public abstract class Player<TValue> where TValue : IEquatable<TValue>
    {
        public int playerOrder { get; protected set; }
        public string Name { get; protected set; }
        protected List<Chip<TValue>> HandChip;
        public int NumChips {get{return HandChip.Count;}}
        protected Player(int playerOrder, string name)
        {
            this.playerOrder = playerOrder;
            Name = name;    
            HandChip = new List<Chip<TValue>>();
        }
    }

    public class HumanPlayer<TValue> : Player<TValue> where TValue : IEquatable<TValue>
    {
        public HumanPlayer(int playerOrder, string name) : base(playerOrder, name){}
        
        /// <summary>
        /// Take the hand chip
        /// </summary>
        
        public void TakeHandChip(List<Chip<TValue>> HandChip)
        {
            this.HandChip = HandChip;
        }

        /// <summary>
        /// return a chip in the chosen posision
        /// </summary>
        public Chip<TValue> GetChipInPos(int pos)
        {
            return this.HandChip[pos];
        }

        /// <summary>
        /// return a chip in the chosen posision and remove it from the hand
        /// </summary>
        public Chip<TValue> PlayChip(int pos)
        {
            Chip<TValue> chip = this.HandChip[pos];
            this.HandChip.RemoveAt(pos);
            return chip;
        }


    }
}