using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    class LevelManager
    {
        #region Variables
        public Platform LastPlatform;


        private float MaximumYDistanceAllowed = 150f;
        private float MinimumPlatformYDistance = 40;
        private float MinimumPlatformXDistance = 250;
        private int NumberOfColums = 10;
        public int IntendedGameLength = 300;
        private int MaxXDistance = 510;
        private int SpawnYInvetervall = 150;
        private int NumberOfPlatforms = 20;
        private int PlatformWidth = 200;
        private int ChanceToSpawnPowerup = 50;


        int ColumWidth;
        ContentManager Content;
        Double WidthAdjustment;
        List<PowerUp> PowerUps;
        List<Platform> Platforms;
        Random Random;
        #endregion
            
        #region Public Method
        public LevelManager(ref List<PowerUp> PowerUps, ref List<Platform> Platforms, ContentManager Content)
        {
            this.Content = Content;
            this.Platforms = Platforms;
            this.PowerUps = PowerUps;
            ColumWidth = 1920 / NumberOfColums;
            Random = new Random();
            InitateLevel();
            CreateNewPlatform();
            Platforms.Add(new Platform(new Vector2(0,1000), Content, 0, 1920, true));
        }

        public void Update(double ElapsedGameTime)
        {
          WidthAdjustment = ElapsedGameTime / IntendedGameLength;
        }

        public void CreateNewPlatform()
        {
            Platform tempPlatform = new Platform(GetPosition(), Content, WidthAdjustment, PlatformWidth);
            Platforms.Add(tempPlatform);
            LastPlatform = tempPlatform;
            if (Random.Next(0, 101) < ChanceToSpawnPowerup)
            {
                CreateNewPowerUp();
            }
        }
        public void CreateNewPowerUp()
        {
            PowerUp temp;
            bool Selector;
            int Index = Random.Next(0,101); 
           bool tempLocked;
            if (Random.Next(0, 101) < 50)
                tempLocked = true;
            else
                tempLocked = false;
            Vector2 tempVelocity;
            if (Random.Next(0, 101) < 50)
               tempVelocity = new Vector2(Random.Next(-2, -1), 0);
            else
                tempVelocity = new Vector2(Random.Next(1,3), 0);

            if (Random.Next(0, 101) < 50)
                Selector = true;
                else
            Selector = false;

            if (Selector)
            {
                if (Index < 50)
                {
                    PowerUps.Add(new PuStun(new Vector2(LastPlatform.Hitbox.Center.X, LastPlatform.Hitbox.Top - 30), Content, tempVelocity));
                }
                else if (Index < 100)
                {
                    PowerUps.Add(new PuSuperJump(new Vector2(LastPlatform.Hitbox.Center.X, LastPlatform.Hitbox.Top - 30), Content, tempVelocity));
                }

            }
            else
            {
                if (Index < 50)
                {
                    PowerUps.Add(new PuStun(LastPlatform, Content, tempVelocity, tempLocked));
                }
                else if (Index < 100)
                {
                    PowerUps.Add(new PuSuperJump(LastPlatform, Content, tempVelocity, tempLocked));
                }
            }


                
                
            


        }
            
        #endregion

        #region Private Methods
        private Vector2 GetPosition()
        {
            Vector2 tempPosition;
            do
            {
                tempPosition.X = GetXPosition();   
            } while (!ValidateXPosition(tempPosition.X));
            
            tempPosition.Y = GetYPosition();
            //tempPosition.Y = Random.Next(-100 - SpawnYInvetervall, -100);
            //TODO Fixa så att dom bara kna vara en tredjedel ifrån skärmkant och en halv skärm ifrån senaste spawnade platform
            //if (tempPosition.X > 
            return tempPosition;
        }

        private float GetXPosition()
        {
            int tempColumNumber = Random.Next(0, NumberOfColums);
            return (Random.Next(0, (1920 - LastPlatform.Hitbox.Width)));
            
        }

        private bool ValidateXPosition(float testX)
        {
            float X, Y, Z;
            if (LastPlatform.position.X > testX)
                X = LastPlatform.position.X - (testX + LastPlatform.Hitbox.Width);
            else
                X = LastPlatform.position.X + LastPlatform.Hitbox.Width - testX;
            if (X < 0)
                X *= (-1);
            Y = LastPlatform.position.X;
            Z = 1920 - (testX + LastPlatform.Hitbox.Width);
            if (X < MaxXDistance && X > MinimumPlatformXDistance)
                return true;
            if (Y + Z < MaxXDistance)
                return true;
            return false;
        }

        private float GetYPosition()
        {
            float tempFloat;
            tempFloat = LastPlatform.position.Y;
            tempFloat -= (MinimumPlatformYDistance + Random.Next(0, SpawnYInvetervall));
            tempFloat = MathHelper.Clamp(tempFloat, LastPlatform.position.Y - MaximumYDistanceAllowed, LastPlatform.position.Y - MinimumPlatformYDistance);
            return tempFloat;
        }

        private void InitateLevel()
        {
            Platforms.Add(new Platform(new Vector2(0, 1920), Content, WidthAdjustment, PlatformWidth, true));
            Platforms.Add(new Platform(new Vector2(20, 800), Content, WidthAdjustment, PlatformWidth, true));
            Platforms.Add(new Platform(new Vector2(1860, 800), Content, WidthAdjustment, PlatformWidth, true));
            LastPlatform = Platforms[0];
            Vector2 tempPosition;

            for (int i = 0; i < NumberOfPlatforms; i++)
            {

                CreateNewPlatform();
                //int tempColumNumber = Random.Next(0, NumberOfColums);
                //tempPosition.Y = Random.Next(0, 1080);
                //tempPosition.X = (tempColumNumber * ColumWidth) + Random.Next(-ColumWidth / 2, ColumWidth / 2);
                //Platforms.Add(new Platform(tempPosition, Content, WidthAdjustment, PlatformWidth));
            }
        }
        #endregion
    }
}
