using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine.obj
{
    public class Team<TValue,T> where TValue : IValue<T>
    {
        Player<TValue,T>[] Players;

        public Team(params Player<TValue, T> players)
        {
            Players = players;
        }
        
    }
}