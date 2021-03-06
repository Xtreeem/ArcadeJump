﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    class Platform : MovableGameObject
    {
        #region Variables
        public Rectangle SurfaceRectangle;
        private double SurfaceHeight = 10;
        private int MinimumWidth = 10;
        private float StartingVelocity = 1;
        private int PlatformHeight = 15;
        public bool Indestructable;
        #endregion
        #region Public Methods
        public Platform(Vector2 pos, ContentManager Content, double WidthAdjustment, int StartingWidth)
            : base(pos, Content)
        {
            velocity = new Vector2(0, StartingVelocity);
            position = pos;
            texture = Content.Load<Texture2D>("Textures/plattform");
            Hitbox = new Rectangle((int)pos.X, (int)pos.Y, (int)(StartingWidth - (StartingWidth * WidthAdjustment)) + MinimumWidth, PlatformHeight);
            DrawRectangle = new Rectangle((int)pos.X, (int)pos.Y, (int)(StartingWidth - (StartingWidth * WidthAdjustment)) + MinimumWidth, PlatformHeight);
            SurfaceRectangle = new Rectangle(Hitbox.X, Hitbox.Y, Hitbox.Width, (int)SurfaceHeight);
            color = Color.Black;
            Indestructable = false;
        }

        public Platform(Vector2 pos, ContentManager Content, double WidthAdjustment, int StartingWidth, bool Indestructable)
            : base(pos, Content)
        {
            velocity = new Vector2(0, StartingVelocity);
            position = pos;
            texture = Content.Load<Texture2D>("Textures/plattform");
            Hitbox = new Rectangle((int)pos.X, (int)pos.Y, (int)(StartingWidth - (StartingWidth * WidthAdjustment)) + MinimumWidth, PlatformHeight);
            DrawRectangle = new Rectangle((int)pos.X, (int)pos.Y, (int)(StartingWidth - (StartingWidth * WidthAdjustment)) + MinimumWidth, PlatformHeight);
            SurfaceRectangle = new Rectangle(Hitbox.X, Hitbox.Y, Hitbox.Width, (int)SurfaceHeight);
            color = Color.Black;
            this.Indestructable = Indestructable;
        }


        public override void Update(GameTime gameTime, float SpeedModifier)
        {
            velocity.Y = StartingVelocity + SpeedModifier;
            base.Update(gameTime);
            
            SurfaceRectangle = new Rectangle(Hitbox.X, Hitbox.Y, Hitbox.Width, (int)SurfaceHeight);

        }

        public override void Draw(SpriteBatch sB)
        {
            base.Draw(sB);
            //sB.Draw(texture, SurfaceRectangle, Color.Red); //Debug line to show the surface recangle

        }
        #endregion
        #region Private Methods
        #endregion
    }



}
