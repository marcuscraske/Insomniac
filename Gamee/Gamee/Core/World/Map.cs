using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class Map
    {
        // Purpose: to handle loading the map, to handle drawing the map tiles and map information etc.
        #region "Variables"
            public Gamemode Gamemode;
            // The root filesystem path of the map currently being played.
            public string Root = "";
            // The tiles of the map.
            public List<Tile> Tiles;
            // The textures of the map (stored in an array and accessed via index for performance to handle e.g. 300x300 maps etc).
            public List<Texture> Textures;
            // Stores map information.
            public MapInfo Info = new MapInfo();
            // Responsible for map post-processing/applying HLDS
            public PostProcessing PostProcessing;
        #endregion

        #region "Core"
            public Map(Gamemode _gamemode)
            {
                // Set the gamemode
                Gamemode = _gamemode;
                // Set the post processing
                PostProcessing = new PostProcessing(_gamemode._Main);
                // Set the textures array
                Textures = new List<Texture>();
                // Adds the error texture to index zero
                Textures.Add(_gamemode._Main.ErrorTexture);
            }
        #endregion

        #region "Loading maps"
            /// <summary>
            /// Loads a map.
            /// </summary>
            /// <param name="data"></param>
            public void LoadMap(string data)
            {
                // Reset post-processing
                PostProcessing.DestroyEffect();
                // Remove new lines
                data = data.Replace("\r", "").Replace("\n", "");
                // New info class
                Info = new MapInfo();
                // Used for temp storing data
                string _t;
                string _t2;
                string[] _t3;
                // Load info
                foreach (Match m in Regex.Matches(data, "INFO\\{(.+)\\}INFO"))
                {
                    _t = m.Groups[1].Value;
                    foreach(string str in _t.Split(';'))
                    {
                        if (str.Length > 3 && str.Contains("="))
                        {
                            if(str.StartsWith("MapWidth"))
                            {
                                Info.TileWidth = Convert.ToInt32(str.Split('=')[1]);
                            }
                            else if(str.StartsWith("MapHeight"))
                            {
                                Info.TileHeight = Convert.ToInt32(str.Split('=')[1]);
                            }
                            else if(str.StartsWith("TileSize"))
                            {
                                Info.TileSize = Convert.ToInt32(str.Split('=')[1]);
                            }
                            else if (str.StartsWith("PostProcessing"))
                            {
                                PostProcessing.LoadEffect(str.Split('=')[1]);
                            }
                            else if (str.StartsWith("AppliedVelocity"))
                            {
                                _t3 = str.Split('=')[1].Split(',');
                                Info.AppliedVelocity = new Vector2(float.Parse(_t3[0]), float.Parse(_t3[1]));
                            }
                            else if (str.StartsWith("ForceMultiplier"))
                            {
                                Info.ForceMultiplier = float.Parse(str.Split('=')[1]);
                            }
                            else
                            {
                                Info.Keys.Add(str.Split('=')[0], str.Split('=')[1]);
                            }
                        }
                        
                    }
                    break;
                }
                // Load textures array
                foreach (Match m in Regex.Matches(data, "TEXTURES\\{(.+)\\}TEXTURES"))
                {
                    _t = m.Groups[1].Value;
                    foreach(string str in _t.Split(';'))
                    {
                        // Minimum length: xxx=x.xxx (9)
                        if (str.Length > 8)
                        {
                            _t2 = str.Split('=')[1];
                            // Format map path
                            _t2 = _t2.Replace("%STARTUP%", Gamemode._Main.Root).Replace("%GAMEMODE%", Gamemode.Root).Replace("%MAP%", "null");
                            // Add texture
                            Textures.Add(new Core.Texture(Gamemode._Main, str.Split('=')[2], Convert.ToInt32(str.Split('=')[1]), Gamemode));
                        }
                    }
                }
                // Load tiles
                Tiles = new List<Tile>();
                foreach (Match m in Regex.Matches(data, "TILES\\{(.+)\\}TILES"))
                {
                    _t = m.Groups[1].Value;
                    int row = -1;
                    int column = -1;
                    foreach (string y in _t.Split(';'))
                    {
                        if (y.Length > 1)
                        {
                            row += 1;
                            column = -1;
                            foreach (string x in y.Split(','))
                            {
                                if (x.Length > 0)
                                {
                                    column += 1;
                                    // Add tile
                                    Tiles.Add(new Tile((column * Info.TileSize), (row * Info.TileSize), Convert.ToInt32(x)));
                                }
                            }
                        }
                    }
                    break;
                }
                // Reload actual tile measurements; acts like a cache for performance.
                Info.CalculateActualSize();
                // Load entities
                foreach (Match m in Regex.Matches(data, "ENTITIES\\{(.+)\\}ENTITIES"))
                {
                    _t = m.Groups[1].Value;
                    // Used to temp create, define and add entities
                    Entity ent;
                    foreach(string str in _t.Split(';'))
                    {
                        // Check the entry is valid
                        if(str.Length > 10 && !str.StartsWith("#"))
                        {
                            // Split the entry by a comma
                            string[] st = str.Split(',');
                            // Add entity
                            Gamemode._Entities.Add(Common.GetEntityByClass(st[0], Gamemode, new Core.Texture(Gamemode._Main, st[1], Convert.ToInt32(st[2]), Gamemode), Convert.ToInt32(st[3]), Convert.ToInt32(st[4]), Convert.ToBoolean(st[5])));
                            // Set temp ent variable
                            ent = Gamemode._Entities[Gamemode._Entities.Count - 1];
                            // Set custom variable values as defined by the entry
                            int count = 0;
                            string[] temp;
                            foreach (string s in st)
                            {
                                if (count > 5)
                                {
                                    temp = s.Split('=');
                                    if (temp.Length > 1)
                                    {
                                        Common.SetObjectVariable(ent, temp[0], Common.StringToObject(Gamemode, temp[1]));
                                    }
                                }
                                else
                                {
                                    count += 1;
                                }
                            }
                        }
                    }
                }
                // Load spawns
                foreach (Match m in Regex.Matches(data, "SPAWNS\\{(.+)\\}SPAWNS"))
                {
                    _t = m.Groups[1].Value;
                    foreach (string str in _t.Split(';'))
                    {
                        if (str.Length > 6)
                        {
                            _t3 = str.Split(',');
                            if (_t3[0] == "0")
                            {
                                Gamemode._SpawnManager.FriendlySpawns.Add(new Spawn(int.Parse(_t3[1]), int.Parse(_t3[2]), int.Parse(_t3[3]), float.Parse(_t3[4])));
                            }
                            else
                            {
                                Gamemode._SpawnManager.EnemySpawns.Add(new Spawn(int.Parse(_t3[1]), int.Parse(_t3[2]), int.Parse(_t3[3]), float.Parse(_t3[4])));
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// Loads a map from file.
            /// </summary>
            /// <param name="path"></param>
            public void LoadMapFromFile(string path)
            {
                // Checks the map file exists
                if (File.Exists(path))
                {
                    // Sets root-path to the directory of where the map file resides
                    Root = Path.GetDirectoryName(path);
                    // Loads the map
                    LoadMap(File.ReadAllText(path));
                }
            }
        #endregion

        #region "Rendering"
            public void Draw(SpriteBatch sb)
            {
                // Draws the map
                foreach (Tile ti in Tiles)
                {
                    sb.Draw(Textures[ti.TEXTURE]._Texture, new Rectangle(ti.X, ti.Y, Info.TileSize, Info.TileSize), Color.White);
                }
            }
        #endregion

        #region "Logic"
            /// <summary>
            /// The maps logic is executed when this function is called.
            /// </summary>
            public void Logic()
            {
                // Update texture frames
                foreach (Core.Texture txt in Textures)
                {
                    txt.Logic();
                }
            }
        #endregion

        #region "Destroy"
            public void Destroy()
            {
                Info = null;
                Gamemode = null;
                PostProcessing = null;
                // Dispose each texture properly
                foreach (Texture txt in Textures)
                {
                    Core.Texture.Dispose2(txt);
                }
                Tiles.Clear();
                Tiles = null;
                Textures = null;
            }
        #endregion
    }
    public class MapInfo
    {
        // Purpose: handles map information such as tilesize and size etc.
        #region "Variables"
            /// <summary>
            /// Multiplies an entities speed by this amount every tick.
            /// </summary>
            public float ForceMultiplier = 1.0F;
            /// <summary>
            /// The velocity XY applied per tick.
            /// </summary>
            public Vector2 AppliedVelocity = Vector2.Zero;
            /// <summary>
            /// Size of the tiles (width and height are the same)
            /// </summary>
            public int TileSize = 0;
            /// <summary>
            /// The number of tiles in the X direction.
            /// </summary>
            public int TileWidth = 0;
            /// <summary>
            /// The number of tiles in the Y direction.
            /// </summary>
            public int TileHeight = 0;
            /// <summary>
            ///  Returns the actual width of the map in pixels.
            /// </summary>
            public int ActualTileWidth = 0;
            /// <summary>
            /// Returns the actual height of the map in pixels.
            /// </summary>
            public int ActualTileHeight = 0;
            /// <summary>
            /// Stores various information keys.
            /// </summary>
            public Hashtable Keys = new Hashtable();
        #endregion

        #region "Functions"
            /// <summary>
            /// Recalculates the actual tile width and height in pixels of the map.
            /// </summary>
            public void CalculateActualSize()
            {
                ActualTileHeight = TileHeight * TileSize;
                ActualTileWidth = TileWidth * TileSize;
            }
        #endregion
    }
    public struct Tile
    {
        // Purpose: represents a single tile with the location and texture index.
        public int X;
        public int Y;
        public int TEXTURE;
        public Tile(int _X, int _Y, int _TEXTURE)
        {
            X = _X;
            Y = _Y;
            TEXTURE = _TEXTURE;
        }
    }
}