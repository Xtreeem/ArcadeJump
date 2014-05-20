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
        Platform LastPlatform;


        float MaximumYDistanceAllowed = 200f;
        int NumberOfColums = 5;
        int IntendedGameLength = 600;
        private int SpawnYInvetervall = 60;
        private int NumberOfPlatforms = 20;
        private int PlatformWidth = 500;
        


        int ColumWidth;
        ContentManager Content;
        Double WidthAdjustment;
        List<Platform> Platforms;
        Random Random;
        #endregion
            
        #region Public Method
        public LevelManager(ref List<Platform> Platforms, ContentManager Content)
        {
            this.Content = Content;
            this.Platforms = Platforms;
            ColumWidth = 1920 / NumberOfColums;
            Random = new Random();
            CreateNewPlatform();
            InitateLevel();
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
        }

        #endregion

        #region Private Methods
        private Vector2 GetPosition()
        {
            Vector2 tempPosition;
            int tempColumNumber = Random.Next(0, NumberOfColums);
            tempPosition.Y = Random.Next(-100-SpawnYInvetervall, -100);
            tempPosition.X = (tempColumNumber * ColumWidth) + Random.Next(-ColumWidth / 2, ColumWidth / 2);
            return tempPosition;
        }

        private void InitateLevel()
        {
            Platforms.Add(new Platform(new Vector2(20, 800), Content, WidthAdjustment, PlatformWidth, true));
            Platforms.Add(new Platform(new Vector2(1860, 800), Content, WidthAdjustment, PlatformWidth, true));
            LastPlatform = Platforms[0];
            Vector2 tempPosition;

            for (int i = 0; i < NumberOfPlatforms; i++)
            {

                int tempColumNumber = Random.Next(0, NumberOfColums);
                tempPosition.Y = Random.Next(0, 1080);
                tempPosition.X = (tempColumNumber * ColumWidth) + Random.Next(-ColumWidth / 2, ColumWidth / 2);
                Platforms.Add(new Platform(tempPosition, Content, WidthAdjustment, PlatformWidth));
            }
        }
        #endregion
    }
}
