using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{                                      
    public class MenuPause:UIWindow
    {
        // Purpose: the UI window displayed when the game is paused (the pause menu); this allows the player to
        // e.g. change the options, exit to menu and exit game etc.
        public MenuPause(Gamee.Main main): base(main, null, 0, 0, 1024, 768)
        {
            // Background
            Background = new Texture(Main, "%MAIN%\\MenuPause\\Background", 100, null);
            // Label
            UILabel Label = new UILabel(this, "Label1", "Pause Menu", Color.White, 20, 30, false);
            Label.Font = main.Content.Load<SpriteFont>("Header");
            NonSelectControls.Add(Label);
            // Buttons
            Controls.Add(CreateButton("Resume", 20, 100));
            Controls.Add(CreateButton("Restart", 20, 140));
            Controls.Add(CreateButton("Cheats", 20, 180));
            Controls.Add(CreateButton("Options", 20, 220));
            Controls.Add(CreateButton("Exit to menu", 20, 260));
            Controls.Add(CreateButton("Exit game", 20, 300));
            Focus_Changed += new FocusChange(MenuPause_Focus_Changed);
        }
        void MenuPause_Focus_Changed()
        {
            Main.PlayChurp();
        }
        /// <summary>
        /// Creates a menu button at a defined location.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public UIButton CreateButton(string text, int x, int y)
        {
            UIButton Temp = new UIButton(this, text, text, x, y);
            Temp.Area.Width = 175;
            Temp.Area.Height = 32;
            Temp.BackgroundNormal = new Texture(Main, "%MAIN%\\Menu\\Button", 100, null);
            Temp.BackgroundSelected = new Texture(Main, "%MAIN%\\Menu\\ButtonSelected", 100, null);
            Temp.Centered = false;
            Temp.Button_Pressed += new UIButton.ButtonPressed(Temp_Button_Pressed);
            return Temp;
        }
        // Called when a button has been pressed.
        void Temp_Button_Pressed(UIButton sender)
        {
            Main.PlayChurpSelect();
            string text = sender.Text;
            if (text == "Resume")
            {
                Main.Gamemode.Paused = false;
                Destroy();
            }
            else if (text == "Restart")
            {
                Main.Gamemode.RestartLevel();
                Destroy();
            }
            else if (text == "Cheats")
            {
                Main.WindowManager.AddWindow(new UIMessageBox(Main, "Why cheat? Too hard? :("));
            }
            else if (text == "Options")
            {
                Main.WindowManager.AddWindow(new Options(Main));
            }
            else if (text == "Exit to menu")
            {
                Main.WindowManager.AddWindow2(new MenuMain(Main));
            }
            else if (text == "Exit game")
            {
                Main.QuitGame();
            }
        }
    }
}