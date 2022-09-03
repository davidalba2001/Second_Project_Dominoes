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
            while(true)
            {
            (int CountChip, int LinkedValues, int countPlayer, int maxNumChip, int ChipForPlayer, int GameType) Customs = InterPrints.Customitation();
            //Aqui se construye el juego segun las respuestas del usuario
                TypeGame typeGame = (TypeGame)Customs.GameType;
                switch (typeGame)
                {
                    case TypeGame.ClasicDominos:
                        { 
                            IWinCondition<Numeric,int>[] winConditions =  {new WinnerByPuntos<Numeric, int>(), new PlayAllChips<Numeric, int>()};
                            IEndCondition<Numeric,int>[] finalConditions =  { new IsLocked<Numeric, int>(), new PlayAllChips<Numeric, int>() };
                            (Rules<Numeric,int> Rules,List<Player<Numeric,int>> Players ) rulesAndPlayer = CustomPlayerAndRules(Customs.countPlayer,winConditions,finalConditions);
                            ClassicGameLogic<Numeric, int> ClasicDominos = new(Customs.LinkedValues,rulesAndPlayer.Rules, Values.ValuesNumerics,rulesAndPlayer.Players);
                            //Echa a andar el juego
                            NewGame<Numeric, int>(ClasicDominos,Customs.ChipForPlayer);
                            break;
                        }
                    case TypeGame.PrittyBoy:
                        {

                            IWinCondition<Emojis, string>[] winConditions = { new WinnerByChips<Emojis, string>(), new PlayAllChips<Emojis, string>() };
                            IEndCondition<Emojis, string>[] finalConditions = { new IsLocked<Emojis, string>(), new PlayAllChips<Emojis, string>() };
                            (Rules<Emojis, string> Rules,List<Player<Emojis, string>> Players ) rulesAndPlayer = CustomPlayerAndRules(Customs.countPlayer,winConditions,finalConditions);
                            ClassicGameLogic<Emojis, string> PrittyBoy = new(Customs.LinkedValues,rulesAndPlayer.Rules, Values.ValuesEmojis,rulesAndPlayer.Players);
                            //Echa a andar el juego
                            NewGame<Emojis, string>(PrittyBoy,Customs.ChipForPlayer);
                            break;
                        }
                    case TypeGame.Stolen:
                        {
                               IWinCondition<Numeric, int>[] winConditions = { new WinnerByPuntos<Numeric, int>(), new PlayAllChips<Numeric, int>() };
                            IEndCondition<Numeric, int>[] finalConditions = { new IsLocked<Numeric, int>(), new PlayAllChips<Numeric, int>() };
                                (Rules<Numeric,int> Rules,List<Player<Numeric,int>> Players ) rulesAndPlayer = CustomPlayerAndRules(Customs.countPlayer,winConditions,finalConditions);
                            StolenLogic<Numeric, int> Stolen = new(Customs.LinkedValues,rulesAndPlayer.Rules, Values.ValuesNumerics,rulesAndPlayer.Players);
                            //Echa a andar el juego
                            NewGame<Numeric, int>(Stolen,Customs.ChipForPlayer);
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
        public (Rules<TValue,T>,List<Player<TValue,T>>) CustomPlayerAndRules<TValue, T> (int countPlayer,IWinCondition<TValue,T>[] winConditions,IEndCondition<TValue,T>[] finalConditions) where TValue : IValue<T>,IRankable
        {
                            List<Player<TValue,T>> players = new();
                            ICollection<string> typePlayer = Enum.GetNames(typeof(TypePlayer));


                            for (int i = 0; i <countPlayer; i++)
                            {
                                int selectTypePlayer = InterPrints.PrintSelect(typePlayer, "Player type",0, typePlayer.Count);
                                InterPrints.AddPlayer(players, selectTypePlayer, i);
                            }
                            Rules<TValue,T> rules = new(winConditions, finalConditions);
                            return (rules,players);
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
      
    }
}
