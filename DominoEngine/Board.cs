using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;

namespace DominoEngine
{
    public class Board<TValue,T> where TValue:IValue<T>
    {
        //Esta LinkedList hace funcion de mesa guardando las fichas jugadas en el orden en que se juegan
        private LinkedList<TValue> BoardChips = new LinkedList<TValue>(); 
        public List<TValue> GetBoard()
        {
            return BoardChips.ToList();
        }
        public int CountChip => BoardChips.Count;
        // GetLinkR y GetLinkL se usan para acceder mas facil a las caras enlazables de la mesa
        public TValue? GetLinkR => BoardChips.LastOrDefault();
        public TValue? GetLinkL => BoardChips.FirstOrDefault();
        // Este metodo agrega la ficha jugada a la mesa, dejando la cara correcta libre para enlazar
        public void AddChip((Chip<TValue,T>,TValue) move)
        {
            if (CountChip == 0)
            {
                BoardChips.AddLast(move.Item1.LinkL);
                BoardChips.AddLast(move.Item1.LinkR);
            }
            else
            {
                if (move.Item2.Equals(GetLinkR))
                {
                    if (move.Item1.LinkL.Equals(GetLinkR))
                    {
                        BoardChips.AddLast(move.Item1.LinkL);
                        BoardChips.AddLast(move.Item1.LinkR);
                    }
                    else if (move.Item1.LinkR.Equals(GetLinkR))
                    {
                        BoardChips.AddLast(move.Item1.LinkR);
                        BoardChips.AddLast(move.Item1.LinkL);
                    }
                }
                else if (move.Item2.Equals(GetLinkL))
                {
                    if (move.Item1.LinkL.Equals(GetLinkL))
                    {
                        BoardChips.AddFirst(move.Item1.LinkL);
                        BoardChips.AddFirst(move.Item1.LinkR);
                    }
                    else if (move.Item1.LinkR!.Equals(GetLinkL))
                    {
                        BoardChips.AddFirst(move.Item1.LinkR);
                        BoardChips.AddFirst(move.Item1.LinkL);
                    }
                }

            }

        }
    }
}