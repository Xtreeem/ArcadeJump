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
        public double Score;
        PowerUp CurrentPowerUp;
        KeyboardState OldState;
        Texture2D DebugTexture;

        bool Stunned = false;
        double StunDuration;
        bool InvertedControls;

        double IdleTimer;
        double AnimationTimer;
        Vector2 LastVelocity;

        bool Busy = false;

        bool Punching = false;
        double PunchingGraceTimer;
        double PunchDelayTimer;
        double PunchLifeTimer;
        double PunchCooldownTimer;
        public Rectangle PunchingRectangle;

        bool Kicking = false;
        double KickingGraceTimer;
        double KickDelayTimer;
        double KickLifeTimer;
        double KickCooldownTimer;
        public Rectangle KickingRectangle;


        public float Kickpower = 15;
        float KickingGrace = 0.5f;
        float PunchingGrace = 0.5f;
        float KickCooldown = 0.8f;
        float PunchCooldown = 0.8f;
        float SlowdownAir = 0.9f;
        float SlowdownGround = 1.5f;
        float SLowdownStunned = 0.3f;
        float SpeedUpAir = 0.5f;
        float SpeedUpGround = 0.5f;
        float MaxXSpeed = 10;
        float JumpPower = 20;
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
            HitBoXDebugTexture = Content.Load<Texture2D>("Textures/DebugTexture");
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
            Input();
            AnimationManager(GameTime);
            DidIDieCheck();

            LastVelocity = velocity;
            TimerManager(GameTime);

            base.Update(GameTime);
            PunchManager(GameTime);
            KickManager(GameTime);
            BusyManager();
            Score += GameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            //if (PunchingRectangle != null)
            //    spritebatch.Draw(DebugTexture, PunchingRectangle, Color.Red);
            //if (KickingRectangle != null)
            //    spritebatch.Draw(DebugTexture, KickingRectangle, Color.Black);
            //spritebatch.Draw(HitBoXDebugTexture, Hitbox, Color.Black);            //Debug Hitbox Display
        }

        public void GetStunned(double StunDuration)
        {
            if (!Stunned)
            {
                Stunned = true;
                this.StunDuration = StunDuration;
                AnimationFallingOver();
            }
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
            if (KickCooldownTimer >= 0)
                KickCooldownTimer -= GameTime.ElapsedGameTime.TotalSeconds;
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
                if (!Stunned)
                {
                    //Player Input to move Left
                    if ((InvertedControls) ? NewState.IsKeyDown(Keys.D) : NewState.IsKeyDown(Keys.A))
                        velocity.X -= (SurfaceObject != null) ? SpeedUpGround : SpeedUpAir;
                    else if (velocity.X < 0)
                        velocity.X = (SurfaceObject != null) ? MathHelper.Clamp(velocity.X + SlowdownGround, -MaxXSpeed, 0) : MathHelper.Clamp(velocity.X + SlowdownAir, -MaxXSpeed, 0);

                    //Player Input to move Right
                    if ((InvertedControls) ? NewState.IsKeyDown(Keys.A) : NewState.IsKeyDown(Keys.D))
                        velocity.X += (SurfaceObject != null) ? SpeedUpGround : SpeedUpAir;
                    else if (velocity.X > 0)
                        velocity.X = (SurfaceObject != null) ? MathHelper.Clamp(velocity.X - SlowdownGround, 0, MaxXSpeed) : MathHelper.Clamp(velocity.X - SlowdownAir, 0, MaxXSpeed);
                    //Player Input to DropDown
                    if ((InvertedControls) ? NewState.IsKeyDown(Keys.W) : NewState.IsKeyDown(Keys.S))
                    {
                        DropDown();
                    }
                    //Player Input to Jump
                    if (
                        ((InvertedControls) ? NewState.IsKeyDown(Keys.S) : NewState.IsKeyDown(Keys.W)) &&
                        (SurfaceObject != null) &&
                        ((InvertedControls) ? !OldState.IsKeyDown(Keys.S) : !OldState.IsKeyDown(Keys.W))
                        )
                        Jump();
                    //Player Input to Punch
                    if (NewState.IsKeyDown(Keys.V) && !OldState.IsKeyDown(Keys.V) && PunchCooldownTimer <= 0 && !Busy)
                    {
                        Punch();
                        Console.WriteLine("punch");
                    }
                    //Player Input to Kick
                    if (NewState.IsKeyDown(Keys.B) && !OldState.IsKeyDown(Keys.B) && KickCooldownTimer <= 0 && !Busy)
                    {
                        Kick();
                        Console.WriteLine("kick");
                    }
                }
                else
                {
                    if (velocity.X < 0)
                        velocity.X = MathHelper.Clamp(velocity.X + SLowdownStunned, -MaxXSpeed, 0);
                    else if (velocity.X > 0)
                        velocity.X = MathHelper.Clamp(velocity.X - SLowdownStunned, 0, MaxXSpeed);
                }



                //Debug Testing buttons
                if (NewState.IsKeyDown(Keys.F1) && SurfaceObject != null && !OldState.IsKeyDown(Keys.F1))
                    SoundManager.PlaySound("PlayerJump");
                if (NewState.IsKeyDown(Keys.F2) && SurfaceObject != null && !OldState.IsKeyDown(Keys.F2))
                    SoundManager.PlaySound("PlayerHit");
                if (NewState.IsKeyDown(Keys.F3) && SurfaceObject != null && !OldState.IsKeyDown(Keys.F3))
                    SoundManager.PlaySound("PowerDown");
                if (NewState.IsKeyDown(Keys.F4) && SurfaceObject != null && !OldState.IsKeyDown(Keys.F4))
                    SoundManager.PlaySound("PowerUp");
                if (NewState.IsKeyDown(Keys.F7) && SurfaceObject != null && !OldState.IsKeyDown(Keys.F7))
                {
                    if (InvertedControls)
                        InvertedControls = false;
                    else
                        InvertedControls = true;
                }

            }

            else
            {
                if (!Stunned)
                {
                    //Player Input to move Left
                    if ((InvertedControls) ? NewState.IsKeyDown(Keys.NumPad3) : NewState.IsKeyDown(Keys.NumPad1))
                        velocity.X -= (SurfaceObject != null) ? SpeedUpGround : SpeedUpAir;
                    else if (velocity.X < 0)
                        velocity.X = (SurfaceObject != null) ? MathHelper.Clamp(velocity.X + SlowdownGround, -MaxXSpeed, 0) : MathHelper.Clamp(velocity.X + SlowdownAir, -MaxXSpeed, 0);

                    //Player Input to move Right
                    if ((InvertedControls) ? NewState.IsKeyDown(Keys.NumPad1) : NewState.IsKeyDown(Keys.NumPad3))
                        velocity.X += (SurfaceObject != null) ? SpeedUpGround : SpeedUpAir;
                    else if (velocity.X > 0)
                        velocity.X = (SurfaceObject != null) ? MathHelper.Clamp(velocity.X - SlowdownGround, 0, MaxXSpeed) : MathHelper.Clamp(velocity.X - SlowdownAir, 0, MaxXSpeed);
                    //Player Input to DropDown
                    if ((InvertedControls) ? NewState.IsKeyDown(Keys.NumPad5) : NewState.IsKeyDown(Keys.NumPad2))
                    {
                        DropDown();
                    }
                    //Player Input to Jump
                    if (
                        ((InvertedControls) ? NewState.IsKeyDown(Keys.NumPad2) : NewState.IsKeyDown(Keys.NumPad5)) &&
                        (SurfaceObject != null) &&
                        ((InvertedControls) ? !OldState.IsKeyDown(Keys.NumPad2) : !OldState.IsKeyDown(Keys.NumPad5))
                        )
                        Jump();
                    //Player Input to Punch
                    if (NewState.IsKeyDown(Keys.Delete) && !OldState.IsKeyDown(Keys.Delete) && PunchCooldownTimer <= 0 && !Busy)
                    {
                        Punch();
                        Console.WriteLine("punch");
                    }
                    //Player Input to Kick
                    if (NewState.IsKeyDown(Keys.PageDown) && !OldState.IsKeyDown(Keys.PageDown) && KickCooldownTimer <= 0 && !Busy)
                    {
                        Kick();
                        Console.WriteLine("kick");
                    }
                }
                else
                {
                    if (velocity.X < 0)
                        velocity.X = MathHelper.Clamp(velocity.X + SLowdownStunned, -MaxXSpeed, 0);
                    else if (velocity.X > 0)
                        velocity.X = MathHelper.Clamp(velocity.X - SLowdownStunned, 0, MaxXSpeed);
                }
            }
            OldState = NewState;
            //Clamps the velocity to ensure no abnormalities
            velocity.X = MathHelper.Clamp(velocity.X, -MaxXSpeed, MaxXSpeed);
        }

        private void DidIDieCheck()
        {
            if (position.Y > 1100)
                isDead = true;
        }

        private void BusyManager()
        {
            if (Punching)
                Busy = true;
            else if (Kicking)
                Busy = true;
            else if (Stunned)
                Busy = true;
            else
                Busy = false;
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
                if (PunchingGraceTimer <= 0)
                {
                    PunchingRectangle.Width = 0;
                    PunchingRectangle.Height = 0;
                }
                else
                {
                    PunchingGraceTimer -= GameTime.ElapsedGameTime.TotalSeconds;
                    if (spriteEffect == SpriteEffects.None)
                        PunchingRectangle = new Rectangle(Hitbox.Center.X, Hitbox.Top + Hitbox.Height / 4, DrawRectangle.Width / 2, 5);
                    else
                        PunchingRectangle = new Rectangle(Hitbox.Center.X - DrawRectangle.Width / 2, Hitbox.Top + Hitbox.Height / 4, DrawRectangle.Width / 2, 5);

                }
            }
        }

        private void Punch()
        {
            Punching = true;
            AnimationPunch();
            PunchCooldownTimer = PunchCooldown;
            PunchLifeTimer = (maxNrFrame * timePerFrame) / 2 - ((maxNrFrame * timePerFrame) / 5);
            PunchingGraceTimer = PunchingGrace;
        }
        #endregion

        #region KickingStuff
        private void KickManager(GameTime GameTime)
        {
            if (Kicking)
            {
                if (KickDelayTimer <= 0)
                {
                    if (spriteEffect == SpriteEffects.None)
                        KickingRectangle = new Rectangle(Hitbox.Center.X, Hitbox.Bottom - Hitbox.Height / 6, DrawRectangle.Width / 2, 5);
                    else
                        KickingRectangle = new Rectangle(Hitbox.Center.X - DrawRectangle.Width / 2, Hitbox.Bottom - Hitbox.Height / 6, DrawRectangle.Width / 2, 5);

                    if (KickLifeTimer >= 0)
                        KickLifeTimer -= GameTime.ElapsedGameTime.TotalSeconds;
                    else
                        Kicking = false;
                }
                else
                    KickDelayTimer -= GameTime.ElapsedGameTime.TotalSeconds;
            }
            {
                if (KickingGraceTimer <= 0)
                {
                    KickingRectangle.Width = 0;
                    KickingRectangle.Height = 0;
                }
                else
                {
                    KickingGraceTimer -= GameTime.ElapsedGameTime.TotalSeconds;
                    if (spriteEffect == SpriteEffects.None)
                        KickingRectangle = new Rectangle(Hitbox.Center.X, Hitbox.Bottom - Hitbox.Height / 6, DrawRectangle.Width / 2, 5);
                    else
                        KickingRectangle = new Rectangle(Hitbox.Center.X - DrawRectangle.Width / 2, Hitbox.Bottom - Hitbox.Height / 6, DrawRectangle.Width / 2, 5);
                }
            }
        }

        private void Kick()
        {
            Kicking = true;
            AnimationKick();
            KickCooldownTimer = KickCooldown;
            KickLifeTimer = (maxNrFrame * timePerFrame) / 2 - ((maxNrFrame * timePerFrame) / 5);
            KickingGraceTimer = KickingGrace;
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

        private void AnimationKick()
        {
            timePerFrame = 0.05;
            frameXOffset = 660;
            frameYOffset = 110;
            maxNrFrame = 9;
            AnimationTimer = maxNrFrame * timePerFrame;
            KickDelayTimer = AnimationTimer / 2;
        }
        #endregion
        #endregion

        #endregion
    }
}
