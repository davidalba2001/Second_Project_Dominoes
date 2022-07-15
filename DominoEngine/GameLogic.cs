using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;



namespace DominoEngine
{


    public class ClassicGameLogic<TValue, T> : IGameLogic<TValue, T> where TValue : IValue<T>
    {
        public int Turn { get; private set; }
        //TODO: No Se que tan necesario sea saber el numero de fichas;
        // public int NumChip { get; private set; }
        public Board<TValue, T> board { get; }
        public List<Player<TValue, T>> Players { get; private set; }
        public IRules<TValue, T> Rules { get; }
        public List<Chip<TValue, T>> Chips { get; }
        public List<Player<TValue, T>>? Winners { get; private set; }
        public Player<TValue, T>? CurrentPlayer { get; private set; }
        public ClassicGameLogic(int countLinkedValues, TValue[] linkedValues, List<Player<TValue, T>> players)
        {
            Turn = 0;
            board = new Board<TValue, T>();
            Players = players;
            Rules = new ClassicRules<TValue, T>();
            Chips = Rules.GenerateChips(countLinkedValues, linkedValues);
            this.CurrentPlayer = Players[0];
        }
        public void HandOutChips(int CountChip)
        {
            //TODO:corregir los indices porque pueden haber mas fichas a asignar que las disponibles
            Random var = new Random();
            List<Chip<TValue, T>> Randomized = Chips.OrderBy(Item => var.Next()).ToList<Chip<TValue, T>>();
            for (int i = 0; i < Players.Count; i++)
            {
                List<Chip<TValue, T>> PlayerHand = new List<Chip<TValue, T>>();
                for (int n = 0, j = 0; n < CountChip; n++)
                {
                    PlayerHand.Add(Randomized[i * CountChip + j++]);
                }
                Players[i].TakeHandChip(PlayerHand);
            }
        }
        // Busca el proximo juguador valido y actualiza el current player y los turnos
        public void ChangeValidCurrentPlayer()
        {
            Player<TValue, T> currentPlayer;
            for (int i = 0; i < Players.Count; i++)
            {
                foreach (var player in Players)
                {
                    if (Rules.IsTurnToPlay(Turn, Players.Count, player.PlayerOrder))
                    {
                        currentPlayer = player;
                        if (player.CanPlay(board, Rules))
                        {
                            CurrentPlayer = currentPlayer;
                            player.Pass = false;
                            return;
                        }
                        else
                        {
                            player.Pass = true;
                            break;
                        }
                    }
                }
                Turn++;
            }
            CurrentPlayer = null;
        }
        public void CurrentTurn()
        {
            bool canPlay = CurrentPlayer.NextPlay(CurrentPlayer, board, Rules, out (Chip<TValue, T>, TValue) playerMove);
            if (canPlay)
            {
                board.AddChip(playerMove);
                CurrentPlayer.PlayChip(playerMove.Item1);
                Turn++;
            }
        }
        public bool EndGame()
        {
            bool isFinal = false;
            List<Player<TValue, T>>? playersWinners;
            if (Rules.IsTie(Players, out playersWinners))
            {
                isFinal = true;
            }
            if (Rules.IsWinner(Players, out var __))
            {
                isFinal = true;
            }
            Winners = playersWinners;
            return isFinal;
        }
    }
}