using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    abstract class MovableGameObject : GameObject
    {
        #region Variables
        public Vector2 velocity;
        public MovableGameObject SurfaceObject;
        protected Rectangle DrawRectangle;

        #endregion

        #region Public Methods
        public MovableGameObject(Vector2 pos, ContentManager Content)
            : base(pos)
        { }

        public void IsHit(MovableGameObject Assailant)
        {
            if (Assailant is Player)
                if (this is Player)
                    (this as Player).GetStunned(5);
                else if (Assailant is PowerUp)
                { }
        }

        public override void Update(GameTime gametime)
        {
            if (velocity.X > 0)
                spriteEffect = SpriteEffects.None;
            else if (velocity.X < 0)
                spriteEffect = SpriteEffects.FlipHorizontally;
                

            position += velocity;
            if (SurfaceObject != null)
                position.Y = SurfaceObject.Hitbox.Top - Hitbox.Height;
            Hitbox = new Rectangle((int)position.X, (int)position.Y, Hitbox.Width, Hitbox.Height);
            DrawRectangle = new Rectangle((int)position.X, (int)position.Y, DrawRectangle.Width, DrawRectangle.Height);
            FallenOfScreenChecker();
        }

        public override void Update(GameTime gametime, float SpeedModifier)
        { }

        #endregion

        #region Private Method
        private void FallenOfScreenChecker()
        {
            if (position.Y > 1080)
                isDead = true;
        }
        #endregion 



    }
}
