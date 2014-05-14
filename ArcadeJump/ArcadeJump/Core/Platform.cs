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
        private int MinimumWidth = 10;
        private int StartingWidth = 100;
        private int PlatformHeight = 15;
        

        public Platform(Vector2 pos, ContentManager Content, double WidthAdjustment)
            : base(pos, Content)
        {
            velocity = new Vector2(0, 4);
            position = pos;
            texture = Content.Load<Texture2D>("Textures/plattform");
            Hitbox = new Rectangle((int)pos.X, (int)pos.Y, (int)(StartingWidth - (StartingWidth * WidthAdjustment)) + MinimumWidth, PlatformHeight);
            SurfaceRectangle = new Rectangle(Hitbox.X, Hitbox.Y, Hitbox.Width, (int)SurfaceHeight);
        }




        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            
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
