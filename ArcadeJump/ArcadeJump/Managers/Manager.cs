using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{

    
    // Wait for platforms and player to try to manage
    // -Remember to swap surfaceRectangle to fullsized rectangle in the platform/platform cocllision.
    class Manager
    {
        #region Variables
        Random Rand;
        List<Platform> Platforms;
        List<Player> Players;
        List<PowerUp> PowerUps;
        List<GameObject> GameObjects;
        #endregion

        #region Public Methods
        public Manager(ref List<Platform> Platforms, ref List<PowerUp> PowerUps, ref List<Player> Players)
        {
            this.Players = Players;
            this.PowerUps = PowerUps;
            this.Platforms = Platforms;
            Rand = new Random();
        }

        public void Update(GameTime GameTime)
        {
            RemoveDeadStuff();
            UpdateStuff(GameTime);
        }


        /// <summary>
        /// Function used to draw every single Player/Platform/Powerup
        /// </summary>
        public void DrawStuff(SpriteBatch SpriteBatch)
        {
            SpriteBatch.Begin();
            foreach (Platform p in Platforms)
            {
                p.Draw(SpriteBatch);
            }

            foreach (Player p in Players)
            {
                p.Draw(SpriteBatch);
            }

            foreach (PowerUp p in PowerUps)
            {
                p.Draw(SpriteBatch);
            }
            SpriteBatch.End();
        }

        #endregion

        #region Private Methods
        #endregion
        /// <summary>
        /// Checks the Distance between two gameobjects to see if they are able to collide
        /// </summary>
        private void FirstCollisionCheck()
        {

        }

        /// <summary>
        /// Function that takes two game objects and checks if they are colliding
        /// </summary>
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

        /// <summary>
        /// Function used to remove any dead gameobject from the game.
        /// </summary>
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

        /// <summary>
        /// Function used to draw every single Player/Platform/Powerup
        /// </summary>
        private void UpdateStuff(GameTime GameTime)
        {
            foreach (Platform p in Platforms)
            {
                p.Update(GameTime);
            }

            foreach (Player p in Players)
            {
                p.Update(GameTime);
            }

            foreach (PowerUp p in PowerUps)
            {
                p.Update(GameTime);
            }
        }

    }
}

