using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DominoEngine
{
    public static class EndConditions<TValue> where TValue:IEquatable<TValue>
    {
        public static bool IsLocked(List<Player<TValue>> players)
        {
            foreach (var item in players)
            {
                if(!item.step) return false;
            }
            
            return true;
        } 
    }
}