using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class UIMessageBox:UIWindow
    {
        // Purpose: creates a message-box which appears over the game with a transparent background. This messagebox
        // alerts the user something e.g. invalid field data etc.
        public UIMessageBox(Gamee.Main main, string text):base(main, null, 0 , 0, 1024, 768)
        {
            // Set background
            Background = new Core.Texture(main, "%MAIN%\\MessageBox", 100, null);
            // Create label
            UILabel Label = new UILabel(this, "Label1", text, Color.White, 312 + 20, 309 + 40, false);
            Label.Font = main.Content.Load<SpriteFont>("UIMessageBox");
            Controls.Add(Label);
        }
        public override void UI_KeyDown(Keys key)
        {
            // When any key is pressed the messagebox is destroyed
            Destroy();
        }
        public override void UI_ButtonDown(Buttons button)
        {
            // When any button is pressed the messagebox is destroyed
            Destroy();
        }
    }
}