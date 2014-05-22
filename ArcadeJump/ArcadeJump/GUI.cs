using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcadeJump
{
   class GUI
   {
       List<Player> Players;
       SpriteFont Font;
       ContentManager Content;
       double ScoreP1, ScoreP2;
       PowerUp CurrentPU_P1, CurrentPU_P2;

       public GUI(ref List<Player> Players, ContentManager Content)
       {
           this.Players = Players;
           this.Content = Content;
           Font = Content.Load<SpriteFont>("Fonts/font");
       }

       /// <summary>
       /// Draw Player One score and current power up
       /// </summary>
       /// <param name="SpriteBatch"></param>
       private void PlayerOne(SpriteBatch SpriteBatch)
       {
           string text = "Player 1\n "+ ScoreP1;
           Vector2 playerPos1 = new Vector2(500, 500);
           SpriteBatch.DrawString(Font, text, playerPos1, Color.Blue);

           if (CurrentPU_P1 != null)
           {
               Vector2 player1PU = new Vector2(800, 200);
               SpriteBatch.Draw(CurrentPU_P1.texture, player1PU, new Rectangle(0, 0, 50, 50), Color.White);
           }
       }

       /// <summary>
       /// Player Two score and Current power up
       /// </summary>
       /// <param name="SpriteBatch"></param>
       private void PlayerTwo(SpriteBatch SpriteBatch)
       {
           
           string text = "Player 2\n " + ScoreP2;
           Vector2 playerPos2 = new Vector2(500, 300);
           SpriteBatch.DrawString(Font, text, playerPos2, Color.Blue);

           if (CurrentPU_P2 != null)
           {
               Vector2 player2PU = new Vector2(800, 200);
               SpriteBatch.Draw(CurrentPU_P2.texture, player2PU, new Rectangle(0, 0, 50, 50), Color.White);
           }
       }

       public void Update(GameTime GameTime)
       {
           ScoreP1 = Players[0].Score;
           CurrentPU_P1 = Players[0].CurrentPowerUp;

           if (Players.Count >= 1)
           {
               ScoreP2 = Players[1].Score;
               CurrentPU_P2 = Players[1].CurrentPowerUp;
           }
           
           
           
       }

       /// <summary>
       /// If there is more than one player, player two score is visible.
       /// </summary>
       /// <param name="SpriteBatch"></param>
       public void Draw(SpriteBatch SpriteBatch)
       {
           SpriteBatch.Begin();
           PlayerOne(SpriteBatch);
           if (Players.Count >= 1)
           {
               PlayerTwo(SpriteBatch);
           }
           SpriteBatch.End();
       }




   }
}