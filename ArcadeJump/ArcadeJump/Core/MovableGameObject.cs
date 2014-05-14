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
        public Vector2 velocity;
        public Vector2 acceleration;
        public Rectangle collisionRectangle;
        public MovableGameObject SurfaceObject;
        private float gravity = 9.81f;

        public MovableGameObject(Vector2 pos, ContentManager Content)
            : base(pos)
        { }

        // thought that player class will call method when pressing buttons. and then it will move 
        // powerups will have call move function with a bool that is always true 
        // plattforms will call move but will not call gravity 

        public void IsHit()
        {

        }

        public override void Update(GameTime gametime)
        {
            position += velocity;
            Hitbox = new Rectangle((int)position.X, (int)position.Y, Hitbox.Width, Hitbox.Height);
            if (SurfaceObject != null)
                position.Y = SurfaceObject.Hitbox.Top;
        }

        public void Gravity(GameTime gameTime)
        {
            acceleration.Y = gravity;
            velocity += acceleration * gameTime.ElapsedGameTime.Milliseconds;
            position += velocity;
        }

        public void Jump()
        {
            SurfaceObject = null;
            velocity.Y += 10;
            position.Y += velocity.Y;
        }
    }
}
