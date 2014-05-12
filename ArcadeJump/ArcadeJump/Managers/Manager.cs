using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{

    //To.do 
    // Wait for platforms and player to try to manage
    // -Remember to swap surfaceRectangle to fullsized rectangle in the platform/platform cocllision.
    class Manager
    {
        #region Variables
        Random Rand;
        List<Platform> Platforms;
        List<Player> Players;
        List<PowerUp> PowerUps;
        #endregion

        #region Public Methods
        public Manager()
        {
            Rand = new Random();
        }

        public void Update(GameTime GameTime)
        {
        }

        #endregion

        #region Private Methods
        #endregion

        private void CollisionChecking(ref MovableGameObject ObjectA, ref MovableGameObject ObjectB)
        {
            if (ObjectA is Player)
            {
                if (ObjectB is Platform && ObjectA.velocity.Y > 0)
                {
                    if (
                        (ObjectB as Platform).SurfaceRectangle.Contains(ObjectA.collisionRectangle.Left, ObjectA.collisionRectangle.Bottom) ||
                        (ObjectB as Platform).SurfaceRectangle.Contains(ObjectA.collisionRectangle.Right, ObjectA.collisionRectangle.Bottom) ||
                        (ObjectB as Platform).SurfaceRectangle.Contains(ObjectA.collisionRectangle.Center.X, ObjectA.collisionRectangle.Bottom)
                        )
                    {
                        ObjectA.velocity.Y = 0;
                        ObjectA.position.Y = (ObjectB as Platform).SurfaceRectangle.Top + 1;
                    }
                }
                else if (ObjectB is Player)
                {
                    //Collision with player and player
                }

                else if (ObjectB is PowerUp)
                {
                    //Collision with player and powerup
                }
            }
            else if (ObjectA is Platform)
                if (ObjectB is PowerUp && ObjectB.velocity.Y > 0)
                {
                    //Collision with platform and powerup
                }
                else if (ObjectB is Platform)
                {
                    if ((ObjectA as Platform).SurfaceRectangle.Contains((ObjectB as Platform).SurfaceRectangle))
                    {
                        if (Rand.Next(0, 2) > 0)
                        {
                            ObjectA.isDead = true;
                        }
                        else
                            ObjectB.isDead = true;
                    }
                }
        }

        private void RemoveDeadStuff()
        {
            for (int i = 0; i < Platforms.Count; i++)
            {
                if (Platforms[i].isDead)
                    Platforms.RemoveAt(i);
            }
            for (int i = 0; i < PowerUps.Count; i++)
            {
                if (PowerUps[i].isDead)
                    PowerUps.RemoveAt(i);
            }
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].isDead)
                    Players.RemoveAt(i);
            }
        }

        private void DrawStuff(SpriteBatch SpriteBatch)
        {
            foreach (Platform p in Platforms)
            {
                p.Draw(SpriteBatch);
            }
        }
    }
}

