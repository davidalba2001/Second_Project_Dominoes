using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public class Game
    {
        public Board<Number> table {get;}
        public List<Player<Number>> players{get; private set;}
        public Rules<Number> gameRules{get;}
        public List<Chip<Number>> Chips {get;}
        public Player<Number>? Winner {get;private set;}
        public Player<Number> CurrentPlayer{get; private set;}
        public int Turn {get; private set;}



        public Game()
        {
            this.table = new Board<Number>();
            this.gameRules = new Rules<Number>();
            this.Chips = gameRules.GetFichas();
            this.Turn = 0;
        }

        public void AddPlayers(List<Player<Number>> Players)
        {
            this.players = Players;
            this.CurrentPlayer = players[0];
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
            this.CurrentPlayer = players[0];
            foreach(var item in this.players)
            {
                if(this.gameRules.IsTurnToPlay(this.Turn, item.PlayerOrder)) CurrentPlayer = item; 
            }
            (Chip<Number>, string)? PlayerMuve = CurrentPlayer.Play(table.GetBoard(), gameRules);
            if(PlayerMuve != null)
            {
                Number? side = null;
                if(this.table.GetBoard().Count!=0)
                {
                    side = table.GetBoard().First.Value;
                    if(PlayerMuve.Value.Item2 == "l") side=table.GetBoard().First.Value;
                    if(PlayerMuve.Value.Item2 == "r") side=table.GetBoard().Last.Value;
                }
                table.AddChip(PlayerMuve.Value.Item1, side);               
            }
            this.Turn++;
        }
        public bool EndGame()
        {
            Player<Number> Temp;
            if(gameRules.IsWinner(this.players, out Temp))
            {
                this.Winner = Temp;
                return true;
            }
            else return false;
        }
    }
}