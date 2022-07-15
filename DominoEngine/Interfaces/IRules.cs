using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine.Interfaces
{
    public interface IRules<TValue, T> where TValue : IValue<T>
    {
        List<Chip<TValue, T>> GenerateChips(int cant, TValue[] values);
        bool IsFinal(List<Player<TValue,T>> players);
        bool IsTie(List<Player<TValue,T>> players, out List<Player<TValue,T>>? winners);
        bool IsTurnToPlay(int turn,int NumPlayers, int playerOrder);
        bool IsWinner(List<Player<TValue,T>> players, out Player<TValue,T> player);
        bool PlayIsValid(Chip<TValue, T> chip,TValue value);
    }
}