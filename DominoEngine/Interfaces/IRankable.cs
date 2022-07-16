using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominoEngine.Interfaces
{
    public interface IRankable: IComparable<IRankable>
    {
        public int Rank();
    }
}