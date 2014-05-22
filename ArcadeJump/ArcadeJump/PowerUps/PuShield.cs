using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    class PuShield : PowerUp
    {
        #region Public Variables
        public PuShield(Vector2 position, ContentManager Content, Vector2 velocity)
            : base(position, Content, velocity)
        {
            texture = Content.Load<Texture2D>("Textures/Shield");
            frameHeight = texture.Height;
            frameWidht = texture.Width;
            origin = new Vector2(texture.Height / 2, texture.Width / 2);
            HitBoXDebugTexture = Content.Load<Texture2D>("Textures/DebugTexture");
            color = Color.Gold;
            PowerUpName = "PuShield";
        }

        public PuShield(Platform SurfaceObject, ContentManager Content, Vector2 velocity, bool LockedToPlatform)
            : base(SurfaceObject, Content, velocity, LockedToPlatform)
        {
            texture = Content.Load<Texture2D>("Textures/Shield");
            HitBoXDebugTexture = Content.Load<Texture2D>("Textures/DebugTexture");
            frameHeight = texture.Height;
            frameWidht = texture.Width;
            origin = new Vector2(texture.Height / 2, texture.Width / 2);
            color = Color.Gold;
            PowerUpName = "PuShield";
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if(!Dummy)
            base.Draw(spritebatch);
            //spritebatch.Draw(HitBoXDebugTexture, Hitbox, Color.Red);                  //debug hitbox display
        }

        public override void Update(GameTime gametime)
        {
            if (!Dummy)
                base.Update(gametime);

            //rotation += CalculateRotation();
            //OldPosition = position;
        }

        public override void PickedUp(ref Player Player)
        {
            base.PickedUp(ref Player);
            if (!this.isDead)
            {
                Player.CurrentPowerUp = this;
                MakePickedUp();
            }
        }
        #endregion
    }
}
