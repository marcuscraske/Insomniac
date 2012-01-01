using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class MenuMain:UIWindow
    {
        // Purpose: the main menu.
        #region "Variables"
            // The sound emitter responsible for the menu music.
            Sound MenuMusic;
            #region "Panel"
                // Location of the panel
                Vector2 Panel = new Vector2(50, 170);
                // Panel textures
                Core.Texture PanelBG;
                Core.Texture Panel_Rank;
                Core.Texture Panel_ProgressBarFull;
                Core.Texture Panel_ProgressBarEmpty;
                // Panel values
                string Panel_TxtRank;
                int Panel_XP;
                string Panel_KD;
                string Panel_Lifespan;
                float Panel_Percentage;
                // Panel percent bar areas
                Rectangle Panel_Percent_Empty_Area;
                Rectangle Panel_Percent_Full_Area;
                // Panel font(s)
                SpriteFont Font1;
            #endregion
        #endregion

        #region "Core"
            public MenuMain(Gamee.Main main):base(main, null, 0, 0, 1024, 768)
            {
                // Background music
                MenuMusic = new Sound(main, "%MAIN%\\Menu.mp3", true, true, null);
                // Background
                Background = new Texture(main, "%MAIN%\\Menu\\Background", 100, null);
                // Menu label
                UILabel Label = new UILabel(this, "Label1", "Menu", Color.White, 829, 160, false);
                Label.Font = main.Content.Load<SpriteFont>("Header");
                NonSelectControls.Add(Label);
                // Name label
                Label = new UILabel(this, "Label2", "Welcome, " + Path.GetFileName(main.Profile) + "!", Color.White, 20, 100, false);
                Label.Font = main.Content.Load<SpriteFont>("MenuName");
                NonSelectControls.Add(Label);
                // Make the menu buttons
                // Play
                Controls.Add(CreateButton("Play", 829, 200));
                // Garage
                Controls.Add(CreateButton("Garage", 829, 240));
                // Achievements
                Controls.Add(CreateButton("Achievements", 829, 280));
                // Map Editor
                Controls.Add(CreateButton("Map Editor", 829, 320));
                // Options
                Controls.Add(CreateButton("Options", 829, 360));
                // Credits
                Controls.Add(CreateButton("Credits", 829, 400));
                // Switch User
                Controls.Add(CreateButton("Switch User", 829, 440));
                // Exit
                Controls.Add(CreateButton("Exit", 829, 480));
                Focus_Changed += new FocusChange(MenuMain_Focus_Changed);
                // Create/define statistics panel
                // Read the stats config file
                ConfigFile Config = new ConfigFile();
                Config.LoadFromFile(main.Profile + "\\Stats.icf");
                // Calculate K/D
                // Temp variables to store stats
                int totaldeaths = 0;
                int totalkills = 0;
                foreach (DictionaryEntry di in ((Hashtable)Config.Groups["Spacegame.Gamemode"]))
                {
                    if (di.Key.ToString().StartsWith("DEATHS_"))
                    {
                        totaldeaths += Convert.ToInt32(di.Value);
                    }
                    else if (di.Key.ToString().StartsWith("KILLS_"))
                    {
                        totalkills += Convert.ToInt32(di.Value);
                    }
                }
                Panel_KD = Common.CalculateKDRatio(totalkills, totaldeaths).ToString();
                // Longest lifetime
                Panel_Lifespan = Config.GetKey("Spacegame.Gamemode", "LongestTime");
                if (Panel_Lifespan != "0" && Panel_Lifespan != "")
                {
                    // Round  from milliseconds to seconds
                    Panel_Lifespan = Math.Round(float.Parse(Panel_Lifespan) / 1000.0F, 0).ToString();
                }
                // Load Insomniac stats
                Config.LoadFromFile(main.Profile + "\\Insomniac.icf");
                // XP
                Panel_XP = Convert.ToInt32(Config.GetKey("Player", "XP"));
                // Set the rank variables based on amount of XP
                if (Panel_XP < 1000000)
                {
                    Panel_TxtRank = "Level 1: Private";
                    Panel_Rank = new Core.Texture(main, "%MAIN%\\Insomniac_Ranks\\Private", 100, null);
                    Panel_Percentage = CalculatePercent(0, 1000000, Panel_XP);
                }
                else if (Panel_XP >= 1000000 && Panel_XP < 2500000)
                {
                    Panel_TxtRank = "Level 2: Corporal";
                    Panel_Rank = new Core.Texture(main, "%MAIN%\\Insomniac_Ranks\\Corporal", 100, null);
                    Panel_Percentage = CalculatePercent(1000000, 2500000, Panel_XP);
                }
                else if (Panel_XP >= 2500000 && Panel_XP < 6000000)
                {
                    Panel_TxtRank = "Level 3: Sergeant";
                    Panel_Rank = new Core.Texture(main, "%MAIN%\\Insomniac_Ranks\\Sergeant", 100, null);
                    Panel_Percentage = CalculatePercent(2500000, 6000000, Panel_XP);
                }
                else if (Panel_XP >= 6000000 && Panel_XP < 10000000)
                {
                    Panel_TxtRank = "Level 4: Sergeant Major";
                    Panel_Rank = new Core.Texture(main, "%MAIN%\\Insomniac_Ranks\\Sergeant_Major", 100, null);
                    Panel_Percentage = CalculatePercent(6000000, 10000000, Panel_XP);
                }
                else if (Panel_XP >= 10000000 && Panel_XP < 15000000)
                {
                    Panel_TxtRank = "Level 5: Lieutenant";
                    Panel_Rank = new Core.Texture(main, "%MAIN%\\Insomniac_Ranks\\Lieutenant", 100, null);
                    Panel_Percentage = CalculatePercent(10000000, 15000000, Panel_XP);
                }
                else if (Panel_XP >= 15000000 && Panel_XP < 21000000)
                {
                    Panel_TxtRank = "Level 6: Commandant";
                    Panel_Rank = new Core.Texture(main, "%MAIN%\\Insomniac_Ranks\\Commandant", 100, null);
                    Panel_Percentage = CalculatePercent(15000000, 21000000, Panel_XP);
                }
                else if (Panel_XP >= 21000000 && Panel_XP < 28000000)
                {
                    Panel_TxtRank = "Level 7: Lt. Colonel";
                    Panel_Rank = new Core.Texture(main, "%MAIN%\\Insomniac_Ranks\\Lt_Colonel", 100, null);
                    Panel_Percentage = CalculatePercent(21000000, 28000000, Panel_XP);
                }
                else if (Panel_XP >= 28000000 && Panel_XP < 36000000)
                {
                    Panel_TxtRank = "Level 8: Colonel";
                    Panel_Rank = new Core.Texture(main, "%MAIN%\\Insomniac_Ranks\\Colonel", 100, null);
                    Panel_Percentage = CalculatePercent(28000000, 36000000, Panel_XP);
                }
                else if (Panel_XP >= 36000000 && Panel_XP < 45000000)
                {
                    Panel_TxtRank = "Level 9: Brigadier";
                    Panel_Rank = new Core.Texture(main, "%MAIN%\\Insomniac_Ranks\\Brigadier", 100, null);
                    Panel_Percentage = CalculatePercent(36000000, 45000000, Panel_XP);
                }
                else if (Panel_XP >= 45000000 && Panel_XP < 55000000)
                {
                    Panel_TxtRank = "Level 10: General";
                    Panel_Rank = new Core.Texture(main, "%MAIN%\\Insomniac_Ranks\\General", 100, null);
                    Panel_Percentage = CalculatePercent(45000000, 55000000, Panel_XP);
                }
                else if (Panel_XP >= 55000000 && Panel_XP < 66000000)
                {
                    Panel_TxtRank = "Level 11: Marshal";
                    Panel_Rank = new Core.Texture(main, "%MAIN%\\Insomniac_Ranks\\Marshal", 100, null);
                    Panel_Percentage = CalculatePercent(55000000, 66000000, Panel_XP);
                }
                else if (Panel_XP >= 66000000)
                {
                    Panel_TxtRank = "Level 12: Game Completed!";
                    Panel_Rank = new Core.Texture(main, "%MAIN%\\Insomniac_Ranks\\Completed", 100, null);
                    Panel_Percentage = 1.0F;
                }
                // Panel background
                PanelBG = new Core.Texture(main, "%MAIN%\\Menu-StatsBG", 100, null);
                // Panel font
                Font1 = main.Content.Load<SpriteFont>("Panel_Font1");
                // Progressbar - textures
                Panel_ProgressBarFull = new Core.Texture(main, "%MAIN%\\ProgressbarFull", 100, null);
                Panel_ProgressBarEmpty = new Core.Texture(main, "%MAIN%\\ProgressbarEmpty", 100, null);
                // Progressbar - areas
                Panel_Percent_Empty_Area = new Rectangle(260 + (int)Panel.X, 230 + (int)Panel.Y, 326, 35);
                Panel_Percent_Full_Area = new Rectangle(260 + (int)Panel.X, 230 + (int)Panel.Y, 326, 35);
                // Resize full area
                Panel_Percent_Full_Area.Width = Convert.ToInt32((float)Panel_Percent_Full_Area.Width * Panel_Percentage);
            }
            /// <summary>
            /// Calculates the percentage of a value between two numbers:
            /// 1.0 - ((C - B) / (C - A)) where:
            /// A = minimum
            /// B = value
            /// C = maximum
            /// 
            /// The number returned is between 0.0 and 1.0.
            /// </summary>
            /// <param name="minimum"></param>
            /// <param name="maximum"></param>
            /// <param name="value"></param>
            float CalculatePercent(float minimum, float maximum, float value)
            {
                return 1.0F - ((maximum - value) / (maximum - minimum));
            }
            public override void DrawOverride(SpriteBatch spriteBatch)
            {
                // Draw panel
                spriteBatch.Draw(PanelBG._Texture, Panel, Color.White);
                // Draw rank image
                spriteBatch.Draw(Panel_Rank._Texture, new Rectangle((int)Panel.X + 20, (int)Panel.Y + 40, 150, 150), Color.White);
                // Draw rank
                spriteBatch.DrawString(Font1, "Rank:   " + Panel_TxtRank, Panel + new Vector2(180, 40), Color.LightGray);
                // Draw XP
                spriteBatch.DrawString(Font1, "XP:       " + Panel_XP.ToString("N"), Panel + new Vector2(180, 80), Color.LightGray);
                // Draw KD
                spriteBatch.DrawString(Font1, "K/D:     " + Panel_KD, Panel + new Vector2(180, 120), Color.LightGray);
                // Draw lifespan
                spriteBatch.DrawString(Font1, "Longest lifespan:" + Environment.NewLine + Panel_Lifespan + " secs", Panel + new Vector2(20, 190), Color.LightGray);
                // Draw progress label
                spriteBatch.DrawString(Font1, "Progression towards next rank:", Panel + new Vector2(260, 190), Color.LightGray);
                // Draw progress bar empty
                spriteBatch.Draw(Panel_ProgressBarEmpty._Texture, Panel_Percent_Empty_Area, Color.White);
                // Draw progress bar progress
                spriteBatch.Draw(Panel_ProgressBarFull._Texture, Panel_Percent_Full_Area, Color.White);
                // Draw progress label
                spriteBatch.DrawString(Font1, Convert.ToInt32(Panel_Percentage * 100.0F).ToString() + "%", Panel + new Vector2(606, 234), Color.LightGray);
            }
            public override void Destroying()
            {
                Sound.Dispose(MenuMusic);
                MenuMusic = null;
            }
            void MenuMain_Focus_Changed()
            {
                Main.PlayChurp();
            }
            void ButtonPressed(UIButton sender)
            {
                // Play selection sound
                Main.PlayChurpSelect();
                // Perform menu operation
                string text = ((UIButton)sender).Text;
                if (text == "Play")
                {
                    Main.WindowManager.AddWindow(new MenuGamemode(Main));
                }
                else if (text == "Garage")
                {
                    Main.WindowManager.AddWindow2(new Garage(Main));
                }
                else if (text == "Achievements")
                {
                    Main.WindowManager.AddWindow2(new MenuAchievements(Main));
                }
                else if (text == "Map Editor")
                {
                    Main.WindowManager.AddWindow2(new MapEditor(Main));
                }
                else if (text == "Options")
                {
                    Main.WindowManager.AddWindow(new Options(Main));
                }
                else if (text == "Credits")
                {
                    Main.WindowManager.AddWindow2(new Credits(Main));
                }
                else if (text == "Switch User")
                {
                    Main.WindowManager.AddWindow2(new MenuProfile(Main));
                }
                else if (text == "Exit")
                {
                    Main.QuitGame();
                }
            }
        #endregion

        #region Functions - creating buttons"
                public UIButton CreateButton(string text, int x, int y)
                {
                    UIButton Temp = new UIButton(this, text, text, x, y);
                    Temp.Area.Width = 175;
                    Temp.Area.Height = 32;
                    Temp.BackgroundNormal = new Core.Texture(Main, "%MAIN%\\Menu\\Button", 100, null);
                    Temp.BackgroundSelected = new Core.Texture(Main, "%MAIN%\\Menu\\ButtonSelected", 100, null);
                    Temp.Centered = false;
                    Temp.Button_Pressed += new UIButton.ButtonPressed(ButtonPressed);
                    return Temp;
                }
            #endregion
    }
}