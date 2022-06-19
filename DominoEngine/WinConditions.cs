using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public class WinConditions<TValue> where TValue:IEquatable<TValue>
    {
        public static bool PlayAllChips(List<Player<TValue>> players ,out Player<TValue> winner)
        {
            foreach (var item in players)
            {
                if(item.NumChips == 0)
                { 
                    winner = item;
                    return true;
                }
            }
            winner = null;
            return false;
        }
        //public static bool WinnerScore(List<Player<TValue>> players,out Player<TValue> winner)
        //{
        //    foreach (var item in player)
        //    {
        //        
        //    }
        //}
        public static bool WinnerForChips(List<Player<TValue>> players,out Player<TValue> winner)
        {
            Player<TValue> temp = players[0];
            int minChips = players[0].NumChips;
            bool flag = false;

            for(int i = 1;i < players.Count;i++ )
            {
                if(minChips > players[i].NumChips)
                {
                    temp = players[i];
                    minChips = players[i].NumChips;
                    flag = true;
                }
            }
            if(flag)
            {
                winner = temp;
                return true;
            }
            winner = null;
            return false; 
        }

    }
}