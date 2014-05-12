using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump.Features
{
    class BouncyPlatform : Platform
    {
        public BouncyPlatform(Texture2D tex, Vector2 pos, float changeWidth)
            : base(tex, pos, changeWidth)
        { }
    }
}
