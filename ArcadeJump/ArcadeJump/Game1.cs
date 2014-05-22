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
        GUI GUI;
        Song BackgroundMusicOne;
        bool songStart = false;

        Manager Manager;

        List<Platform> Platforms;
        List<Player> Players;
        List<PowerUp> PowerUps;
        #endregion

        #region StartUp
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            PowerUps = new List<PowerUp>();
            Players = new List<Player>();
            Platforms = new List<Platform>();
            GUI = new GUI(ref Players, Content);
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            BackgroundMusicOne = Content.Load<Song>("BgMusic");
            MediaPlayer.IsRepeating = true;

            SoundManager.InitializeSound(Content);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Manager = new Manager(ref Platforms, ref PowerUps, ref Players, Content);
            
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
            if(!songStart)
            {
                MediaPlayer.Play(BackgroundMusicOne);
                songStart = true;
            }
            Manager.Update(gameTime);
            GUI.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);
            Manager.DrawStuff(spriteBatch);
            GUI.Draw(spriteBatch);
            base.Draw(gameTime);
        }
        #endregion
    }
}
