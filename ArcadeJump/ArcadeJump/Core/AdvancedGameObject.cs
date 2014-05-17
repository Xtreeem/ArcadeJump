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
        //Misc
        protected Texture2D HitBoXDebugTexture;
        protected float Gravitation = 0.7f;
        //Collision Related
        protected int HitBoxXAdjustment = 0;
        protected int HitBoxYAdjustment = 0;
        public Rectangle BottomRectangle;
        private int FallOfGrace = 0;
        protected double DroppingDownTimer;
        //Animation Related
        private Rectangle textureRectangle; 
        protected int currentFrame;
        protected int maxNrFrame; 
        private double animationTimer;
        protected double timePerFrame = 0.1;
        protected int frameHeight;
        protected int frameWidht;
        protected int frameXOffset = 0;
        protected int frameYOffset = 0;
        #endregion

        #region Public Methods
        public AdvancedGameObject(Vector2 pos, ContentManager Content)
            : base(pos, Content)
        {
        }
        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
            Hitbox.X += HitBoxXAdjustment;
            Hitbox.Y += HitBoxYAdjustment;
            FallOfChecker();
            Gravity(gametime);
            OffTheSideChecker();
            Animate(gametime);
            
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
            //spritebatch.Draw(HitBoXDebugTexture, Hitbox, textureRectangle, Color.Red * 0.5f, rotation, origin, spriteEffect, 0);    //Debug Line used to display hitbox
            spritebatch.Draw(texture, DrawRectangle, textureRectangle, Color.White, rotation, origin, spriteEffect, 0);
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
                else if (SurfaceObject.isDead)
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
                velocity.Y = MathHelper.Clamp(velocity.Y + Gravitation, -100, 15);
        }

        private void Animate(GameTime gameTime)
        {
            animationTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer < 0)
            {
                if (currentFrame >= maxNrFrame)
                {
                    currentFrame = 0;
                }
                //else
                //{
                float temp = currentFrame * frameXOffset;
                temp = MathHelper.Clamp((int)temp, 0, frameXOffset);
                    textureRectangle = new Rectangle((currentFrame * frameWidht) + frameXOffset, frameYOffset, frameWidht, frameHeight);
                    animationTimer = timePerFrame;
                    currentFrame++;
                //}
            }
        }

        protected void DropDown()
        {
            DroppingDownTimer = 0.07;
            SurfaceObject = null;
            BottomRectangle = new Rectangle(-300, 0, BottomRectangle.Width, BottomRectangle.Height);
        }
        #endregion

    }
}

