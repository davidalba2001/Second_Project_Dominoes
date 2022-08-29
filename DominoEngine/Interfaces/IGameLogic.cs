using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine.Interfaces
{
    public interface IGameLogic<TValue,T> where TValue:IValue<T>
    {
        // Turn guarda la cantidad de turnos desde el comienzo de la partida
        int Turn { get; }
        // board es la mesa de donde se juegan las fichas
        Board<TValue, T> board { get; }
        List<Player<TValue, T>> Players { get; }
        // Rules guarda las reglas basicas del juego
        Rules<TValue, T> Rules { get; }
        List<Chip<TValue, T>> Chips { get; }
        // CurrentPlayer guarda la referencia del jugador que tiene que jugar en el turnto dado
        Player<TValue, T>? CurrentPlayer { get; }
        
        // Este metodo actualiza a CurrentPlayer
        void ChangeValidCurrentPlayer();
        
        // Este metodo hace que el jugador que le toque juegue mientras pueda jugar
        void CurrentTurn();

        // Dvuelve True si el juego termó, false en el caso contrario
        bool EndGame();
        
        // Reparte las fichas a los jugadores
        void HandOutChips(int CountChip);
    }
}