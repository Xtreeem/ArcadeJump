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
        //Misc
        public int PlayerNumber;
        public double Score;
        public PowerUp CurrentPowerUp;
        KeyboardState OldState;
        Texture2D DebugTexture;
        //Debuff Related
        bool Stunned = false;
        double StunDuration;
        double StunImmunityTimer;
        bool InvertedControls;
        public double InvertedControlsDuration;
        //Animation Related
        double IdleTimer;
        double AnimationTimer;
        double BlinkingTimer;
        Color OriginalColor; 
        Vector2 LastVelocity;

        //Action Related
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

        //Balancing Variables
        float BlinkingInterval = 0.1f;
        public float Kickpower = 15;
        float KickingGrace = 0.5f;
        float PunchingGrace = 0.5f;
        float KickCooldown = 0.8f;
        float PunchCooldown = 0.8f;
        public float PunchStunDuration = 3f;
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
            SetColor();
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
            //if (SurfaceObject != null)                                                //Debug Feature to display when a player is considered on a platform
            //    color = Color.Red;
            //else
            //    color = OriginalColor;
            Input();
            AnimationManager(GameTime);
            DidIDieCheck();
            LastVelocity = velocity;
            TimerManager(GameTime);
            base.Update(GameTime);
            if (DroppingDownTimer < 0)                                          //Checks to see if its time to restore the players BottomRectangle yet
            {
                BottomRectangle.X = Hitbox.X;
                BottomRectangle.Y = Hitbox.Bottom;
            }
            else
                DroppingDownTimer -= GameTime.ElapsedGameTime.TotalSeconds;
            PunchManager(GameTime);
            KickManager(GameTime);
            BusyManager();
            Blinking(GameTime);
            Score += GameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            //if (PunchingRectangle != null)                                    //Debug Hitbox Display
            //    spritebatch.Draw(DebugTexture, PunchingRectangle, Color.Red);
            //if (KickingRectangle != null)
            //    spritebatch.Draw(DebugTexture, KickingRectangle, Color.Black);
            //spritebatch.Draw(HitBoXDebugTexture, Hitbox, Color.Black);            
        }
        /// <summary>
        /// Function used to stun the player,
        /// also checks to see if the player is stun immune
        /// or has any form of protection.
        /// 
        /// If the player gets stunned sets an immunity timer.
        /// </summary>
        /// <param name="StunDuration">The length of the intended stun</param>
        public void GetStunned(double StunDuration)
        {
            if (!Stunned)
                if (StunImmunityTimer < 0)
                    if (!ShieldChecker())
                    {
                        Stunned = true;
                        this.StunDuration = StunDuration;
                        AnimationFallingOver();
                        StunImmunityTimer = StunDuration * 2;
                    }
                    else
                        StunImmunityTimer = 0.6;
        }
        /// <summary>
        /// Function that tries to invert the players controls
        /// Checks to see if the player is shielded against debuffs
        /// </summary>
        /// <param name="InvertionDuration">the intended length of the inversion</param>
        public void GetInverted(double InvertionDuration)
        {
            if (!InvertedControls)
                if (!ShieldChecker())
                {
                    InvertedControls = true;
                    InvertedControlsDuration = InvertionDuration;
                }
        }
        /// <summary>
        /// Makes the player jump
        /// </summary>
        public void Jump()
        {
            Console.WriteLine("Jump");
            if (SurfaceObject != null)
                AnimationJumping();
            SurfaceObject = null;
            velocity.Y -= JumpPower;
        }
        /// <summary>
        /// Makes the player jump really high
        /// </summary>
        public void SuperJump()
        {
            Console.WriteLine("SuperJump");
            AnimationJumping();
            SurfaceObject = null;
            velocity.Y -= JumpPower * 2;
            CurrentPowerUp.isDead = true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Function that runs each update and updates all the timers controlling the players behaviour
        /// </summary>
        private void TimerManager(GameTime GameTime)
        {
            if (PunchCooldownTimer >= 0)
                PunchCooldownTimer -= GameTime.ElapsedGameTime.TotalSeconds;
            if (KickCooldownTimer >= 0)
                KickCooldownTimer -= GameTime.ElapsedGameTime.TotalSeconds;
            if (StunImmunityTimer >= 0)
                StunImmunityTimer -= GameTime.ElapsedGameTime.TotalSeconds;
            if (StunDuration >= 0)
                StunDuration -= GameTime.ElapsedGameTime.TotalSeconds;
            else
                Stunned = false;
            if (InvertedControlsDuration >= 0)
                InvertedControlsDuration -= GameTime.ElapsedGameTime.TotalSeconds;
            else
                InvertedControls = false;
        }
        /// <summary>
        /// Initialization method used to set the players color based if its player 1 or 2
        /// </summary>
        private void SetColor()
        {
            if (PlayerNumber == 1)
                color = Color.Black;
            else
                color = Color.Red;
            OriginalColor = color;
        }
        /// <summary>
        /// Function used to illustrate the fact that the players controls have been inverted 
        /// by rapidly shifting between a bright and dark color
        /// </summary>
        private void Blinking(GameTime GameTime)
        {
            if (InvertedControlsDuration > 0)
            {
                BlinkingTimer += GameTime.ElapsedGameTime.TotalSeconds;
                if (BlinkingTimer > BlinkingInterval)
                {
                    if (color == OriginalColor)
                    {
                        if (PlayerNumber != 1)
                            color = Color.DarkRed;
                        else
                            color = Color.SlateGray;
                    }
                    else
                        color = OriginalColor;
                    BlinkingTimer = 0;
                }
            }
            else if (color != OriginalColor)
                color = OriginalColor;
        }
        /// <summary>
        /// Function that is called when the PowerUp button is pressed
        /// </summary>
        private void PowerUpKeyManager()
        {
            if (CurrentPowerUp != null)
                if ((CurrentPowerUp.UsableWhileStunned && Stunned) || !Stunned)
                    switch (CurrentPowerUp.PowerUpName)
                    {
                        case ("PuStun"):
                            break;
                        case ("PuSuperJump"):
                            SuperJump();
                            CurrentPowerUp = null;
                            break;
                        case ("PuShield"):
                            CurrentPowerUp.isDead = true;
                            CurrentPowerUp = null;
                            break;
                        default:
                            throw new Exception("Trying to use none implemented powerup");
                    }
        }
        /// <summary>
        /// Function designed to Scan for the control inputs and move the player accordingly
        /// </summary>
        private void Input()
        {
            var NewState = Keyboard.GetState();         //Keyboard state that will be used to ensure no spamming is unintended.
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
                    if ((InvertedControls) ? NewState.IsKeyDown(Keys.J) : NewState.IsKeyDown(Keys.S))
                    {
                        DropDown();
                    }
                    //Player Input to Jump
                    if (
                        ((InvertedControls) ? NewState.IsKeyDown(Keys.S) : NewState.IsKeyDown(Keys.J)) &&
                        (SurfaceObject != null) &&
                        ((InvertedControls) ? !OldState.IsKeyDown(Keys.S) : !OldState.IsKeyDown(Keys.J))
                        )
                        Jump();
                    //Player Input to Punch
                    if (NewState.IsKeyDown(Keys.N) && !OldState.IsKeyDown(Keys.N) && PunchCooldownTimer <= 0 && !Busy)
                    {
                        Punch();
                        Console.WriteLine("punch");
                    }
                    //Player Input to Kick
                    if (NewState.IsKeyDown(Keys.K) && !OldState.IsKeyDown(Keys.K) && KickCooldownTimer <= 0 && !Busy)
                    {
                        Kick();
                        Console.WriteLine("kick");
                    }

                }
                else
                {   
                    //Ensures that the player will still slow down even if he is stunned
                    if (velocity.X < 0)
                        velocity.X = MathHelper.Clamp(velocity.X + SLowdownStunned, -MaxXSpeed, 0);
                    else if (velocity.X > 0)
                        velocity.X = MathHelper.Clamp(velocity.X - SLowdownStunned, 0, MaxXSpeed);
                }
                //Player input to trigger stored PowerUp
                if (NewState.IsKeyDown(Keys.C) && !OldState.IsKeyDown(Keys.C) && KickCooldownTimer <= 0)
                {
                    PowerUpKeyManager();
                    Console.WriteLine("PowerUp");
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
                if (NewState.IsKeyDown(Keys.F7) && !OldState.IsKeyDown(Keys.F7))
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
                    //Ensures that the player will still slowdown even if he is stunned
                    if (velocity.X < 0)
                        velocity.X = MathHelper.Clamp(velocity.X + SLowdownStunned, -MaxXSpeed, 0);
                    else if (velocity.X > 0)
                        velocity.X = MathHelper.Clamp(velocity.X - SLowdownStunned, 0, MaxXSpeed);
                }
                //Player Input to trigger stored PowerUp
                if (NewState.IsKeyDown(Keys.End) && !OldState.IsKeyDown(Keys.End) && KickCooldownTimer <= 0)
                {
                    PowerUpKeyManager();
                    Console.WriteLine("PowerUp");
                }
            }
            OldState = NewState;
            //Clamps the velocity to ensure no abnormalities
            velocity.X = MathHelper.Clamp(velocity.X, -MaxXSpeed, MaxXSpeed);
        }
        /// <summary>
        /// Function used to hide the players bottom rectangle away of screen 
        /// allowing them to fall down
        /// </summary>
        private void DropDown()
        {
            DroppingDownTimer = 0.07;
            SurfaceObject = null;
            BottomRectangle = new Rectangle(-300, 0, BottomRectangle.Width, BottomRectangle.Height);
        }
        /// <summary>
        /// Function that checks if the player is holding the shield powerup
        /// then removes the powerup from the player and returns true
        /// </summary>
        /// <returns>true if the player had the powerup</returns>
        private bool ShieldChecker()
        {
            if (CurrentPowerUp != null)
                if (CurrentPowerUp.PowerUpName == "PuShield")
                {
                    CurrentPowerUp.isDead = true;
                    CurrentPowerUp = null;
                    return true;
                }
            return false;
        }
        /// <summary>
        /// Checks to see if the player has fallen off the screen
        /// </summary>
        private void DidIDieCheck()
        {
            if (position.Y > 1100)
                isDead = true;
        }
        /// <summary>
        /// Checks to see if the player is busy kicking, punching or being stunned
        /// </summary>
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
        /// <summary>
        /// Function that manages everything that has to do with the punching mechanics
        /// </summary>
        private void PunchManager(GameTime GameTime)
        {
            if (Punching)                       //Checks to see if the player is punching
            {
                if (PunchDelayTimer <= 0)       //Checks to see if its time to spawn the punch rectangle yet
                {
                    if (spriteEffect == SpriteEffects.None) //Checks to see what direction the player is facing and spawns the rectangle
                        PunchingRectangle = new Rectangle(Hitbox.Center.X, Hitbox.Top + Hitbox.Height / 4, DrawRectangle.Width / 2, 5);
                    else
                        PunchingRectangle = new Rectangle(Hitbox.Center.X - DrawRectangle.Width / 2, Hitbox.Top + Hitbox.Height / 4, DrawRectangle.Width / 2, 5);

                    if (PunchLifeTimer >= 0)    //counts down before removing the rectangle
                        PunchLifeTimer -= GameTime.ElapsedGameTime.TotalSeconds;
                    else
                        Punching = false;
                }
                else
                    PunchDelayTimer -= GameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (PunchingGraceTimer <= 0) //checks to see if the grace period where the rectangle remains after the animation is still going on or not, if not removed the rectangle.
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
            if (Stunned) //Checks if the player gets stunned while punching and if so aborts the punch
            {
                PunchingRectangle.Width = 0;
                PunchingRectangle.Height = 0;
                Punching = false;
            }
        }
        /// <summary>
        /// Function called to start the punch mechanic
        /// </summary>
        private void Punch()
        {
            Punching = true;
            AnimationPunch();
            PunchCooldownTimer = PunchCooldown;
            PunchLifeTimer = (maxNrFrame * timePerFrame) / 2 - ((maxNrFrame * timePerFrame) / 5);
            PunchingGraceTimer = PunchingGrace;
        }
        #endregion
        /// <summary>
        /// Function that manages everything that has to do with kicking
        /// Mirror copy of the punching function.
        /// </summary>
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
            else
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
            if (Stunned)
            {
                PunchingRectangle.Width = 0;
                PunchingRectangle.Height = 0;
                Punching = false;
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
        /// <summary>
        /// Function that manages eveyrthing that has to do with the players animations
        /// What animation should be played when and what order that has priority.
        /// </summary>
        /// <param name="GameTime"></param>
        private void AnimationManager(GameTime GameTime)
        {
            if (!Stunned)                                                       //Checks so that the player aint stunned
            {
                if (AnimationTimer <= 0)                                        //Checks to see so no animation is running atm 
                {
                    if (velocity == Vector2.Zero)                               //checks if the player is moving atm 
                        IdleTimer += GameTime.ElapsedGameTime.TotalSeconds;     //Updates how long the player has been doing nothing
                    else 
                        IdleTimer = 0;

                    if (velocity.Y < 0 && velocity.Y > 0 - (6 * Gravitation))   //Tests to see if its time to start leveling out in a jump by multiplying the velocity decrease by the number of frames in the leveling out animation and how long each one will take
                        AnimationLevelingOut();
                    else if (LastVelocity.Y != 0 && velocity.Y == 0)            //Checks if the player just has landed (aka his old velocity was moving downwards and now he aint)
                        AnimationLanding();
                    else if (velocity.Y < 0)                                    //checks if the player is moving upwards
                        AnimationAscending();                       
                    else if (velocity.Y > 0)                                    //Checks if the player is moving downwards
                        AnimationDescending();                                  
                    else if (velocity.X != 0)                                   //Checks to see if the player is running
                        AnimationRunning();
                    else if (IdleTimer > 3 && velocity == Vector2.Zero)         //Checks to see if the player is still standing still and the idle time has triggerd a bored event
                    {
                        AnimationProlongedIdle();
                        if (currentFrame + 1 > maxNrFrame)                      //Ensures that the bored animation gets to finish before resetting the idle timer
                            IdleTimer = 0;
                    }
                    else
                        AnimationIdle();                                        //If all else fails makes the player stand still
                }

            }
            else if (AnimationTimer <= 0)                                       //If the player is stunned and no animation is running
            {
                if (StunDuration <= 0.20)                                       //if the stun is about to expire show the getting up animation
                {
                    AnimationGettingUp();
                }
                else
                    AnimationLayingDown();                                      //otherwise show the player laying on his back
            }

            if (AnimationTimer > 0)                                             //if an animation is running count down its timer
                AnimationTimer -= GameTime.ElapsedGameTime.TotalSeconds;
            else
            {
                AnimationTimer = 0;
            }
        }
        #region Animations
        /// <summary>
        /// A long list of differant animations following the formula of 
        /// where int he spritesheet to start
        /// how many frames the animation is 
        /// and how long each frame should take
        /// and finally if its an animation that overrides other animations an animation timer is started.
        /// </summary>

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
            maxNrFrame = 10;
            AnimationTimer = maxNrFrame * timePerFrame;
            KickDelayTimer = AnimationTimer / 2;
        }
        #endregion
        #endregion
        #endregion
    }
}
