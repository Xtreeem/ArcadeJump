using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ArcadeJump
{
   class GUI
   {
       List<Player> Players;
       SpriteFont Font, HighScoreFont;
       ContentManager Content;
       int ScoreP1, ScoreP2, ScoreToBeat;
       Texture2D PowerUpFrame;
       Rectangle P1PowerUpFrameRec, P2PowerUpFrameRec;
       Vector2 PlayerPos1, PlayerPos2;
       Vector2 PuPos1, PuPos2;
       Vector2 P1TextPos, P2TextPos;
       Vector2 P1InvertTextPos, P2InvertTextPos;
       public GUI(ref List<Player> Players, ContentManager Content)
       {
           this.Players = Players;
           this.Content = Content;
           PlayerPos1 = new Vector2(10, 10);
           PlayerPos2 = new Vector2(1850, 10);
           P1TextPos = PlayerPos1;
           P2TextPos = new Vector2(PlayerPos2.X - 120, PlayerPos2.Y);
           P1PowerUpFrameRec = new Rectangle((int)PlayerPos1.X, (int)PlayerPos1.Y + 45, 55, 55);
           P2PowerUpFrameRec = new Rectangle((int)PlayerPos2.X+5, (int)PlayerPos2.Y + 45, 55, 55);
           P1InvertTextPos = new Vector2(P1PowerUpFrameRec.Right + 10, P1PowerUpFrameRec.Top);
           P2InvertTextPos = new Vector2(P2PowerUpFrameRec.Left - 10 -55, P2PowerUpFrameRec.Top);
           PuPos1 = new Vector2(P1PowerUpFrameRec.X + 5, P1PowerUpFrameRec.Y + 5);
           PuPos2 = new Vector2(P2PowerUpFrameRec.X + 5, P2PowerUpFrameRec.Y + 5);
           PowerUpFrame = Content.Load<Texture2D>("PowerUpFrame");
           Font = Content.Load<SpriteFont>("Fonts/font");
           HighScoreFont = Content.Load<SpriteFont>("Fonts/HighScoreFont");
           ReadInHighScore();
       }

       public void Update(GameTime GameTime)
       {
               foreach (Player p in Players)
               {
                   if (p.PlayerNumber == 1)
                   {
                       ScoreP1 = (int)p.Score;
                       if (p.CurrentPowerUp != null)
                       {
                           p.CurrentPowerUp.DrawRectangle = new Rectangle((int)PuPos1.X, (int)PuPos1.Y, 45, 45);
                       }
                   }
                   else if (p.PlayerNumber == 2)
                   {
                       ScoreP2 = (int)p.Score;
                       if (p.CurrentPowerUp != null)
                       {
                           p.CurrentPowerUp.DrawRectangle = new Rectangle((int)PuPos2.X, (int)PuPos2.Y, 45, 45);
                       }
                   }
               }
               HighScoreCheck();
       }

       /// <summary>
       /// If there is more than one player, player two score is visible.
       /// </summary>
       /// <param name="SpriteBatch"></param>
       public void Draw(SpriteBatch SpriteBatch)
       {
           SpriteBatch.Begin();
           //if (Players.Count != 0)
               for (int i = 0; i < Players.Count; i++)
               {
                   if (Players[i].PlayerNumber == 1)
                       PlayerOne(SpriteBatch, Players[i]);
                   else
                       PlayerTwo(SpriteBatch, Players[i]);
               }
           //else

           SpriteBatch.End();
       }

       /// <summary>
       /// Draw Player One score and current power up
       /// </summary>
       /// <param name="SpriteBatch"></param>
       private void PlayerOne(SpriteBatch SpriteBatch, Player Player)
       {
           string text = "Score:" + ScoreP1;

           SpriteBatch.DrawString(Font, text, P1TextPos, Color.Black);
           SpriteBatch.Draw(PowerUpFrame, P1PowerUpFrameRec, Color.White);
           if (Player.InvertedControlsDuration > 0)
           {
               string InvertText = Player.InvertedControlsDuration.ToString();
               if (InvertText.Length > 3)
                   InvertText = InvertText.Remove(3);
               SpriteBatch.DrawString(Font, InvertText, P1InvertTextPos, Color.Red);
           }

           if (Player.CurrentPowerUp != null)
               SpriteBatch.Draw(Player.CurrentPowerUp.texture, Player.CurrentPowerUp.DrawRectangle, Player.CurrentPowerUp.color);
       }

       /// <summary>
       /// Player Two score and Current power up
       /// </summary>
       /// <param name="SpriteBatch"></param>
       private void PlayerTwo(SpriteBatch SpriteBatch, Player Player)
       {
           string text = "Score:" + ScoreP2;
           SpriteBatch.DrawString(Font, text, P2TextPos, Color.Black);
           SpriteBatch.Draw(PowerUpFrame, P2PowerUpFrameRec, Color.White);
           if (Player.InvertedControlsDuration > 0)
           {
               string InvertText = Player.InvertedControlsDuration.ToString();
               if (InvertText.Length > 3)
                   InvertText = InvertText.Remove(3);
               SpriteBatch.DrawString(Font, InvertText, P2InvertTextPos,  Color.Red);
           }
           if (Player.CurrentPowerUp != null)
           {
               SpriteBatch.Draw(Player.CurrentPowerUp.texture, Player.CurrentPowerUp.DrawRectangle, Player.CurrentPowerUp.color);
           }
       }

       private void HighScoreCheck()
       {
           int tempScore = ScoreToBeat;
           if (ScoreP1 > ScoreToBeat)
               ScoreToBeat = ScoreP1;
           if (ScoreP2 > ScoreToBeat)
               ScoreToBeat = ScoreP2;
           if (tempScore != ScoreToBeat)
               WriteDownHighScore();
       }

       /// <summary>
       /// Reads in the Score to beat from a text file
       /// </summary>
       private void ReadInHighScore()
       {
           if (File.Exists("HighScore.txt"))
           {
               string tempString;
               StreamReader _sReader = new StreamReader(@"HighScore.txt", true);
               {
                   while (!_sReader.EndOfStream)
                   {
                       tempString = _sReader.ReadLine();
                       int.TryParse(tempString, out ScoreToBeat);
                   }
               }
               _sReader.Close();
           }
       }

       /// <summary>
       /// Writes the new Score to beat to a text file
       /// </summary>
       private void WriteDownHighScore()
       {
               StreamWriter _sWriter = new StreamWriter(@"HighScore.txt", false);
               {
                   _sWriter.Write(ScoreToBeat);
               }
               _sWriter.Close();
       }

   }
}