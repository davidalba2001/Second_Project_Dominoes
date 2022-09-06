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
                //------------------------------------------------------------------------------------------------------
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
                //-------------------------------------------------------------------------------------
                //-------------------------------------------------------------------------------------
                /// //Bloque de impresiones
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
                //-------------------------------------------------------------------------------------
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

        public bool AskHumanNextPlay(Player<TValue, T> player, Board<TValue, T> board, Rules<TValue, T> rules, out (Chip<TValue, T>, TValue) value)
        {
            bool canPlay = false;
            int pos;
            Chip<TValue, T> move;
            // Primero verifica si hay fichas en el tablero
            if (board.CountChip != 0)
            {
                // Aqui guardo en una variable booleana si existen jugadas validas
                canPlay = player.CanPlay(board, rules);
                // si no existe devuelve false directamente
                if (!canPlay)
                {
                    value = default((Chip<TValue, T>, TValue));
                    return canPlay;
                }
                bool IsValidMove;
                do
                {
                    bool isNumeric;
                    do
                    {
                        // Pegunta al usuario por la ficha que desea jugar
                        Console.WriteLine("Chouse a number between 0 and " + (player.NumChips - 1) + " dependig of the position of the chip you wanna play");
                        // Si la respuesta no es numerica guarda false en IsNumeric para volver a preguntar por la ficha que desea jugar
                        isNumeric = int.TryParse(Console.ReadLine(), out pos);
                        if (!isNumeric) Console.WriteLine("String is not a numeric representation");
                    } while (!isNumeric || (pos >= player.NumChips || pos < 0));
                    // Guarda la posision dada luego de sabe que es una posision valida
                    move = player.GetChipInPos(pos);
                    // Revisa que la ficha pueda jugarse correctamente, de lo contrario se repite el ciclo
                    IsValidMove = rules.PlayIsValid(move, board.GetLinkL) || rules.PlayIsValid(move, board.GetLinkR);
                    if (!IsValidMove) Console.WriteLine("Is not valid");
                } while (!IsValidMove);

                bool ValidMoveRight = rules.PlayIsValid(move, board.GetLinkR);
                bool ValidMoveLeft = rules.PlayIsValid(move, board.GetLinkL);
                // valido por ambos lados
                if (ValidMoveRight && ValidMoveLeft)
                {
                    Console.WriteLine("Press booton <-- if you wanna paly in the Left side or --> in the Rigth side");
                    ConsoleKey key = new ConsoleKey();
                    do
                    {
                        key = Console.ReadKey().Key;
                        if (key == ConsoleKey.RightArrow)
                        {
                            value = (move, board.GetLinkR);
                            return true;
                        }
                        if (key == ConsoleKey.LeftArrow)
                        {
                            value = (move, board.GetLinkL);
                            return true;
                        }
                    } while (key != ConsoleKey.LeftArrow && key != ConsoleKey.RightArrow);
                }
                // Se puede jugar por la derecha
                if (ValidMoveRight)
                {
                    value = (move, board.GetLinkR);
                    return true;
                }
                // Se puede jugar por la Isquierda
                value = (move, board.GetLinkL);
                return true;
            }
            // esto es en el caso de que sea la primera jugada
            else
            {
                bool isNumeric;
                do
                {
                    Console.WriteLine("Chouse a number between 0 and " + (player.NumChips-1) + " dependig of the position of the chip you wanna play");
                    isNumeric = int.TryParse(Console.ReadLine(), out pos);
                    if (!isNumeric) Console.WriteLine("String is not a numeric representation");
                } while (!isNumeric || (pos >= player.NumChips || pos < 0));
                value = (player.GetChipInPos(pos), default(TValue));
                return true;
            }
        }


    }
}