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
        public ClassicGameLogic(int countLinkedValues,Rules<TValue,T> rules, TValue[] linkedValues, List<Player<TValue, T>> players)
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
            return Rules.IsFinal(Players);
        }
    }

     public class TapadinLogic<TValue, T> : IGameLogic<TValue, T> where TValue : IValue<T>
    {

        public int Turn {get; private set;}

        public Board<TValue, T> board {get;}

        public List<Player<TValue, T>> Players {get;}

        public Rules<TValue, T> Rules {get;}

        public List<Chip<TValue, T>> Chips {get;}
        private Dictionary<Player<TValue,T>,List<HidenChip>> HidenChips;

        public Player<TValue, T>? CurrentPlayer {get; private set;}
        public TapadinLogic(int countLinkedValues,Rules<TValue,T> rules, TValue[] linkedValues, List<Player<TValue, T>> players)
        {
            Turn = 0;
            this.board = new();
            Players = players;
            Rules = rules;
            Chips = rules.GenerateChips(countLinkedValues, linkedValues);
        }

        public void ChangeValidCurrentPlayer()
        {
            Player<TValue, T> currentPlayer;
            foreach (var player in Players)
            {
                if (Rules.IsTurnToPlay(Turn, Players.Count, player.PlayerOrder))
                {
                    currentPlayer = player;
                }
            }
            
        }

        public void CurrentTurn()
        {   
            if(NoMoreHidenChips(HidenChips[CurrentPlayer])&&!CurrentPlayer.CanPlay(board, Rules))
            { 
                CurrentPlayer.Pass = true;
                return;
            }
            bool EndTurn = false;
            CurrentPlayer.Pass = false;
            do
            {
                if(!CurrentPlayer.CanPlay(board, Rules))
                {
                    EndTurn = true;
                    CurrentPlayer.TakeHandChip(RefreshHand(HidenChips[CurrentPlayer]));
                }
                else
                {
                    bool canPlay = CurrentPlayer.NextPlay(CurrentPlayer, board, Rules, out (Chip<TValue, T>, TValue) playerMove);
                    if (canPlay)
                    {
                        RemoveChip(HidenChips[CurrentPlayer], playerMove.Item1);
                        board.AddChip(playerMove);
                        CurrentPlayer.PlayChip(playerMove.Item1);
                    }
                }

            }while(CurrentPlayer.CanPlay(board, Rules) || !EndTurn);
            Turn++;    
            
        }

        public bool EndGame()
        {
            return Rules.IsFinal(Players);
        }

        public void HandOutChips(int CountChip)
        {
            Random RDM = new Random();
            List<Chip<TValue, T>> Randomized = Chips.OrderBy(Item => RDM.Next()).ToList<Chip<TValue, T>>();
            for (int i = 0; i < Players.Count; i++)
            {
                List<HidenChip> PlayerHand = new();
                for (int n = 0, j = 0; n < CountChip; n++)
                {
                    PlayerHand.Add(new HidenChip(Randomized[i * CountChip + j++]));
                }
                PlayerHand[0].IsHiden = false;
                Players[i].TakeHandChip(RefreshHand(PlayerHand));
                HidenChips.Add(Players[i],PlayerHand);
                
            }
        }

        private List<Chip<TValue,T>> RefreshHand(List<HidenChip> hidenChips)
        {
            bool IsFirstHidenChip = true;
            List<Chip<TValue,T>> Hand = new();
            foreach(var chip in hidenChips)
            {
                if(chip.IsHiden&&IsFirstHidenChip)
                {
                    chip.IsHiden = false;
                    IsFirstHidenChip = false;
                    Hand.Add(chip.Chip);
                }
                if(chip.IsHiden)
                {
                    Hand.Add(new());
                }
                else Hand.Add(chip.Chip);
            }
            return Hand;
        }
        private void RemoveChip(List<HidenChip> hidenChips, Chip<TValue,T> chip)
        {
            foreach(var item in hidenChips)
            {
                if(item.Chip.Equals(chip))
                {
                    hidenChips.Remove(item);
                    break;
                }
            }
        }
        private bool NoMoreHidenChips(List<HidenChip> chips)
        {
            foreach(var chip in chips)
            {
                if(chip.IsHiden) return false;
            }
            return true;
        }
        private class HidenChip
        {
            public bool IsHiden;
            public Chip<TValue,T> Chip {get;private set;}
            public HidenChip(Chip<TValue, T> chip)
            {
                IsHiden = false;
                Chip = chip;
            }
            
        }
    }
  
   




}