using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    class PowerUp : AdvancedGameObject
    {
        public PowerUp(Vector2 pos, ContentManager Content, Vector2 Velocity)
            : base(pos, Content)
        { }

        public PowerUp(Platform HomePlatform, ContentManager Content, Vector2 Velocity)
            : base(new Vector2(0,0), Content)
        {
            position = new Vector2(HomePlatform.SurfaceRectangle.Center.X, HomePlatform.SurfaceRectangle.Top);
            texture = Content.Load<Texture2D>("Textures/Rock");
        }


    }
}
