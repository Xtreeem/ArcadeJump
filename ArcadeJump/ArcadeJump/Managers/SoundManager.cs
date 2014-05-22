using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
    /// <summary>
    /// Static sound manager used to play sound from anywhere inside the program. 
    /// </summary>
    static class SoundManager
    {
        #region Variable(s)
        private static SoundEffect PlayerHit, PlayerJump, PowerUp, PowerDown;
        #endregion

        #region Constructor (Tom)
        #endregion

        #region Method(s)
        /// <summary>
        /// Function used to load all the sounds into their sound variables.
        /// </summary>
        public static void InitializeSound(ContentManager Content)
        {
            PlayerHit = Content.Load<SoundEffect>("Sound/Hit");
            PlayerJump = Content.Load<SoundEffect>("Sound/Jump");
            PowerUp = Content.Load<SoundEffect>("Sound/Power-down");
            PowerDown = Content.Load<SoundEffect>("Sound/Power-up");
        }
        /// <summary>
        /// Function called from anywhere to play any sound wanted
        /// </summary>
        /// <param name="Name">Name of the sound file requested</param>
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