using Microsoft.Xna.Framework;
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
        public LevelManager(List<Platform> list)
        {
            this.platformList = list;
        }

        public void Update(GameTime gameTime, double gameTimer)
        { }

       
    }
}
