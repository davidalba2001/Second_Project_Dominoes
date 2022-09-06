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
    
}

