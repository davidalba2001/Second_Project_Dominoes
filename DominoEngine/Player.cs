using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public abstract class Player<TValue> where TValue : IEquatable<TValue>
    {
        public int PlayerOrder { get; protected set; }
        public bool step =  false;
        public string Name { get; protected set; }
        protected List<Chip<TValue>> HandChip;
        public int NumChips {get{return HandChip.Count;}}
        protected Player(int playerOrder, string name)
        {
            this.PlayerOrder = playerOrder;
            Name = name;    
            HandChip = new List<Chip<TValue>>();
        }
        public abstract void TakeHandChip(List<Chip<TValue>> HandChip);
        public abstract (Chip<TValue>, string)? Play(LinkedList<TValue> board, Rules<TValue> Rules);
        
    }
    
    public class HumanPlayer<TValue> : Player<TValue> where TValue : IEquatable<TValue>
    {
        public HumanPlayer(int playerOrder, string name) : base(playerOrder, name){}
        
        /// <summary>
        /// Take the hand chip
        /// </summary>
        
        override public void TakeHandChip(List<Chip<TValue>> HandChip)
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
         
        public List<Chip<TValue>> CanPlay(TValue value, Rules<TValue> Rules)
        {
            List<Chip<TValue>> chips  = new List<Chip<TValue>>();
            foreach (var chip in HandChip)
            {
                if(Rules.PlayIsValid(chip,value))
                {
                    chips.Add(chip);
                }
            }
            return chips;
        }

        public override (Chip<TValue>, string)? Play(LinkedList<TValue> board, Rules<TValue> Rules)
        {
            var MuvesInRigth = CanPlay(board.Last.Value, Rules);
            var MuvesInLeft = CanPlay(board.First.Value, Rules);
            if(MuvesInLeft.Count==0 && MuvesInRigth.Count == 0)
            {
                this.step = true;
                return null;
            }
            else this.step = false;
            string? Side;
            int pos = 0;
            Chip<TValue> Muve = this.HandChip[0];
            do{
                System.Console.WriteLine("Chouse a number between 0 and "+ this.HandChip.Count+"dependig of the position of the chip you wanna play");
                pos = int.Parse(Console.ReadLine());
                Muve = GetChipInPos(pos);
            }while(!Rules.PlayIsValid(Muve, board.Last.Value)&&!Rules.PlayIsValid(Muve,board.First.Value));
            Muve = PlayChip(pos);
            if(!Rules.PlayIsValid(Muve, board.Last.Value)) return (Muve, "l");
            if(!Rules.PlayIsValid(Muve, board.First.Value)) return (Muve, "r");
            do 
            {   
                System.Console.WriteLine("Write <l> if you wanna paly in the Left side or <r> in the Rigth side");
                Side = Console.ReadLine();
            }while(Side != "l" || Side != "r");
            return (Muve,Side);
        }
    }
}