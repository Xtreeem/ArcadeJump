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
    class Player : AnimatedGameObject
    {
        #region Variables
        PowerUp CurrentPowerUp;
        int Score;
        bool Stunned;
        double StunDuration;
        bool InvertedControls;




        #endregion

        #region Public Methods
        public Player(Vector2 pos, ContentManager Content)
            : base(pos, Content)
        {
            position = pos;
            texture = Content.Load<Texture2D>("Textures/DummyPlayer");
            Hitbox = new Rectangle((int)position.X, (int)position.Y, 100, 100);

            frameHeight = 100;
            frameWidht = 100;
            maxNrFrame = 2;
        }

        public override void Update(GameTime gametime)
        {
            Input();
            base.Update(gametime);
        }

        #endregion

        #region Private Methods
        private void Input()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                velocity.X -= 0.2f;
            }
            else if (velocity.X < 0)
                velocity.X = MathHelper.Clamp(velocity.X + 0.4f, -100, 0);
            
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                velocity.X += 0.2f;
            else if (velocity.X > 0)
                velocity.X = MathHelper.Clamp(velocity.X - 0.4f, 0, 100);
        }
        #endregion
    }
}
