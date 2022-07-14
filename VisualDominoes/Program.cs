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

            Dictionary<int, List<IValue<int>>> chips = new Dictionary<int, List<IValue<int>>>();
            List<IValue<int>> numbers = new();
            for (int i = 0; i < 20; i++)
            {
                numbers.Add(new Number<int>(i));
            }
            chips.Add(0, numbers);

            ControllerGame<int> game = new(chips);
            game.NewGame();
        }
    }
    enum TypeChips
    {
        ClasicChips = 0,
    }
    enum TypePlayer
    {
        HumanPlayer
    }


    public class ControllerGame<T>
    {
        Dictionary<int, List<IValue<T>>> chips = new Dictionary<int, List<IValue<T>>>();

        public ControllerGame(Dictionary<int, List<IValue<T>>> chips)
        {
            this.chips = chips;
        }

        public void NewGame()
        {
            Menu<T> menu = new Menu<T>();
            //if (menu.FrontMenu() == 1)
            //{
            //ICollection<string> colection = Enum.GetNames(typeof(TypeChips));
            //int select1 = menu.PrintSelect(colection, "TypeChips", colection.Count);
            //List<IValue<T>> values = chips[select1];
            //// Tengo que hacer un calculo de chip contra player por ahora son 4;
            //int CountChip = menu.PrintSelect(new List<string>(), "Number de caras distintas",20);
            //int countPlayer = menu.PrintSelect(new List<string>(), "Number de Player", 4);
            //List<Player<T>> players = new();
            //for (int i = 0; i < countPlayer; i++)
            //{
            //    int select2 = menu.PrintSelect(Enum.GetNames(typeof(TypePlayer)), "TypeChips", colection.Count);
            //    menu.AddPlayer(players, select2, i);
            //}
            //// Voy a mantenerlo asi por ahora pero el constructor puede tomar players tambien
            int CountChip = 7;
            var values = chips[0];
            List<Player<T>> players = new List<Player<T>>();
            players.Add(new Player<T>("name1", 0, new HumanStrategies<T>()));
            players.Add(new Player<T>("name2", 1, new HumanStrategies<T>()));
            List<IWinCondition<T>> winConditions = new List<IWinCondition<T>>();
            List<IEndCondition<T>> finalConditions = new List<IEndCondition<T>>();
            winConditions.Add(new PlayAllChips<T>());
            winConditions.Add(new WinnerByChips<T>());
            finalConditions.Add(new PlayAllChips<T>());
            finalConditions.Add(new IsLocked<T>());
            int numChipPlayer = menu.PrintSelect(new List<string>(), "Cant chip ", 10);


            Rules<T> rules = new Rules<T>(winConditions,finalConditions,players.Count,numChipPlayer);
            GameLogic<T> game = new GameLogic<T>(CountChip, values, players,rules);

            // tengo que hacer el calculo para valanciar fichas cant y jugadores cant;
            game.GiveChips(numChipPlayer);

            foreach (var item in players)
            {
                Console.WriteLine(item.Name);
                menu.PrintHand(item.GetHand());
            }
            while (true)
            {
                do
                {
                    game.ChangeCurrentPlayer();
                    menu.PrintGame(game);
                    if (game.EndGame()) break;
                    menu.PrintHand(game.CurrentPlayer.GetHand());
                    game.CurrentTurn();
                } while (true);
            }
            Console.Clear();
            Console.WriteLine("the winer is: " + game.Winner.Name);

        }
    }

    public class Menu<T>
    {
        public int FrontMenu()
        {

            int select;
            bool flag;
            Console.Clear();
            do
            {
                Console.WriteLine("Welcom to Dominos");
                Console.WriteLine("Chouse the Kind of game you wonna lay");
                Console.WriteLine("1: Clasic domino");
                Console.WriteLine("2: Quit");
                flag = int.TryParse(Console.ReadLine(), out select);
                if (!flag && select != 1 && select != 2) Console.WriteLine("seleccion incorrecta");
            } while (!flag && select != 1 && select != 2);
            return select;
        }
        public int PrintSelect(ICollection<string> selected, string description, int count)
        {
            int select;
            bool flag;
            Console.WriteLine("Select " + description);
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
                flag = int.TryParse(Console.ReadLine(), out select);
                if (flag = false || select > count || select < 0) Console.WriteLine("Seleccion incorrecta");
            } while (flag = false || select > count || select < 0);
            return select;
        }
        public void AddPlayer(List<Player<T>> players, int select, int order)
        {
            Console.WriteLine("Say the name of player ");
            TypePlayer player = (TypePlayer)select;

            switch (player)
            {
                case TypePlayer.HumanPlayer:
                    {
                        string name = Console.ReadLine();
                        players.Add(new Player<T>(name,order,new HumanStrategies<T>()));
                        break;
                    }
            }
        }
        public void PrintTable(Board<T> table)
        {
            List<IValue<T>> Table = table.GetBoard().ToList();
            for (int i = 0; i < Table.Count; i = i + 2)
            {
                Console.Write("[" + Table[i].value + "|" + Table[i + 1].value + "]");
            }
            System.Console.WriteLine();
        }
        public void PrintHand(List<Chip<T>> Hand)
        {
            for (int i = 0; i < Hand.Count; i++)
            {
                Console.Write("[" + Hand[i].LinkL.value + "]");
            }
            System.Console.WriteLine();
            for (int i = 0; i < Hand.Count; i++)
            {
                System.Console.Write("[" + Hand[i].LinkR.value + "]");
            }
            System.Console.WriteLine();
        }
        public void PrintGame(GameLogic<T> game)
        {
            Console.Clear();
            System.Console.WriteLine("Turn: " + game.CurrentPlayer.Name);
            System.Console.WriteLine();
            PrintTable(game.board);
            System.Console.WriteLine("Hand:");

        }
    }
}
