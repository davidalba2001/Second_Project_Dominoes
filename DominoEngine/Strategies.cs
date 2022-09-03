using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;

namespace DominoEngine
{
    public class HumanStrategies<TValue, T> : IStrategy<TValue, T> where TValue : IValue<T>
    {
        private AskHumanNextPlay<TValue, T> Play;
        public HumanStrategies(AskHumanNextPlay<TValue, T> move)
        {
            Play = move;
        }

        public bool ValidMove(Player<TValue, T> player, Board<TValue, T> board, Rules<TValue, T> rules, out (Chip<TValue, T>, TValue) value)
        {
            return Play(player, board, rules, out value);
        }
    }






    public class RandomStrategies<TValue, T> : IStrategy<TValue, T> where TValue : IValue<T>
    {
        // Guarda las jugadas validas en una lista, las ramdomiza y devuelve la 1era ficha
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
        // Guarda las jugadas validas en una lista, que organiza de mayor a menor segun el valor de la ficha, y devuelve la 1era fcha
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
    public class AlmostCleverStrategies<TValue, T> : IStrategy<TValue, T> where TValue : IValue<T>
    {
        // Lista de caras cuya frecuencia sea igual a la maxima frecuencia de caras en la mano
        List<IValue<T>> BestData= new();
        List<Chip<TValue,T>> Hand;
        Rules<TValue,T> Rules;
        public bool ValidMove(Player<TValue, T> player, Board<TValue, T> board, Rules<TValue, T> rules, out (Chip<TValue, T>, TValue) move)
        {
            // Actualiza BestData con la mano que le queda
            GetBestData(player.GetHand());
            Hand = player.GetHand();
            Rules = rules;
            //Guarda las jugadas Validas en una lista 
            List<Chip<TValue, T>> ValidMoves = player.GetValidPlay(board.GetLinkL, rules);
            ValidMoves.AddRange(player.GetValidPlay(board.GetLinkR, rules));
            // si no hay jugadas validas es que no lleva y devuelve false directamente
            if(ValidMoves.Count == 0)
            {
                move = default;
                return false;
            }
            //Le asigna a cada jugada valida un valor dependiendo de la camtidad de fichas que tiene
            //En caso de que la ficha doble la mesa a la cara que mas tiene, la jugada obtiene el mejor score
            List<MoveWeighter> Moves = GetRankedValidMoves(ValidMoves,board).ToList();
            //Ordena las fichas por score de mayor a menor y devuelve la 1era
            move = Moves.OrderByDescending(item => item.Score).ToList()[0].Move;
            return true;
        }
        private void GetBestData(List<Chip<TValue,T>> Hand)
        {
            int cant = 0;
            List<IValue<T>> Data = new();
            foreach(var Chip in Hand)
            {
                foreach(var face in GetFaces(Chip))
                {
                    int count = AmountOfChips(face, Hand);
                    if(count>=cant)
                    {
                        cant = count;
                    }
                }
                foreach(var face in GetFaces(Chip))
                {
                    if(AmountOfChips(face, Hand) == cant)
                    {
                        Data.Add(face);
                    }
                }
                
            }
            BestData = Data;
        }
        private int AmountOfChips(IValue<T> Face, List<Chip<TValue,T>> Hand)
        {            
            int count = 0;
            foreach (var item in Hand)
            {
                if(ConteinsFace(item, Face)) count++;
            }       
            return count; 
        }
        static bool ConteinsFace(Chip<TValue,T> Chip, IValue<T> Face)
        {
            return Chip.LinkL.Equals(Face) || Chip.LinkR.Equals(Face);
        }
        private IEnumerable<IValue<T>> GetFaces(Chip<TValue,T> Chip)
        {
            yield return Chip.LinkL;
            yield return Chip.LinkR;
        }
        private IEnumerable<MoveWeighter> GetRankedValidMoves(List<Chip<TValue,T>> ValidMoves, Board<TValue,T> board)
        {
            foreach(var move in ValidMoves)
            {
                if(Rules.PlayIsValid(move, board.GetLinkL)) yield return new MoveWeighter((move,board.GetLinkL),GetScore((move,board.GetLinkL),board));
                if(Rules.PlayIsValid(move, board.GetLinkR)) yield return new MoveWeighter((move,board.GetLinkR),GetScore((move,board.GetLinkR),board));
            }
        } 
        private double GetScore((Chip<TValue,T>, TValue) Move, Board<TValue,T> board)
        {
            double Score = 0;
            int Frquence = AmountOfChips(Move.Item1.LinkL,Hand)+ AmountOfChips(Move.Item1.LinkR,Hand);
            Score += (double)Frquence/100;
            if(board.CountChip == 0) return Score;
            if(Move.Item2.Equals(BestData)) Score = Score-1;
            if(BestData.Contains(DifrentFace((Move.Item1.LinkL,Move.Item1.LinkR),Move.Item2))
            &&  BestData.Contains(DifrentFace((board.GetLinkL,board.GetLinkR),Move.Item2))) Score = Score + 1;
            return Score;

        }
        private IValue<T> DifrentFace((TValue,TValue) Faces, TValue face)
        {
            IValue<T> result = face;
            if(Faces.Item1.Equals(face)) result = Faces.Item1;
            else result = Faces.Item2;
            return result;
        }

        private class MoveWeighter
        {
            public (Chip<TValue,T>, TValue) Move {get;}
            public double Score {get;}

            public MoveWeighter((Chip<TValue,T>, TValue) Move,double Score)
            {
                this.Move = Move;
                this.Score = Score;
            }
        }
        
    }

}
