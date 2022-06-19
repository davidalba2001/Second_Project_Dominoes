using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public class Board<TValue> where TValue :IEquatable<TValue>
    {
       private LinkedList<TValue> BoardChips = new LinkedList<TValue>();
       public LinkedList<TValue> GetBoard()
       {
            return BoardChips;
       }
       public void AddChip(Chip<TValue> chip, TValue value)
       {
            if(BoardChips.Count == 0)
            {
                BoardChips.AddLast(chip.LinkL);
                BoardChips.AddLast(chip.LinkR);
            }
            else if(BoardChips.First!.Value.Equals(chip.LinkR))
            {
                BoardChips.AddFirst(chip.LinkR);
                BoardChips.AddFirst(chip.LinkL);
            }
            else if(BoardChips.First.Value.Equals(chip.LinkL))
            {
                BoardChips.AddFirst(chip.LinkL);
                BoardChips.AddFirst(chip.LinkR);
            }
            else if(BoardChips.Last!.Value.Equals(chip.LinkR))
            {
                BoardChips.AddLast(chip.LinkR);
                BoardChips.AddLast(chip.LinkL);
            }
            else if(BoardChips.First.Value.Equals(chip.LinkL))
            {
                BoardChips.AddLast(chip.LinkL);
                BoardChips.AddLast(chip.LinkR);
            }
       }
        
    }
}