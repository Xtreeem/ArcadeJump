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
        ContentManager Content;
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
            this.Content = Content;
            this.Players = Players;
            this.PowerUps = PowerUps;
            this.Platforms = Platforms;
            MovableGameObjects = new List<MovableGameObject>();
            Rand = new Random();
            LevelManager = new LevelManager(ref Platforms, Content);
            UpdateGameObjectList();
            Players.Add(new Player(new Vector2(40, 0), Content, 1));
            Players.Add(new Player(new Vector2(1880, 0), Content, 2));


            PowerUps.Add(new PowerUp(new Vector2(100, 100), Content, new Vector2(3, 0)));

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
            if (Vector2.Distance(PointToVector2(ObjectA.Hitbox.Center), PointToVector2(ObjectB.Hitbox.Center)) < ObjectA.Hitbox.Width)
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
                if (ObjectB is Platform && ObjectA.velocity.Y > 0)  //Collision between a player (going downward) and a platform
                {
                    CollisionPlayerPlatform((ObjectA as Player), (ObjectB as Platform));
                }
                else if (ObjectB is Player)     //Collision with player and player
                {
                    CollisionPlayerPlayer((ObjectA as Player), (ObjectB as Player));
                }

                else if (ObjectB is PowerUp)    //Collision with player and powerup
                {
                    CollisionPlayerPowerUp((ObjectA as Player), (ObjectB as PowerUp));
                }
            }
            else if (ObjectA is Platform)
            {
                if (ObjectB is Player && ObjectB.velocity.Y > 0)  //Collision between a player (going downward) and a platform
                {
                    CollisionPlayerPlatform((ObjectB as Player), (ObjectA as Platform));
                }
                if (ObjectB is PowerUp && ObjectB.velocity.Y > 0)   //Collision with platform and powerup (Going downward)
                {
                    CollisionPlatformPowerUp((ObjectA as Platform), (ObjectB as PowerUp));
                }
                else if (ObjectB is Platform)
                {
                    CollisionPlatformPlatform((ObjectA as Platform), (ObjectB as Platform));
                }
            }
            else if (ObjectA is PowerUp)
            {
                if (ObjectB is Player)  //Collision between a player and a powerup
                {
                    CollisionPlayerPowerUp((ObjectB as Player), (ObjectA as PowerUp));
                }
                else if (ObjectB is Platform)     //Collision with PowerUp and Platform
                {
                    CollisionPlatformPowerUp((ObjectB as Platform), (ObjectA as PowerUp));
                }

                else if (ObjectB is PowerUp)    //Collision with PowerUp and PowerUp
                {
                    CollisionPowerUpPowerUp((ObjectA as PowerUp), (ObjectB as PowerUp));
                }
            }


        }

        private void CollisionPlayerPlatform(Player Player, Platform Platform)
        {
            if (Platform.SurfaceRectangle.Intersects(Player.BottomRectangle))
                {
                    Player.SurfaceObject = Platform;
                    Player.velocity.Y = 0;
                }
        }

        private void CollisionPlayerPlayer(Player PlayerA, Player PlayerB)
        { 
            if (PlayerA.PunchingRectangle.Width != 0)
            {
                if( PlayerA.PunchingRectangle.Intersects(PlayerB.Hitbox))
                {
                    Console.WriteLine("Hit");
                    PlayerB.IsHit((PlayerA as MovableGameObject));
                }
            }
        
        }

        private void CollisionPlayerPowerUp(Player Player, PowerUp PowerUp)
        { }

        private void CollisionPlatformPowerUp(Platform Platform, PowerUp PowerUp)
        {
            if (Platform.Hitbox.Intersects(PowerUp.Hitbox))
                PowerUp.SurfaceObject = Platform;
        }

        private void CollisionPlatformPlatform(Platform PlatformA, Platform PlatformB)
        {
            if (PlatformA.Hitbox.Intersects(PlatformB.Hitbox))
            {
                if (Rand.Next(0, 2) > 0)
                {
                    PlatformA.isDead = true;
                }
                else
                    PlatformB.isDead = true;
            }
        }
        
        private void CollisionPowerUpPowerUp(PowerUp PowerUpA, PowerUp PowerUpB)
        { }


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
                    Players.Add(new Player(new Vector2(0,0), Content, Players[i].PlayerNumber)); //Debug line
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

