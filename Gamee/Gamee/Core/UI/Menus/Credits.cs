using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class Credits:Window
    {
        // Purpose: the credits UI window, to inform the gamer who was responsible for constructing the game.

        #region "Variables"
            // The background music.
            Sound CreditsMusic;
            // The background texture.
            Texture Background;
            // The texture to the left which changes every so often between various pictures.
            Texture PhotoViewer;
            // The texture to the left that slides the credits images (the images with all the names etc).
            Texture Pages;
            // The position of the pages texture.
            Rectangle Pages_Position = new Rectangle(524, 134, 370, 445);
            // The position of the photoviewer texture.
            Rectangle PhotoViewer_Position = new Rectangle(34, 75, 305, 255);
            Gamee.Main Main;
        #endregion

        #region "Core"
            public Credits(Gamee.Main main)
            {
                // Defines objects
                Main = main;
                Background = new Core.Texture(main, "%MAIN%\\Credits", 100, null);
                PhotoViewer = new Core.Texture(main, "%MAIN%\\Credits-PhotoViewer", 2000, null);
                Pages = new Core.Texture(main, "%MAIN%\\Credits-Pages", 6000, null);
                CreditsMusic = new Sound(main, "%MAIN%\\Credits.mp3", true, true, null);
            }
            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, Resolution.Matrix);
                // Draw the background
                spriteBatch.Draw(Background._Texture, new Rectangle(0, 0, 1024, 768), Color.White);
                // Draw the photoviewer
                spriteBatch.Draw(PhotoViewer._Texture, PhotoViewer_Position, Color.White);
                // Draw the pages
                spriteBatch.Draw(Pages._Texture, Pages_Position, Color.White);
                spriteBatch.End();
            }
            public override void Logic(GameTime gameTime)
            {
                // Check if the menu select key has been pressed to exit
                if (Input.IsKeyDown(Input.MENU_KEY_SELECT) || Input.IsButtonDown(Input.MENU_BUTTON_SELECT)
                    || Input.IsKeyDown(Input.MENU_KEY_BACK) || Input.IsButtonDown(Input.MENU_BUTTON_BACK))
                {
                    Main.WindowManager.AddWindow2(new MenuMain(Main));
                }
                // Photoviewer logic
                PhotoViewer.Logic();
                // Pages logic
                Pages.Logic();
            }
            public override void Destroying()
            {
                // Destroys all the resources used by this class/window
                Sound.Dispose(CreditsMusic);
                Core.Texture.Dispose(Background);
                Core.Texture.Dispose(PhotoViewer);
                Core.Texture.Dispose(Pages);
                Main = null;
            }
        #endregion
    }
}