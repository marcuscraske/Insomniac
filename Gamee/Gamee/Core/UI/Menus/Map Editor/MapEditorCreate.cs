using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class MapEditorCreate:UIWindow
    {
        // Purpose: creates a new map, which saves having to type out e.g. a 100x100 tile map. Then the creator can
        // edit their map in an application such as notepad etc.

        #region "Variables"
            // All of these variables are used for when the create button is pressed (its quicker
            // in terms of performance to retrieve each control via ID.
            UITextbox _MapName;
            UITextbox _MapWidth;
            UITextbox _MapHeight;
            UIScrollbox2 _MapTxtDD;
            UITextbox _MapTxtPath;
            UITextbox _MapTilesize;
            UITextbox _MapVelX;
            UITextbox _MapVelY;
            // Stores the gamemode in-which the map is being created.
            string Gamemode;
        #endregion
        public MapEditorCreate(Gamee.Main main, string gamemode):base(main, null, 0, 0, 1024, 768)
        {
            // Set the gamemode
            Gamemode = gamemode;
            // Set the background
            Background = new Texture(main, "%MAIN%\\MapEditorMenu", 100, null);
            // Create label
            UILabel temp = new UILabel(this, "Label1", "New Map", Color.DarkBlue, 20, 90, false);
            temp.Font = main.Content.Load<SpriteFont>("Header");
            NonSelectControls.Add(temp);
            // Map name label
            NonSelectControls.Add(new UILabel(this, "MapLabel", "Map name:", Color.DarkBlue, 20, 130, false));
            // Map name textbox
            _MapName = new UITextbox(this, "MapName", 220, 125);
            _MapName.Text = "New Map";
            Controls.Add(_MapName);
            // Width label
            NonSelectControls.Add(new UILabel(this, "WidthLabel", "Num tiles accross:", Color.DarkBlue, 20, 180, false));
            // Width textbox
            _MapWidth = new UITextbox(this, "MapWidth", 220, 175);
            _MapWidth.Text = "30";
            Controls.Add(_MapWidth);
            // Height label
            NonSelectControls.Add(new UILabel(this, "WidthLabel", "Num tiles down:", Color.DarkBlue, 20, 230, false));
            // Height textbox
            _MapHeight = new UITextbox(this, "MapHeight", 220, 225);
            _MapHeight.Text = "30";
            Controls.Add(_MapHeight);
            // Background texture label
            NonSelectControls.Add(new UILabel(this, "BgTexture", "Background texture:", Color.DarkBlue, 20, 280, false));
            // Background texture area dropdown
            _MapTxtDD = new UIScrollbox2(this, "MapTxtDD", new Texture(main, "%MAIN%\\UI\\Scrollbox", 100, null), 220, 275, 200, 30);
            _MapTxtDD.AddItem("%MAIN%");
            _MapTxtDD.AddItem("%GAMEMODE%");
            _MapTxtDD.AddItem("%MAP%");
            _MapTxtDD.Focused = Color.DarkBlue;
            _MapTxtDD.NonFocused = Color.DarkBlue;
            Controls.Add(_MapTxtDD);
            // Background texture textbox path
            _MapTxtPath = new UITextbox(this, "MapTxtPath", 440, 275);
            _MapTxtPath.Text = "Error";
            Controls.Add(_MapTxtPath);
            // Tile size label
            NonSelectControls.Add(new UILabel(this, "TileSize", "Tile size:", Color.DarkBlue, 20, 330, false));
            // Tile size textbox
            _MapTilesize = new UITextbox(this, "MapTileSize", 220, 325);
            _MapTilesize.Text = "64";
            Controls.Add(_MapTilesize);
            // Applied velocity label
            NonSelectControls.Add(new UILabel(this, "AppVel", "Applied Velocity:", Color.DarkBlue, 20, 380, false));
            // Applied velocity X textbox
            _MapVelX = new UITextbox(this, "VelX", 220, 375);
            _MapVelX.Area.Width = 40;
            _MapVelX.Text = "0";
            Controls.Add(_MapVelX);
            // Add comma label
            NonSelectControls.Add(new UILabel(this, "Comma", ",", Color.DarkBlue, 265, 375, false));
            // Applied velocity Y textbox
            _MapVelY = new UITextbox(this, "VelY", 280, 375);
            _MapVelY.Area.Width = 40;
            _MapVelY.Text = "0";
            Controls.Add(_MapVelY);
            // Cancel button
            UIButton Temp = new UIButton(this, "Cancel", "Cancel", 20, 718);
            Temp.Button_Pressed += new UIButton.ButtonPressed(Temp_Button_Pressed);
            Controls.Add(Temp);
            // Create button
            Temp = new UIButton(this, "Create", "Create", 904, 718);
            Temp.Button_Pressed+=new UIButton.ButtonPressed(Temp_Button_Pressed);
            Controls.Add(Temp);
        }
        void Temp_Button_Pressed(UIButton sender)
        {
            // A button has been pressed...
            if (sender.Text == "Cancel")
            {
                // Go back
                Main.WindowManager.AddWindow2(new MapEditorMaps(Main, Gamemode));
            }
            else if(sender.Text == "Create")
            {
                // Validate the input; if a field is invalid, an alert
                // is shown and the input control is selected
                if(_MapName.Text.Contains("\\"))
                {
                    Alert("Map name cotnains invalid chars: \\");
                    SetSelected(_MapName);
                    return;
                }
                else if(!IsNumeric(_MapWidth.Text))
                {
                    Alert("Map width is not a valid number!");
                    SetSelected(_MapWidth);
                    return;
                }
                else if (!IsNumeric(_MapHeight.Text))
                {
                    Alert("Map height is not a valid number!");
                    SetSelected(_MapHeight);
                    return;
                }
                else if (!IsNumeric(_MapTilesize.Text))
                {
                    Alert("Tilesize is not a valid number!");
                    SetSelected(_MapTilesize);
                    return;
                }
                else if (!IsNumeric(_MapVelX.Text))
                {
                    Alert("Velocity X is not a valid number!");
                    SetSelected(_MapVelX);
                    return;
                }
                else if(!IsNumeric(_MapVelY.Text))
                {
                    Alert("Velocity Y is not a valid number!");
                    SetSelected(_MapVelY);
                    return;
                }
                else if(Directory.Exists(Main.Root + "\\Content\\Gamemodes\\" + Gamemode + "\\Maps\\" + _MapName.Text))
                {
                    Alert("Map path is invalid!");
                    SetSelected(_MapName);
                    return;
                }
                // Create map
                // Variable will store the new maps text, which will be written to a file
                string _newmap = "";
                // Stores the newline character(s)
                string nl = Environment.NewLine;
                // Create info section
                _newmap += "INFO{" + nl;
                _newmap += "TileSize=" + _MapTilesize.Text + ";" + nl;
                _newmap += "AppliedVelocity=" + _MapVelX.Text + "," + _MapVelY.Text + ";" + nl;
                _newmap += "MapWidth=" + _MapWidth.Text + ";" + nl;
                _newmap += "MapHeight=" + _MapHeight.Text + ";" + nl;
                _newmap += "}INFO" + nl;
                // Create textures section
                _newmap += "TEXTURES{" + nl;
                _newmap += "001=100=" + _MapTxtDD.Items[_MapTxtDD.SelectedIndex] + "\\" + _MapTxtPath.Text + ";" + nl;
                _newmap += "}TEXTURES" + nl;
                // Create tiles section
                _newmap += "TILES{";
                for (int y = 0; y <= Convert.ToInt32(_MapHeight.Text) - 1; y++ )
                {
                    _newmap += nl;
                    for (int x = 0; x <= Convert.ToInt32(_MapWidth.Text) - 1; x++)
                    {
                        if (x >= Convert.ToInt32(_MapWidth.Text) - 1)
                        {
                            _newmap += "001;";
                        }
                        else
                        {
                            _newmap += "001,";
                        }
                    }
                }
                _newmap += nl + "}TILES" + nl;
                // Create entities section
                _newmap += "ENTITIES{" + nl;
                // Example entity
                _newmap += "Core.Entity,%ERROR%,800,100,100,true,Position=vector:400#400,Velocity=vector:0#20;" + nl;
                _newmap += "}ENTITIES" + nl;
                _newmap += "SPAWNS{" + nl;
                _newmap += "0,100,100,180,8.0;" + nl;
                _newmap += "1,100,400,0,8.0;" + nl;
                _newmap += "}SPAWNS" + nl;
                // Create map directories
                string map_root = Main.Root + "\\Content\\Gamemodes\\" + Gamemode + "\\Maps\\" + _MapName.Text;
                // Creates root directory
                Directory.CreateDirectory(map_root);
                // Creates textures and thumbnail directory
                Directory.CreateDirectory(map_root + "\\Textures\\Thumbnail");
                // Copy new-map image to thumbnail directory
                File.Copy(Main.Root + "\\Content\\Settings\\Misc\\newmap.png", map_root + "\\Textures\\Thumbnail\\newmap.png");
                // Write map to file
                File.WriteAllText(map_root + "\\Raw.map", _newmap);
                // Go back to the maps menu
                Main.WindowManager.AddWindow2(new MapEditorMaps(Main, Gamemode));
            }
        }
        /// <summary>
        /// Checks if a string is numeric (a number); exactly the same as IsNumeric from VisualBasic.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        bool IsNumeric(string str)
        {
            if (str == null || str == "")
            {
                return false;
            }
            foreach (char c in str)
            {
                if (!char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Creates an alert message-box, which is failed for failed validation to indicate what field failed.
        /// </summary>
        /// <param name="info"></param>
        public void Alert(string info)
        {
            Main.WindowManager.AddWindow(new UIMessageBox(Main, info));
        }
    }
}