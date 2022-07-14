using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;



namespace DominoEngine
{
    public class GameLogic<T>
    {
        public int Turn { get; private set; }
        public Board<T> board { get; }
        public List<Player<T>> players { get; private set; }
        public IRules<T> Rules { get; }
        public List<Chip<T>> Chips { get; }
        public Player<T>? Winner { get; private set; }
        public Player<T>? CurrentPlayer { get; private set; }

        public GameLogic(int cant, List<IValue<T>> linkedValues, List<Player<T>> players, IRules<T> rules)
        {
            board = new Board<T>();
            Rules = rules;
            //players = new List<Player<T>>();
            Chips = rules.GenerateChips(cant, linkedValues);
            Turn = 0;
            this.players = players;
            this.CurrentPlayer = players[0];
            rules.SetNumPlayers(players.Count);
        }

        public void GiveChips(int cant)
        {
            //corregir los indices porque pueden haber mas fichas a asignar que las disponibles
            Random var = new Random();
            List<Chip<T>> Randomized = Chips.OrderBy(Item => var.Next()).ToList<Chip<T>>();
            for (int i = 0; i < players.Count; i++)
            {
                List<Chip<T>> PlayerHand = new List<Chip<T>>();
                for (int n = 0, j = 0; n < cant; n++)
                {
                    PlayerHand.Add(Randomized[i * cant + j++]);
                }
                players[i].TakeHandChip(PlayerHand);
            }
        }
        // debe ser una interface
        public void ChangeCurrentPlayer()
        {
            Player<T> currentPlayer;
            for (int i = 0; i < players.Count; i++)
            {
                foreach (var player in players)
                {
                    if (Rules.IsTurnToPlay(Turn, player.PlayerOrder))
                    {
                        currentPlayer = player;
                        if (player.CanPlay(board, Rules))
                        {
                            CurrentPlayer = currentPlayer;
                            player.Pass = false;
                            return;
                        }
                        else player.Pass = true;
                    }
                }
                Turn++;
            }
            CurrentPlayer = null;
        }
        public void CurrentTurn()
        {
            bool canPlay = CurrentPlayer.NextPlay(CurrentPlayer, board, Rules, out (Chip<T>,IValue<T>) playerMove);
            if (canPlay)
            {
                board.AddChip(playerMove);
                Turn++;
            }
        }
        public bool EndGame()
        {
            Player<T> Temp;
            if (Rules.IsWinner(this.players, out Temp))
            {
                this.Winner = Temp;
                return true;
            }

            return false;
        }

    }
}