using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public abstract class Player<T>
    {
        protected delegate (Chip<T>, IValue<T>? value)? Strategy(Player<T> player, Board<T> board, Rules<T> rules,out bool canPlay);
        public int PlayerOrder { get; protected set; }
        public bool step = false;
        public string Name { get; protected set; }
        protected List<Chip<T>> HandChip;
        public int NumChips { get { return HandChip.Count; } }
        protected Player(int playerOrder, string name)
        {
            this.PlayerOrder = playerOrder;
            Name = name;
            HandChip = new List<Chip<T>>();
        }
        public abstract void TakeHandChip(List<Chip<T>> HandChip);
        public abstract (Chip<T>, IValue<T>? value)? NextPlay(Player<T> player, Board<T> board, Rules<T> rules,out bool canPlay);
        public abstract List<Chip<T>> GetHand();
        public abstract Chip<T> GetChipInPos(int pos);
        public abstract void PlayChip(Chip<T> chip);
        public abstract bool CanPlay(Board<T> board, Rules<T> Rules);
        public abstract List<Chip<T>> GetValidPlay(IValue<T> value, Rules<T> rules);
    }
    public class HumanPlayer<T> : Player<T>
    {
        public HumanPlayer(int playerOrder, string name) : base(playerOrder, name) { }
        public override void TakeHandChip(List<Chip<T>> HandChip)
        {
            this.HandChip = HandChip;
        }
        public override List<Chip<T>> GetHand()
        {
            return this.HandChip;
        }
        public override Chip<T> GetChipInPos(int pos)
        {
            return HandChip[pos];
        }
        public override void PlayChip(Chip<T> chip)
        {
            HandChip.Remove(chip);
        }
        
        public override bool CanPlay(Board<T> board,Rules<T> rules)
        {
            foreach (var chip in HandChip)
            {
                if (rules.PlayIsValid(chip,board.GetLinkL())||rules.PlayIsValid(chip,board.GetLinkR()))
                {
                    return true;
                }
            }
            return false;
        }
        public override List<Chip<T>> GetValidPlay(IValue<T> value, Rules<T> rules)
        {
            List<Chip<T>> chips = new List<Chip<T>>();
            foreach (var chip in HandChip)
            {
                if (rules.PlayIsValid(chip, value))
                {
                    chips.Add(chip);
                }
            }
            return chips;
        }
        public override (Chip<T>,IValue<T>? value)? NextPlay(Player<T> player, Board<T> board, Rules<T> rules,out bool canPlay)
        {
            Strategy playStrategy = new Strategy(Strategies<T>.Play);
            var chip = playStrategy(player, board, rules,out canPlay);
            if (chip == null) return null;
            else PlayChip(chip.Value.Item1);
            return chip;
        }
    }
}