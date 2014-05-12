using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    class Platform : MovableGameObject
    {
        public Rectangle SurfaceRectangle;
        public Rectangle Hitbox;
        float changeWidth;

        public Platform(Texture2D tex, Vector2 pos, float changeWidth) : base (tex, pos)
        {
            this.changeWidth = changeWidth;
            source = new Rectangle(0, 0, tex.Width * (int)changeWidth, tex.Height);
        }


    }



}
