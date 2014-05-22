using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    class PuSuperJump : PowerUp
    {
        #region Variables
        Vector2 OldPosition;
         

        double StunDuration = 3;
        #endregion

        #region Public Variables
        public PuSuperJump(Vector2 position, ContentManager Content, Vector2 velocity)
            : base(position, Content, velocity)
        {
            texture = Content.Load<Texture2D>("Textures/superjump");
            frameHeight = texture.Height;
            frameWidht = texture.Width;
            origin = new Vector2(texture.Height / 2, texture.Width / 2);
            HitBoXDebugTexture = Content.Load<Texture2D>("Textures/DebugTexture");
            color = Color.DarkGreen;
            PowerUpName = "PuSuperJump";
        }

        public PuSuperJump(Platform SurfaceObject, ContentManager Content, Vector2 velocity, bool LockedToPlatform)
            : base(SurfaceObject, Content, velocity, LockedToPlatform)
        {
            texture = Content.Load<Texture2D>("Textures/superjump");
            HitBoXDebugTexture = Content.Load<Texture2D>("Textures/DebugTexture");
            frameHeight = texture.Height;
            frameWidht = texture.Width;
            origin = new Vector2(texture.Height / 2, texture.Width / 2);
            color = Color.DarkGreen;
            PowerUpName = "PuSuperJump";
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            //spritebatch.Draw(HitBoXDebugTexture, Hitbox, Color.Red);                  //debug hitbox display
        }

        public override void Update(GameTime gametime)
        {
            if(!Dummy)
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
                //LockedToPlatform = false;
                MakePickedUp();
            }
        }

        #endregion

        #region Private Variables
        private float CalculateRotation()
        {
            if (Vector2.Distance(position, OldPosition) > 1)
            {
                float Distance = Vector2.Distance(position, OldPosition);
                float Circumferance = 2 * MathHelper.Pi * (DrawRectangle.Width / 2);
                float rot = Distance / Circumferance;
                if (spriteEffect == SpriteEffects.None)
                    return (float)(Math.PI * 2) * rot;
                else
                    return (float)(-(Math.PI * 2) * rot);
            }
            else return 0;
        }

        #endregion









    }
}
