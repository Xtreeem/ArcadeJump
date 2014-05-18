using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    class Player : AdvancedGameObject
    {
        #region Variables
        public int PlayerNumber;
        float JumpPower = 20;
        PowerUp CurrentPowerUp;
        int Score;
        bool Stunned;
        double StunDuration;
        bool InvertedControls;
        double IdleTimer;
        double AnimationTimer;
        Vector2 LastVelocity;

        float SlowdownAir = 0.9f;
        float SlowdownGround = 1.5f;
        float SpeedUpAir = 0.5f;
        float SpeedUpGround = 0.5f;

        KeyboardState OldState;

        
        #endregion

        #region Public Methods
        public Player(Vector2 pos, ContentManager Content, int PlayerNumber)
            : base(pos, Content)
        {
            this.PlayerNumber = PlayerNumber;
            position = pos;
            HitBoxXAdjustment = 7;
            HitBoxYAdjustment = 0;
            texture = Content.Load<Texture2D>("Textures/Test");
            HitBoXDebugTexture = Content.Load<Texture2D>("Textures/Test2");
            Hitbox = new Rectangle((int)position.X, (int)position.Y, 15, 70);
            DrawRectangle = new Rectangle((int)position.X, (int)position.Y, 30, 70);
            BottomRectangle = new Rectangle(Hitbox.X, Hitbox.Bottom, Hitbox.Width, 5);



            velocity.Y = 0.001f;
            timePerFrame = 0.08;
            frameHeight = 110;
            frameWidht = 110;

            //frameXOffset = 0;
            //frameYOffset = 110;
            //maxNrFrame = 7;
            
        }

        public override void Update(GameTime GameTime)
        {
            if (SurfaceObject != null)
                color = Color.Red;
            else
                color = Color.White;
            Input();
            AnimationManager(GameTime);
            DidIDieCheck();
            
            LastVelocity = velocity;
            base.Update(GameTime);
        }

        private void DidIDieCheck()
        {
            if (position.Y > 1100)
                isDead = true;
        }

        public void Jump()
        {
            Console.WriteLine("Jump");
            if (SurfaceObject != null)
                AnimationJumping();
            SurfaceObject = null;
            velocity.Y -= JumpPower;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Function designed to Scan for the control inputs and move the player accordingly
        /// </summary>
        private void Input()
        {
            var NewState = Keyboard.GetState();

            if (PlayerNumber == 1)
            {
                if (NewState.IsKeyDown(Keys.A))
                    velocity.X -= (SurfaceObject != null) ? SpeedUpGround : SpeedUpAir;
                else if (velocity.X < 0)
                    velocity.X = (SurfaceObject != null) ? MathHelper.Clamp(velocity.X + SlowdownGround, -100, 0) : MathHelper.Clamp(velocity.X + SlowdownAir, -100, 0);

                if (NewState.IsKeyDown(Keys.D))
                    velocity.X += (SurfaceObject != null) ? SpeedUpGround : SpeedUpAir;
                else if (velocity.X > 0)
                    velocity.X = (SurfaceObject != null) ? MathHelper.Clamp(velocity.X - SlowdownGround, 0, 100) : MathHelper.Clamp(velocity.X - SlowdownAir, 0, 100);

                if (NewState.IsKeyDown(Keys.S))
                {
                    DropDown();
                }

                if (NewState.IsKeyDown(Keys.W) && SurfaceObject != null && !OldState.IsKeyDown(Keys.NumPad5))
                    Jump();


                if (NewState.IsKeyDown(Keys.F1) && SurfaceObject != null && !OldState.IsKeyDown(Keys.NumPad5))
                    SoundManager.PlaySound("PlayerJump");
                if (NewState.IsKeyDown(Keys.F2) && SurfaceObject != null && !OldState.IsKeyDown(Keys.NumPad5))
                    SoundManager.PlaySound("PlayerHit");
                if (NewState.IsKeyDown(Keys.F3) && SurfaceObject != null && !OldState.IsKeyDown(Keys.NumPad5))
                    SoundManager.PlaySound("PowerDown");
                if (NewState.IsKeyDown(Keys.F4) && SurfaceObject != null && !OldState.IsKeyDown(Keys.NumPad5))
                    SoundManager.PlaySound("PowerUp");



            }

            else
            {
                if (NewState.IsKeyDown(Keys.NumPad1))
                    velocity.X -= (SurfaceObject != null) ? SpeedUpGround : SpeedUpAir;
                else if (velocity.X < 0)
                    velocity.X = (SurfaceObject != null) ? MathHelper.Clamp(velocity.X + SlowdownGround, -100, 0) : MathHelper.Clamp(velocity.X + SlowdownAir, -100, 0);

                if (NewState.IsKeyDown(Keys.NumPad3))
                    velocity.X += (SurfaceObject != null) ? SpeedUpGround : SpeedUpAir;
                else if (velocity.X > 0)
                    velocity.X = (SurfaceObject != null) ? MathHelper.Clamp(velocity.X - SlowdownGround, 0, 100) : MathHelper.Clamp(velocity.X - SlowdownAir, 0, 100);

                if (NewState.IsKeyDown(Keys.NumPad2))
                {
                    DropDown();
                }

                if (NewState.IsKeyDown(Keys.NumPad5) && SurfaceObject != null && !OldState.IsKeyDown(Keys.NumPad5))
                    Jump();
            }

            OldState = NewState;
        }

        private void AnimationManager(GameTime GameTime)
        {
            if (AnimationTimer <= 0)
            {
                if (velocity == Vector2.Zero)
                    IdleTimer += GameTime.ElapsedGameTime.TotalSeconds;
                else IdleTimer = 0;

                if (velocity.Y < 0 && velocity.Y > 0 - (6 * Gravitation))
                    AnimationLevelingOut();
                else if (LastVelocity.Y != 0 && velocity.Y == 0)
                    AnimationLanding();
                else if (velocity.Y < 0)
                    AnimationAscending();
                else if (velocity.Y > 0)
                    AnimationDescending();
                else if (velocity.X != 0)
                    AnimationRunning();
                else if (IdleTimer > 3 && velocity == Vector2.Zero)
                {
                    AnimationProlongedIdle();
                    if (currentFrame + 1 > maxNrFrame)
                        IdleTimer = 0;
                }
                else
                    AnimationIdle();
            }
                if (AnimationTimer > 0)
                    AnimationTimer -= GameTime.ElapsedGameTime.TotalSeconds;
                else
                {
                    AnimationTimer = 0;
                }
                
        }
        #region Animations
        private void AnimationClearAnimation()
        {
            frameXOffset = 0;
            frameYOffset = 110;
            maxNrFrame = 1;
            AnimationTimer = 0;
        }

        private void AnimationAscending()
        {
            timePerFrame = 0.09;
            frameXOffset = 330;
            frameYOffset = 220;
            maxNrFrame = 1;
        }

        private void AnimationLevelingOut()
        {
            frameXOffset = 330;
            frameYOffset = 220;
            maxNrFrame = 4;
        }

        private void AnimationDescending()
        {
            frameXOffset = 660;
            frameYOffset = 220;
            maxNrFrame = 1;
        }

        private void AnimationJumping()
        {
            timePerFrame = 0.09;
            frameXOffset = 0;
            frameYOffset = 220;
            maxNrFrame = 3;
            AnimationTimer = maxNrFrame * timePerFrame;
        }

        private void AnimationLanding()
        {
            Console.WriteLine("landing");
            timePerFrame = 0.05;
            frameXOffset = 880;
            frameYOffset = 220;
            maxNrFrame = 3;
            AnimationTimer = maxNrFrame * timePerFrame;
        }

        private void AnimationIdle()
        {
            frameXOffset = 0;
            frameYOffset = 110;
            maxNrFrame = 1;
        }

        private void AnimationProlongedIdle()
        {
            timePerFrame = 0.08;
            frameXOffset = 0;
            frameYOffset = 110;
            maxNrFrame = 7;
        }

        private void AnimationRunning()
        {
            timePerFrame = 0.05;
            frameYOffset = 0;
            frameXOffset = 0;
            maxNrFrame = 7;
        }

        private void AnimationFallingOver()
        {
            timePerFrame = 0.05;
            frameYOffset = 440;
            frameXOffset = 0;
            maxNrFrame = 8;
            AnimationTimer = maxNrFrame * timePerFrame;
        }

        private void AnimationGettingUp()
        {
            timePerFrame = 0.05;
            frameYOffset = 550;
            frameXOffset = 0;
            maxNrFrame = 10;
            AnimationTimer = maxNrFrame * timePerFrame;
        }

        private void AnimationLayingDown()
        {
            timePerFrame = 0.05;
            frameYOffset = 440;
            frameXOffset = 770;
            maxNrFrame = 1;
        }
        #endregion




        #endregion
    }
}
