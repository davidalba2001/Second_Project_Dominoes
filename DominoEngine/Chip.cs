using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominoEngine.Interfaces;

namespace DominoEngine
{
    public class Chip<TValue,T> where TValue : IValue<T>
    {
        public TValue LinkR { get; }
        public TValue LinkL { get; }
        public Chip(TValue linkR, TValue linkL)
        {
            this.LinkR = linkR;
            this.LinkL = linkL;
        }
    }
}