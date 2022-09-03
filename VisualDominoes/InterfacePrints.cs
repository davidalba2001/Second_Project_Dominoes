
using DominoEngine;
using DominoEngine.Interfaces;

namespace VisualDominoes
{
    // Opciones de la versión
    enum VersionDomioes
    {
        Doble7,
        Doble8,
        Doble9,
        Doble10,
    }
    // Tipo de palyer  
    enum TypePlayer
    {
        HumanPlayer,
        RandomPlyer,
        BotaGorda,
        AlmostClever,

    }
    //Tipo de juego 
    enum TypeGame
    {
        ClasicDominos,
        PrittyBoy,
        Stolen,
    }

    public static class InterPrints
    {
        //Imprime la parte frontal de juego(Primera portada)
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
        //Imprime la barra de progreso que se muestra al inicio
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
        //Selector (Imprime devuelve e interactuá con la entrada de la selecciones del usuario)
        public static int PrintSelect(ICollection<string> selected, string description,int min, int count)
        {
            int select;
            bool isNumeric;
            Console.Clear();
            Console.WriteLine(description);
            Console.WriteLine("Press: ");
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
                    Console.WriteLine("El tipo debe ser numerico y estar en el rago " + "("+ min + " -- " + (count - 1) + ")");
                    Console.WriteLine("Seleccione otro numero");

                }
            } while (!isNumeric || select >= count || select < min);
            return select;
        }
        //Menú para construir la lista de jugadores según la opción
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
                        players.Add(new Player<TValue, T>(name, order, new HumanStrategies<TValue, T>(new AskHumanNextPlay<TValue, T>(AskNextPlay))));
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
        // Según la opción elegida permite tomar el numero de caras del domino
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
        // Imprime la mesa
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
        //Imprime las manos
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
        //Imprime las secuencia del juego
        public static void PrintGame<TValue, T>(IGameLogic<TValue, T> Game) where TValue : IValue<T>
        {

            Console.Clear();
            PrintTable(Game.board);
            Console.WriteLine("Player Turn: " + Game.CurrentPlayer.Name);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
        }

        public static (int CountChip, int LinkedValues, int countPlayer, int maxNumChip, int ChipForPlayer, int GameType) Customitation()
        {
            int selectCountChip;
            int countLinkedValues;
            int countPlayer;
            int maxNumChip;
            int numChipForPlayer;
            int selectGameType;
                //--------------------------------------------------------------------------------------------------------
                //--------------------------------------------------------------------------------------------------------
                //Bloque de construcción del juego
                //Aqui se le pregunta al usuario por preferencias de juego guardando las respuestas
                ICollection<string> versionDominoes = Enum.GetNames(typeof(VersionDomioes));
                 selectCountChip=
                    InterPrints.PrintSelect(versionDominoes, "Domino Version", 0, versionDominoes.Count);

                countLinkedValues = InterPrints.VersionChips(selectCountChip);

                countPlayer =
                    InterPrints.PrintSelect(new List<string>(), "Amount of players", 1, countLinkedValues);

                maxNumChip = ((countLinkedValues * (countLinkedValues + 1)) / 2) / countPlayer;
                numChipForPlayer =
                    InterPrints.PrintSelect(new List<string>(), "Amount of chips in hand", 1, maxNumChip + 1);

                ICollection<string> typesGames = Enum.GetNames(typeof(TypeGame));
                selectGameType = InterPrints.PrintSelect(typesGames, "Game type", 0, typesGames.Count);
                

            return (selectCountChip, countLinkedValues, countPlayer, maxNumChip, numChipForPlayer, selectGameType);
            //--------------------------------------------------------------------------------------------------------
        }

          public static bool AskNextPlay<TValue, T>(Player<TValue, T> player, Board<TValue, T> board, Rules<TValue, T> rules, out (Chip<TValue, T>, TValue) value)where TValue : IValue<T>
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