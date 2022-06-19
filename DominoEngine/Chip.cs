using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine
{
    public class Chip<TValue> where TValue : IEquatable<TValue>
    {
        public TValue LinkR{get;}
        public TValue LinkL{get;}
        public Chip(TValue linkR, TValue linkL)
        {
            LinkR = linkR;
            LinkL = linkL;
        }
    }
}