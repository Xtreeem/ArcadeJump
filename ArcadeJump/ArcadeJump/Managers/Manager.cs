using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        KeyboardState OldState;
        ContentManager Content;
        float SpeedModifier;
        float maxSpeedmodifier = 2;
        public double ElapsedGameTime = 0;
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
            LevelManager = new LevelManager(ref PowerUps, ref Platforms, Content);
            UpdateGameObjectList();
        }

        public void Update(GameTime GameTime)
        {
            RemoveDeadStuff();
            UpdateStuff(GameTime);
            CollisionManager();
            Input();
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
        /// <summary>
        /// Function used to update the list used in Collion detection
        /// </summary>
        private void UpdateGameObjectList()
        {
            MovableGameObjects.Clear();
            MovableGameObjects.AddRange(Platforms);
            MovableGameObjects.AddRange(Players);
            MovableGameObjects.AddRange(PowerUps);
        }

        /// <summary>
        /// Function called to Create a player
        /// </summary>
        /// <param name="PlayerIndex">Either 1 or 2, depending on what player you want to spawn</param>
        private void SpawnPlayer(int PlayerIndex)
        {
            bool ValidSpawn = true;                     //Starts of by assuming you can spawn a player

            foreach (Player P in Players)               //Checks all active players to see if any of them have the same playerIndex as the one you want to spawn
            {
                if(P.PlayerNumber == PlayerIndex)
                    ValidSpawn = false;                 //if so it tells you that you can not
            }

            if (ValidSpawn)                             //if you still can spawn a player
            {
                for (int i = 0; i < Players.Count; i++)
                {
                    Players[i].Score = 0;               //Resets each players score
                }

                ElapsedGameTime = 0;                    //Reset the elapsed game (controls the width adjustment and speed adjustment of the platforms)

                if (PlayerIndex == 1)                   //Spawns a player on the correct side of the screen, as well as a small platoform for them to stand on.
                {
                    Players.Add(new Player(new Vector2(50, 0), Content, 1));
                    Platforms.Add(new Platform(new Vector2(0, 150), Content, 0, 100, true));
                    UpdateGameObjectList();
                }
                else
                {
                    Players.Add(new Player(new Vector2(1880, 0), Content, 2));
                    Platforms.Add(new Platform(new Vector2(1830, 150), Content, 0, 100, true));
                    UpdateGameObjectList();
                }
            }
        }

        #region CollisionStuff
        /// <summary>
        /// Manager function that will do do our collision checks for us
        /// </summary>
        private void CollisionManager()
        {
            int adjuster = 0;                                                                   //Variable that will be used to ensure taht we do not check A vs B and then B vs A again.
            for (int a = 0; a < MovableGameObjects.Count; a++)
            {
                adjuster++;
                for (int b = 0+ adjuster; b < MovableGameObjects.Count; b++)
                {
                    if (FirstCollisionCheck(MovableGameObjects[a], MovableGameObjects[b]))      //Checks if an object is close enough to qualify for future collisioncheck
                    {
                        CollisionChecking(MovableGameObjects[a], MovableGameObjects[b]);        //Does a closer collision check to see if they really have colided
                    }
                }
            }
        }

        /// <summary>
        /// Function used to check if the distance between the center of two objects is closer than their combined width (plus the width adjustment applied to platforms over time)
        /// </summary>
        private bool FirstCollisionCheck(GameObject ObjectA, GameObject ObjectB)
        {
            if (Vector2.Distance(PointToVector2(ObjectA.Hitbox.Center), PointToVector2(ObjectB.Hitbox.Center)) < ObjectA.Hitbox.Width + ObjectB.Hitbox.Width + LevelManager.WidthAdjustment)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Function that takes two game objects and checks if they are colliding
        /// </summary>
        private void CollisionChecking(MovableGameObject ObjectA, MovableGameObject ObjectB)
        {
            if (ObjectA is Player && ObjectA.position.Y > -(ObjectA.texture.Height / 2))        //Checks if the first object is a player and ensures that it is not of the top of the screen
            {
                if (ObjectB is Platform && ObjectA.velocity.Y > 0)                              //Collision between a player (going downward) and a platform
                {
                    CollisionPlayerPlatform((ObjectA as Player), (ObjectB as Platform));
                }
                else if (ObjectB is Player)                                                     //Collision with player and player
                {
                    CollisionPlayerPlayer((ObjectA as Player), (ObjectB as Player));
                }

                else if (ObjectB is PowerUp)                                                    //Collision with player and powerup
                {
                    CollisionPlayerPowerUp((ObjectA as Player), (ObjectB as PowerUp));
                }
            }
            else if (ObjectA is Platform)
            {
                if (ObjectB is Player && ObjectB.velocity.Y > 0 && ObjectB.position.Y > -(ObjectA.texture.Height / 2))  //Collision between a player (going downward) and a platform
                {
                    CollisionPlayerPlatform((ObjectB as Player), (ObjectA as Platform));
                }
                if (ObjectB is PowerUp && ObjectB.velocity.Y > 0)                               //Collision with platform and powerup (Going downward)
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
                if (ObjectB is Player && ObjectB.position.Y > -(ObjectB.texture.Height / 2))    //Collision between a player and a powerup
                {
                    CollisionPlayerPowerUp((ObjectB as Player), (ObjectA as PowerUp));
                }
                else if (ObjectB is Platform)                                                   //Collision with PowerUp and Platform
                {
                    CollisionPlatformPowerUp((ObjectB as Platform), (ObjectA as PowerUp));
                }

                else if (ObjectB is PowerUp)                                                    //Collision with PowerUp and PowerUp
                {
                    CollisionPowerUpPowerUp((ObjectA as PowerUp), (ObjectB as PowerUp));
                }
            }
        }
        /// <summary>
        /// Function that is called when a player and a platform collide
        /// </summary>
        private void CollisionPlayerPlatform(Player Player, Platform Platform)
        {
            if (Platform.SurfaceRectangle.Intersects(Player.BottomRectangle))
                {
                    Player.SurfaceObject = Platform;
                    Player.velocity.Y = 0;
                }
        }
        /// <summary>
        /// Function called when a player and another player collides
        /// </summary>
        private void CollisionPlayerPlayer(Player PlayerA, Player PlayerB)
        {
            if (PlayerA.PunchingRectangle.Width != 0)                       //checks if the first player is punching
            {
                if (PlayerA.PunchingRectangle.Intersects(PlayerB.Hitbox))   //checks to see if he has hit the other player
                {
                    Console.WriteLine("Hit");
                    PlayerB.GetStunned(PlayerA.PunchStunDuration);          //Stunst he second player
                }
            }
            
            if (PlayerB.PunchingRectangle.Width != 0)                       //checks if the second player is punching
            {
                if (PlayerB.PunchingRectangle.Intersects(PlayerA.Hitbox))   //Checks to see if he has hit the other player
                {
                    Console.WriteLine("Hit");
                    PlayerA.GetStunned(PlayerB.PunchStunDuration);          //Stuns the first player
                }
            }
        
        }

        /// <summary>
        /// Checks if a Player and a PowerUp collided
        /// </summary>
        private void CollisionPlayerPowerUp(Player Player, PowerUp PowerUp)
        { 
            //If the players punch box hits the powerup
            if (Player.PunchingRectangle.Intersects(PowerUp.Hitbox))
                if (Player.spriteEffect != SpriteEffects.FlipHorizontally)
                {
                    PowerUp.velocity = new Vector2(Player.velocity.X + 10, PowerUp.velocity.Y);
                    PowerUp.Kicked(Player.Kickpower);
                }
                else
                {
                    PowerUp.velocity = new Vector2(-Player.velocity.X - 10, PowerUp.velocity.Y);
                    PowerUp.Kicked(Player.Kickpower);
                }
            //If the players kick box hits the powerup
            if (Player.KickingRectangle.Intersects(PowerUp.Hitbox))
            {
                Console.WriteLine("Kicked");
                if (Player.spriteEffect != SpriteEffects.FlipHorizontally)
                {
                    PowerUp.velocity = new Vector2(Player.velocity.X + 10, PowerUp.velocity.Y);
                    PowerUp.Kicked(Player.Kickpower);
                }
                else
                {
                    PowerUp.velocity = new Vector2(-Player.velocity.X - 10, PowerUp.velocity.Y);
                    PowerUp.Kicked(Player.Kickpower);
                }
            }
            //If the Players hitbox is hit rather than his kick/punchbox
            if (Player.Hitbox.Intersects(PowerUp.Hitbox))
            {
                Console.WriteLine("ping");
                PowerUp.PickedUp(ref Player);   
            }
        }
 
        /// <summary>
        /// Checks if a platform and a PowerUp collided
        /// </summary>
        private void CollisionPlatformPowerUp(Platform Platform, PowerUp PowerUp)
        {
            if (Platform.Hitbox.Intersects(PowerUp.Hitbox))
            {
                PowerUp.SurfaceObject = Platform;
                PowerUp.velocity.Y = 0;
            }
        }

        /// <summary>
        /// Checks if two platforms intersects and if so destroys one of them
        /// </summary>
        private void CollisionPlatformPlatform(Platform PlatformA, Platform PlatformB)
        {
            if (PlatformA.Hitbox.Intersects(PlatformB.Hitbox))
            {
                if (Rand.Next(0, 2) > 0)
                {
                    if (!PlatformA.Indestructable)
                        PlatformA.isDead = true;
                }
                else
                    if (!PlatformB.Indestructable)
                        PlatformB.isDead = true;
            }
        }
        
        /// <summary>
        /// Unused method for possible future expandability
        /// </summary>
        private void CollisionPowerUpPowerUp(PowerUp PowerUpA, PowerUp PowerUpB)
        { }
        #endregion

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
                    //Players.Add(new Player(new Vector2(0,0), Content, Players[i].PlayerNumber)); //Debug line
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
            if (Players.Count < 0)
                ElapsedGameTime += GameTime.ElapsedGameTime.TotalSeconds;
            SpeedModifier = (float)((maxSpeedmodifier / LevelManager.IntendedGameLength) * ElapsedGameTime); 
            LevelManager.Update(ElapsedGameTime);


            foreach (Platform p in Platforms)
            {
                p.Update(GameTime, SpeedModifier);
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

        /// <summary>
        /// Used to check for input to the manager
        /// Currently accepts:
        /// F9 to Spawn player 1 
        /// F10 to Spawn player 2
        /// </summary>
        private void Input()
        {
            KeyboardState NewState = Keyboard.GetState();
            if (NewState.IsKeyDown(Keys.F9) && OldState.IsKeyUp(Keys.F9))
                SpawnPlayer(1);
            if (NewState.IsKeyDown(Keys.F10) && OldState.IsKeyUp(Keys.F10))
                SpawnPlayer(2);
            OldState = NewState;
        }
        #endregion
    }
}

