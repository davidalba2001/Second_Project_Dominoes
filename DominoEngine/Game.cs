using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public class Game
    {
        public Board<Number> table {get;}
        private List<Player<Number>> players;
        private Rules<Number> gameRules;
        public List<Chip<Number>> Chips {get;}
        public Player<Number>? Winner {get;}
        public int Turn {get;}



        public Game()
        {
            this.table = new Board<Number>();
            this.gameRules = new Rules<Number>();
            this.Chips = gameRules.GetFichas();
            this.Turn = 1;
        }

        public void AddPlayers(List<Player<Number>> Players)
        {
            this.players = Players;
            gameRules.SetNumPlayers(Players.Count);
        }
        public void GiveChips()
        {
            Random var = new Random(); 
            List<Chip<Number>> Randomized = this.Chips.OrderBy(Item => var.Next()).ToList<Chip<Number>>();
            for(int i =0, j = 0; i < players.Count; i++)
            {
                List<Chip<Number>> PlayerHand = new List<Chip<Number>>();
                for (int n = 0; n < 10 ; n++)
                {
                    PlayerHand.Add(Randomized[j++]);
                }
                players[i].TakeHandChip(PlayerHand); 
            }
        }
        public void CurrentTurn()
        {
            Player<Number> CurrentPlayer = players[0];
            foreach(var item in this.players)
            {
                if(this.gameRules.IsTurnToPlay(this.Turn, item.PlayerOrder)) CurrentPlayer = item; 
            }
            (Chip<Number>, string)? PlayerMuve = CurrentPlayer.Play(table.GetBoard(), gameRules);
            if(PlayerMuve != null)
            {
                Number side = table.GetBoard().First.Value;
                if(PlayerMuve.Value.Item2 == "l") side=table.GetBoard().First.Value;
                if(PlayerMuve.Value.Item2 == "r") side=table.GetBoard().Last.Value;
                table.AddChip(PlayerMuve.Value.Item1, side);               
            }

        }
    }
}