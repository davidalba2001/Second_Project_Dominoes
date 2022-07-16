using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;

namespace DominoEngine
{
    public class Player<TValue, T> where TValue : IValue<T>
    {
        protected IStrategy<TValue,T> Strategy;
        public bool Pass { get; set; }
        public string Name { get; protected set; }
        protected List<Chip<TValue, T>> HandChip;
        public int PlayerOrder { get; protected set; }
        public int NumChips { get { return HandChip.Count; } }
        public Player(string name, int playerOrder, IStrategy<TValue,T> strategy)
        {
            Name = name;
            this.PlayerOrder = playerOrder;
            Strategy = strategy;
        }
        public void TakeHandChip(List<Chip<TValue, T>> HandChip)
        {
            this.HandChip = HandChip;
        }
        public List<Chip<TValue, T>> GetHand()
        {
            return this.HandChip;
        }
        public Chip<TValue, T> GetChipInPos(int pos)
        {
            return HandChip[pos];
        }
        public void PlayChip(Chip<TValue, T> chip)
        {
            HandChip.Remove(chip);
        }
        public bool CanPlay(Board<TValue,T> board,Rules<TValue, T> rules)
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
        public List<Chip<TValue, T>> GetValidPlay(TValue value,Rules<TValue, T> rules)
        {
            List<Chip<TValue, T>> chips = new List<Chip<TValue, T>>();
            foreach (var chip in HandChip)
            {
                if (rules.PlayIsValid(chip, value))
                {
                    chips.Add(chip);
                }
            }
            return chips;
        }
        
        public bool NextPlay(Player<TValue, T> player, Board<TValue,T> board,Rules<TValue, T> rules, out (Chip<TValue, T>, TValue) move)
        {
            return Strategy.ValidMove(player, board, rules, out move);

        }
    }
}