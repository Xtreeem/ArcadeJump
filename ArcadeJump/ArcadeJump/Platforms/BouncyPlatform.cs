using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump.Features
{
    class BouncyPlatform : Platform
    {
        public BouncyPlatform(Vector2 pos, ContentManager Content, double WidthAdjustment, int StartingWidth)
            : base(pos, Content, WidthAdjustment, StartingWidth)
        { }
    }
}
