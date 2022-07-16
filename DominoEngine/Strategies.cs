using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;

namespace DominoEngine
{
    public class HumanStrategies<TValue, T> : IStrategy<TValue, T> where TValue : IValue<T>
    {
        public bool ValidMove(Player<TValue, T> player, Board<TValue, T> board, Rules<TValue, T> rules, out (Chip<TValue, T>, TValue) value)
        {
            bool canPlay = false;
            int pos;
            Chip<TValue, T> move;

            if (board.CountChip != 0)
            {
                canPlay = player.CanPlay(board, rules);
                if (!canPlay)
                {
                    value = default((Chip<TValue, T>, TValue));
                    return canPlay;
                }
                // Esto en interface grafica creo que seria un metodo con  un evento click a una fichha y esa ficha se devuelva
                bool IsValidMove;
                do
                {
                    bool isNumeric;
                    do
                    {
                        Console.WriteLine("Chouse a number between 0 and " + (player.NumChips - 1) + "dependig of the position of the chip you wanna play");
                        isNumeric = int.TryParse(Console.ReadLine(), out pos);
                        if (!isNumeric) Console.WriteLine("String is not a numeric representation");
                    } while (!isNumeric);

                    move = player.GetChipInPos(pos);
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

            else
            {
                bool isNumeric;
                do
                {
                    Console.WriteLine("Chouse a number between 0 and " + player.NumChips + "dependig of the position of the chip you wanna play");
                    isNumeric = int.TryParse(Console.ReadLine(), out pos);
                    if (!isNumeric) Console.WriteLine("String is not a numeric representation");
                } while (!isNumeric);
                value = (player.GetChipInPos(pos), default(TValue));
                return true;
            }
        }
    }
    public class RandomStrategies<TValue, T> : IStrategy<TValue, T> where TValue : IValue<T>
    {
        public bool ValidMove(Player<TValue, T> player, Board<TValue, T> board,Rules<TValue, T> rules, out (Chip<TValue, T>, TValue) move)
        {
            List<Chip<TValue, T>> ValidMoves = player.GetValidPlay(board.GetLinkL, rules);
            ValidMoves.AddRange(player.GetValidPlay(board.GetLinkR, rules));
            if (ValidMoves.Count != 0)
            {
                Random RDM = new Random();
                List<Chip<TValue, T>> Randomized = ValidMoves.OrderBy(Item => RDM.Next()).ToList<Chip<TValue, T>>();
                if (rules.PlayIsValid(ValidMoves[0], board.GetLinkL))
                {
                    move = (ValidMoves[0], board.GetLinkL);
                    return true;
                }
                if (rules.PlayIsValid(ValidMoves[0], board.GetLinkR))
                {
                    move = (ValidMoves[0], board.GetLinkR);
                    return true;
                }
            }
            move = default;
            return false;
        }
    }
    public class BotaGordaStategies<TValue, T> : IStrategy<TValue, T> where TValue : IValue<T>, IRankable
    {
        public bool ValidMove(Player<TValue, T> player, Board<TValue, T> board, Rules<TValue, T> rules, out (Chip<TValue, T>, TValue) move)
        {
            List<Chip<TValue, T>> ValidMoves = player.GetValidPlay(board.GetLinkL, rules);
            ValidMoves.AddRange(player.GetValidPlay(board.GetLinkR, rules));
            if(ValidMoves.Count != 0)
            {
                Chip<TValue,T> BestRanked = GetBestRankedChip(ValidMoves);
                if (rules.PlayIsValid(BestRanked, board.GetLinkL))
                {
                    move = (BestRanked, board.GetLinkL);
                    return true;
                }
                if (rules.PlayIsValid(BestRanked, board.GetLinkR))
                {
                    move = (BestRanked, board.GetLinkR);
                    return true;
                }                
            }
            move = default;
            return false;
        }
        private Chip<TValue,T> GetBestRankedChip(List<Chip<TValue,T>> ValidMoves)
        {
            Chip<TValue,T> BestRanked = ValidMoves[0];
            foreach(var chip in ValidMoves)
            {
                if(ChipScore(BestRanked)<ChipScore(chip)) BestRanked = chip;
            }
            return BestRanked;
        }
        private int ChipScore(Chip<TValue,T> Chip)
        {
            return Chip.LinkL.Rank()+Chip.LinkR.Rank();
        }

    }

}
