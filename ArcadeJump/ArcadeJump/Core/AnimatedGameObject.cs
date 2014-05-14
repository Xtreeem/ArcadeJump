using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ArcadeJump
{
    class AnimatedGameObject : MovableGameObject
    {
        public Rectangle BottomRectangle;

        private Rectangle textureRectangle; 
        private int currentFrame;
        protected int maxNrFrame; 
        private double animationTimer;
        private double timePerFrame = 0.2;
        protected int frameHeight;
        protected int frameWidht;

        public AnimatedGameObject(Vector2 pos, ContentManager Content)
            : base(pos, Content)
        {
            
        }
        public override void Update(GameTime gametime)
        {
            Animate(gametime);
            base.Update(gametime);
            BottomRectangle.X = Hitbox.X;
            BottomRectangle.Y = Hitbox.Bottom;
        }
        public void Animate(GameTime gameTime)
        {
            animationTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer < 0)
            {
                if (currentFrame == maxNrFrame)
                {
                    currentFrame = 0;
                }
                else
                {
                    textureRectangle = new Rectangle(currentFrame * frameWidht, textureRectangle.Y, frameWidht, frameHeight);
                    animationTimer = timePerFrame;
                    currentFrame++;
                }
            }
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, Hitbox, textureRectangle, color, rotation, origin, spriteEffect, 0);
        }
    }
}

