using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public class Chip<T> 
    {
        public IValue<T> LinkR{get;}
        public IValue<T> LinkL{get;}
        public Chip(IValue<T> linkR, IValue<T> linkL)
        {
            this.LinkR = linkR;
            this.LinkL = linkL;
        }
    }
}