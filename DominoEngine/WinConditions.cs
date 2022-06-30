using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public class WinConditions<IValue>
    {
        public static bool PlayAllChips(List<Player<IValue>> players ,out Player<IValue>? winner)
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
        public static bool WinnerForChips(List<Player<IValue>> players,out Player<IValue>? winner)
        {
            Player<IValue> temp = players[0];
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