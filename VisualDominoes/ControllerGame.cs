using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine;
using DominoEngine.Interfaces;

namespace VisualDominoes
{
    public class ControllerGame
    {
        TValuesContents Values = new();
        public void MakeGame()
        {
            InterPrints.Front();
            while (true)
            {
                //--------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------
                //Bloque de construcción del juego
                //Aqui se le pregunta al usuario por preferencias de juego guardando las respuestas
                ICollection<string> versionDominoes = Enum.GetNames(typeof(VersionDomioes));
                int selectCountChip = InterPrints.PrintSelect(versionDominoes, "Domino Version",0, versionDominoes.Count);
                int countLinkedValues = InterPrints.VersionChips(selectCountChip);
                int countPlayer = InterPrints.PrintSelect(new List<string>(), "Amount of players",1, countLinkedValues);
                int maxNumChip = ((countLinkedValues * (countLinkedValues + 1)) / 2) / countPlayer;
                int numChipForPlayer = InterPrints.PrintSelect(new List<string>(), "Amount of chips in hand",1, maxNumChip + 1);

                ICollection<string> typesGames = Enum.GetNames(typeof(TypeGame));
                int selectTypeGame = InterPrints.PrintSelect(typesGames, "Game type",0, typesGames.Count);
                TypeGame typeGame = (TypeGame)selectTypeGame;
                //--------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------
                //Aqui se construye el juego segun las respuestas del usuario
                switch (typeGame)
                {
                    case TypeGame.ClasicDominos:
                        {
                            List<Player<Numeric, int>> players = new();
                            ICollection<string> typePlayer = Enum.GetNames(typeof(TypePlayer));


                            for (int i = 0; i < countPlayer; i++)
                            {
                                int selectTypePlayer = InterPrints.PrintSelect(typePlayer, "Player type",0, typePlayer.Count);
                                InterPrints.AddPlayer(players, selectTypePlayer, i);
                            }
                            IWinCondition<Numeric, int>[] winConditions = { new WinnerByPuntos<Numeric, int>(), new PlayAllChips<Numeric, int>() };
                            IEndCondition<Numeric, int>[] finalConditions = { new IsLocked<Numeric, int>(), new PlayAllChips<Numeric, int>() };
                            Rules<Numeric, int> rules = new(winConditions, finalConditions);
                            ClassicGameLogic<Numeric, int> gameLogic = new(countLinkedValues, rules, Values.ValuesNumerics, players);
                            //Echa a andar el juego
                            NewGame<Numeric, int>(gameLogic, numChipForPlayer);
                            break;
                        }
                    case TypeGame.PrittyBoy:
                        {
                            List<Player<Emojis, string>> emoplayer = new();
                            ICollection<string> typePlayer = Enum.GetNames(typeof(TypePlayer));
                            for (int i = 0; i < countPlayer; i++)
                            {
                                int selectTypePlayer = InterPrints.PrintSelect(typePlayer, "Player type",0, typePlayer.Count);
                                InterPrints.AddPlayer(emoplayer, selectTypePlayer, i);
                            }
                            IWinCondition<Emojis, string>[] winConditions = { new WinnerByChips<Emojis, string>(), new PlayAllChips<Emojis, string>() };
                            IEndCondition<Emojis, string>[] finalConditions = { new IsLocked<Emojis, string>(), new PlayAllChips<Emojis, string>() };
                            Rules<Emojis, string> rules = new(winConditions, finalConditions);
                            ClassicGameLogic<Emojis, string> gameLogic = new(countLinkedValues, rules, Values.ValuesEmojis, emoplayer);

                            NewGame<Emojis, string>(gameLogic, numChipForPlayer);
                            break;
                        }
                    case TypeGame.Stolen:
                        {
                            List<Player<Numeric, int>> players = new();
                            ICollection<string> typePlayer = Enum.GetNames(typeof(TypePlayer));
                            for (int i = 0; i < countPlayer; i++)
                            {
                                int selectTypePlayer = InterPrints.PrintSelect(typePlayer, "Player type",0, typePlayer.Count);
                                InterPrints.AddPlayer(players, selectTypePlayer, i);
                            }
                            IWinCondition<Numeric, int>[] winConditions = { new WinnerByPuntos<Numeric, int>(), new PlayAllChips<Numeric, int>() };
                            IEndCondition<Numeric, int>[] finalConditions = { new IsLocked<Numeric, int>(), new PlayAllChips<Numeric, int>() };
                            Rules<Numeric, int> rules = new(winConditions, finalConditions);
                            StolenLogic<Numeric, int> gameLogic = new StolenLogic<Numeric, int>(countLinkedValues, rules, Values.ValuesNumerics, players);

                            NewGame<Numeric, int>(gameLogic, numChipForPlayer);
                            break;
                        }
                }
                //--------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------
                //Cuando termina el juego se le pregunta al usuario si desea jugar de nuevo
                int key = InterPrints.PrintSelect(new string[] {"New Game","Exit" }, "Desea Continuar?",0, 2);
                if(key == 1) return;
            }

        }
        public void NewGame<TValue, T>(IGameLogic<TValue, T> Game, int numChipPlayer) where TValue : IValue<T>
        {

            //Genera las fichas
            Game.HandOutChips(numChipPlayer);

            do
            {
                 //Busca al jugador que le toca jugar(al que le toca y no esta pasado y pasa y pasa de turno si al jugador que le toca no puede jugar)
                Game.ChangeValidCurrentPlayer();
                 //Pregunta si ya se acabo el juego
                if (Game.EndGame())
                {
                    break;
                }
                //-----------------------------------------------------------------------------------------
                //-----------------------------------------------------------------------------------------
                //Bloque de impresiones
                InterPrints.PrintGame(Game);
                InterPrints.PrintHand(Game.CurrentPlayer.GetHand());
                Console.WriteLine("\n");
                Console.WriteLine("Mano de Todos los jugadores");
                Console.WriteLine("---------------------------------------------------------------------");
                Console.WriteLine("---------------------------------------------------------------------");
                foreach (var player in Game.Players)
                {
                    Console.WriteLine(player.Name);
                    InterPrints.PrintHand(player.GetHand());
                }
                Console.WriteLine("---------------------------------------------------------------------");
                Console.WriteLine("---------------------------------------------------------------------");
                //-----------------------------------------------------------------------------------------
                //-----------------------------------------------------------------------------------------
                //ejecuta todo lo que pasa en un turno
                Game.CurrentTurn();
                Console.ReadKey();

            } while (true);
            // busca si hay un ganador
            if (Game.Rules.IsWinner(Game.Players, out Player<TValue, T> winner))
            {
                Console.WriteLine("The winner is " + winner.Name);
            }
            //busca si hay un empate
            if (Game.Rules.IsTie(Game.Players, out List<Player<TValue, T>> winners))
            {
                Console.WriteLine("Tie");
                foreach (var item in winners)
                {
                    Console.Write(" " + item.Name);
                }

            }
            Console.ReadKey();
            Console.Clear();
        }
    }
}