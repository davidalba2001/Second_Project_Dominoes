using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;

namespace DominoEngine
{

    public class Player<T>
    {
        protected IStrategy<T> Strategy;
        public bool Pass { get; set; }
        public string Name { get; protected set; }
        protected List<Chip<T>> HandChip;
        public int PlayerOrder { get; protected set; }
        public int NumChips { get { return HandChip.Count; } }
        public Player( string name,int playerOrder, IStrategy<T> strategy)
        {
            Name = name;
            this.PlayerOrder = playerOrder;
            Strategy = strategy;
        }
        public void TakeHandChip(List<Chip<T>> HandChip)
        {
            this.HandChip = HandChip;
        }
        public List<Chip<T>> GetHand()
        {
            return this.HandChip;
        }
        public Chip<T> GetChipInPos(int pos)
        {
            return HandChip[pos];
        }
        public void PlayChip(Chip<T> chip)
        {
            HandChip.Remove(chip);
        }
        public bool CanPlay(Board<T> board, IRules<T> rules)
        {
            foreach (var chip in HandChip)
            {
                if (rules.PlayIsValid(chip, board.GetLinkL) || rules.PlayIsValid(chip, board.GetLinkR))
                {
                    return true;
                }
            }
            return false;
        }
        public List<Chip<T>> GetValidPlay(IValue<T> value, IRules<T> rules)
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
        public bool NextPlay(Player<T> player, Board<T> board, IRules<T> rules, out (Chip<T>, IValue<T>) move)
        {
            return Strategy.ValidMove(player, board, rules, out move);

        }
    }
}