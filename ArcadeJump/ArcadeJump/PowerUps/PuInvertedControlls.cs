using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    class PuInvertedControlls : PowerUp
    {
        #region Variables
        double InvertionDuration = 3;
        #endregion

        #region Public Functions
        public PuInvertedControlls(Vector2 position, ContentManager Content, Vector2 velocity)
            : base(position, Content, velocity)
        {
            texture = Content.Load<Texture2D>("Textures/Inverted");
            color = Color.Purple;
            frameHeight = texture.Height;
            frameWidht = texture.Width;
            origin = new Vector2(texture.Height / 2, texture.Width / 2);
            HitBoXDebugTexture = Content.Load<Texture2D>("Textures/DebugTexture");
        }

        public PuInvertedControlls(Platform SurfaceObject, ContentManager Content, Vector2 velocity, bool LockedToPlatform)
            : base(SurfaceObject, Content, velocity, LockedToPlatform)
        {
            texture = Content.Load<Texture2D>("Textures/Inverted");
            color = Color.Purple;
            frameHeight = texture.Height;
            frameWidht = texture.Width;
            origin = new Vector2(texture.Height / 2, texture.Width / 2);
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
        }

        public override void PickedUp(ref Player Player)
        {
            Player.GetInverted(InvertionDuration);
            this.isDead = true;
        }
        #endregion
    }
}
