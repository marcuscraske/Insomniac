using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class MenuAchievements:UIWindow
    {
        // Purpose: displays the players statistical information.

        #region "Variables"
            // The players stats is stored here.
            ConfigFile Stats = new ConfigFile();
        #endregion

        #region "Core"
            public MenuAchievements(Gamee.Main main):base(main, null, 0, 0, 1024, 768)
            {
                Background = new Core.Texture(main, "%MAIN%\\MenuAchievements", 100, null);
                // Label - Select gamemode
                NonSelectControls.Add(new UILabel(this, "Label1", "Select gamemode:", Color.White, 20, 70, false));
                // Load stats file
                Stats.LoadFromFile(main.Profile + "\\Stats.icf");
                // Used temp, its better for optimisation to define one variable and to reuse it
                UIButton temp;
                // Add gamemode buttons
                int maxheight = 0;
                foreach (DictionaryEntry di in Stats.Groups)
                {
                    temp = new UIButton(this, di.Key.ToString(), di.Key.ToString().Replace(".Gamemode", ""), 20, 100 + maxheight);
                    temp.Area.Width = 250;
                    temp.BackgroundNormal = new Core.Texture(main, "%MAIN%\\Menu\\ButtonProfile", 100, null);
                    temp.BackgroundSelected = new Core.Texture(main, "%MAIN%\\Menu\\ButtonProfileSelected", 100, null);
                    temp.Button_Pressed += new UIButton.ButtonPressed(temp_Button_Pressed);
                    Controls.Add(temp);
                    maxheight += 40;
                }
                // Back button
                temp = new UIButton(this, "Back", "Back", 904, 728);
                temp.Button_Pressed +=new UIButton.ButtonPressed(temp_Button_Pressed);
                Focus_Changed += new FocusChange(MenuAchievements_Focus_Changed);
                Controls.Add(temp);
            }
            void MenuAchievements_Focus_Changed()
            {
                Main.PlayChurp();
            }
        #endregion

        #region "Button pressed/load stats/load achievements"
            void temp_Button_Pressed(UIButton sender)
            {
                Main.PlayChurpSelect();
                string t = ((UIButton)sender).Name;
                if (t == "Back")
                {
                    GoBack();
                }
                else
                {
                    // Clear all over label controls
                    NonSelectControls.Clear();
                    // Label - Select gamemode
                    NonSelectControls.Add(new UILabel(this, "Label1", "Select gamemode:", Color.White, 20, 70, false));
                    // The button is a gamemode, therefore display its stats
                    // Temp variables
                    int column1_1_x = 300;
                    int column1_2_x = 425;
                    int column2_1_x = 650;
                    int column2_2_x = 775;
                    int row1_y = 100;
                    int row2_y = 100;
                    UILabel temp;
                    SpriteFont Font1 = Main.Content.Load<SpriteFont>("Achievements_font1");
                    SpriteFont Font2 = Main.Content.Load<SpriteFont>("Achievements_font2");
                    // Kills header
                    temp = new UILabel(this, "LabelKills", "Kills", Color.White, column1_1_x, row1_y, false);
                    temp.Font = Font1;
                    NonSelectControls.Add(temp);
                    // Death header
                    temp = new UILabel(this, "LabelDeaths", "Deaths", Color.White, column2_1_x, row2_y, false);
                    temp.Font = Font1;
                    NonSelectControls.Add(temp);
                    // Increment rows
                    row1_y += 40;
                    row2_y += 40;
                    // Add items
                    // Temp variables for loop
                    string str;
                    int total_deaths = 0;
                    int total_kills = 0;
                    foreach (DictionaryEntry di in ((Hashtable)Stats.Groups[t]))
                    {
                        str = di.Key.ToString();
                        if (str.StartsWith("KILLS_"))
                        {
                            str = str.Remove(0, 6);
                            // Add row name
                            temp = new UILabel(this, "Label", str, Color.White, column1_1_x, row1_y, false);
                            temp.Font = Font2;
                            NonSelectControls.Add(temp);
                            // Add row value (amount of deaths)
                            temp = new UILabel(this, "Label", di.Value.ToString(), Color.White, column1_2_x, row1_y, false);
                            temp.Font = Font2;
                            NonSelectControls.Add(temp);
                            row1_y += 40;
                            total_kills += Convert.ToInt32(di.Value.ToString());
                        }
                        else if (str.StartsWith("DEATHS_"))
                        {
                            str = str.Remove(0, 7);
                            // Add row name
                            temp = new UILabel(this, "Label", str, Color.White, column2_1_x, row2_y, false);
                            temp.Font = Font2;
                            NonSelectControls.Add(temp);
                            // Add row value (amount of deaths)
                            temp = new UILabel(this, "Label", di.Value.ToString(), Color.White, column2_2_x, row2_y, false);
                            temp.Font = Font2;
                            NonSelectControls.Add(temp);
                            row2_y += 40;
                            total_deaths += Convert.ToInt32(di.Value.ToString());
                        }
                    }
                    // Set row-size to the biggest y
                    row1_y = Convert.ToInt32(Common.Max(row1_y, row2_y));
                    row2_y = row1_y;
                    // Total kills
                    temp = new UILabel(this, "LabelKills", "Total Kills:", Color.White, column1_1_x, row1_y, false);
                    temp.Font = Font1;
                    NonSelectControls.Add(temp);
                    row1_y += 40;
                    temp = new UILabel(this, "LabelKills", total_kills.ToString(), Color.White, column1_1_x, row1_y, false);
                    temp.Font = Font2;
                    NonSelectControls.Add(temp);
                    row1_y += 40;
                    // Total deaths
                    temp = new UILabel(this, "LabelKills", "Total Deaths:", Color.White, column2_1_x, row2_y, false);
                    temp.Font = Font1;
                    NonSelectControls.Add(temp);
                    row2_y += 40;
                    temp = new UILabel(this, "LabelKills", total_deaths.ToString(), Color.White, column2_1_x, row2_y, false);
                    temp.Font = Font2;
                    NonSelectControls.Add(temp);
                    row2_y += 40;
                    // K:D
                    temp = new UILabel(this, "LabelKD", "K/D Ratio:", Color.White, column1_1_x, row1_y, false);
                    temp.Font = Font1;
                    NonSelectControls.Add(temp);
                    temp = new UILabel(this, "LabelKD", Common.CalculateKDRatio(total_kills, total_deaths).ToString(), Color.White, column2_1_x, row1_y, false);
                    temp.Font = Font2;
                    NonSelectControls.Add(temp);
                    row1_y += 40;
                    row2_y += 40;
                    // Number of spawns
                    temp = new UILabel(this, "LabelSpawns", "Spawn Count:", Color.White, column1_1_x, row1_y, false);
                    temp.Font = Font1;
                    NonSelectControls.Add(temp);
                    row1_y += 40;
                    str = Stats.GetKey(t, "NumSpawns");
                    if (str == "")
                    {
                        str = "N/A";
                    }
                    temp = new UILabel(this, "LabelSpawns", str, Color.White, column1_1_x, row1_y, false);
                    temp.Font = Font2;
                    NonSelectControls.Add(temp);
                    row1_y += 40;
                    // Longest time alive
                    temp = new UILabel(this, "LabelAlive", "Longest lifespan:", Color.White, column2_1_x, row2_y, false);
                    temp.Font = Font1;
                    NonSelectControls.Add(temp);
                    row2_y += 40;
                    str = Stats.GetKey(t, "LongestTime");
                    if (str == "")
                    {
                        str = "N/A";
                    }
                    else
                    {
                        str = Math.Round((float)(Convert.ToInt64(str) / 1000), 2).ToString() + " seconds";
                    }
                    temp = new UILabel(this, "LabelAlive", str, Color.White, column2_1_x, row2_y, false);
                    temp.Font = Font2;
                    NonSelectControls.Add(temp);
                    row2_y += 40;
                }
            }
        #endregion

        #region "Go back"
            public override void UI_KeyDown(Keys key)
            {
                if (key == Input.MENU_KEY_BACK)
                {
                    GoBack();
                }
            }
            public override void UI_ButtonDown(Buttons button)
            {
                if (button == Input.MENU_BUTTON_BACK)
                {
                    GoBack();
                }
            }
            public void GoBack()
            {
                Main.PlayChurpSelect();
                Main.WindowManager.AddWindow2(new MenuMain(Main));
            }
        #endregion
    }
}