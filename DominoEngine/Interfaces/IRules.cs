using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine.Interfaces
{
     public interface IRules<T>
    {
        int NumPlayers { get; }
        int NumChips { get; }

        List<Chip<T>> GenerateChips(int cant, List<IValue<T>> values);
        bool IsFinal(List<Player<T>> players);
        bool IsTie(List<Player<T>> players, out List<Player<T>>? winners);
        bool IsTurnToPlay(int turn, int playerOrder);
        bool IsWinner(List<Player<T>> players, out Player<T> player);
        bool PlayIsValid(Chip<T> chip, IValue<T>? value);
        void SetNumPlayers(int n);
    }
}