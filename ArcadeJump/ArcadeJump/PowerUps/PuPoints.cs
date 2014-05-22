using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    class PuPoints : PowerUp
    {
        #region Variables
        double ScoreValue = 50;
        #endregion

        #region Public Variables
        public PuPoints(Vector2 position, ContentManager Content, Vector2 velocity)
            : base(position, Content, velocity)
        {
            this.velocity = Vector2.Zero;
            texture = Content.Load<Texture2D>("Textures/Points");
            HitBoXDebugTexture = Content.Load<Texture2D>("Textures/DebugTexture");
            frameHeight = texture.Height;
            frameWidht = texture.Width;
            origin = new Vector2(texture.Height / 2, texture.Width / 2);
            //color = Color.Cyan;
        }

        public PuPoints(Platform SurfaceObject, ContentManager Content, Vector2 velocity, bool LockedToPlatform)
            : base(SurfaceObject, Content, velocity, LockedToPlatform)
        {
            this.velocity = Vector2.Zero;
            texture = Content.Load<Texture2D>("Textures/Points");
            frameHeight = texture.Height;
            frameWidht = texture.Width;
            origin = new Vector2(texture.Height / 2, texture.Width / 2);
            HitBoXDebugTexture = Content.Load<Texture2D>("Textures/DebugTexture");
            //color = Color.Cyan;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            //spritebatch.Draw(HitBoXDebugTexture, Hitbox, Color.Red);                  //debug hitbox display
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

        }

        public override void PickedUp(ref Player Player)
        {
            Player.Score += ScoreValue;
            this.isDead = true;
        }

        #endregion
    }
}
