using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public class Board<T>
    {
        private LinkedList<IValue<T>> BoardChips = new LinkedList<IValue<T>>();
        public LinkedList<IValue<T>> GetBoard()
        {
            return BoardChips;
        }

        public int CountChip => BoardChips.Count;
        public IValue<T> GetLinkR()
        {
            return BoardChips.LastOrDefault();
        }
        public IValue<T> GetLinkL()
        {
            return BoardChips.FirstOrDefault();
        }
        public void AddChip((Chip<T>, IValue<T>?)? chip)
        {
            if (CountChip == 0)
            {
                BoardChips.AddLast(chip!.Value.Item1.LinkL);
                BoardChips.AddLast(chip!.Value.Item1.LinkR);
            }
            else
            {
                if (chip.Value.Item2.Equals(GetLinkR()))
                {
                    if (chip!.Value.Item1.LinkL!.Equals(GetLinkR()))
                    {
                        BoardChips.AddLast(chip!.Value.Item1.LinkL);
                        BoardChips.AddLast(chip!.Value.Item1.LinkR);
                    }
                    else if (chip!.Value.Item1.LinkR!.Equals(GetLinkR()))
                    {
                        BoardChips.AddLast(chip!.Value.Item1.LinkR);
                        BoardChips.AddLast(chip!.Value.Item1.LinkL);
                    }
                }
                else if (chip.Value.Item2.Equals(GetLinkL()))
                {
                    if (chip!.Value.Item1.LinkL!.Equals(GetLinkL()))
                    {
                        BoardChips.AddFirst(chip!.Value.Item1.LinkL);
                        BoardChips.AddFirst(chip!.Value.Item1.LinkR);
                    }
                    else if (chip!.Value.Item1.LinkR!.Equals(GetLinkL()))
                    {
                        BoardChips.AddFirst(chip!.Value.Item1.LinkR);
                        BoardChips.AddFirst(chip!.Value.Item1.LinkL);
                    }
                }

            }
        }
    }
}