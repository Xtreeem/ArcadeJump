using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ArcadeJump
{
    class AdvancedGameObject : MovableGameObject
    {
        #region Variables
        //Collision Related
        public Rectangle BottomRectangle;
        private int FallOfGrace = 0;
        protected double DroppingDownTimer;
        //Animation Related
        private Rectangle textureRectangle; 
        private int currentFrame;
        protected int maxNrFrame; 
        private double animationTimer;
        private double timePerFrame = 0.2;
        protected int frameHeight;
        protected int frameWidht;
        #endregion

        #region Public Methods
        public AdvancedGameObject(Vector2 pos, ContentManager Content)
            : base(pos, Content)
        {
            
        }
        public override void Update(GameTime gametime)
        {
            FallOfChecker();
            Gravity(gametime);
            OffTheSideChecker();
            Animate(gametime);
            base.Update(gametime);
            if (DroppingDownTimer < 0)
            {
                BottomRectangle.X = Hitbox.X;
                BottomRectangle.Y = Hitbox.Bottom;
            }
            else
                DroppingDownTimer -= gametime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, Hitbox, textureRectangle, color, rotation, origin, spriteEffect, 0);
        }
        #endregion

        #region Private Methods
        private void FallOfChecker()
        {
            if (SurfaceObject != null)  //If it has a surfaceobject
            {
                if (Hitbox.Right - FallOfGrace < SurfaceObject.Hitbox.Left)
                    SurfaceObject = null;
                else if (Hitbox.Left + FallOfGrace > SurfaceObject.Hitbox.Right)
                    SurfaceObject = null;
            }
        }

        private void OffTheSideChecker()
        {
            if (position.X > 1920)
            {
                position.X = -(Hitbox.Width - 1);
            }
            if (position.X < -Hitbox.Width)
            {
                position.X = 1919;
            }
        }

        private void Gravity(GameTime gameTime)
        {
            if (SurfaceObject == null)
                velocity.Y = MathHelper.Clamp(velocity.Y + 0.7f, -100, 15);
        }

        private void Animate(GameTime gameTime)
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

        protected void DropDown()
        {
            DroppingDownTimer = 0.05;
            SurfaceObject = null;
            BottomRectangle = new Rectangle(-300, 0, BottomRectangle.Width, BottomRectangle.Height);
        }
        #endregion

    }
}

