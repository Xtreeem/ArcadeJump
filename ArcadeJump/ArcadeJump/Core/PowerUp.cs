﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    class PowerUp : AdvancedGameObject
    {
        #region Variables
        protected bool Dummy = false;
        protected bool LockedToPlatform = false;    //variable used to decide if the powerup should behaive like a red shell in mario
        public String PowerUpName;
        public bool UsableWhileStunned = false;
        protected int PowerUpWidth = 45;            //The screen width of the PowerUp
        protected int PowerUpHeight = 45;           //The screen height of the PowerUp 
        #endregion

        #region Public Methods
        //Constructor used to spawn a PowerUp at a given position
        public PowerUp(Vector2 position, ContentManager Content, Vector2 velocity)
            : base(position, Content)
        {
            color = Color.Black;
            this.position = position;
            Hitbox = new Rectangle((int)position.X, (int)position.Y, PowerUpWidth, PowerUpHeight);
            DrawRectangle = Hitbox;
            texture = Content.Load<Texture2D>("Textures/Rock");
            this.velocity = velocity;
            frameHeight = 131;
            frameWidht = 130;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }
        //Constructor used to spawn a PowerUp ontop of a given Platform
        public PowerUp(Platform SurfaceObject, ContentManager Content, Vector2 velocity, bool LockedToPlatform)
            : base(new Vector2(0, 0), Content)
        {
            color = Color.Black;
            position = new Vector2(SurfaceObject.SurfaceRectangle.Center.X, SurfaceObject.SurfaceRectangle.Top - PowerUpHeight);
            Hitbox = new Rectangle((int)position.X, (int)position.Y, PowerUpWidth, PowerUpHeight);
            DrawRectangle = Hitbox;
            texture = Content.Load<Texture2D>("Textures/Rock");
            this.velocity = velocity;
            this.LockedToPlatform = LockedToPlatform;
            this.SurfaceObject = SurfaceObject;
            frameHeight = 131;
            frameWidht = 130;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
            if (LockedToPlatform && SurfaceObject != null)
            {
                position.Y = SurfaceObject.Hitbox.Top;                      //Ensures that if the PowerUp is locked to the platform it wont fall off.
                if (this.Hitbox.Left < SurfaceObject.Hitbox.Left)
                    velocity.X = ForceNegativePosetive(velocity.X, false);
                else if (this.Hitbox.Right > SurfaceObject.Hitbox.Right)
                    velocity.X = ForceNegativePosetive(velocity.X, true);
            }
            if (SurfaceObject != null)
                if (SurfaceObject.isDead)
                    isDead = true;
            DrawRectangle.Y += PowerUpHeight / 2;                           //Adjusts the draw rectangle to compensate for the fact that we are using the origin
            DrawRectangle.X += PowerUpWidth / 2;
        }
        /// <summary>
        /// Function called when the PowerUp is kicked by a player
        /// </summary>
        /// <param name="KickPower">The Players kicking power</param>
        public void Kicked(float KickPower)
        {
            LockedToPlatform = false;
            SurfaceObject = null;
            velocity.Y -= KickPower;
        }
        #endregion
        
        /// <summary>
        /// Base of the function used when a powerup encounters a player
        /// </summary>
        /// <param name="Player">Referance to the player that picked up the powerup</param>
        public virtual void PickedUp(ref Player Player)
        {
            if (Player.CurrentPowerUp != null)
                this.isDead = true;
        }

        #region Private Methods
        /// <summary>
        /// Function called to create a inactive powerup in preparation for the player picking it up
        /// </summary>
        protected void MakePickedUp()
        {
            Dummy = true;
            Hitbox.Width = 0;
            Hitbox.Height = 0;
        }
        /// <summary>
        /// Quick function used to ensure the value is either posetive or negative
        /// </summary>
        /// <param name="Velocity">the speed you with to ensure is negative or posetivt</param>
        /// <param name="Posetive">true = posetivt velocity, false = negative velocity</param>
        /// <returns></returns>
        private float ForceNegativePosetive(float Velocity, bool Posetive)
        {
            if (Posetive)
            {
                if (Velocity > 0)
                {
                    Velocity = Velocity * (-1);
                }
            }
            else if (!Posetive)
            {
                if (Velocity < 0)
                {
                    Velocity = Velocity * (-1);
                }
            }
            return Velocity;
        }
        #endregion
    }
}
