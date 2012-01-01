using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class MenuPlayers:UIWindow
    {
        // Purpose: 
        #region "Variables"
            string temp_gamemode = "";
            string temp_mappath = "";
            UIScrollbox2 Player1;
            UIScrollbox2 Player1_Team;
            UIScrollbox2 Player2;
            UIScrollbox2 Player2_Team;
            UIScrollbox2 Player3;
            UIScrollbox2 Player3_Team;
            UIScrollbox2 Player4;
            UIScrollbox2 Player4_Team;
        #endregion

        #region "Core"
            public MenuPlayers(Gamee.Main main, string gamemode, string mappath):base(main, null, 20, 100, 550, 480)
            {
                // Allow user closure
                AllowClose = true;
                // Set background
                Background = new Core.Texture(main, "%MAIN%\\MenuPlayers\\Background", 100, null);
                // Set temp variables
                temp_gamemode = gamemode;
                temp_mappath = mappath;
                // Generate list of items for the buttons
                List<string> Temp = new List<string>();
                Temp.Add("None");
                foreach (DirectoryInfo di in new DirectoryInfo(main.Root + "\\Content\\Settings\\Profiles").GetDirectories())
                {
                    Temp.Add(di.Name);
                }
                // Define buttons
                // Player 1 - player
                Player1 = new UIScrollbox2(this, "Player1", new Core.Texture(main, "%MAIN%\\MenuPlayers\\Player1", 100, null), 30, 65, 350, 32);
                Player1.Items = Temp;
                Player1.Scroller_Pressed += new UIScrollbox2.ScrollerPressed(Scroller_Pressed);
                // Player 1 - team
                Player1_Team = new UIScrollbox2(this, "Player1_Team", null, 390, 65, 120, 32);
                Player1_Team.Background = new Core.Texture(main, "%MAIN%\\UI\\ButtonSmall", 100, null);
                Player1_Team.TextOffset = new Vector2(8, 4);
                Player1_Team.AddItem("Ubertarians");
                Player1_Team.AddItem("Yutamos");
                Player1_Team.Scroller_Pressed += new UIScrollbox2.ScrollerPressed(Scroller_Pressed);
                // Player 2 - player
                Player2 = new UIScrollbox2(this, "Player2", new Core.Texture(main, "%MAIN%\\MenuPlayers\\Player2", 100, null), 30, 107, 350, 32);
                Player2.Items = Temp;
                Player2.Scroller_Pressed += new UIScrollbox2.ScrollerPressed(Scroller_Pressed);
                // Player 2 - team
                Player2_Team = new UIScrollbox2(this, "Player2_Team", null, 390, 107, 120, 32);
                Player2_Team.TextOffset = Player1_Team.TextOffset;
                Player2_Team.Items = Player1_Team.Items;
                Player2_Team.Background = Player1_Team.Background;
                Player2_Team.Scroller_Pressed += new UIScrollbox2.ScrollerPressed(Scroller_Pressed);
                // Player 3 - player
                Player3 = new UIScrollbox2(this, "Player3", new Core.Texture(main, "%MAIN%\\MenuPlayers\\Player3", 100, null), 30, 149, 350, 32);
                Player3.Items = Temp;
                Player3.Scroller_Pressed += new UIScrollbox2.ScrollerPressed(Scroller_Pressed);
                // Player 3 - team
                Player3_Team = new UIScrollbox2(this, "Player3_Team", null, 390, 149, 120, 32);
                Player3_Team.TextOffset = Player1_Team.TextOffset;
                Player3_Team.Items = Player1_Team.Items;
                Player3_Team.Background = Player1_Team.Background;
                Player3_Team.Scroller_Pressed += new UIScrollbox2.ScrollerPressed(Scroller_Pressed);
                // Player 4 - player
                Player4 = new UIScrollbox2(this, "Player4", new Core.Texture(main, "%MAIN%\\MenuPlayers\\Player4", 100, null), 30, 191, 350, 32);
                Player4.Items = Temp;
                Player4.Scroller_Pressed += new UIScrollbox2.ScrollerPressed(Scroller_Pressed);
                // Player 4 - team
                Player4_Team = new UIScrollbox2(this, "Player4_Team", null, 390, 191, 120, 32);
                Player4_Team.TextOffset = Player1_Team.TextOffset;
                Player4_Team.Items = Player1_Team.Items;
                Player4_Team.Background = Player1_Team.Background;
                Player4_Team.Scroller_Pressed += new UIScrollbox2.ScrollerPressed(Scroller_Pressed);
                // Set player1 to the current user
                Player1.SetSelected(Path.GetFileName(main.Profile));
                // Add buttons
                Controls.Add(Player1);
                Controls.Add(Player1_Team);
                Controls.Add(Player2);
                Controls.Add(Player2_Team);
                Controls.Add(Player3);
                Controls.Add(Player3_Team);
                Controls.Add(Player4);
                Controls.Add(Player4_Team);
                // Add handlers for selected index change
                Focus_Changed += new FocusChange(MenuPlayers_Focus_Changed);
            }
        #endregion

        #region "Functions - button/scroller2 pressed"
                void Scroller_Pressed(UIScrollbox2 sender)
                {
                    // Check the same player hasnt been selected multiple times
                    // Easiest and quickest method is to add them to an array and filter
                    List<string> Plys = new List<string>();
                    Plys.Add(Player1.GetSelected());
                    Plys.Add(Player2.GetSelected());
                    Plys.Add(Player3.GetSelected());
                    Plys.Add(Player4.GetSelected());
                    // Remove all instances of none
                    Plys.RemoveAll(NoneChecker);
                    // Check the same name hasnt been selected twice
                    List<string> Temp = new List<string>();
                    foreach (string str in Plys)
                    {
                        if (Temp.Contains(str))
                        {
                            Main.WindowManager.AddWindow(new UIMessageBox(Main, "One or more players have been selected" + Environment.NewLine + "multiple times!"));
                            return;
                        }
                        else
                        {
                            Temp.Add(str);
                        }
                    }
                    // Create gamemode
                    Main.CreateGame(File.ReadAllText(Main.Root + "\\Content\\Gamemodes\\" + temp_gamemode + "\\info.txt"), temp_gamemode, Main.Root + "\\Content\\Gamemodes\\" + temp_gamemode + "\\Maps\\" + temp_mappath + "\\Raw.map");
                    // Add players
                    PlayerIndex total = PlayerIndex.One;
                    if(Plys.Count == 2)
                    {
                        total = PlayerIndex.Two;
                    }
                    else if(Plys.Count == 3)
                    {
                        total = PlayerIndex.Three;
                    }
                    else if (Plys.Count == 4)
                    {
                        total = PlayerIndex.Four;
                    }
                    int count = 0;
                    foreach (string str in Plys)
                    {
                        count +=1;
                        PlayerIndex pi = PlayerIndex.One;
                        if(count == 2)
                        {
                            pi = PlayerIndex.Two;
                        }
                        else if (count == 3)
                        {
                            pi = PlayerIndex.Three;
                        }
                        else if (count == 4)
                        {
                            pi = PlayerIndex.Four;
                        }
                        // Add to gamemode
                        Main.Gamemode._Players.Add(new Player(Main.Gamemode, Main.Root + "\\Content\\Settings\\Profiles\\" + str, pi, total, TeamOfPlayer(str)));
                    }
                    // Call function to load players (to define them)
                    Main.Gamemode.LoadPlayers();
                }
                // Returns the team a player is on.
                bool TeamOfPlayer(string name)
                {
                    if (Player1.Items[Player1.SelectedIndex] == name)
                    {
                        if (Player1_Team.SelectedIndex == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (Player2.Items[Player2.SelectedIndex] == name)
                    {
                        if (Player2_Team.SelectedIndex == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (Player3.Items[Player3.SelectedIndex] == name)
                    {
                        if (Player3_Team.SelectedIndex == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (Player4.Items[Player4.SelectedIndex] == name)
                    {
                        if (Player4_Team.SelectedIndex == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                bool NoneChecker(string s)
                {
                    if (s == "None")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                void MenuPlayers_Focus_Changed()
                {
                    Main.PlayChurp();
                }
            #endregion
    }    
}