using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    class Model
    {
        private bool _d1 = false;
        private bool _d2 = false;
        Random rnd = new Random();
        private int [] pointX = new int[24];

        public Model()
        { 

            // sparar x kordinater
            pointX[0] = 365;
            pointX[1] = 336;
            pointX[2] =308;
            pointX[3] =279;
            pointX[4] =250;
            pointX[5] =223;
            pointX[6] =143;
            pointX[7] =116;
            pointX[8] =88;
            pointX[9] =58;
            pointX[10] = 30;
            pointX[11] = 0;
            int b = 12;
            for (int i = 11; i >=0; i--)
            {
                pointX[b] = pointX[i];
                b++;
            }

            //skapar players startvärden

        }
        
        public static bool ModelSelftest()
        {
            return false;
        }

        // returnerar true om flytt är giltlig
        public bool check(double newY, double newX, Player inActivePlayer)
        {
            for(int i=0; i<24; i++)
            {
                if (i <= 11 && newY <= 150 && newX < (pointX[i]+28) && newX > (pointX[i]-28) && inActivePlayer._laces[i] != 0)
            { return false; }
                else if (i > 11 && newY > 150 && newX < (pointX[i] + 28) && newX > (pointX[i] - 28) && inActivePlayer._laces[i] != 0) 
            { return false; }
            }
            return true;
        } // check

       // Anropas efter giltlig flytt, uppdaterar till array till rätt spelplan
        public void changeArray(double newY, double newX, double oldY, double oldX, Player activePlayer)
        {

            for (int i = 0; i < 24; i++)
            {
                if (i <= 11 && newY <= 150 && newX < (pointX[i] + 28) && newX > (pointX[i] - 28))
                {
                    activePlayer._laces[i]++;
                }
                else if (i > 11 && newY > 150 && newX < (pointX[i] + 28) && newX > (pointX[i] - 28))
                { 
                    activePlayer._laces[i]++; 
                }
            }

            for (int i = 0; i < 24; i++)
            {
                if (i <= 11 && oldY <= 150 && oldX < (pointX[i] + 28) &&  oldX > (pointX[i] - 28))
                {
                    activePlayer._laces[i]--;
                }
                else if (i > 11 && oldY > 150 && oldX < (pointX[i] + 28) && oldX > (pointX[i] - 28))
                {
                    activePlayer._laces[i]--;
                }
            }
          

        } // changeArray


      
        // Tar emot position och retunerar alla positioner som är giltiga i form av en string
        // exempel: str = "123" alltså plats 1, 2 och 3 går att flytta till
        public string validMoves(int pos, int d1, int d2, Player turnPlayer, Player player2)
        {
            string str;

            if (d1 == d2)
            {
                str = validMovesSame(pos, d1, turnPlayer, player2);
            }
            else
            {
                str = validMovesNotSame(pos, d1, d2, turnPlayer, player2);
            }
            return str;
        }

        private string validMovesSame(int pos, int d1, Player turnPlayer, Player player2)
        {
            return "p";
        }

        private string validMovesNotSame(int pos, int d1, int d2, Player turnPlayer, Player player2)
        {
            
            if (!_d1)
            {
                // Lägger till positionen för d1 i en string
            }
            if (!_d2)
            {
                // Lägger till positionen för d2 i samma string
            }
            return "";
        }

        // Retunerar ett slumptal mellan 1-6
        public int dice()
        {
           
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

        // Kontrollerar om turnPlayer slått ut en av player2's brickor
        public void brickTaken(Player turnPlayer, Player player2)
        {
            for (int i = 0; i < 24; i++)
            {
                if ((turnPlayer._laces[i] == 1) && (player2._laces[i] == 1))
                {
                    player2._laces[i] = 0;
                    player2._out++;
                }
            }
        }
    }
}
     
