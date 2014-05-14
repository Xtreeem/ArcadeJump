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
    class Player : AdvancedGameObject
    {
        #region Variables
        int PlayerNumber;
        float JumpPower = 20;
        PowerUp CurrentPowerUp;
        int Score;
        bool Stunned;
        double StunDuration;
        bool InvertedControls;

        KeyboardState OldState;


        #endregion

        #region Public Methods
        public Player(Vector2 pos, ContentManager Content, int PlayerNumber)
            : base(pos, Content)
        {
            this.PlayerNumber = PlayerNumber;
            position = pos;
            texture = Content.Load<Texture2D>("Textures/DummyPlayer");
            Hitbox = new Rectangle((int)position.X, (int)position.Y, 30, 70);
            BottomRectangle = new Rectangle(Hitbox.X, Hitbox.Bottom, Hitbox.Width, 10);

            velocity.Y = 0.001f;
            frameHeight = 100;
            frameWidht = 100;
            maxNrFrame = 5;
        }

        public override void Update(GameTime gametime)
        {
            if (SurfaceObject != null)
                color = Color.Red;
            else
                color = Color.White;


            Input();
            base.Update(gametime);
        }

        public void Jump()
        {
            Console.WriteLine("Jump");
            SurfaceObject = null;
            velocity.Y -= JumpPower;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Function designed to Scan for the control inputs and move the player accordingly
        /// </summary>
        private void Input()
        {
            var NewState = Keyboard.GetState();

            if (PlayerNumber == 1)
            {
                if (NewState.IsKeyDown(Keys.A))
                    velocity.X -= 0.2f;
                else if (velocity.X < 0)
                    velocity.X = MathHelper.Clamp(velocity.X + 0.4f, -100, 0);

                if (NewState.IsKeyDown(Keys.D))
                    velocity.X += 0.2f;
                else if (velocity.X > 0)
                    velocity.X = MathHelper.Clamp(velocity.X - 0.4f, 0, 100);

                if (NewState.IsKeyDown(Keys.W) && SurfaceObject != null && !OldState.IsKeyDown(Keys.W))
                    Jump();
            }

            else
            {
                if (NewState.IsKeyDown(Keys.NumPad1))
                    velocity.X -= 0.2f;
                else if (velocity.X < 0)
                    velocity.X = MathHelper.Clamp(velocity.X + 0.4f, -100, 0);

                if (NewState.IsKeyDown(Keys.NumPad3))
                    velocity.X += 0.2f;
                else if (velocity.X > 0)
                    velocity.X = MathHelper.Clamp(velocity.X - 0.4f, 0, 100);

                if (NewState.IsKeyDown(Keys.NumPad5) && SurfaceObject != null && !OldState.IsKeyDown(Keys.NumPad5))
                    Jump();
            }

            OldState = NewState;
        }

        #endregion
    }
}
