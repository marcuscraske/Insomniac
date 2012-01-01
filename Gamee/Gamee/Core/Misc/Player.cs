using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core
{
    public class Player
    {
        // Purpose: represents a player, allowing: entity handling, player indexing and functions and classes to e.g.
        // handle score etc.

        #region "Variables"
            /// <summary>
            /// The game-mode.
            /// </summary>
            public Gamemode Gamemode;
            /// <summary>
            /// The number of the player is stored here (1-4).
            /// </summary>
            public PlayerIndex Index;
            /// <summary>
            /// Used to store the total players for viewport rebuild.
            /// </summary>
            public PlayerIndex TotalPlayers;
            /// <summary>
            /// Stores the players ship.
            /// </summary>
            public Entity Entity;
            /// <summary>
            /// Stores the players name.
            /// </summary>
            public string Alias;
            /// <summary>
            /// Used to temp store a score for a game.
            /// </summary>
            public Score Score = new Score();
            /// <summary>
            /// The players camera.
            /// </summary>
            public Camera Camera;
            /// <summary>
            /// The root-path of the players configuration directory.
            /// </summary>
            public string Root;
            // Indicates if the player is alive or dead.
            public bool Dead = false;
            // The post-processing to be applied to the player.
            public PostProcessing PostProcessing;
            // A table that stores data for the player during a game such as e.g. modules in Insomniac.
            public Hashtable Data = new Hashtable();
            #region "PlayerEntityLogic Variables"
                // Uses this tick to calculate life-span
                float LastTick = 0.0F;
                // Stores the lifespan of a player for statistics
                float LifeSpan = 0.0F;
            #endregion
            // If the player is on the enemy team or not.
            public bool Enemy = false;
        #endregion

        #region "Core"
            public Player(Gamemode gm, string profile_path, PlayerIndex index, PlayerIndex totalplayers, bool enemy)
            {
                // Sets the game the player is in
                Gamemode = gm;
                // Sets the players name
                Alias = Path.GetFileName(profile_path);
                // Sets the root file-path of the players profile
                Root = profile_path;
                Index = index;
                TotalPlayers = totalplayers;
                Enemy = enemy;
                // Creates the players view (camera,area)
                Camera = new Camera(gm);
                SetupViewport(this, index, totalplayers);
                // Create entity
                Entity = new Core.Entity(gm, gm._Main.ErrorTexture, 100, 100, false);
                Entity.Player = this;
                Entity.Enemy = Enemy;
                // For spawn-count logging
                Entity.ENT_Respawned += new Entity.Ev_Respawned(Entity_ENT_Respawned);
                Entity.ENT_Killed += new Entity.Ev_Killed(Entity_ENT_Killed);
                // Add entity to gamemode
                AddEntityToGamemode();
                // Define post-processing effect class/object
                PostProcessing = new PostProcessing(gm._Main);
                // Load post-processing from config
                ConfigFile Config = new ConfigFile();
                Config.LoadFromFile(Root + "\\Config.icf");
                PostProcessing.LoadEffect(Config.GetKey("Resolution", "PostProcessing"));
            }
            /// <summary>
            /// Sets the players entity.
            /// </summary>
            /// <param name="ent"></param>
            public void SetEntity(Entity ent)
            {
                // Destroy existing player
                if (Entity != null)
                {
                    Entity.ENT_Respawned -= new Entity.Ev_Respawned(Entity_ENT_Respawned);
                    Entity.ENT_Killed -= new Entity.Ev_Killed(Entity_ENT_Killed);
                    Entity.Player = null;
                    Entity.Alive = false;
                    Gamemode._RemoveEnts.Add(Entity);
                }
                // Setup new player
                Entity = ent;
                Entity.Player = this;
                Entity.ENT_Respawned+=new Entity.Ev_Respawned(Entity_ENT_Respawned);
                Entity.ENT_Killed+=new Entity.Ev_Killed(Entity_ENT_Killed);
                // Readd player to gamemode (reset cam etc)
                AddEntityToGamemode();
            }
            void Entity_ENT_Killed(Entity ent)
            {
                // Resets the players life-span (for statistical reasons)
                LifeSpan = 0.0F;
            }
            void Entity_ENT_Respawned(Vector2 position)
            {
                // Adds to the spawn-count (for statistical reasons)
                Score.SPAWNCOUNT += 1.0F;
            }
            /// <summary>
            /// Executes logic for player-entities; default: lifespan calculation.
            /// </summary>
            public virtual void PlayerEntityLogic()
            {
                if (LastTick != 0.0F)
                {
                    LifeSpan += Environment.TickCount - LastTick;
                }
                if (LifeSpan > Score.LONGESTLIFESPAN)
                {
                    Score.LONGESTLIFESPAN = LifeSpan;
                }
                LastTick = Environment.TickCount;
            }
        #endregion

        #region "Functions"
            /// <summary>
            /// Adds the players entity to the gamemode, resetting player and entity.
            /// </summary>
            public void AddEntityToGamemode()
            {
                // Reset camera
                Camera.ResetCamera();
                // Reset score
                Score = new Score();
                // Reset death
                Dead = false;
                // Reset entity
                Entity.ResetEntity();
                // Set the ents team
                Entity.Enemy = Enemy;
                // Set the player as dead to respawn
                Entity.Alive = false;
                // Add entity
                Gamemode._Entities.Add(Entity);
                // Respawn the entity
                Gamemode.RespawnEnt(Entity);
                // Attach camera
                Camera.AttachToEntity(Entity);
            }
            /// <summary>
            /// Destroys inner-objects used by the player object to free memory and resources etc.
            /// </summary>
            public void Destroy()
            {
                Entity.Dispose();
                Entity = null;
                Camera = null;
                PostProcessing.Destroy();
                PostProcessing = null;
            }
            /// <summary>
            /// Sets up the viewport for a player; this is done by default, no need to call manually.
            /// </summary>
            /// <param name="ply"></param>
            /// <param name="pi"></param>
            /// <param name="totalplayers"></param>
            public static void SetupViewport(Player ply, PlayerIndex pi, PlayerIndex totalplayers)
            {
                if (totalplayers == PlayerIndex.One)
                {
                    ply.Camera.Viewport.X = 0;
                    ply.Camera.Viewport.Y = 0;
                    ply.Camera.Viewport.Width = (int)Resolution.GameResolution.X;
                    ply.Camera.Viewport.Height = (int)Resolution.GameResolution.Y;
                }
                else if (totalplayers == PlayerIndex.Two)
                {
                    if (pi == PlayerIndex.One)
                    {
                        ply.Camera.Viewport.X = 0;
                        ply.Camera.Viewport.Y = 0;
                        ply.Camera.Viewport.Width = (int)(Resolution.GameResolution.X);
                        ply.Camera.Viewport.Height = (int)(Resolution.GameResolution.Y / 2) - 1;
                    }
                    else
                    {
                        ply.Camera.Viewport.X = 0;
                        ply.Camera.Viewport.Y = (int)(Resolution.GameResolution.Y / 2) + 1;
                        ply.Camera.Viewport.Width = (int)(Resolution.GameResolution.X);
                        ply.Camera.Viewport.Height = (int)(Resolution.GameResolution.Y / 2) - 1;
                    }
                }
                else
                {
                    if (pi == PlayerIndex.One)
                    {
                        ply.Camera.Viewport.X = 0;
                        ply.Camera.Viewport.Y = 0;
                        ply.Camera.Viewport.Width = (int)(Resolution.GameResolution.X / 2) - 1;
                        ply.Camera.Viewport.Height = (int)(Resolution.GameResolution.Y / 2) - 1;
                    }
                    else if (pi == PlayerIndex.Two)
                    {
                        ply.Camera.Viewport.X = (int)(Resolution.GameResolution.X / 2) + 1;
                        ply.Camera.Viewport.Y = 0;
                        ply.Camera.Viewport.Width = (int)(Resolution.GameResolution.X / 2) - 1;
                        ply.Camera.Viewport.Height = (int)(Resolution.GameResolution.Y / 2) - 1;
                    }
                    else if (pi == PlayerIndex.Three)
                    {
                        ply.Camera.Viewport.X = 0;
                        ply.Camera.Viewport.Y = (int)(Resolution.GameResolution.Y / 2) + 1;
                        ply.Camera.Viewport.Width = (int)(Resolution.GameResolution.X / 2) - 1;
                        ply.Camera.Viewport.Height = (int)(Resolution.GameResolution.Y / 2) - 1;
                    }
                    else if (pi == PlayerIndex.Four)
                    {
                        ply.Camera.Viewport.X = (int)(Resolution.GameResolution.X / 2) + 1;
                        ply.Camera.Viewport.Y = (int)(Resolution.GameResolution.Y / 2) + 1;
                        ply.Camera.Viewport.Width = (int)(Resolution.GameResolution.X / 2) - 1;
                        ply.Camera.Viewport.Height = (int)(Resolution.GameResolution.Y / 2) - 1;
                    }
                }
            }
        #endregion

        #region "Score-related"
            /// <summary>
            /// Saves the score of the player to their profile.
            /// </summary>
            public void SaveScore()
            {
                // Load score config file
                string Gamemode_name = Gamemode.GetType().Namespace + "." + Gamemode.GetType().Name;
                ConfigFile Temp = new ConfigFile();
                Temp.LoadFromFile(Root + "\\Stats.icf");
                // Add kills
                foreach(DictionaryEntry di in Score.TOTAL_KILLS)
                {
                    Temp.AddToKey(Gamemode_name, "KILLS_" + di.Key, Convert.ToInt32(di.Value.ToString()));
                }
                // Add deaths
                foreach(DictionaryEntry di in Score.TOTAL_DEATHS)
                {
                    Temp.AddToKey(Gamemode_name, "DEATHS_" + di.Key, Convert.ToInt32(di.Value.ToString()));
                }
                // Add spawncount
                Temp.AddToKey(Gamemode_name, "NumSpawns", Convert.ToInt32(Score.SPAWNCOUNT));
                // Longest Lifespan
                string temp2 = Temp.GetKey(Gamemode_name, "LongestTime");
                if (temp2 != "")
                {
                    if (Score.LONGESTLIFESPAN > float.Parse(temp2))
                    {
                        Temp.AddKey(Gamemode_name, "LongestTime", Score.LONGESTLIFESPAN.ToString());
                    }
                }
                else
                {
                    Temp.AddKey(Gamemode_name, "LongestTime", Score.LONGESTLIFESPAN.ToString());
                }
                // Save stats
                Temp.SaveToFile(Root + "\\Stats.icf");
                // Save XP
                Temp = new ConfigFile();
                Temp.LoadFromFile(Gamemode._Main.Root + "\\Content\\Settings\\Profiles\\" + Alias + "\\Insomniac.icf");
                Temp.AddToKey("Player", "XP", Convert.ToInt32(Score.SCORE));
                Temp.SaveToFile(Gamemode._Main.Root + "\\Content\\Settings\\Profiles\\" + Alias + "\\Insomniac.icf");
                // Nullify to free memory
                Temp = null;
            }
        #endregion
    }
}