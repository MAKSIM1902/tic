using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace _5.Models
{
    public class Game
    {

        public const int size = 3;
        public const int maxNumberOfUsers = 2;
        public int numberOfUsers = 0;

        public bool visible { get; private set; }

        int[,] field = new int[size, size];
        public int firstMoveId { get; set; }
        public int currentMove { get; set; }

        public string[] id = new string[maxNumberOfUsers]; 

        public Game(string _id)
        {
            Random rnd = new Random();
            firstMoveId = rnd.Next(2);
            currentMove = 0;
            id[0] = _id;
            visible = true;
            ++numberOfUsers;
            for (int i = 0; i < size; ++i)
                for (int j = 0; j < size; ++j)
                    field[i, j] = -1;
        }
        public int AddId(string _id)
        {
            id[numberOfUsers] = _id;
            ++numberOfUsers;
            if(numberOfUsers == maxNumberOfUsers)
            {
                visible = false;
            }
            return numberOfUsers - 1;
        }

        public int WhoIsWin()
        {
            int move = currentMove;
            currentMove = (currentMove + 1) % numberOfUsers;
            int i, j;
            for(i = 0; i < size; ++i)
            {
                for (j = 0; j < size - 1 && field[i, j] == move; ++j);
                if(j == size - 1 && field[i, j] == move)
                {
                    return 1;
                }
            }
            for (i = 0; i < size; ++i)
            {
                for (j = 0; j < size - 1 && field[j, i] == move; ++j);
                if (j == size - 1 && field[j, i] == move)
                {
                    return 1;
                }
            }
            for (i = 0; i < size && field[i, i] == move; ++i);
            if (i == size)
            {
                return 1;
            }
            for (i = 0; i < size && field[size - i - 1, i] == move; ++i);
            if (i == size)
            {
                return 1;
            }
            for (i = 0; i < size; ++i)
                for (j = 0; j < size; ++j)
                    if(field[i,j] == -1)
                    {
                        return -1;
                    }
           return 0;
        }
        public bool Move(int i, int j, int k)
        {
            if(!visible)
            {
                if(currentMove == k)
                {
                    if(field[i, j] == -1)
                    {
                        field[i, j] = k;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
