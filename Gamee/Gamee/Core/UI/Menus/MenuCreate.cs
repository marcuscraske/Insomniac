using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class MenuCreate:UIWindow
    {
        // Purpose: creates a new player.

        #region "Variables"
            UITextbox Name;
        #endregion
        public MenuCreate(Gamee.Main main):base(main, null, 0, 0, 1024, 768)
        {
            // Set the background
            Background = new Core.Texture(main, "%MAIN%\\Menu\\Background", 100, null);
            // Enter your name label
            UILabel Label = new UILabel(this, "Label1", "Enter Your Player's Name", Color.White, 362, 250, false);
            Label.Font = main.Content.Load<SpriteFont>("Header");
            NonSelectControls.Add(Label);
            // Name textbox
            Name = new UITextbox(this, "Name", 387, 300);
            Name.Text = "";
            Controls.Add(Name);
            // Create Button
            UIButton Create = new UIButton(this, "Create", "Create", 462, 340);
            Create.Button_Pressed += new UIButton.ButtonPressed(CreatePressed);
            Controls.Add(Create);
            // Cancel button
            UIButton Cancel = new UIButton(this, "Cancel", "Cancel", 914, 728);
            Cancel.Button_Pressed += new UIButton.ButtonPressed(Cancel_Button_Pressed);
            Controls.Add(Cancel);
            // Catch focus index change
            Focus_Changed += new FocusChange(MenuCreate_Focus_Changed);
        }
        void Cancel_Button_Pressed(UIButton sender)
        {
            Main.PlayChurpSelect();
            Main.WindowManager.AddWindow2(new MenuProfile(Main));
        }
        void MenuCreate_Focus_Changed()
        {
            Main.PlayChurp();
        }
        void CreatePressed(UIButton sender)
        {
            // Ensure theres no slashes, else abort and tell the user
            if (Name.Text.Contains("\\"))
            {
                Main.WindowManager.AddWindow(new UIMessageBox(Main, "Your name cannot contain slashes!"));
                return;
            }
            // Creates the new profile
            Main.PlayChurpSelect();
            Common.CopyFolder(Main.Root + "\\Content\\Settings\\Default Profile", Main.Root + "\\Content\\Settings\\Profiles\\" + Name.Text);
            Main.WindowManager.AddWindow2(new MenuProfile(Main));
        }
    }
}