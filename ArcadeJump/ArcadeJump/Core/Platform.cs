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
        
        float changeWidth;
        float surfaceHeight;

        public Platform(Texture2D tex, Vector2 pos, float changeWidth) : base (tex, pos)
        {
            this.changeWidth = changeWidth;
            surfaceHeight = tex.Height * 0.5f;
            source = new Rectangle(0, 0, tex.Width * (int)changeWidth, tex.Height);
            Hitbox = source;
            SurfaceRectangle = new Rectangle((int)pos.X, (int)pos.Y, (int)changeWidth *tex.Width, (int)surfaceHeight);
            velocity = new Vector2(0, 4);
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            Hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(changeWidth * texture.Width), texture.Height);
            SurfaceRectangle = new Rectangle(Hitbox.X, Hitbox.Y, Hitbox.Width, (int)surfaceHeight);
            //source = new Rectangle((int)position.X, (int)position.Y, (int)(changeWidth*texture.Width), texture.Height);
        }

        public override void Draw(SpriteBatch sB)
        {
            base.Draw(sB);
            sB.Draw(texture, SurfaceRectangle, Color.Red);
            
        }


    }



}
