
using DominoEngine;
using DominoEngine.Interfaces;

namespace VisualDominoes
{
    enum VersionDomioes
    {
        Doble7,
        Doble8,
        Doble9,
        Doble10,
    }
    enum TypePlayer
    {
        HumanPlayer,
        RandomPlyer,
        BotaGorda,
        AlmostClever,

    }
    enum TypeGame
    {
        ClasicDominos,
        PrittyBoy,
        Stolen,
    }

    public static class InterPrints
    {
        public static void Front()
        {
            Console.Clear();
            string banner = @"
██████╗  ██████╗ ███╗   ███╗██╗███╗   ██╗ ██████╗ ███████╗███████╗
██╔══██╗██╔═══██╗████╗ ████║██║████╗  ██║██╔═══██╗██╔════╝██╔════╝
██║  ██║██║   ██║██╔████╔██║██║██╔██╗ ██║██║   ██║█████╗  ███████╗
██║  ██║██║   ██║██║╚██╔╝██║██║██║╚██╗██║██║   ██║██╔══╝  ╚════██║
██████╔╝╚██████╔╝██║ ╚═╝ ██║██║██║ ╚████║╚██████╔╝███████╗███████║
╚═════╝  ╚═════╝ ╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝ ╚═════╝ ╚══════╝╚══════╝
";
            Console.WriteLine(banner);
            Console.WriteLine("\n");
            BarProgress(300,100);
            Console.Clear();
        }
        private static void BarProgress(int progreso, int total = 100) //Default 100
        {
            //Dibujar la barra vacia
            Console.CursorLeft = 0;
            Console.Write(""); //inicio
            Console.CursorLeft = 32;
            Console.Write(""); //fin
            Console.CursorLeft = 1; //Colocar el cursor al inicio
            float onechunk = 30.0f / total;

            //Rellenar la parte indicada
            int position = 1;
            for (int i = 0; i < onechunk * progreso; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //Pintar la otra parte
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //Escribir el total al final
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progreso.ToString() + "% de " + total.ToString() + "    ");
        }
        public static int PrintSelect(ICollection<string> selected, string description, int count)
        {
            int select;
            bool isNumeric;
            Console.Clear();
            Console.WriteLine(description);
            Console.WriteLine("Select: ");
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
                if (!isNumeric || select >= count || select < 0)
                {
                    Console.WriteLine("Seleccion incorrecta");
                    Console.WriteLine("El tipo debe ser numerico y estar en el rago " + "(0 -" + (count - 1) + ")");
                    Console.WriteLine("Seleccione otro numero");

                }
            } while (!isNumeric || select >= count || select < 0);
            return select;
        }
        public static void AddPlayer<TValue, T>(List<Player<TValue, T>> players, int select, int order) where TValue : IValue<T>, IRankable
        {
            Console.Clear();
            Console.WriteLine("Write the player name");
            TypePlayer player = (TypePlayer)select;
            switch (player)
            {
                case TypePlayer.HumanPlayer:
                    {
                        string name = Console.ReadLine();
                        players.Add(new Player<TValue, T>(name, order, new HumanStrategies<TValue, T>()));
                        break;
                    }
                case TypePlayer.RandomPlyer:
                    {
                        string name = Console.ReadLine();
                        players.Add(new Player<TValue, T>(name, order, new RandomStrategies<TValue, T>()));
                        break;
                    }
                case TypePlayer.BotaGorda:
                    {
                        string name = Console.ReadLine();
                        players.Add(new Player<TValue, T>(name, order, new BotaGordaStategies<TValue, T>()));
                        break;
                    }
                case TypePlayer.AlmostClever:
                    {
                        string name = Console.ReadLine();
                        players.Add(new Player<TValue, T>(name, order, new AlmostCleverStrategies<TValue, T>()));
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
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Table");
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("------------------------------------------------------------------------");
            if (Table.Count == 0) Console.WriteLine("Table Is Empty");

            for (int i = 0; i < Table.Count; i = i + 2)
            {
                Console.Write("[" + Table[i].Value + "|" + Table[i + 1].Value + "]");
            }
            System.Console.WriteLine("\n");
            System.Console.WriteLine("\n");
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("------------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.White;

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
        public static void PrintGame<TValue, T>(IGameLogic<TValue, T> Game) where TValue : IValue<T>
        {

            Console.Clear();
            PrintTable(Game.board);
            Console.WriteLine("Player Turn: " + Game.CurrentPlayer.Name);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
        }
    }
}