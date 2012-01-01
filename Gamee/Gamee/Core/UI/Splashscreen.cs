using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class Splashscreen : Core.Window
    {
        // Purpose: to play textures at the start of the game, e.g. title of the game, engine etc.
        #region "Variables"
            Gamee.Main Main;
            // The index currently selected of the scenes array.
            public int SelectedScene = 0;
            // The length of each scene.
            public int MillisecondsPerScene = 3000;
            // Used to calculate when to change the frame.
            public int LastTick = 0;
            // All the splashscreens.
            List<Texture> Scenes = new List<Texture>();
        #endregion

        #region "Core"
            public Splashscreen(Gamee.Main main)
            {
                Main = main;
                LastTick = Environment.TickCount;
            }
            /// <summary>
            /// Loads all the splashscreen scenes into an array to be played.
            /// </summary>
            /// <param name="path"></param>
            public void LoadScenes(string path)
            {
                // Load all the textures from a folder into the splashscreen array
                foreach (DirectoryInfo di in new DirectoryInfo(path).GetDirectories())
                {
                    Scenes.Add(new Texture(Main, di.FullName, 100, null));
                }
                // Check if theres any scenes or end splash
                if (Scenes.Count == 0)
                {
                    EndSplashscreen();
                }
            }
            /// <summary>
            /// Draws the splashscreen.
            /// </summary>
            /// <param name="spriteBatch"></param>
            public override void Draw(SpriteBatch spriteBatch)
            {
                if (Scenes[SelectedScene]._Texture != null)
                {
                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, Resolution.Matrix);
                    // Draw the current scene
                    spriteBatch.Draw(Scenes[SelectedScene]._Texture, new Rectangle(0, 0, 1024, 768), Color.White);
                    spriteBatch.End();
                }
            }
            /// <summary>
            /// Runs the splashscreen's logic.
            /// </summary>
            public override void Logic(GameTime gameTime)
            {
                if (Environment.TickCount - LastTick >= MillisecondsPerScene)
                {
                    // Set the latest tick
                    LastTick = Environment.TickCount;
                    // Increment the scene or end the splash
                    NextScene();
                }
                if (base.HasInput == true)
                {
                    // Skips the current scene
                    if (Input.IsKeyDown(Input.MENU_KEY_SELECT) || Input.IsButtonDown(Input.MENU_BUTTON_SELECT))
                    {
                        NextScene();
                    }
                }
            }
            public override void Destroying()
            {
                Main = null;
                Scenes = null;
            }
        #endregion

        #region "Functions"
            /// <summary>
            /// Increments to the next scene.
            /// </summary>
            public void NextScene()
            {
                if (SelectedScene + 1 >= Scenes.Count)
                {
                    // No more scenes are available, load the profile menu
                    EndSplashscreen();
                }
                else
                {
                    SelectedScene += 1;
                }
            }
            /// <summary>
            /// Terminates the splashscreen and loads the profile selection menu.
            /// </summary>
            public void EndSplashscreen()
            {
                Main.WindowManager.AddWindow2(new MenuProfile(Main));
            }
        #endregion
    }
}