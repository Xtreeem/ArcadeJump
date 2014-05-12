using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ArcadeJump
{

    //testing
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        LevelManager LevelManager;
        Manager Manager;

        List<Platform> Platforms;
        List<Player> Players;
        List<PowerUp> PowerUps;
        #endregion

        #region StartUp
        public Game1()
        {
            Platforms = new List<Platform>();
            LevelManager = new LevelManager(ref Platforms, Content);
            Manager = new Manager(ref Platforms, ref PowerUps, ref Players);


            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //LevelManager = new LevelManager(
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }
        #endregion

        #region GameLoop
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            Manager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Manager.DrawStuff(spriteBatch);
            base.Draw(gameTime);
        }
        #endregion
    }
}
