using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    class Player
    {
        public int[] _laces;
        private int _bricksAmount;

        public Player()
        {
            _laces = new int[24];
            _bricksAmount = 15;
        }

        public int BricksAmount
        {
            get { return _bricksAmount; }
        }

    }
}

 
