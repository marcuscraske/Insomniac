using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class MenuGamemode:UIWindow
    {
        // Purpose: allows the player to select the gamemode they want to play.
        public MenuGamemode(Gamee.Main main):base(main, null, 20, 100, 550, 480)
        {
            // Background
            Background = new Core.Texture(main, "%MAIN%\\MenuGamemode\\Background", 100, null);
            // Load gamemode selection
            int totalheight = 0;
            // Add buttons for each gamemode available to play
            UIButton Temp;
            foreach(DirectoryInfo di in new DirectoryInfo(main.Root + "\\Content\\Gamemodes").GetDirectories())
            {
                // Create and define button
                Temp = new UIButton(this, di.Name, "", 30, 65 + totalheight);
                Temp.Area.Width = 490;
                Temp.Area.Height = 85;
                Temp.BackgroundNormal = new Core.Texture(main, di.FullName + "\\Textures\\Config\\MenuButton", 100, null);
                Temp.BackgroundSelected = new Core.Texture(main, di.FullName + "\\Textures\\Config\\MenuButtonSelected", 100, null);
                // Add handler to catch button press
                Temp.Button_Pressed += new UIButton.ButtonPressed(ButtonPressed);
                // Add to controls
                Controls.Add(Temp);
                totalheight += 95;
            }
            // Add handler for focus index change
            Focus_Changed += new FocusChange(MenuGamemode_Focus_Changed);
            // Alow user closure
            AllowClose = true;
        }
        void ButtonPressed(UIButton sender)
        {
            Main.PlayChurpSelect();
            // Load map selector
            Main.WindowManager.AddWindow(new MenuMaps(Main, sender.Name));
            Destroy();
        }
        void MenuGamemode_Focus_Changed()
        {
            Main.PlayChurp();
        }
    }
}