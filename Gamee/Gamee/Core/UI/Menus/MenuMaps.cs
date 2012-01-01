using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class MenuMaps:UIWindow
    {
        #region "Variables"
            public string Gamemode = "";
        #endregion
        
        public MenuMaps(Gamee.Main main, string gamemode):base(main, null, 0, 0, 1024, 768)
        {
            // Set gamemode
            Gamemode = gamemode;
            // Background
            Background = new Core.Texture(main, "%MAIN%\\MenuMaps\\Background", 100, null);
            // Allow user closure
            AllowClose = true;
            // Add map buttons
            int Columns = 2;
            int NumColumns = -1;
            int NumRows = 0;
            UIMapButton Temp;
            foreach (DirectoryInfo di in new DirectoryInfo(main.Root + "\\Content\\Gamemodes\\" + Gamemode + "\\Maps").GetDirectories())
            {
                NumColumns += 1;
                if (NumColumns >= Columns)
                {
                    NumColumns = 0;
                    NumRows += 1;
                }
                Temp = new UIMapButton(this, di.Name, new Core.Texture(main, di.FullName + "\\Textures\\Thumbnail", 100, null), di.Name, 10 + (NumColumns * 490), 85 + (NumRows * 85));
                Temp.Area.Width = 490;
                Temp.Area.Height = 85;
                // Add handler to catch button press
                Temp.Button_Pressed += new UIMapButton.ButtonPressed(Button_Pressed);
                // Add to controls
                Controls.Add(Temp);
            }
            // Churp noise on focus index change
            Focus_Changed += new FocusChange(MenuMaps_Focus_Changed);
        }
        void MenuMaps_Focus_Changed()
        {
            Main.PlayChurp();
        }
        void Button_Pressed(UIMapButton sender)
        {
            Main.PlayChurpSelect();
            Main.WindowManager.AddWindow(new MenuPlayers(Main, Gamemode, sender.Text));
            Destroy();
        }    
    }
}