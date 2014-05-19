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
        public double Score;
        bool Stunned = false;
        double StunDuration;
        bool InvertedControls;
        double IdleTimer;
        double AnimationTimer;
        Vector2 LastVelocity;

        bool Punching = false;
        double PunchDelayTimer;
        double PunchLifeTimer;
        double PunchCooldownTimer;
        public Rectangle PunchingRectangle;
        

        Texture2D DebugTexture;
        float PunchCooldown = 0.8f;
        float SlowdownAir = 0.9f;
        float SlowdownGround = 1.5f;
        float SpeedUpAir = 0.5f;
        float SpeedUpGround = 0.5f;
        float MaxXSpeed = 10;



        KeyboardState OldState;


        #endregion

        #region Public Methods
        public Player(Vector2 pos, ContentManager Content, int PlayerNumber)
            : base(pos, Content)
        {
            this.PlayerNumber = PlayerNumber;
            position = pos;
            Score = 0;
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

            DebugTexture = Content.Load<Texture2D>("Textures/plattform");
        }

        public override void Update(GameTime GameTime)
        {
            if (SurfaceObject != null)
                color = Color.Red;
            else
                color = Color.White;
            if(!Stunned)
                Input();
            AnimationManager(GameTime);
            DidIDieCheck();

            LastVelocity = velocity;
            TimerManager(GameTime);
            
            base.Update(GameTime);
            PunchManager(GameTime);
            Score += GameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            if (PunchingRectangle != null)
                spritebatch.Draw(DebugTexture, PunchingRectangle, Color.Red);
        }

        public void GetStunned(double StunDuration)
        {
            Stunned = true;
            this.StunDuration = StunDuration;
            AnimationFallingOver();
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
        private void TimerManager(GameTime GameTime)
        {
            if (PunchCooldownTimer >= 0)
                PunchCooldownTimer -= GameTime.ElapsedGameTime.TotalSeconds;
            if (StunDuration >= 0)
                StunDuration -= GameTime.ElapsedGameTime.TotalSeconds;
            else
                Stunned = false;
        }

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
                    velocity.X = (SurfaceObject != null) ? MathHelper.Clamp(velocity.X + SlowdownGround, -MaxXSpeed, 0) : MathHelper.Clamp(velocity.X + SlowdownAir, -MaxXSpeed, 0);

                if (NewState.IsKeyDown(Keys.D))
                    velocity.X += (SurfaceObject != null) ? SpeedUpGround : SpeedUpAir;
                else if (velocity.X > 0)
                    velocity.X = (SurfaceObject != null) ? MathHelper.Clamp(velocity.X - SlowdownGround, 0, MaxXSpeed) : MathHelper.Clamp(velocity.X - SlowdownAir, 0, MaxXSpeed);

                if (NewState.IsKeyDown(Keys.S))
                {
                    DropDown();
                }
                if (NewState.IsKeyDown(Keys.W) && SurfaceObject != null && !OldState.IsKeyDown(Keys.NumPad5))
                    Jump();

                velocity.X = MathHelper.Clamp(velocity.X, -MaxXSpeed, MaxXSpeed);

                //Debug Sound Test Buttons
                if (NewState.IsKeyDown(Keys.F1) && SurfaceObject != null && !OldState.IsKeyDown(Keys.F1))
                    SoundManager.PlaySound("PlayerJump");
                if (NewState.IsKeyDown(Keys.F2) && SurfaceObject != null && !OldState.IsKeyDown(Keys.F2))
                    SoundManager.PlaySound("PlayerHit");
                if (NewState.IsKeyDown(Keys.F3) && SurfaceObject != null && !OldState.IsKeyDown(Keys.F3))
                    SoundManager.PlaySound("PowerDown");
                if (NewState.IsKeyDown(Keys.F4) && SurfaceObject != null && !OldState.IsKeyDown(Keys.F4))
                    SoundManager.PlaySound("PowerUp");
                if (NewState.IsKeyDown(Keys.F5) && !OldState.IsKeyDown(Keys.F5) && PunchCooldownTimer <= 0)
                {
                    Punch();
                    Console.WriteLine("punch");
                }
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
                    DropDown();
                
                if (NewState.IsKeyDown(Keys.NumPad5) && SurfaceObject != null && !OldState.IsKeyDown(Keys.NumPad5))
                    Jump();
            }
            OldState = NewState;
        }



        #region PunchingStuff
        private void PunchManager(GameTime GameTime)
        {
            if (Punching)
            {
                if (PunchDelayTimer <= 0)
                {
                    if (spriteEffect == SpriteEffects.None)
                        PunchingRectangle = new Rectangle(Hitbox.Center.X, Hitbox.Top + Hitbox.Height / 4, DrawRectangle.Width / 2, 5);
                    else
                        PunchingRectangle = new Rectangle(Hitbox.Center.X - DrawRectangle.Width / 2, Hitbox.Top + Hitbox.Height / 4, DrawRectangle.Width / 2, 5);

                    if (PunchLifeTimer >= 0)
                        PunchLifeTimer -= GameTime.ElapsedGameTime.TotalSeconds;
                    else
                        Punching = false;
                }
                else
                    PunchDelayTimer -= GameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                PunchingRectangle.Width = 0;
                PunchingRectangle.Height = 0;
            }
        }

        private void Punch()
        {
            Punching = true;
            AnimationPunch();
            PunchCooldownTimer = PunchCooldown;
            PunchLifeTimer = (maxNrFrame * timePerFrame) / 2 - ((maxNrFrame * timePerFrame) / 5);
        }
        #endregion

        #region AnimationStuff
        private void AnimationManager(GameTime GameTime)
        {
            if (!Stunned)
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

            }
            else if (AnimationTimer <= 0)
            {
                if (StunDuration <= 0.20)
                {
                    AnimationGettingUp();
                }
                else
                    AnimationLayingDown();
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
            currentFrame = 0;
            AnimationTimer = maxNrFrame * timePerFrame;
        }

        private void AnimationLanding()
        {
            Console.WriteLine("landing");
            timePerFrame = 0.05;
            frameXOffset = 880;
            frameYOffset = 220;
            maxNrFrame = 3;
            currentFrame = 0;
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
            currentFrame = 0;
            AnimationTimer = maxNrFrame * timePerFrame;
        }

        private void AnimationGettingUp()
        {
            timePerFrame = 0.05;
            frameYOffset = 550;
            frameXOffset = 0;
            maxNrFrame = 10;
            currentFrame = 0;
            AnimationTimer = maxNrFrame * timePerFrame;
        }

        private void AnimationLayingDown()
        {
            timePerFrame = 0.05;
            frameYOffset = 440;
            frameXOffset = 770;
            maxNrFrame = 1;
        }

        private void AnimationPunch()
        {
            timePerFrame = 0.05;
            frameYOffset = 660;
            frameXOffset = 0;
            maxNrFrame = 12;
            AnimationTimer = maxNrFrame * timePerFrame;
            PunchDelayTimer = AnimationTimer / 2;
        }
        #endregion
        #endregion
        #endregion
    }
}
