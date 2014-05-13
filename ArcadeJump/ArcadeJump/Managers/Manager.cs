using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        double ElapsedGameTime = 0;
        LevelManager LevelManager;
        List<Platform> Platforms;
        List<Player> Players;
        List<PowerUp> PowerUps;
        List<MovableGameObject> MovableGameObjects;
        #endregion

        #region Public Methods
        public Manager(ref List<Platform> Platforms, ref List<PowerUp> PowerUps, ref List<Player> Players, ContentManager Content)
        {
            this.Players = Players;
            this.PowerUps = PowerUps;
            this.Platforms = Platforms;
            MovableGameObjects = new List<MovableGameObject>();
            Rand = new Random();
            LevelManager = new LevelManager(ref Platforms, Content);
            UpdateGameObjectList();
        }

        public void Update(GameTime GameTime)
        {
            RemoveDeadStuff();
            UpdateStuff(GameTime);
            CollisionManager();
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

        private void UpdateGameObjectList()
        {
            MovableGameObjects.Clear();
            MovableGameObjects.AddRange(Platforms);
            MovableGameObjects.AddRange(Players);
            MovableGameObjects.AddRange(PowerUps);
        }


        /// <summary>
        /// Checks the Distance between two gameobjects to see if they are able to collide
        /// </summary>
        private void CollisionManager()
        {
            int adjuster = 0;
            for (int a = 0; a < MovableGameObjects.Count; a++)
            {
                adjuster++;
                for (int b = 0+ adjuster; b < MovableGameObjects.Count; b++)
                {
                    if (FirstCollisionCheck(MovableGameObjects[a], MovableGameObjects[b]))
                    {
                        CollisionChecking(MovableGameObjects[a], MovableGameObjects[b]);
                    }
                }
            }
        }

        private bool FirstCollisionCheck(GameObject ObjectA, GameObject ObjectB)
        {
            if (Vector2.Distance(PointToVector2(ObjectA.Hitbox.Center), PointToVector2(ObjectB.Hitbox.Center)) < 200)
                return true;
            else
                return false;
        }



        /// <summary>
        /// Function that takes two game objects and checks if they are colliding
        /// </summary>
        private void CollisionChecking(MovableGameObject ObjectA, MovableGameObject ObjectB)
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
                    //if ((ObjectA as Platform).Hitbox.Intersects((ObjectB as Platform).Hitbox))
                    if (Vector2.Distance(PointToVector2(ObjectA.Hitbox.Center), PointToVector2(ObjectB.Hitbox.Center)) < ObjectA.Hitbox.Width)
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
                {
                    Platforms.RemoveAt(i);
                    LevelManager.CreateNewPlatform();
                    UpdateGameObjectList();
                }
            }
            for (int i = 0; i < PowerUps.Count; i++)
            {
                if (PowerUps[i].isDead)
                {
                    PowerUps.RemoveAt(i);
                    UpdateGameObjectList();
                }
            }
            for (int i = 0; i < Players.Count; i++)
            {
                if (Players[i].isDead)
                {
                    Players.RemoveAt(i);
                    UpdateGameObjectList();
                }
            }
        }

        /// <summary>
        /// Function used to draw every single Player/Platform/Powerup
        /// </summary>
        private void UpdateStuff(GameTime GameTime)
        {
            ElapsedGameTime += GameTime.ElapsedGameTime.TotalSeconds;
            LevelManager.Update(ElapsedGameTime);


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

        private Vector2 PointToVector2(Point Point)
        {
            return new Vector2(Point.X, Point.Y);
        }
        #endregion


    }
}

