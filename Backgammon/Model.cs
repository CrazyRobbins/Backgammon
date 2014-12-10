using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    class Model
    {
        public static bool ModelSelftest()
        {
            return false;
        }

        // Tar emot position och retunerar alla positioner som är giltiga
        public int validMoves(int pos, int d1, int d2, Player player)
        {
            return 0;
        }

        // Retunerar ett slumptal mellan 1-6
        public int dice()
        {
            Random rnd = new Random();
            int dice = rnd.Next(1, 7);
            return dice;
        }

        // Tar bort bokstäver från str och gör om den till en int
        public int StringToInt(string str)
        {
            foreach (char c in str)
            {
                if (!((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')))
                {
                    string C = Convert.ToString(c);
                    str = str.Replace(C, "");
                }
            }

            return Convert.ToInt32(str);
        }

        // Returnerar true om spelare får gå in i mål
        public bool GoalReady(Player player, bool tf)
        {
            bool result = true;
            if (tf)//player1
            {
                for (int i = 0; (i < 18) ; i++)
                {
                    if (player._laces[i] != 0)
                    {
                        result = false;
                    }  
                }
            }

            if (!tf)//player2
            {
                for (int i = 23; (i > 5); i--)
                {
                    if (player._laces[i] != 0)
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        // Bricka blir tagen
        public int brickTaken(Player player1, Player player2)
        {
            return 0;
        }
    }
}
     
