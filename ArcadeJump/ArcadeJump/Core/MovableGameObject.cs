using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    /// <summary>
    /// A game object capable of moving one way or another
    /// </summary>
    abstract class MovableGameObject : GameObject
    {
        #region Variables
        public Vector2 velocity;
        public MovableGameObject SurfaceObject;                     //A rectangle that lines the very top of the hitbox.
        public Rectangle DrawRectangle;

        #endregion

        #region Public Methods
        public MovableGameObject(Vector2 pos, ContentManager Content)
            : base(pos)
        { }

        public override void Update(GameTime gametime)
        {
            FlipCheck();
            position += velocity;
            if (SurfaceObject != null)
                position.Y = SurfaceObject.Hitbox.Top - Hitbox.Height;                                                  //Ensures that the object is sticking to whatever surface its on
            Hitbox = new Rectangle((int)position.X, (int)position.Y, Hitbox.Width, Hitbox.Height);
            DrawRectangle = new Rectangle((int)position.X, (int)position.Y, DrawRectangle.Width, DrawRectangle.Height);
            FallenOfScreenChecker();
        }

        public override void Update(GameTime gametime, float SpeedModifier)
        { }

        #endregion

        #region Private Method
        /// <summary>
        /// Checks to see what direction that object should be facing.
        /// </summary>
        private void FlipCheck()
        {
            if (velocity.X > 0)                                     
                spriteEffect = SpriteEffects.None;
            else if (velocity.X < 0)
                spriteEffect = SpriteEffects.FlipHorizontally;
        }
        /// <summary>
        /// Marks the object as dead if its off the screen
        /// </summary>
        private void FallenOfScreenChecker()
        {
            if (position.Y > 1080)
                isDead = true;
        }
        #endregion 



    }
}
