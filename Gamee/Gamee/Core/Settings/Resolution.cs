using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public static class Resolution
    {
        // Purpose: this class is a shared class responsible for the games current graphical state.

        #region "Variables"
            // Defines if the game is fullscreen.
            public static bool Fullscreen = false;
            // Defines if effects are enabled.
            public static bool Effects = true;
            // Defines if ambient sound is enabled.
            public static bool Ambient = true;
            // Defines the resolution the game is actually rendered at (this is not what the player nessicarily sees).
            public static readonly Vector2 RenderResolution = new Vector2(1024, 768);
            // Defines the resolution of the game to the player.
            public static Vector2 GameResolution = new Vector2(1280, 1024);
            // The matrix responsible for resolution transformations.
            public static Matrix Matrix = new Matrix();
            // The default viewport/area/window on the games winodw of where the game is rendered.
            public static Viewport DefaultViewport = new Viewport();
        #endregion

        #region "Applying settings"
            /// <summary>
            /// Applies the graphical settings to the game.
            /// </summary>
            /// <param name="graphics"></param>
            public static void ApplySettings(GraphicsDeviceManager graphics)
            {
                // Rebuild the viewport
                DefaultViewport.X = 0;
                DefaultViewport.Y = 0;
                DefaultViewport.Width = (int)GameResolution.X;
                DefaultViewport.Height = (int)GameResolution.Y;
                // Rebuild the render matrix
                Matrix = Matrix.CreateScale(new Vector3(GameResolution.X / RenderResolution.X, GameResolution.Y / RenderResolution.Y, 0));
                // Apply settings to the graphics obj
                graphics.PreferredBackBufferWidth = Convert.ToInt32(GameResolution.X);
                graphics.PreferredBackBufferHeight = Convert.ToInt32(GameResolution.Y);
                graphics.IsFullScreen = Fullscreen;
                graphics.ApplyChanges();
            }
        #endregion

        #region "Configuration"
            /// <summary>
            /// Reads a configuration file and applies it to the game (read the configuration
            /// class for more comments about how it works).
            /// </summary>
            /// <param name="path"></param>
            public static void ReadConfig(GraphicsDeviceManager graphics, string config_path)
            {
                ConfigFile Config = new ConfigFile();
                // Load config
                Config.LoadFromFile(config_path);
                // Read config
                // Width and height
                GameResolution = new Vector2(float.Parse(Config.GetKey("Resolution", "Width")), float.Parse(Config.GetKey("Resolution", "Height")));
                // Fullscreen
                if (Config.GetKey("Resolution", "Fullscreen") == "1")
                {
                    Fullscreen = true;
                }
                else
                {
                    Fullscreen = false;
                }
                // Effects
                if (Config.GetKey("Resolution", "Effects") == "1")
                {
                    Effects = true;
                }
                else
                {
                    Effects = false;
                }
                // Ambient
                if (Config.GetKey("Resolution", "Ambient") == "1")
                {
                    Ambient = true;
                }
                else
                {
                    Ambient = false;
                }
                // Input - not resolution but still easier and more efficient
                if (Config.GetKey("Controls", "UseKeyboardAsGamepad") == "1")
                {
                    Input.UseGamepadPly1ForPly2 = true;
                }
                else
                {
                    Input.UseGamepadPly1ForPly2 = false;
                }
                // Apply configuration
                ApplySettings(graphics);
            }
        #endregion
    }
}