﻿using Microsoft.Xna.Framework;
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
        public PowerUp(Vector2 pos, ContentManager Content)
            : base(pos, Content)
        { }
    }
}
