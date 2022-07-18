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
                ICollection<string> versionDominoes = Enum.GetNames(typeof(VersionDomioes));
                int selectCountChip = InterPrints.PrintSelect(versionDominoes, "Domino Version", versionDominoes.Count);
                int countLinkedValues = InterPrints.VersionChips(selectCountChip);
                int countPlayer = InterPrints.PrintSelect(new List<string>(), "Amount of players", countLinkedValues);
                int maxNumChip = ((countLinkedValues * (countLinkedValues + 1)) / 2) / countPlayer;
                int numChipForPlayer = InterPrints.PrintSelect(new List<string>(), "Amount of chips in hand", maxNumChip + 1);

                ICollection<string> typesGames = Enum.GetNames(typeof(TypeGame));
                int selectTypeGame = InterPrints.PrintSelect(typesGames, "Game type", typesGames.Count);
                TypeGame typeGame = (TypeGame)selectTypeGame;
                switch (typeGame)
                {
                    case TypeGame.ClasicDominos:
                        {
                            List<Player<Numeric, int>> players = new();
                            ICollection<string> typePlayer = Enum.GetNames(typeof(TypePlayer));


                            for (int i = 0; i < countPlayer; i++)
                            {
                                int selectTypePlayer = InterPrints.PrintSelect(typePlayer, "Player type", typePlayer.Count);
                                InterPrints.AddPlayer(players, selectTypePlayer, i);
                            }
                            IWinCondition<Numeric, int>[] winConditions = { new WinnerByPuntos<Numeric, int>(), new PlayAllChips<Numeric, int>() };
                            IEndCondition<Numeric, int>[] finalConditions = { new IsLocked<Numeric, int>(), new PlayAllChips<Numeric, int>() };
                            Rules<Numeric, int> rules = new(winConditions, finalConditions);
                            ClassicGameLogic<Numeric, int> gameLogic = new(countLinkedValues, rules, Values.ValuesNumerics, players);
                            NewGame<Numeric, int>(gameLogic, numChipForPlayer);
                            break;
                        }
                    case TypeGame.PrittyBoy:
                        {
                            List<Player<Emojis, string>> emoplayer = new();
                            ICollection<string> typePlayer = Enum.GetNames(typeof(TypePlayer));
                            for (int i = 0; i < countPlayer; i++)
                            {
                                int selectTypePlayer = InterPrints.PrintSelect(typePlayer, "Player type", typePlayer.Count);
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
                                int selectTypePlayer = InterPrints.PrintSelect(typePlayer, "Player type", typePlayer.Count);
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
                int key = InterPrints.PrintSelect(new string[] {"New Game","Exit" }, "Desea Continuar?", 2);
                if(key == 1) return;
            }

        }
        public void NewGame<TValue, T>(IGameLogic<TValue, T> Game, int numChipPlayer) where TValue : IValue<T>
        {

            //TODO: tengo que hacer el calculo para valanciar fichas cant y jugadores cant;
            Game.HandOutChips(numChipPlayer);

            do
            {
                Game.ChangeValidCurrentPlayer();
                if (Game.EndGame())
                {
                    break;
                }
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
                Game.CurrentTurn();
                Console.ReadKey();

            } while (true);
            if (Game.Rules.IsWinner(Game.Players, out Player<TValue, T> winner))
            {
                Console.WriteLine("The winner is " + winner.Name);
            }
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