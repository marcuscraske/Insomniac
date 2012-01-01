using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class MenuControls:UIWindow
    {
        // Purpose: displays the controls of the game.
        #region "Variables"
            // Control textures
            Core.Texture Controller;
            Core.Texture Keyboard;
            // The controller texture is selected by default.
            bool ControllerSelected = true;
        #endregion
        public MenuControls(Gamee.Main main):base(main, null, 0, 0, 1024, 768)
        {
            // Define control textures
            Controller = new Texture(main, "%MAIN%\\Controls-Controller", 100, null);
            Keyboard = new Texture(main, "%MAIN%\\Controls-Keyboard", 100, null);
            // Set the background
            Background = Controller;
        }
        public override void Logic(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                Toggle();
            }
            else if(Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Close();
            }
        }
        // Toggles the controls being shown.
        void Toggle()
        {
            if (ControllerSelected)
            {
                Background = Keyboard;
                ControllerSelected = false;
            }
            else
            {
                Background = Controller;
                ControllerSelected = true;
            }
        }
        // Closes the window and returns to the profile menu.
        void Close()
        {
            // Return to the profile menu
            Main.WindowManager.AddWindow2(new MenuProfile(Main));
            Destroy();
        }
    }
}