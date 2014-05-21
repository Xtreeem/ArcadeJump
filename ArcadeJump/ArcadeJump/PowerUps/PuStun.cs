using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    class PuStun: PowerUp
    {
        #region Variables
        Vector2 OldPosition;


        double StunDuration = 3;
        #endregion

        #region Public Variables
        public PuStun(Vector2 position, ContentManager Content, Vector2 velocity)
            : base(position, Content, velocity)
        {
            texture = Content.Load<Texture2D>("Textures/Rock");
            HitBoXDebugTexture = Content.Load<Texture2D>("Textures/DebugTexture");
        }

        public PuStun(Platform SurfaceObject, ContentManager Content, Vector2 velocity, bool LockedToPlatform)
            : base(SurfaceObject, Content, velocity, LockedToPlatform)
        {
            texture = Content.Load<Texture2D>("Textures/Rock");
            HitBoXDebugTexture = Content.Load<Texture2D>("Textures/DebugTexture");
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            //spritebatch.Draw(HitBoXDebugTexture, Hitbox, Color.Red);                  //debug hitbox display
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            rotation += CalculateRotation();
            OldPosition = position;
        }

        public override void PickedUp(ref Player Player)
        {
            Player.GetStunned(StunDuration);
            LockedToPlatform = false;
            //this.isDead = true;
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
                    return (float)(-(Math.PI *2) *rot);
            }
            else return 0;
        }

        #endregion









    }
}
