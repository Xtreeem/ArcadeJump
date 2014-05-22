using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    // AG:
    //  Lägg till samtliga ljudeffekter. - JvA [2014-05-10]
    static class SoundManager
    {
        #region Variable(s)
        private static SoundEffect PlayerHit, PlayerJump, PowerUp, PowerDown;
        #endregion

        #region Constructor (Tom)
        #endregion

        #region Method(s)
        public static void InitializeSound(ContentManager Content)
        {
            PlayerHit = Content.Load<SoundEffect>("Sound/Hit");
            PlayerJump = Content.Load<SoundEffect>("Sound/Jump");
            PowerUp = Content.Load<SoundEffect>("Sound/Power-down");
            PowerDown = Content.Load<SoundEffect>("Sound/Power-up");
        }

        public static void PlaySound(string Name)
        {
            switch (Name)
            { 
                case "PlayerJump" :
                    PlayerJump.Play();
                    break;
                case "PlayerHit":
                    PlayerHit.Play();
                    break;
                case "PowerUp":
                    PowerUp.Play();
                    break;
                case "PowerDown":
                    PowerDown.Play();
                    break;
            }
        }
        #endregion
    }
}