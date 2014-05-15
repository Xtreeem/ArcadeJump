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
        private static SoundEffect a, b, c;
        #endregion

        #region Constructor (Tom)
        #endregion

        #region Method(s)
        public static void InitializeSound(ContentManager Content)
        {
            //a = Content.Load<SoundEffect>("NamnPåEffekt");
            //b = Content.Load<SoundEffect>("NamnPåEffekt");
            //c = Content.Load<SoundEffect>("NamnPåEffekt");

            
        }

        public static void PlaySound(string Name)
        {
            switch (Name)
            { 
                case "a" :
                    a.Play();
                    break;
                case "b" :
                    b.Play();
                    break;
                case "c" :
                    c.Play();
                    break;
            }
        }
        #endregion
    }
}