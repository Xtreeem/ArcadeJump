using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcadeJump
{
    class AnimatedGameObject : MovableGameObject
    {
        private int numFrame;
        public Rectangle textureRectangle; 
        private int currentFrame;
        private int maxNrFrame; 
        private double animationTimer;
        private double timePerFrame = 0.2;
        protected int frameHeight;
        protected int frameWidht;

        public AnimatedGameObject(Texture2D tex, Vector2 pos)
            : base(tex, pos)
        { }
        public override void Update(GameTime gametime)
        {
            Animate(gametime);
            base.Update(gametime);
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
                    currentFrame++;
                    textureRectangle = new Rectangle(currentFrame*frameWidht,textureRectangle.Y,texture.Width,texture.Height);
                    animationTimer = timePerFrame;
                }
            }
        }
    }
}

