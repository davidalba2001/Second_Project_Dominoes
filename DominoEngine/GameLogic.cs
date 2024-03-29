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
        public Rules<TValue, T> Rules { get; }
        public List<Chip<TValue, T>> Chips { get; }
        public Player<TValue, T>? CurrentPlayer { get; private set; }
        public ClassicGameLogic(int countLinkedValues, Rules<TValue, T> rules, TValue[] linkedValues, List<Player<TValue, T>> players)
        {
            Turn = 0;
            board = new Board<TValue, T>();
            Players = players;
            Rules = rules;
            Chips = Rules.GenerateChips(countLinkedValues, linkedValues);
            this.CurrentPlayer = Players[0];
        }
        public void HandOutChips(int CountChip)
        {
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
            bool canPlay = CurrentPlayer.NextPlay(board, Rules, out (Chip<TValue, T>, TValue) playerMove);
            if (canPlay)
            {
                board.AddChip(playerMove);
                CurrentPlayer.PlayChip(playerMove.Item1);
                Turn++;
            }
        }
        public bool EndGame()
        {
            return Rules.IsFinal(Players);
        }
    }

    public class StolenLogic<TValue, T> : IGameLogic<TValue, T> where TValue : IValue<T>
    {
        public int Turn { get; private set; }
        public Board<TValue, T> board { get; private set; }
        public List<Player<TValue, T>> Players { get; private set; }
        public Rules<TValue, T> Rules { get; private set; }
        public List<Chip<TValue, T>> Chips { get; private set; }
        public Player<TValue, T>? CurrentPlayer { get; private set; }

        public StolenLogic(int countLinkedValues, Rules<TValue, T> rules, TValue[] linkedValues, List<Player<TValue, T>> players)
        {
            Turn = 0;
            board = new Board<TValue, T>();
            Players = players;
            Rules = rules;
            Chips = Rules.GenerateChips(countLinkedValues, linkedValues);
            this.CurrentPlayer = Players[0];
        }
        // Al igual que en el clasico, busca el jugador que le toca jugar, y en caso de no llevar
        // roba fichas hasta que lleve
        public void ChangeValidCurrentPlayer()
        {
            foreach (var player in Players)
            {
                if (Rules.IsTurnToPlay(Turn, Players.Count, player.PlayerOrder))
                {
                    CurrentPlayer = player;
                    while(!CurrentPlayer.CanPlay(board, Rules))
                    {
                        if(Chips.Count==0) break;
                        CurrentPlayer.TakeChip(Chips[0]);
                        Chips.Remove(Chips[0]);
                    }
                }
            }
        }
       
        public void CurrentTurn()
        {
            (Chip<TValue, T>, TValue) move = new();
             // Si luego de que no queden fichas por robar, aun no puede jugar, juega el otro jugador
            if(!CurrentPlayer.NextPlay(board, Rules, out move)&&(Chips.Count == 0))
            {
                CurrentPlayer.Pass = true;
                Turn++;
                return;
            }
            CurrentPlayer.PlayChip(move.Item1);
            board.AddChip(move);
            Turn++;
        }

        public bool EndGame()
        {
            return Rules.IsFinal(Players);
        }

        public void HandOutChips(int CountChip)
        {
            int Last = 0;
            Random RDM = new Random();
            List<Chip<TValue, T>> Randomized = Chips.OrderBy(Item => RDM.Next()).ToList<Chip<TValue, T>>();
            for (int i = 0; i < Players.Count; i++)
            {
                List<Chip<TValue, T>> PlayerHand = new();
                for (int n = 0, j = 0; n < CountChip; n++)
                {
                    Last = i * CountChip + j;
                    PlayerHand.Add(Randomized[i * CountChip + j++]);
                }
                Players[i].TakeHandChip(PlayerHand);    
            }
            Chips = After(Randomized, Last);
        }
        private List<Chip<TValue,T>> After(List<Chip<TValue,T>> list, int pos)
        {
            List<Chip<TValue,T>> result = new();
            for (int i = pos+1; i < list.Count; i++)
            {
                result.Add(list[i]);
            }
            return result;
        }
    }

}