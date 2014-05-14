using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{

    abstract class GameObject
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin = new Vector2();
        public bool isDead;
        public SpriteEffects spriteEffect;
        public float scale = 1f;
        public Color color = Color.White;
        public float rotation;
        public Rectangle Hitbox;

        public GameObject(Vector2 pos)
        {
            this.position = pos;
        }

        public virtual void Update(GameTime gameTime)
        {
        
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, Hitbox, null, color, rotation, origin, spriteEffect, 0);
        }
    }
}
