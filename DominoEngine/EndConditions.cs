using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DominoEngine
{
    public static class EndConditions<T>
    {
        public static bool IsLocked(List<Player<T>> players)
        {
            foreach (var item in players)
            {
                if(!item.step) return false;
            }
            
            return true;
        } 
    }
}