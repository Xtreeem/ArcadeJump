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
        float surfaceHeight;

        public Platform(Texture2D tex, Vector2 pos, float changeWidth) : base (tex, pos)
        {
            this.changeWidth = changeWidth;
            surfaceHeight = tex.Height * 0.2f;
            source = new Rectangle(0, 0, tex.Width * (int)changeWidth, tex.Height);
            Hitbox = source;
            SurfaceRectangle = new Rectangle(0, 0, (int)changeWidth *tex.Width, (int)surfaceHeight);
            velocity = new Vector2(0, 4);
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
        }

        public override void Draw(SpriteBatch sB)
        {
            sB.Draw(texture, position, SurfaceRectangle, Color.Red);
        }


    }



}
