using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class Garage:UIWindow
    {
        // Purpose: allows the player to modify their profile and in-game player for Insomniac, giving them something to work towards
        // (which is buying items from this garage).
        #region "Variables"
            #region "Profile"
                // The players configuration is stored here.
                ConfigFile Config = new ConfigFile();
            #endregion
            #region "Controls"
                // All of these are controls define here for performance reasons, since they're all
                // accessed when the profile is e.g. saved etc (its easier too to get their values).
                // The players ship texture selector.
                UIScrollbox3 Ship;
                // The players modules which they can select.
                UIScrollbox3 Module1;
                UIScrollbox3 Module2;
                UIScrollbox3 Module3;
                UIScrollbox3 Module4;
                UIScrollbox3 Module5;
                UIScrollbox3 Module6;
                // The players current speed ability.
                UILabel CurrentSpeed;
                // A scrollbox with speed that can be purchased.
                UIScrollbox2 BuySpeed;
                // The players current health ability.
                UILabel CurrentHealth;
                // A scrollbox with health that can be purchased.
                UIScrollbox2 BuyHealth;
                // The players current XP.
                UILabel CurrentXP;
            #endregion
        #endregion

        public Garage(Gamee.Main main):base(main, new Core.Texture(main, "%MAIN%\\Garage\\Background", 100, null), 0, 0, 1024, 768)
        {
            // Load config to get XP and calculate level
            Config.LoadFromFile(main.Profile + "\\Insomniac.icf");
            // Add Controls
            // Current XP
            CurrentXP = new UILabel(this, "CurrentXP", "XP: " + Config.GetKey("Player", "XP"), Color.White, 240, 32, false);
            CurrentXP.Font = main.Content.Load<SpriteFont>("Garage_XP");
            NonSelectControls.Add(CurrentXP);
            // Ship type
            Ship = new UIScrollbox3(this, "Ship", 245, 133, 200, 200);
            Ship.AddItem("%GAMEMODE%\\Ships\\ClassU_A", new Core.Texture(main, main.Root + "\\Content\\Gamemodes\\Insomniac\\Textures\\Ships\\ClassU_A", 100, null));
            Ship.AddItem("%GAMEMODE%\\Ships\\ClassY_A", new Core.Texture(main, main.Root + "\\Content\\Gamemodes\\Insomniac\\Textures\\Ships\\ClassY_A", 100, null));
            Ship.SetSelected(Config.GetKey("Ship", "Texture"));
            Controls.Add(Ship);
            // Used to temp store a UIButton
            UIButton Temp;
            // Current speed
            CurrentSpeed = new UILabel(this, "CurrentSpeed", "Current Speed: " + Config.GetKey("Ship", "MaxSpeed"), Color.White, 480, 130, false);
            NonSelectControls.Add(CurrentSpeed);
            // Buy speed
            NonSelectControls.Add(new UILabel(this, "BuySpeedLbl", "Buy:", Color.White, 480, 180, false));
            BuySpeed = new UIScrollbox2(this, "Speed", new Core.Texture(main, "%MAIN%\\UI\\Scrollbox", 100, null), 480, 220, 220, 30);
            BuySpeed.Focused = Color.DarkBlue;
            BuySpeed.NonFocused = Color.DarkBlue;
            BuySpeed.TextOffset = new Vector2(24, 4);
            // Adds the speed and price items to the scrollbox
            BuySpeed.AddItem("1 - 500,000 XP");
            BuySpeed.AddItem("2 - 900,000 XP");
            BuySpeed.AddItem("3 - 1,200,000 XP");
            BuySpeed.AddItem("4 - 1,400,000 XP");
            BuySpeed.AddItem("5 - 1,500,000 XP");
            BuySpeed.AddItem("6 - 1,590,000 XP");
            BuySpeed.AddItem("7 - 1,670,000 XP");
            BuySpeed.AddItem("8 - 1,740,000 XP");
            BuySpeed.AddItem("9 - 1,800,000 XP");
            BuySpeed.AddItem("10 - 1,850,000 XP");
            BuySpeed.AddItem("11 - 1,890,000 XP");
            BuySpeed.AddItem("12 - 1,920,000 XP");
            Controls.Add(BuySpeed);
            // Speed button
            Temp = new UIButton(this, "SpeedButton", "Buy", 600, 260);
            Temp.Button_Pressed += new UIButton.ButtonPressed(Temp_Button_Pressed);
            Controls.Add(Temp);
            // Current health
            CurrentHealth = new UILabel(this, "CurrentHealth", "Current Health: " + Config.GetKey("Ship", "MaxHealth"), Color.White, 700, 130, false);
            NonSelectControls.Add(CurrentHealth);
            // Buy health
            NonSelectControls.Add(new UILabel(this, "BuyHealthLbl", "Buy:", Color.White, 700, 180, false));
            BuyHealth = new UIScrollbox2(this, "BuyHealth", new Core.Texture(main, "%MAIN%\\UI\\Scrollbox", 100, null), 700, 220, 220, 30);
            BuyHealth.Focused = Color.DarkBlue;
            BuyHealth.NonFocused = Color.DarkBlue;
            BuyHealth.TextOffset = new Vector2(24, 4);
            BuyHealth.AddItem("10 - 200,000 XP");
            BuyHealth.AddItem("20 - 300,000 XP");
            BuyHealth.AddItem("30 - 400,000 XP");
            BuyHealth.AddItem("40 - 500,000 XP");
            BuyHealth.AddItem("50 - 600,000 XP");
            BuyHealth.AddItem("60 - 700,000 XP");
            BuyHealth.AddItem("70 - 800,000 XP");
            BuyHealth.AddItem("80 - 900,000 XP");
            BuyHealth.AddItem("90 - 1,000,000 XP");
            BuyHealth.AddItem("100 - 2,000,000 XP");
            BuyHealth.AddItem("200 - 3,000,000 XP");
            BuyHealth.AddItem("300 - 4,000,000 XP");
            Controls.Add(BuyHealth);
            // Health button
            Temp = new UIButton(this, "HealthButton", "Buy", 820, 260);
            Temp.Button_Pressed += new UIButton.ButtonPressed(Temp_Button_Pressed);
            Controls.Add(Temp);
            // Module 1
            Module1 = new UIScrollbox3(this, "Module1", 244, 342, 250, 100);
            // Check which modules the player can have, based on their level
            int XP = Convert.ToInt32(Config.GetKey("Player", "XP"));
            // Allowed - empty module
            Module1.AddItem("None", new Core.Texture(main, "%MAIN%\\Garage\\Module_None", 100, null));
            // Allowed - default module
            Module1.AddItem("LaserDroplet", new Core.Texture(main, "%MAIN%\\Garage\\Module_LaserDroplet", 100, null));
            // Level 2 modules
            if (XP >= 1000000)
            {
                Module1.AddItem("PlasmaBurst", new Core.Texture(main, "%MAIN%\\Garage\\Module_PlasmaBurst", 100, null));
            }
            // Level 3 modules
            if (XP >= 2500000)
            {
                Module1.AddItem("NanoCM", new Core.Texture(main, "%MAIN%\\Garage\\Module_NanoCM", 100, null));
            }
            // Level 4 modules
            if (XP >= 6000000)
            {
                Module1.AddItem("Flak86", new Core.Texture(main, "%MAIN%\\Garage\\Module_Flak86", 100, null));
            }
            // Level 5 modules
            if (XP >= 10000000)
            {
                Module1.AddItem("ASMDestroyer", new Core.Texture(main, "%MAIN%\\Garage\\Module_ASMDestroyer", 100, null));
            }
            // Level 6 modules
            if (XP >= 15000000)
            {
                Module1.AddItem("X32Laser", new Core.Texture(main, "%MAIN%\\Garage\\Module_X32Laser", 100, null));
            }
            // Level 7 modules
            if (XP >= 21000000)
            {
                Module1.AddItem("Hyperdrive", new Core.Texture(main, "%MAIN%\\Garage\\Module_Hyperdrive", 100, null));
            }
            // Level 8 modules
            if (XP >= 28000000)
            {
                Module1.AddItem("Nuke", new Core.Texture(main, "%MAIN%\\Garage\\Module_Nuke", 100, null));
            }
            Module1.SetSelected(Config.GetKey("Ship", "Module1"));
            Controls.Add(Module1);
            // Module 2
            Module2 = new UIScrollbox3(this, "Module2", 244, 453, 250, 100);
            Module2.Items = Module1.Items;
            Module2.SetSelected(Config.GetKey("Ship", "Module2"));
            Controls.Add(Module2);
            // Module 3
            Module3 = new UIScrollbox3(this, "Module3", 244, 565, 250, 100);
            Module3.Items = Module1.Items;
            Module3.SetSelected(Config.GetKey("Ship", "Module3"));
            Controls.Add(Module3);
            // Module 4
            Module4 = new UIScrollbox3(this, "Module4", 678, 342, 250, 100);
            Module4.Items = Module1.Items;
            Module4.SetSelected(Config.GetKey("Ship", "Module4"));
            Controls.Add(Module4);
            // Module 5
            Module5 = new UIScrollbox3(this, "Module5", 678, 453, 250, 100);
            Module5.Items = Module1.Items;
            Module5.SetSelected(Config.GetKey("Ship", "Module5"));
            Controls.Add(Module5);
            // Module 6
            Module6 = new UIScrollbox3(this, "Module6", 678, 565, 250, 100);
            Module6.Items = Module1.Items;
            Module6.SetSelected(Config.GetKey("Ship", "Module6"));
            Controls.Add(Module6);
            // Back button
            Temp = new UIButton(this, "Cancel", "Cancel", 24, 720);
            Temp.Button_Pressed+=new UIButton.ButtonPressed(Temp_Button_Pressed);
            Controls.Add(Temp);
            // Save button
            Temp = new UIButton(this, "Save", "Save", 900, 720);
            Temp.Button_Pressed += new UIButton.ButtonPressed(Temp_Button_Pressed);
            Controls.Add(Temp);
            // To make the churp noise when the selected control changes
            Focus_Changed += new FocusChange(Garage_Focus_Changed);
        }
        void Temp_Button_Pressed(UIButton sender)
        {
            if (sender.Name == "HealthButton")
            {
                // Get the XP and health requested to be bought
                string[] temp = BuyHealth.Items[BuyHealth.SelectedIndex].Replace(" ", "").Replace(",", "").Replace("XP", "").Split('-');
                // Check the purchase is possible
                if (Convert.ToInt32(Config.GetKey("Player", "XP")) - Convert.ToInt32(temp[1]) > 0)
                {
                    // Update config
                    Config.AddToKey("Player", "XP", Convert.ToInt32(temp[1]) * -1);
                    Config.AddToKey("Ship", "MaxHealth", Convert.ToInt32(temp[0]));
                    // Update XP and health labels
                    CurrentXP.Text = "XP: " + Config.GetKey("Player", "XP");
                    CurrentHealth.Text = "Current Health: " + Config.GetKey("Ship", "MaxHealth");
                }
                else
                {
                    Main.WindowManager.AddWindow(new UIMessageBox(Main, "Not enough XP available!"));
                }
            }
            else if(sender.Name == "SpeedButton")
            {
                // Get the XP and speed requested to be bought
                string[] temp = BuySpeed.Items[BuySpeed.SelectedIndex].Replace(" ", "").Replace(",", "").Replace("XP", "").Split('-');
                // Check the purchase is possible
                if (Convert.ToInt32(Config.GetKey("Player", "XP")) - Convert.ToInt32(temp[1]) > 0)
                {
                    // Update config
                    Config.AddToKey("Player", "XP", Convert.ToInt32(temp[1]) * -1);
                    Config.AddToKey("Ship", "MaxSpeed", Convert.ToInt32(temp[0]));
                    // Update XP and speed labels
                    CurrentXP.Text = "XP: " + Config.GetKey("Player", "XP");
                    CurrentSpeed.Text = "Current Speed: " + Config.GetKey("Ship", "MaxSpeed");
                }
                else
                {
                    Main.WindowManager.AddWindow(new UIMessageBox(Main, "Not enough XP available!"));
                }
            }
            else if (sender.Name == "Save")
            {
                // Set ship texture
                Config.AddKey("Ship", "Texture", Ship.Items[Ship.SelectedIndex].NAME);
                // Set module 1
                Config.AddKey("Ship", "Module1", Module1.Items[Module1.SelectedIndex].NAME);
                // Set module 2
                Config.AddKey("Ship", "Module2", Module2.Items[Module2.SelectedIndex].NAME);
                // Set module 3
                Config.AddKey("Ship", "Module3", Module3.Items[Module3.SelectedIndex].NAME);
                // Set module 4
                Config.AddKey("Ship", "Module4", Module4.Items[Module4.SelectedIndex].NAME);
                // Set module 5
                Config.AddKey("Ship", "Module5", Module5.Items[Module5.SelectedIndex].NAME);
                // Set module 6
                Config.AddKey("Ship", "Module6", Module6.Items[Module6.SelectedIndex].NAME);
                // Save configuration file
                Config.SaveToFile(Main.Profile + "\\Insomniac.icf");
                // Go back to the menu
                Main.WindowManager.AddWindow2(new MenuMain(Main));
            }
            else if (sender.Name == "Cancel")
            {
                // Goes back to the main menu without saving changes
                Main.WindowManager.AddWindow2(new MenuMain(Main));
            }
        }
        void Garage_Focus_Changed()
        {
            Main.PlayChurp();
        }
        public override void UI_ButtonDown(Buttons button)
        {
            if (button == Input.MENU_BUTTON_BACK)
            {
                Main.WindowManager.AddWindow2(new MenuMain(Main));
            }
        }
        public override void UI_KeyDown(Keys key)
        {
            if (key == Input.MENU_KEY_BACK)
            {
                Main.WindowManager.AddWindow2(new MenuMain(Main));
            }
        }
    }
}