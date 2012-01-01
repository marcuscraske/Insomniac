using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Spacegame
{
    public class ModulesSystem
    {
        // Purpose: player heads-up display (HUD), displaying the players: modules,
        // health and more.
        #region "Variables"
            public Core.Gamemode Gamemode;
            #region "Textures"
                // The texture displayed over the game when the player is dead.
                public Core.Texture ScreenDead;
                // The texture used for the bar at the bottom of the screen.
                public Core.Texture BottomBar;
                // The texture used for the health icon at the bottom of the screen.
                public Core.Texture BottomHealth;
                // The texture used for the score icon at the bottom of the screen.
                public Core.Texture BottomScore;
                // The font used to draw text at the bottom of the screen.
                public SpriteFont BottomFont;
                // The texture used as a frame for the modules at the top.
                public Core.Texture TopBar;
                // Used to check if the game resolution has changed to rebuild module positions
                public Vector2 TempResolution;
            #endregion
        #endregion

        #region "Core"
            public ModulesSystem(Gamee.Main main, Core.Gamemode gm)
            {
                Gamemode = gm;
                ScreenDead = new Core.Texture(main, "%GAMEMODE%\\UI\\GameOver", 100, gm);
                BottomBar = new Core.Texture(main, "%GAMEMODE%\\UI\\Bar", 100, gm);
                BottomHealth = new Core.Texture(main, "%GAMEMODE%\\UI\\Bar_Health", 100, gm);
                BottomScore = new Core.Texture(main, "%GAMEMODE%\\UI\\Bar_Score", 100, gm);
                BottomFont = main.Content.Load<SpriteFont>("PlayerUI");
                TopBar = new Core.Texture(main, "%GAMEMODE%\\UI\\Topbar", 100, gm);
                TempResolution = Core.Resolution.GameResolution;
            }
            public void Draw(SpriteBatch sb)
            {
                // Loop through each player and draw their UI
                foreach (Core.Player ply in Gamemode._Players)
                {
                    if (ply.Dead)
                    {
                        // Draw death screen
                        sb.Draw(ScreenDead._Texture, new Rectangle(ply.Camera.Viewport.X, ply.Camera.Viewport.Y, ply.Camera.Viewport.Width, ply.Camera.Viewport.Height), Color.White);
                    }
                    else
                    {
                        #region "Top bar"
                            // Draw module icons
                            ((Module.Module)ply.Data["M1"]).Draw(sb);
                            ((Module.Module)ply.Data["M2"]).Draw(sb);
                            ((Module.Module)ply.Data["M3"]).Draw(sb);
                            ((Module.Module)ply.Data["M4"]).Draw(sb);
                            ((Module.Module)ply.Data["M5"]).Draw(sb);
                            ((Module.Module)ply.Data["M6"]).Draw(sb);
                            // Draw bar
                            sb.Draw(TopBar._Texture, new Rectangle(
                                (int)(ply.Camera.Viewport.X + (ply.Camera.Viewport.Width / 2) - TopBar.Centre.X),
                                (int)(ply.Camera.Viewport.Y), TopBar._Texture.Width, TopBar._Texture.Height), Color.White);
                        #endregion
                        #region "Bottom bar"
                            // Draw bar
                            sb.Draw(BottomBar._Texture, new Rectangle(ply.Camera.Viewport.X, ply.Camera.Viewport.Y + ply.Camera.Viewport.Height - 32, ply.Camera.Viewport.Width, 32), Color.White);
                            // Draw health
                            sb.Draw(BottomHealth._Texture, new Rectangle(ply.Camera.Viewport.X + 2, ply.Camera.Viewport.Height + ply.Camera.Viewport.Y - 32, 32, 32), Color.White);
                            if (ply.Entity != null)
                            {
                                sb.DrawString(BottomFont, ply.Entity.Health + " (" + ply.Entity.Lives + " lives)", new Vector2(ply.Camera.Viewport.X + 38, ply.Camera.Viewport.Y + ply.Camera.Viewport.Height - 27), Color.White);
                            }
                            // Draw score
                            sb.Draw(BottomScore._Texture, new Rectangle(ply.Camera.Viewport.X + ply.Camera.Viewport.Width - 32, ply.Camera.Viewport.Y + ply.Camera.Viewport.Height - 32, 32, 32), Color.White);
                            sb.DrawString(BottomFont, Core.Common.ConvertToScore(ply.Score.SCORE), new Vector2(ply.Camera.Viewport.X + ply.Camera.Viewport.Width - 38 - BottomFont.MeasureString(Core.Common.ConvertToScore(ply.Score.SCORE)).X, ply.Camera.Viewport.Y + ply.Camera.Viewport.Height - 27), Color.White);
                        #endregion
                    }
                }
            }
            public void Logic()
            {
                // Check if the resolution has changed
                if (Core.Resolution.GameResolution != TempResolution)
                {
                    RebuildModulePositions();
                    TempResolution = Core.Resolution.GameResolution;
                }
                // Texture logic
                ScreenDead.Logic();
                BottomBar.Logic();
                BottomHealth.Logic();
                BottomScore.Logic();
                TopBar.Logic();
                // Module logic
                foreach (Core.Player ply in Gamemode._Players)
                {
                    ((Module.Module)ply.Data["M1"]).Logic(); // Module 1
                    ((Module.Module)ply.Data["M2"]).Logic(); // Module 2
                    ((Module.Module)ply.Data["M3"]).Logic(); // Module 3
                    ((Module.Module)ply.Data["M4"]).Logic(); // Module 4
                    ((Module.Module)ply.Data["M5"]).Logic(); // Module 5
                    ((Module.Module)ply.Data["M6"]).Logic(); // Module 6
                }
            }
            // Disposes all the resources used by this class.
            public void Destroy()
            {
                Core.Texture.Dispose(ScreenDead);
                Core.Texture.Dispose(BottomBar);
                Core.Texture.Dispose(BottomHealth);
                Core.Texture.Dispose(BottomScore);
                Core.Texture.Dispose(TopBar);
            }
        #endregion

        #region "Functions"
            /// <summary>
            /// Rebuilds the screen position of each module for each player on the screen.
            /// </summary>
            public void RebuildModulePositions()
            {
                // Total width of the topbar
                float tbw = 290.0F;
                foreach (Core.Player ply in Gamemode._Players)
                {
                    ((Module.Module)ply.Data["M1"]).Area = new Rectangle((int)(ply.Camera.Viewport.X + (ply.Camera.Viewport.Width / 2) - (tbw / 2)), ply.Camera.Viewport.Y, 40, 40);
                    ((Module.Module)ply.Data["M2"]).Area = new Rectangle((int)(ply.Camera.Viewport.X + (ply.Camera.Viewport.Width / 2) - (tbw / 2) + 50), ply.Camera.Viewport.Y, 40, 40);
                    ((Module.Module)ply.Data["M3"]).Area = new Rectangle((int)(ply.Camera.Viewport.X + (ply.Camera.Viewport.Width / 2) - (tbw / 2) + 100), ply.Camera.Viewport.Y, 40, 40);
                    ((Module.Module)ply.Data["M4"]).Area = new Rectangle((int)(ply.Camera.Viewport.X + (ply.Camera.Viewport.Width / 2) - (tbw / 2) + 150), ply.Camera.Viewport.Y, 40, 40);
                    ((Module.Module)ply.Data["M5"]).Area = new Rectangle((int)(ply.Camera.Viewport.X + (ply.Camera.Viewport.Width / 2) - (tbw / 2) + 200), ply.Camera.Viewport.Y, 40, 40);
                    ((Module.Module)ply.Data["M6"]).Area = new Rectangle((int)(ply.Camera.Viewport.X + (ply.Camera.Viewport.Width / 2) - (tbw / 2) + 250), ply.Camera.Viewport.Y, 40, 40);
                }
            }
        #endregion

        #region "Functions - static"
        /// <summary>
            /// Creates a module object from a string representation of its namespace and classname,
            /// e.g. it takes Nuke to form Spacegame.Module.Nuke which is then returns that class.
            /// </summary>
            /// <param name="classname"></param>
            /// <param name="gm"></param>
            /// <param name="ply"></param>
            /// <returns></returns>
            public static Module.Module GetModuleByClass(string classname, Core.Gamemode gm, Core.Player ply)
            {
                return (Module.Module)Activator.CreateInstance(Type.GetType("Spacegame.Module." + classname, true, true), new object[] { ply, gm });
            }
        #endregion
    }
}