using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DominoEngine;
using DominoEngine.Interfaces;

namespace VisualDominoes
{
    class Program
    {
        static void Main(string[] args)
        {
            ControllerGame game = new();
            game.MakeGame();
        }
    }
    enum VersionDomioes
    {
        Doble7,
        Doble8,
        Doble9,
        Doble10,
    }
    enum TypePlayer
    {
        HumanPlayer
    }
    enum TypeGame
    {
        ClasicDominos
    }
    public class ControllerGame
    {
        TValuesContents Values = new();
        public void MakeGame()
        {
            InterPrints.Front();

            ICollection<string> versionDominoes = Enum.GetNames(typeof(VersionDomioes));
            int selectCountChip = InterPrints.PrintSelect(versionDominoes, "Version de Domino", versionDominoes.Count);
            int countLinkedValues = InterPrints.VersionChips(selectCountChip);

            int countPlayer = InterPrints.PrintSelect(new List<string>(), "Number Player", countLinkedValues);

            int maxNumChip = ((countLinkedValues * countLinkedValues + 1) / 2) / countPlayer;
            int numChipForPlayer = InterPrints.PrintSelect(new List<string>(), "Number Chip for Player", maxNumChip);

            ICollection<string> typesGames = Enum.GetNames(typeof(TypeGame));
            int selectTypeGame = InterPrints.PrintSelect(typesGames, "Type Game", typesGames.Count);
            TypeGame typeGame = (TypeGame)selectTypeGame;
            switch (typeGame)
            {
                case TypeGame.ClasicDominos:
                    {
                        List<Player<Numeric, int>> players = new();
                        Rules<Numeric, int> rules = new();

                        ICollection<string> typePlayer = Enum.GetNames(typeof(TypePlayer));
                        for (int i = 0; i < countPlayer; i++)
                        {
                            int selectTypePlayer = InterPrints.PrintSelect(typePlayer, "TypePlayer", typePlayer.Count);
                            InterPrints.AddPlayer(players, selectTypePlayer, i);
                        }

                        NewGame<Numeric, int>(countLinkedValues, numChipForPlayer, Values.ValuesNumerics, players, rules);
                        break;
                    }
            }




        }
        public void NewGame<TValue, T>(int countValue, int numChipPlayer, TValue[] values, List<Player<TValue, T>> players, IRules<TValue, T> rules) where TValue : IValue<T>
        {
            GameLogic<TValue, T> game = new GameLogic<TValue, T>(countValue, values, players, rules);
            //TODO: tengo que hacer el calculo para valanciar fichas cant y jugadores cant;
            game.HandOutChips(numChipPlayer);

            do
            {
                game.ChangeValidCurrentPlayer();
                InterPrints.PrintGame(game);
                if (game.EndGame()) break;
                InterPrints.PrintHand(game.CurrentPlayer.GetHand());
                Console.WriteLine("\n");
                Console.WriteLine("\n");
                foreach (var player in players)
                {
                    Console.WriteLine(player.Name);
                    InterPrints.PrintHand(player.GetHand());
                }
                game.CurrentTurn();

            } while (true);

            Console.Clear();
        }

    }
    public static class InterPrints
    {
        public static void Front()
        {
            Console.Clear();
            Console.WriteLine("Welcom to Dominos");
        }
        //TODO: Este todo es para recordar yo que tengo que ver si quitar o no el parametro count
        public static int PrintSelect(ICollection<string> selected, string description, int count)
        {
            int select;
            bool isNumeric;
            Console.Clear();
            Console.WriteLine(description);
            Console.WriteLine("Select:");
            int i = 0;
            if (selected.Count > 0)
            {
                foreach (var item in selected)
                {
                    Console.WriteLine(i + " " + item);
                    i++;
                }
            }
            do
            {
                isNumeric = int.TryParse(Console.ReadLine(), out select);
                if (!isNumeric || select > count || select < 0)
                {
                    Console.WriteLine("Seleccion incorrecta");
                    Console.WriteLine("El tipo debe ser numerico y estar en el rago" + "(0 -" + count + ")");
                    Console.WriteLine("Seleccione otro numero");

                }
            } while (!isNumeric || select > count || select < 0);
            return select;
        }
        public static void AddPlayer<TValue, T>(List<Player<TValue, T>> players, int select, int order) where TValue : IValue<T>
        {
            Console.Clear();
            Console.WriteLine("Say the name of player");
            TypePlayer player = (TypePlayer)select;
            switch (player)
            {
                case TypePlayer.HumanPlayer:
                    {
                        string name = Console.ReadLine();
                        players.Add(new Player<TValue, T>(name, order, new HumanStrategies<TValue, T>()));
                        break;
                    }
            }
        }
        public static int VersionChips(int select)
        {

            VersionDomioes version = (VersionDomioes)select;
            switch (version)
            {
                case VersionDomioes.Doble7:
                    {
                        return 8;
                    }
                case VersionDomioes.Doble8:
                    {
                        return 9;
                    }
                case VersionDomioes.Doble9:
                    {
                        return 10;
                    }
                case VersionDomioes.Doble10:
                    {
                        return 11;
                    }
                default: return -1;
            }
        }

        public static void PrintTable<TValue, T>(Board<TValue, T> table) where TValue : IValue<T>
        {
            List<TValue> Table = table.GetBoard();

            for (int i = 0; i < Table.Count; i = i + 2)
            {
                Console.Write("[" + Table[i].Value + "|" + Table[i + 1].Value + "]");
            }
            System.Console.WriteLine();
        }
        public static void PrintHand<TValue, T>(List<Chip<TValue, T>> Hand) where TValue : IValue<T>
        {
            Console.WriteLine("Hand:");
            for (int i = 0; i < Hand.Count; i++)
            {
                Console.Write("[" + Hand[i].LinkL.Value + "]");
            }
            Console.WriteLine();
            for (int i = 0; i < Hand.Count; i++)
            {
                Console.Write("[" + Hand[i].LinkR.Value + "]");
            }
            System.Console.WriteLine();
        }
        public static void PrintGame<TValue, T>(GameLogic<TValue, T> game) where TValue : IValue<T>
        {
            Console.Clear();
            PrintTable(game.board);
            Console.WriteLine("Turn the: " + game.CurrentPlayer.Name);
            Console.WriteLine();
        }
    }
}

