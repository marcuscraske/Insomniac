using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class Scoreboard:UIWindow
    {
        // Purpose: to show the player(s) their statistics during or at the end of gameplay such as kills and scores etc.
        #region "Variables"
            bool CanClose;
            Gamemode Gamemode;
            Texture MapThumbnail;
            string MapName;
            SpriteFont Scoreboard_mapfont;
            SpriteFont Scoreboard_font1;
            SpriteFont Scoreboard_font2;
            SpriteFont Scoreboard_font3;
            string BestPlayer = "";
        #endregion

        public Scoreboard(Gamemode gm, bool canclose):base(gm._Main, null, 87, 69, 850, 630)
        {
            Gamemode = gm;
            // Sets the background texture
            Background = new Core.Texture(gm._Main, "%MAIN%\\Scoreboard\\Background", 100, null);
            // Defines if the scoreboard can be closed by the player
            CanClose = canclose;
            // Defines the map thumbnail texture
            MapThumbnail = new Core.Texture(gm._Main, "%MAP%\\Thumbnail", 100, gm);
            // Gets the map name
            MapName = Path.GetFileName(gm._Map.Root);
            // Fonts used by the scoreboard
            Scoreboard_mapfont = gm._Main.Content.Load<SpriteFont>("Scoreboard_Mapfont");
            Scoreboard_font1 = gm._Main.Content.Load<SpriteFont>("Scoreboard_Font1");
            Scoreboard_font2 = gm._Main.Content.Load<SpriteFont>("Scoreboard_Font2");
            Scoreboard_font3 = gm._Main.Content.Load<SpriteFont>("Scoreboard_Font3");
            // Calculate best player (which is K/D + score) and save scores
            float _bestkd = 0.0F;
            float _temp;
            foreach (Player ply in gm._Players)
            {
                _temp = ply.Score.SCORE + Common.CalculateKDRatio(ply.Score.ACTUAL_TOTAL_KILLS, ply.Score.ACTUAL_TOTAL_DEATHS);
                // Check if its the best yet
                if (_temp > _bestkd || _bestkd == 0.0F)
                {
                    _bestkd = _temp;
                    BestPlayer = ply.Alias;
                }
            }
            BestPlayer += " (" + Common.ConvertToScore((float)Math.Round(_bestkd, 2)) + " K/D+Score)";
        }
        public override void DrawOverride(SpriteBatch spriteBatch)
        {
            // Draw map thumbnail
            spriteBatch.Draw(MapThumbnail._Texture, new Rectangle(Area.X + 22, Area.Y + 56, 72, 62), Color.White);
            // Draw map label
            spriteBatch.DrawString(Scoreboard_mapfont, MapName, new Vector2(Area.X + 103, Area.Y + 70), Color.DarkBlue);
            // Draw best player
            spriteBatch.DrawString(Scoreboard_font1, BestPlayer, new Vector2(Area.X + 123, Area.Y + 187), Color.DarkBlue);
            // Draw player info
            int i = 0;
            int t;
            foreach (Player ply in Gamemode._Players)
            {
                // Temp int
                t = 213 * i;
                // Draw name
                spriteBatch.DrawString(Scoreboard_font3, ply.Alias, new Vector2(Area.X + 3 + t, Area.Y + 224), Color.DarkBlue);
                // Draw score
                spriteBatch.DrawString(Scoreboard_font1, "Score:", new Vector2(Area.X + 3 + t, Area.Y + 264), Color.Black);
                spriteBatch.DrawString(Scoreboard_font2, Common.ConvertToScore(ply.Score.SCORE), new Vector2(Area.X + 3 + t, Area.Y + 284), Color.Black);
                // Draw kills
                spriteBatch.DrawString(Scoreboard_font1, "Kills:", new Vector2(Area.X + 3 + t, Area.Y + 324), Color.Black);
                spriteBatch.DrawString(Scoreboard_font2, ply.Score.ACTUAL_TOTAL_KILLS.ToString(), new Vector2(Area.X + 3 + t, Area.Y + 344), Color.Black);
                // Draw deaths
                spriteBatch.DrawString(Scoreboard_font1, "Deaths:", new Vector2(Area.X + 3 + t, Area.Y + 384), Color.Black);
                spriteBatch.DrawString(Scoreboard_font2, ply.Score.ACTUAL_TOTAL_DEATHS.ToString(), new Vector2(Area.X + 3 + t, Area.Y + 404), Color.Black);
                // Draw K/D
                spriteBatch.DrawString(Scoreboard_font1, "K/D:", new Vector2(Area.X + 3 + t, Area.Y + 444), Color.Black);
                spriteBatch.DrawString(Scoreboard_font2, Core.Common.CalculateKDRatio(ply.Score.ACTUAL_TOTAL_KILLS, ply.Score.ACTUAL_TOTAL_DEATHS).ToString(), new Vector2(Area.X + 3 + t, Area.Y + 464), Color.Black);
                i += 1;
            }
        }
        public override void Logic(GameTime gameTime)
        {
            // Check this window has input
            if (HasInput)
            {
                // Checks if the player can close the scoreboard
                if (CanClose && (Input.IsButtonDown(Input.GAME_BUTTON_SCOREBOARD) || Input.IsKeyDown(Input.GAME_KEY_SCOREBOARD)))
                {
                    CloseWindow();
                }
                // Checks if the user wants the pause menu (since the game has ended)
                else if (Input.IsKeyDown(Input.GAME_KEY_PAUSE) || Input.IsButtonDown(Input.GAME_BUTTON_PAUSE))
                {
                    Gamemode._Main.WindowManager.AddWindow(new MenuPause(Gamemode._Main));
                }
            }
        }
        // Closes this window.
        public void CloseWindow()
        {
            Gamemode.Paused = false;
            Destroy();
        }
        // Releases all the resources used by this scoreboard, this is called by UI Window base when the window is being removed.
        public override void Destroying()
        {
            // Destroy fonts and textures within this class
            Scoreboard_font1 = null;
            Scoreboard_font2 = null;
            Scoreboard_font3 = null;
            Scoreboard_mapfont = null;
            Core.Texture.Dispose(MapThumbnail);
            Gamemode = null;
            // Destroy base
            base.Destroying();
        }
    }
}