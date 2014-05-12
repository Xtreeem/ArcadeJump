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

        public LevelManager(List<Platform> list, ContentManager content)
        {
            this.platformList = list;
            platformTex = content.Load<Texture2D>("Textures/plattform");
        }

        

        public void Update(GameTime gameTime, double gameTimer)
        { }

        public void CreateNewPlatform()
        {
            platformList.Add(new Platform( platformTex, new Vector2(100, 100), 1f));
        }
       
    }
}
