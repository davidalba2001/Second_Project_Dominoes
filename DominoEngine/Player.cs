using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;

namespace DominoEngine
{
    public class Player<TValue, T> where TValue : IValue<T>
    {
        protected IStrategy<TValue, T> Strategy; // Estrategia que implemeta el jugador
        public bool Pass { get; set; }  // Estado del jugador si se paso o no
        public string Name { get; protected set; } // Identificador del jugador
        protected List<Chip<TValue, T>> HandChip; // Lista de fichas que tiene el jugador en la mano
        public int PlayerOrder { get; protected set; }// Orden del jugador ejemplo 3er jugador
        public int NumChips { get { return HandChip.Count; } }// Numero de fichas
        //Constructor 
        public Player(string name, int playerOrder, IStrategy<TValue, T> strategy)
        {
            Name = name;
            this.PlayerOrder = playerOrder;
            Strategy = strategy;
        }
        // Se le pasa la mano al jugador
        public void TakeHandChip(List<Chip<TValue, T>> HandChip)
        {
            this.HandChip = HandChip;
        }
        // Agrega una ficha a la mano
        public void TakeChip(Chip<TValue, T> chip)
        {
            this.HandChip.Add(chip);
        }
        // Devuelve la mano
        public List<Chip<TValue, T>> GetHand()
        {
            return this.HandChip;
        }
         // Devuelve la ficha en la posición que se le entra
        public Chip<TValue, T> GetChipInPos(int pos)
        {
            return HandChip[pos];
        }
        // Elimina una ficha de la mano
        public void PlayChip(Chip<TValue, T> chip)
        {
            HandChip.Remove(chip);
        }
        // Determina si el jugador tiene fichas en la mano con las que pueda jugar
        public bool CanPlay(Board<TValue, T> board, Rules<TValue, T> rules)
        {
            foreach (var chip in HandChip)
            {
                if (rules.PlayIsValid(chip, board.GetLinkL) || rules.PlayIsValid(chip, board.GetLinkR))
                {
                    return true;
                }
            }
            return false;
        }
        // Devuelve una lista con todas las posibles fichas que el jugador puede poner
        public List<Chip<TValue, T>> GetValidPlay(TValue value, Rules<TValue, T> rules)
        {
            List<Chip<TValue, T>> chips = new List<Chip<TValue, T>>();
            foreach (var chip in HandChip)
            {
                if (rules.PlayIsValid(chip, value))
                {
                    chips.Add(chip);
                }
            }
            return chips;
        }
         // Encapsula la estrategia del jugador
        public bool NextPlay(Board<TValue, T> board, Rules<TValue, T> rules, out (Chip<TValue, T>, TValue) move)
        {
            return Strategy.ValidMove(this,board, rules, out move);

        }
    }
}