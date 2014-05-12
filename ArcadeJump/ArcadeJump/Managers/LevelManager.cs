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
        List<Platform> platformList;
        Texture2D platformTex;
        Random random;
        float startingHeight = -50f;

        public LevelManager(ref List<Platform> list, ContentManager content)
        {
            this.platformList = list;
            platformTex = content.Load<Texture2D>("Textures/plattform");
            random = new Random();

            for (int i = 0; i < 10; i++)
            {
                CreateNewPlatform(platformTex, GetPosition(), GetLength());
            }
        }

        

        public void Update(GameTime gameTime, double gameTimer)
        {
            foreach (Platform p in platformList)
            {
                if (p.isDead)
                    CreateNewPlatform(platformTex, GetPosition(), GetLength());
            }
        }

        private float GetLength()
        {
            float length = random.Next(20, 100);
            length = length / 100;

            return length;
        }

        #region GetPosition
        private Vector2 GetPosition()
        {
            int nr = random.Next(1,5);
            Vector2 pos = new Vector2();
            if (nr == 1)
            {
                int randPos = random.Next(0, 230);
                pos = new Vector2(randPos, startingHeight);
            }
            else if (nr == 2)
            {
                int randPos = random.Next(231, 460);
                pos = new Vector2(randPos, startingHeight);
            }
            else if (nr == 3)
            {
                int randPos = random.Next(460, 690);
                pos = new Vector2(randPos, startingHeight);
            }
            else if (nr == 4)
            {
                int randPos = random.Next(690, 860);
                pos = new Vector2(randPos, startingHeight);
            }
            return pos;
        }
        #endregion

        public void CreateNewPlatform(Texture2D tex, Vector2 pos, float changeWidth)
        {
            platformList.Add(new Platform(tex, pos, changeWidth));
        }
       
    }
}
