using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class MapEditor:UIWindow
    {
        // Purpose: lists all the gamemodes available to modify, which is passed onto "MapEditorMaps".

        public MapEditor(Gamee.Main main):base(main, null, 0, 0, 1024, 768)
        {
            // Set the background
            Background = new Core.Texture(main, "%MAIN%\\MapEditorMenu", 100, null);
            // Add label
            NonSelectControls.Add(new UILabel(this, "Label1", "Select gamemode:", Color.DarkBlue, 20, 90, false));
            // Add gamemode buttons
            UIButton Button;
            int count = 0;
            // For each gamemode, a button is added to the list
            foreach (DirectoryInfo di in new DirectoryInfo(main.Root + "\\Content\\Gamemodes").GetDirectories())
            {
                count += 1;
                // Defines a button
                Button = new UIButton(this, di.Name, di.Name, 20, 120 + (40 * (count - 1)));
                Button.Area.Width = 300;
                // To catch when a button has been pressed
                Button.Button_Pressed += new UIButton.ButtonPressed(Button_Button_Pressed);
                // Adds the button to this windows control
                Controls.Add(Button);
            }
            // Catches when the selected control has changed (allowing a churp to be played)
            Focus_Changed += new FocusChange(MapEditor_Focus_Changed);
        }
        void MapEditor_Focus_Changed()
        {
            // Plays a churp sound because the selected control has been changed
            Main.PlayChurp();
        }
        void Button_Button_Pressed(UIButton sender)
        {
            // Play selection sound
            Main.PlayChurpSelect();
            // Load map editor maps menu
            Main.WindowManager.AddWindow2(new MapEditorMaps(Main, sender.Text));
        }
        public override void UI_KeyDown(Keys key)
        {
            // A button has been pressed, if its the menu back key the main menu is shown
            if (key == Input.MENU_KEY_BACK)
            {
                Main.WindowManager.AddWindow2(new MenuMain(Main));
            }
        }
        public override void UI_ButtonDown(Buttons button)
        {
            // Same as the above function, but for gamepads
            if (button == Input.MENU_BUTTON_BACK)
            {
                Main.WindowManager.AddWindow2(new MenuMain(Main));
            }
        }
    }
}