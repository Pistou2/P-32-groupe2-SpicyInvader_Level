﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

namespace SpicyInvader
{

    class SpaceShip
    {
        int x = 35;
        int y = 23;
        public int startShotY;
        public int startShotX;
        const string PLAYERAMMO = "¦";
        const string TOPSHIP = " | | ";
        const string MIDSHIP = "</^\\>";
        const string BOTTOMSHIP = " \\H/ ";
        const int MAXBORDERRIGHT = 75;
        const int MAXBORDERLEFT = 0;
        const int SHOOTSPEED = 30;
        bool isFinish = true;
        string[] SPACESHIP = new string[] { TOPSHIP, MIDSHIP, BOTTOMSHIP };
        public void Move()
        {

            int previousPos = 0;
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Spacebar)
                {
                    if (isFinish)
                    {
                        isFinish = false;
                        new Thread(Shoot).Start();
                    }
                }
                else if (key.Key == ConsoleKey.D || key.Key == ConsoleKey.RightArrow && x < MAXBORDERRIGHT)
                {
                    previousPos = x;
                    x++;
                    RefreshSpaceShip(x, previousPos);
                }
                else if (key.Key == ConsoleKey.A || key.Key == ConsoleKey.LeftArrow && x > MAXBORDERLEFT)
                {
                    previousPos = x;
                    x--;
                    RefreshSpaceShip(x, previousPos);
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();



                }


            } while (true);

        }

        public void spawnSpaceShip()
        {
            Level.Write(x, y - 1, SPACESHIP);
        }

        public void RefreshSpaceShip(int x, int previousPos)
        {
            Level.Erase(previousPos, y - 1, SPACESHIP);
            Level.Write(x, y - 1, SPACESHIP);
        }

        public void Shoot()
        {

            startShotY = y - 1;
            startShotX = x + 2;
            while (!isFinish)
            {
                OnTimedEvent(ref isFinish);
                Thread.Sleep(SHOOTSPEED);
            }

        }

        public void OnTimedEvent(ref bool isFinish)
        {
            int? objectHit = Level.CheckIfObjectHere(startShotX, startShotY);
            if (objectHit == Constant.Level.ID_BARRICADE)
            {
                Level.ShootBaricade(startShotX, startShotY);
                isFinish = true;
            }
            else if (objectHit == Constant.Level.ID_ENNEMY)
            {
                Program.HitEnnemy(startShotX, startShotY);
                isFinish = true;
            }
            else
            {
                Level.Erase(startShotX, startShotY, new string[] { PLAYERAMMO });
                Level.Write(startShotX, startShotY -= 1, new string[] { PLAYERAMMO }, ConsoleColor.Green);

                if (startShotY < 1)
                {
                    Level.Erase(startShotX, startShotY, new string[] { PLAYERAMMO });
                    isFinish = true;

                }
            }
        }
    }
}
