using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public static class Strategies<T>
    {
        public static (Chip<T>, IValue<T>? value)? Play(Player<T> player, Board<T> board, Rules<T> rules,out bool canPlay)
        {
            int pos;
            Chip<T> muve;

            if (board.CountChip != 0)
            {
                var muvesInRigth = player.GetValidPlay(board.GetLinkR(), rules);
                var muvesInLeft = player.GetValidPlay(board.GetLinkL(), rules);
                if (muvesInLeft.Count == 0 && muvesInRigth.Count == 0)
                {
                    canPlay = false;
                    return null;
                }
                bool flag1;
                do
                {
                    bool flag2;
                    do
                    {
                        Console.WriteLine("Chouse a number between 0 and " + (player.NumChips - 1) + "dependig of the position of the chip you wanna play");
                        flag2 = int.TryParse(Console.ReadLine(), out pos);
                        if (!flag2) Console.WriteLine("String is not a numeric representation");
                    } while (!flag2);

                    muve = player.GetChipInPos(pos);
                    flag1 = !rules.PlayIsValid(muve, board.GetLinkL()) && !rules.PlayIsValid(muve, board.GetLinkR());
                    if (flag1) Console.WriteLine("Is not valid");
                } while (flag1);

                if (rules.PlayIsValid(muve, board.GetLinkR()))
                {

                    canPlay = true;
                    return (muve, board.GetLinkR());
                }
                if (rules.PlayIsValid(muve, board.GetLinkL()))
                {
                    canPlay = true;
                    return (muve, board.GetLinkL());
                }
                else
                {

                    Console.WriteLine("Press booton <-- if you wanna paly in the Left side or --> in the Rigth side");
                    do
                    {
                        if (Console.ReadKey().Key == ConsoleKey.RightArrow)
                        {
                            canPlay = true;
                            return (muve, board.GetLinkR());
                        }

                        if (Console.ReadKey().Key == ConsoleKey.LeftArrow)
                        {
                            canPlay = true;
                            return (muve, board.GetLinkL());
                        }
                    } while (!(Console.ReadKey().Key == ConsoleKey.LeftArrow) || !(Console.ReadKey().Key == ConsoleKey.RightArrow));
                    canPlay = false;
                    return null;
                }
            }
            else
            {
                bool flag3;
                do
                {
                    Console.WriteLine("Chouse a number between 0 and " + player.NumChips + "dependig of the position of the chip you wanna play");
                    flag3 = int.TryParse(Console.ReadLine(), out pos);
                    if (!flag3) Console.WriteLine("String is not a numeric representation");
                } while (!flag3);
                canPlay = true;
                return (player.GetChipInPos(pos), default(IValue<T>));
            }
        }
    }
}
