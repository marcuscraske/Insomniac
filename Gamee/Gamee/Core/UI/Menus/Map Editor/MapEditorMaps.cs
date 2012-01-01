using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class MapEditorMaps:UIWindow
    {
        // Purpose: makes a list of maps that in the future could launch an actual map editor to edit them. However currently this
        // window only lists maps and allows you to go to MapEditorCreate (another UI window for creating maps).

        #region "Variables"
            // The current gamemode (the name as s a string) being viewed.
            string Gamemode = "";
        #endregion
        public MapEditorMaps(Gamee.Main main, string gamemode):base(main, null, 0, 0, 1024, 768)
        {
            // Set the gamemode of the map
            Gamemode = gamemode;
            // Set the background
            Background = new Core.Texture(main, "%MAIN%\\MapEditorMenu", 100, null);
            // Add label
            NonSelectControls.Add(new UILabel(this, "Label1", "Select map:", Color.DarkBlue, 20, 90, false));
            // Add maps
            UIButton Button;
            int count = 0;
            foreach (DirectoryInfo di in new DirectoryInfo(main.Root + "\\Content\\Gamemodes\\" + Gamemode + "\\Maps").GetDirectories())
            {
                count += 1;
                Button = new UIButton(this, di.Name, di.Name, 20, 120 + (40 * (count - 1)));
                Button.Area.Width = 300;
                Button.Button_Pressed += new UIButton.ButtonPressed(Button_Button_Pressed);
                Controls.Add(Button);
            }
            Focus_Changed += new FocusChange(MapEditorMaps_Focus_Changed);
            // Add create button
            Button = new UIButton(this, "Create", "Create", 20, 728);
            Button.Button_Pressed+=new UIButton.ButtonPressed(Button_Button_Pressed);
            Controls.Add(Button);
            // Add Exit
            Button = new UIButton(this, "Exit", "Exit", 874, 728);
            Button.Button_Pressed+=new UIButton.ButtonPressed(Button_Button_Pressed);
            Controls.Add(Button);
            // Add label about editing
            NonSelectControls.Add(new UILabel(this, "Label2", "Note: map editing is currently not available in this edition; however you can create maps below.", Color.DarkBlue, 20, 680, false));
        }
        void MapEditorMaps_Focus_Changed()
        {
            Main.PlayChurp();
        }
        void Button_Button_Pressed(UIButton sender)
        {
            // Play selection sound
            Main.PlayChurpSelect();
            // Check what button was pressed
            if (sender.Text == "Create")
            {
                Main.WindowManager.AddWindow2(new MapEditorCreate(Main, Gamemode));
            }
            else if (sender.Text == "Exit")
            {
                Main.WindowManager.AddWindow2(new MenuMain(Main));
            }
            else
            {
                // The button is a map
            }
        }
        public override void UI_KeyDown(Keys key)
        {
            if (key == Input.MENU_KEY_BACK)
            {
                Main.WindowManager.AddWindow2(new MapEditor(Main));
            }
        }
        public override void UI_ButtonDown(Buttons button)
        {
            if (button == Input.MENU_BUTTON_BACK)
            {
                Main.WindowManager.AddWindow2(new MapEditor(Main));
            }
        }
    }
}