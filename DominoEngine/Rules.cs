using DominoEngine.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;


namespace DominoEngine
{
    public class Rules<TValue, T> where TValue : IValue<T>
    {
        //Colección de condiciones para determinar un Ganador
        ICollection<IWinCondition<TValue, T>> WinConditions;
 
        //Colección de condiciones para determinar si hay fin del juego
        ICollection<IEndCondition<TValue, T>> FinalCondition;
        
        // Constructor
        public Rules(ICollection<IWinCondition<TValue, T>> winConditions, ICollection<IEndCondition<TValue, T>> finalCondition)
        {
            WinConditions = winConditions;
            FinalCondition = finalCondition;
        }
        //Determina si una jugada es valida se le pasa una ficha y una cara y devuelve true si esa ficha se puede jugar
        public bool PlayIsValid(Chip<TValue, T> chip, TValue value)
        {
            return chip.LinkL.Equals(value) || chip.LinkR.Equals(value) || value == null;
        }
        // Se usa para verificar si es el turno de un jugador
        public bool IsTurnToPlay(int turn, int NumPlayers, int playerOrder)
        {
            return turn % NumPlayers == playerOrder;
        }
        // Generador de fichas de domino se le pasa la cantidad de caras y genera las fichas ejemplo construye las fichas para la variante doble 9
        public List<Chip<TValue, T>> GenerateChips(int cant, TValue[] values)
        {
            List<Chip<TValue, T>> Chips = new List<Chip<TValue, T>>();
            for (int i = 0; i < cant; i++)
            {
                for (int j = i; j < cant; j++)
                {
                    Chips.Add(new Chip<TValue, T>(values[i], values[j]));
                }
            }
            return Chips;
        }
        // Evalúa todas las condiciones de finalización y determina si se cumple alguna
        public bool IsFinal(List<Player<TValue, T>> players)
        {
            bool isFinal = false;
            foreach (var finalCondition in FinalCondition)
            {
                isFinal = isFinal || finalCondition.IsFinal(players);
            }
            return isFinal;
        }
        // Determina si hay un empate determinado si existen mas de un jugador que cumplen las condiciones de ganador
        public bool IsTie(List<Player<TValue, T>> players, out  List<Player<TValue, T>> winners)
        {
            List<Player<TValue, T>> playersWinners = new();
            foreach (var winCondition in WinConditions)
            {
                foreach (var player in players)
                {
                    if (winCondition.IsWinner(player, players))
                    {
                        if (!playersWinners.Contains(player))
                        {
                            playersWinners.Add(player);
                        }
                    }
                }
            }
            if(playersWinners.Count > 1)
            {
                winners = playersWinners;
                return true;
            }
            winners = playersWinners;
            return false;
        }
        // determina si un jugador gano
        public bool IsWinner(List<Player<TValue, T>> players, out Player<TValue, T> player)
        {
            List<Player<TValue, T>> playerWin;
            if(!IsTie(players,out playerWin))
            {
                if(playerWin.Count == 1)
                {
                    player = playerWin[0];
                    return true;
                }
            }
            player = default(Player<TValue, T>);
            return false;
        }
    }
}