using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private double SurfaceHeight = 10;
        

        public Platform(Vector2 pos, ContentManager Content) : base (pos, Content)
        {
            source = new Rectangle(0, 0, texture.Width, texture.Height);
            Hitbox = source;
            SurfaceRectangle = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, (int)SurfaceHeight);
            velocity = new Vector2(0, 4);
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            Hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width), texture.Height);
            SurfaceRectangle = new Rectangle(Hitbox.X, Hitbox.Y, Hitbox.Width, (int)SurfaceHeight);
            if (position.Y > 1080)
                isDead = true; 
        }

        public override void Draw(SpriteBatch sB)
        {
            base.Draw(sB);
            sB.Draw(texture, SurfaceRectangle, Color.Red); //Debug line to show the surface recangle
            
        }


    }



}
