using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DominoEngine.Interfaces
{
    public interface IStrategy<TValue, T> where TValue : IValue<T>
    {
        // Devuelve true si tiene una jugada Valida, y en el parametro de move se devuelve la jugada con la posisión
        public bool ValidMove(Player<TValue, T> player, Board<TValue, T> board, Rules<TValue, T> rules, out (Chip<TValue, T>,TValue) move);
    }


}