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
        private int [] pointsY = new int[23];
        private int[] pointsX = new int[23];


         public Model()
        {
            System.Diagnostics.Debug.WriteLine("oelle"); 
        }

        public static bool ModelSelftest()
        {
            return false;
        }



        // Tar emot position och retunerar alla positioner som är giltiga i form av en string
        // exempel: str = "123" alltså plats 1, 2 och 3 går att flytta till
        //public string validMoves(int pos, int d1, int d2, Player turnPlayer, Player player2)
        //{
        //    string str;

        //    if (d1 == d2)
        //    {
        //        str = validMovesSame(pos, d1, turnPlayer, player2);
        //    }
        //    else
        //    {
        //        str = validMovesNotSame(pos, d1, d2, turnPlayer, player2);
        //    }
        //    return str;
        //}

        //private string validMovesSame(int pos, int d1, Player turnPlayer, Player player2)
        //{
        //    return "p";
        //}

        //private string validMovesNotSame(int pos, int d1, int d2, Player turnPlayer, Player player2)
        //{

        //    if (!_d1)
        //    {
        //        // Lägger till positionen för d1 i en string
        //    }
        //    if (!_d2)
        //    {
        //        // Lägger till positionen för d2 i samma string
        //    }
        //}


        // Returnerar true eller false om senast bricka flyttad är giltlig flyttning 
        private bool check(double newY, double newX, Player activePlayer, Player inactivePlayer)
        {
            for (int i = 0; i < 24; i++)
            {
                if (newY <= pointsY[i] && newX < pointsX[i] && inactivePlayer._laces[i] != 0)
                { return false; }
            }
            return true;
        } // check

        //Ändrar arrayen till "rätt" spelplan. 
        //private void changeArray(double oldY, double oldX)
        //{
        //    string str = _pieceSelected.Name;
        //    double y = _posOfEllipseOnHit.Y;
        //    double x = _posOfEllipseOnHit.X;

        //    if (str[0] == 'b')
        //    {
        //        if (oldY <= 147 && oldX < 243) // Green
        //        { black[0]++; }
        //        else if (oldY <= 147 && oldX >= 243) // Yellow
        //        { black[1]++; }
        //        else if (oldY > 147 && oldX < 243) // Red
        //        { black[2]++; }
        //        else if (oldY > 147 && oldX >= 243) // Blue
        //        { black[3]++; }

        //        if (y <= 147 && x < 243) // Green
        //        { black[0]--; }
        //        else if (y <= 147 && x >= 243) // Yellow
        //        { black[1]--; }
        //        else if (y > 147 && x < 243) // Red
        //        { black[2]--; }
        //        else if (y > 147 && x >= 243) // Blue
        //        { black[3]--; }
        //    }

        //    else if (str[0] == 'w')
        //    {
        //        if (oldY <= 147 && oldX < 243) // Green
        //        { white[0]++; }
        //        if (oldY <= 147 && oldX >= 243) // Yellow
        //        { white[1]++; }
        //        if (oldY > 147 && oldX < 243) // Red
        //        { white[2]++; }
        //        if (oldY > 147 && oldX >= 243) // Blue
        //        { white[3]++; }

        //        if (y <= 147 && x < 243) // Green
        //        { white[0]--; }
        //        else if (y <= 147 && x >= 243) // Yellow
        //        { white[1]--; }
        //        else if (y > 147 && x < 243) // Red
        //        { white[2]--; }
        //        else if (y > 147 && x >= 243) // Blue
        //        { white[3]--; }
        //    }

        //} // changeArray

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
     
