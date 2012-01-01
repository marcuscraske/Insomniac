using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class Options:UIWindow
    {
        // Purpose: allows the user to modify the games run-time settings such as resolution, if to disable effects and more etc.
        #region "Variables"
            // The options configuration for the player is loaded into this object.
            ConfigFile Config = new ConfigFile();
            // Input controls for the user to change option properties; these are defined for performance
            // and ease.
            UITextbox UI_Width;
            UITextbox UI_Height;
            UILabel UI_Warning;
            UIScrollBox UI_Fullscreen;
            UIScrollBox UI_Effects;
            UIScrollBox UI_Ambient;
            UIScrollBox UI_Gamepad;
            UIScrollbox2 UI_PostProcessing;
        #endregion

        public Options(Gamee.Main main):base(main, null, 192, 144, 640, 480)
        {
            // Allow user closure
            AllowClose = true;
            // Load previous configuration
            Config.LoadFromFile(Main.Profile + "\\Config.icf");
            // Set background
            Background = new Core.Texture(main, "%MAIN%\\Options\\Background", 100, null);
            // Add controls
            // Width
            NonSelectControls.Add(new UILabel(this, "Label1", "Width:", Color.White, 12, 65, false));
            UI_Width = new UITextbox(this, "Width", 85, 60);
            UI_Width.Text = Config.GetKey("Resolution", "Width");
            Controls.Add(UI_Width);
            // Height
            NonSelectControls.Add(new UILabel(this, "Label2", "Height:", Color.White, 12, 105, false));
            UI_Height = new UITextbox(this, "Height", 85, 100);
            UI_Height.Text = Config.GetKey("Resolution", "Height");
            Controls.Add(UI_Height);
            // Warning
            UI_Warning = new UILabel(this, "Label3", "Error: both the width and height must be numeric (0 - 9)!", Color.Yellow, 12, 140, true);
            NonSelectControls.Add(UI_Warning);
            // Fullscreen
            NonSelectControls.Add(new UILabel(this, "Label4", "Fullscreen:", Color.White, 12, 170, false));
            UI_Fullscreen = new UIScrollBox(this, "Fullscreen", 110, 167, 100, 30);
            UI_Fullscreen.AddItem(new Core.Texture(main, "%MAIN%\\Options\\Disabled", 100, null));
            UI_Fullscreen.AddItem(new Core.Texture(main, "%MAIN%\\Options\\Enabled", 100, null));
            UI_Fullscreen.SelectedIndex = Convert.ToInt32(Config.GetKey("Resolution", "Fullscreen"));
            Controls.Add(UI_Fullscreen);
            // Effects
            NonSelectControls.Add(new UILabel(this, "Label5", "Effects:", Color.White, 12, 210, false));
            UI_Effects = new UIScrollBox(this, "Effects", 110, 207, 100, 30);
            UI_Effects.AddItem(new Core.Texture(main, "%MAIN%\\Options\\Disabled", 100, null));
            UI_Effects.AddItem(new Core.Texture(main, "%MAIN%\\Options\\Enabled", 100, null));
            UI_Effects.SelectedIndex = Convert.ToInt32(Config.GetKey("Resolution", "Effects"));
            Controls.Add(UI_Effects);
            // Ambient
            NonSelectControls.Add(new UILabel(this, "Label6a", "Ambient:", Color.White, 12, 250, false));
            UI_Ambient = new UIScrollBox(this, "Ambient", 110, 247, 100, 30);
            UI_Ambient.AddItem(new Core.Texture(main, "%MAIN%\\Options\\Disabled", 100, null));
            UI_Ambient.AddItem(new Core.Texture(main, "%MAIN%\\Options\\Enabled", 100, null));
            UI_Ambient.SelectedIndex = Convert.ToInt32(Config.GetKey("Resolution", "Ambient"));
            Controls.Add(UI_Ambient);
            // Effects notice
            NonSelectControls.Add(new UILabel(this, "Label6", "Note: effects are things such as trails and explosions etc.", Color.LightGray, 12, 290, false));
            // Gamepad
            NonSelectControls.Add(new UILabel(this, "Label7", "Use keyboard as seperate gamepad:", Color.White, 12, 330, false));
            UI_Gamepad = new UIScrollBox(this, "Gamepad", 350, 327, 100, 30);
            UI_Gamepad.AddItem(new Core.Texture(main, "%MAIN%\\Options\\Disabled", 100, null));
            UI_Gamepad.AddItem(new Core.Texture(main, "%MAIN%\\Options\\Enabled", 100, null));
            UI_Gamepad.SelectedIndex = Convert.ToInt32(Config.GetKey("Controls", "UseKeyboardAsGamepad"));
            Controls.Add(UI_Gamepad);
            // Post-processing
            NonSelectControls.Add(new UILabel(this, "Label8", "Post-processing:", Color.White, 12, 370, false));
            UI_PostProcessing = new UIScrollbox2(this, "PostProcessing", new Core.Texture(main, "%MAIN%\\UI\\Scrollbox", 100, null), 160, 367, 200, 30);
            UI_PostProcessing.AddItem("None");
            // Gets a list of post-processing shaders and adds them to the scrollbox
            foreach(FileInfo fi in new DirectoryInfo(main.Root + "\\Content\\Effects").GetFiles("*.fx"))
            {
                UI_PostProcessing.AddItem(fi.Name.Remove(fi.Name.Length - 3));
            }
            UI_PostProcessing.Focused = Color.DarkBlue;
            UI_PostProcessing.NonFocused = Color.DarkBlue;
            UI_PostProcessing.TextOffset = new Vector2(25, 4);
            UI_PostProcessing.SetSelected(Config.GetKey("Resolution", "PostProcessing"));
            Controls.Add(UI_PostProcessing);
            // Post-processing notice
            NonSelectControls.Add(new UILabel(this, "Label9", "Note: you will need to restart a game for post-processing to be applied.", Color.LightGray, 12, 410, false));
            // Save button
            UIButton Save = new UIButton(this, "Save", "Save", 535, 440);
            Save.Button_Pressed +=new UIButton.ButtonPressed(Save_Button_Pressed);
            Controls.Add(Save);
        }
        void Save_Button_Pressed(UIButton sender)
        {
            // Validate width and height
            int temp;
            if (!Int32.TryParse(UI_Width.Text, out temp) || !Int32.TryParse(UI_Height.Text, out temp))
            {
                UI_Warning.Hidden = false;
                return;
            }
            else
            {
                UI_Warning.Hidden = true;
            }
            // Write to config
            Config.AddKey("Resolution", "Width", UI_Width.Text);
            Config.AddKey("Resolution", "Height", UI_Height.Text);
            Config.AddKey("Resolution", "Fullscreen", UI_Fullscreen.SelectedIndex.ToString());
            Config.AddKey("Resolution", "Effects", UI_Effects.SelectedIndex.ToString());
            Config.AddKey("Resolution", "Ambient", UI_Ambient.SelectedIndex.ToString());
            Config.AddKey("Resolution", "PostProcessing", UI_PostProcessing.GetSelected());
            Config.AddKey("Controls", "UseKeyboardAsGamepad", UI_Gamepad.SelectedIndex.ToString());
            // Save config
            Config.SaveToFile(Main.Profile + "\\Config.icf");
            // Apply to game
            Resolution.ReadConfig(Main.graphics, Main.Profile + "\\Config.icf");
            if (Main.Gamemode != null)
            {
                foreach (Player ply in Main.Gamemode._Players)
                {
                    Player.SetupViewport(ply, ply.Index, ply.TotalPlayers);
                }
            }
            // Close window
            Destroy();
        }
    }
}