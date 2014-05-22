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
        //Misc
        private ContentManager Content;
        public Double WidthAdjustment;      //Gradually increasing modifier shrinking the platforms
        private List<PowerUp> PowerUps;
        private List<Platform> Platforms;
        private Random Random;
        public Platform LastPlatform;
        //Balancing Variables
        private float MaximumYDistanceAllowed = 150f;   //The maximum distance Y allowed between a new platform and the one before it
        private float MinimumPlatformYDistance = 40;
        private float MinimumPlatformXDistance = 250;
        public int IntendedGameLength = 600;
        private int MaxXDistance = 510;                 //The maximum distance X allowed between a new platform and the one before it 
        private int SpawnYInvetervall = 150;            //The height of the intervall new platforms will try to spawn from the old one
        private int NumberOfPlatforms = 20;
        private int PlatformWidth = 200;
        private int ChanceToSpawnPowerup = 50;
        #endregion

        #region Public Method
        public LevelManager(ref List<PowerUp> PowerUps, ref List<Platform> Platforms, ContentManager Content)
        {
            this.Content = Content;
            this.Platforms = Platforms;
            this.PowerUps = PowerUps;
            Random = new Random();
            InitateLevel();
            CreateNewPlatform();
            Platforms.Add(new Platform(new Vector2(0, 1000), Content, 0, 1920, true));
        }

        public void Update(double ElapsedGameTime)
        {
            WidthAdjustment = ElapsedGameTime / IntendedGameLength;
        }

        /// <summary>
        /// Function called to create a new platform at a valid position
        /// </summary>
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

        /// <summary>
        /// Function called to create a random PowerUp ontop of the latest Platform
        /// </summary>
        public void CreateNewPowerUp()
        {
            bool Selector;
            Vector2 TempVelocity;
            int Index = Random.Next(0, 101);
            bool tempLocked;

            if (Random.Next(0, 101) < 50)
                tempLocked = true;
            else
                tempLocked = false;

            if (Random.Next(0, 101) < 50)
                TempVelocity = new Vector2(Random.Next(-2, -1), 0);
            else
                TempVelocity = new Vector2(Random.Next(1, 3), 0);

            if (Random.Next(0, 101) < 50)
                Selector = true;
            else
                Selector = false;

            if (Selector)
            {
                if (Index < 20)
                {
                    PowerUps.Add(new PuStun(new Vector2(LastPlatform.Hitbox.Center.X, LastPlatform.Hitbox.Top - 30), Content, TempVelocity));
                }
                else if (Index < 40)
                {
                    PowerUps.Add(new PuSuperJump(new Vector2(LastPlatform.Hitbox.Center.X, LastPlatform.Hitbox.Top - 30), Content, TempVelocity));
                }
                else if (Index < 60)
                {
                    PowerUps.Add(new PuPoints(new Vector2(LastPlatform.Hitbox.Center.X, LastPlatform.Hitbox.Top - 30), Content, TempVelocity));
                }
                else if (Index < 80)
                {
                    PowerUps.Add(new PuInvertedControlls(new Vector2(LastPlatform.Hitbox.Center.X, LastPlatform.Hitbox.Top - 30), Content, TempVelocity));
                }
                else if (Index < 100)
                {
                    PowerUps.Add(new PuShield(new Vector2(LastPlatform.Hitbox.Center.X, LastPlatform.Hitbox.Top - 30), Content, TempVelocity));
                }
            }
            else
            {
                if (Index < 20)
                {
                    PowerUps.Add(new PuStun(LastPlatform, Content, TempVelocity, tempLocked));
                }
                else if (Index < 40)
                {
                    PowerUps.Add(new PuSuperJump(LastPlatform, Content, TempVelocity, tempLocked));
                }
                else if (Index < 60)
                {
                    PowerUps.Add(new PuPoints(LastPlatform, Content, TempVelocity, tempLocked));
                }
                else if (Index < 80)
                {
                    PowerUps.Add(new PuInvertedControlls(LastPlatform, Content, TempVelocity, tempLocked));
                }
                else if (Index < 100)
                {
                    PowerUps.Add(new PuShield(LastPlatform, Content, TempVelocity, tempLocked));
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Function called to return a random valid position to spawn the next platform
        /// </summary>
        private Vector2 GetPosition()
        {
            Vector2 tempPosition;
            do                                             //Will keep randoming a new X position untill a valid one has been aquired
            {
                tempPosition.X = GetXPosition();
            } while (!ValidateXPosition(tempPosition.X));

            tempPosition.Y = GetYPosition();                //Randoms up a Y position inside the given intervall
            return tempPosition;
        }

        /// <summary>
        /// Function that will random a X position on screen
        /// </summary>
        private float GetXPosition()
        {
            return (Random.Next(0, (1920 - LastPlatform.Hitbox.Width)));
        }

        /// <summary>
        /// Function used to ensure that the player will always be able to jump 
        /// from the latest platform to a platform anchored at the input X value
        /// </summary>
        /// <param name="testX">The X Value to try</param>
        /// <returns>True if the value is within range</returns>
        private bool ValidateXPosition(float testX)
        {
            float X, Y, Z;
            if (LastPlatform.position.X > testX)                                    //Checks to see if the new platform tries to appear to the left of the old one
                X = LastPlatform.position.X - (testX + LastPlatform.Hitbox.Width);  //Measures the distance between the left most point of the old platform to the right most point of the new one
            else
                X = LastPlatform.position.X + LastPlatform.Hitbox.Width - testX;    //Measures the distance between the right most point of the old platform to the left most point of the new one
            if (X < 0)                                                              //Converts any negative number to a posetivt for comparison
                X *= (-1);
            Y = LastPlatform.position.X;                                            //Checks the new platforms distance to the left side of the screen
            Z = 1920 - (testX + LastPlatform.Hitbox.Width);                         //Calculates the distance from the new platforms right most point to the right edge of the screen
            if (X < MaxXDistance && X > MinimumPlatformXDistance)                   //Checks if the new platform is close enough in one jump
                return true;                                                        //Validates the new position
            if (Y + Z < MaxXDistance)                                               //Checks if the new platform is close enough to jump across the border of the screen and still reach
                return true;                                                        //Validates the new position
            return false;                                                           //If all else fails the new position is rejected
        }

        /// <summary>
        /// Generates a Y coordinate that is close enough to the old one to be allowed
        /// </summary>
        private float GetYPosition()
        {
            float tempFloat;
            tempFloat = LastPlatform.position.Y;
            tempFloat -= (MinimumPlatformYDistance + Random.Next(0, SpawnYInvetervall));
            tempFloat = MathHelper.Clamp(tempFloat, LastPlatform.position.Y - MaximumYDistanceAllowed, LastPlatform.position.Y - MinimumPlatformYDistance);
            return tempFloat;
        }

        /// <summary>
        /// Function called to spawn the initial platforms seen on screen
        /// </summary>
        private void InitateLevel()
        {
            Platforms.Add(new Platform(new Vector2(0, 1920), Content, WidthAdjustment, PlatformWidth, true));
            Platforms.Add(new Platform(new Vector2(20, 800), Content, WidthAdjustment, PlatformWidth, true));
            Platforms.Add(new Platform(new Vector2(1860, 800), Content, WidthAdjustment, PlatformWidth, true));
            LastPlatform = Platforms[0];
            for (int i = 0; i < NumberOfPlatforms; i++)
            {
                CreateNewPlatform();
            }
        }
        #endregion
    }
}
