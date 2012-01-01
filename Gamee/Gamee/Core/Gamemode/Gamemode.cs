using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Core
{
    public class Gamemode:Window
    {
        // Purpose: defines the game(mode) window and handles an active game.

        #region "Events"
            #region "New Level"
                /// <summary>
                /// Raised when a new level has been started.
                /// </summary>
                public delegate void NewLevel();
                public event NewLevel New_Level;
            #endregion

            #region "Game Over"
                /// <summary>
                /// Raised when the game has ended.
                /// </summary>
                public delegate void GameOver();
                public event GameOver Game_Over;
                /// <summary>
                /// Raises the event responsible for calling game over.
                /// </summary>
                public void _Event_Game_Over()
                {
                    if (Game_Over != null)
                    {
                        Game_Over();
                    }
                }
            #endregion

            #region "Player Died"
                /// <summary>
                /// Raised when a player has died.
                /// </summary>
                /// <param name="ply"></param>
                public delegate void PlayerDied(Player ply);
                public event PlayerDied Player_Died;
                /// <summary>
                /// Raises the event responsible for when a player dies permanently.
                /// </summary>
                /// <param name="ply"></param>
                public void _Event_Player_Died(Player ply)
                {
                    if (Player_Died != null)
                    {
                        Player_Died(ply);
                    }
                }
            #endregion

            #region "Entity Killed"
                /// <summary>
                /// Raised when an entity has died.
                /// </summary>
                /// <param name="victim"></param>
                /// <param name="killer"></param>
                public delegate void EntKilled(Entity victim, Entity killer);
                public event EntKilled ENT_Killed;
                /// <summary>
                /// Raises the event thats responsible for raising events about an entity murder.
                /// </summary>
                /// <param name="victim"></param>
                /// <param name="killer"></param>
                public void _Event_ENT_Killed(Entity victim, Entity killer)
                {
                    if (ENT_Killed != null)
                    {
                        ENT_Killed(victim, killer);
                    }
                }
            #endregion
        #endregion

        #region "Variables"
            // The main class, which contents the spritebatch etc.
            public Gamee.Main _Main;
            // Indicates if the game is paused or not (causing the logic of the game to be aborted).
            public bool Paused = true;
            // File path root of the gamemode folder.
            public string Root = "";
            /// Responsible for handling the scores of players.
            public ScoreManager _ScoreManager;
            // Contains an array of players in this game.
            public List<Player> _Players = new List<Player>();
            // Responsible for respawning entities.
            public SpawnManager _SpawnManager;
            // Responsible for the map (loading the map, drawing the tiles etc).
            public Map _Map;
            // Responsible for the games physics.
            public Physics _Physics;
            // An array which contains the active entities in the game.
            public List<Entity> _Entities = new List<Entity>();
            // An array of entities to be added (to avoid system.collection modified).
            public List<Entity> _AddEnts = new List<Entity>();
            // An array of entities to be removed from the game (to avoid system.collection modified).
            public List<Entity> _RemoveEnts = new List<Entity>();
            // The random generator class responsible for generating random numbers used by (primarily) external classes.
            private Random r = new Random(DateTime.Now.Second);
            // An array of commonly used and shared texturs (for performance).
            public TextureArray _TexturesArray;
            // An array of effects (like entities but they're only drawn - good for performance purposes).
            public List<Effect> Effects = new List<Effect>();
            // These three objects are responsible for audio using XACT (XNA audio thats cross-platform compatible).
            public AudioEngine Audio_Engine;
            public WaveBank Audio_Wave;
            public SoundBank Audio_Control;
        #endregion

        #region "Core"
            public Gamemode(Gamee.Main main, string RootName)
            {
                // Set file root
                Root = main.Root + "\\Content\\Gamemodes\\" + RootName;
                // Load audio
                Audio_Engine = new AudioEngine(main.Root + "\\Content/Sound/Insomniac.xgs");
                Audio_Wave = new WaveBank(Audio_Engine, main.Root + "\\Content/Sound/Insomniac.xwb");
                Audio_Control = new SoundBank(Audio_Engine, main.Root + "\\Content/Sound/Insomniac.xsb");
                // Create and load objects
                _Main = main;
                _TexturesArray = new TextureArray(main);
                _SpawnManager = new SpawnManager(this);
                _ScoreManager = new ScoreManager(this);
                _Map = new Map(this);
                _Physics = new Physics(this);
                // Reset player cameras
                foreach (Player ply in _Players)
                {
                    ply.Camera.ResetCamera();
                }
                // Add handler to capture player deaths to check if the game is over
                Player_Died += new PlayerDied(Gamemode_Player_Died);
                Game_Over += new GameOver(Gamemode_Game_Over);
            }
            public virtual void LoadPlayers()
            {
                // Called when its suitable to reload the players; this is a virtual
                // function intended for inheriting gamemodes.
            }
            public virtual void Gamemode_Game_Over()
            {
                // Save the scores since the game has ended
                _ScoreManager.SaveScores();
            }
            public virtual void Gamemode_Player_Died(Player ply)
            {
                // If this boolean doesn't turn false from a player being alive,
                // the game is terminated
                bool gameover = true;
                foreach (Player t in _Players)
                {
                    if (!t.Dead)
                    {
                        // A player is still alive, do not abort game
                        gameover = false;
                    }
                }
                if (gameover)
                {
                    // Aborts the game (since no alive player was found)
                    // Pauses the game to disallow further logic
                    Paused = true;
                    // Show scoreboard
                    _Main.WindowManager.AddWindow(new Scoreboard(this, false));
                    // Raises the game-over event which triggers e.g. scores to be saved
                    if (Game_Over != null)
                    {
                        Game_Over();
                    }
                }
            }
            /// <summary>
            /// Respawns an entity at a random spawn using the spawn-manager class.
            /// </summary>
            /// <param name="ent"></param>
            public virtual void RespawnEnt(Entity ent)
            {
                _SpawnManager.RespawnEnt(ent);
            }
            public override void Draw(SpriteBatch sb)
            {
                // Check a map has been created and loaded, else abort drawing
                if (_Map == null || _Map.Tiles == null)
                {
                    return;
                }
                // Draw each viewport (which is basically an area on the screen)
                foreach (Player ply in _Players)
                {
                    // Set the viewport being drawn
                    _Main.graphics.GraphicsDevice.Viewport = ply.Camera.Viewport;
                    // Prepare the spritebatch for drawing
                    sb.Begin(
                    SpriteBlendMode.AlphaBlend,
                    SpriteSortMode.Immediate,
                    SaveStateMode.None,
                    ply.Camera.CameraTransformation * Resolution.Matrix);
                    _Main.graphics.GraphicsDevice.Clear(Color.Black);
                    // Start post-processing
                    _Map.PostProcessing.Start();
                    ply.PostProcessing.Start();
                    // Draw map
                    _Map.Draw(sb);
                    // Draw entities
                    foreach (Entity ent in _Entities)
                    {
                        ent.Draw(sb);
                    }
                    // Draw effects - (they will always be on-top of entities then
                    foreach (Effect effect in Effects)
                    {
                        effect.Draw(sb);
                    }
                    // Stop post processing
                    _Map.PostProcessing.Stop();
                    ply.PostProcessing.Stop();
                    // Draw override function
                    DrawOverride(sb);
                    // End drawing
                    sb.End();
                }
                // Set viewport back to the default
                _Main.graphics.GraphicsDevice.Viewport = Resolution.DefaultViewport;
                // Draw override - secondary for UI
                sb.Begin();
                DrawOverride2(sb);
                sb.End();
            }
            /// <summary>
            /// This function can be overrided for drawing custom gamemode things at in-game co-ords.
            /// </summary>
            public virtual void DrawOverride(SpriteBatch sb)
            {
                // A virtual function intended to be overriden by an inheriting gamemode.
            }
            /// <summary>
            /// Draws using screen co-ords.
            /// </summary>
            /// <param name="sb"></param>
            public virtual void DrawOverride2(SpriteBatch sb)
            {
                // A virtual function intended to be overriden by an inheriting gamemode.
            }
            public override void Logic(GameTime gt)
            {
                // Clear-up audio (to clear memory etc)
                Audio_Engine.Update();
                // Pause logic
                if (Input.IsKeyDown(Input.GAME_KEY_PAUSE) || Input.IsButtonDown(Input.GAME_BUTTON_PAUSE))
                {
                    if (HasInput)
                    {
                        Paused = true;
                        _Main.WindowManager.AddWindow(new MenuPause(_Main));
                    }
                }
                // Check if the player is requesting the scoreboard (player one only since they're the host)
                if (!Paused && Input.IsKeyDown(Input.GAME_KEY_SCOREBOARD) || Input.IsButtonDown(Input.GAME_BUTTON_SCOREBOARD))
                {
                    Paused = true;
                    _Main.WindowManager.AddWindow(new Scoreboard(this, true));
                }
                // Execute overriden logic first (from an inherited gamemode)
                LogicOverride(gt);
                // Checks if the game is paused, else the logic within this if statement is not executed
                if (!Paused && _Map != null && _Players.Count != 0)
                {
                    // Add new entities safely to the game
                    AddAdditionalEntities();
                    // Execute each entities logic
                    foreach (Entity ent in _Entities)
                    {
                        if (ent.Alive)
                        {
                            ent.Logic();
                        }
                    }
                    // Effects
                    // Get dead effects
                    List<Effect> DeadEffects = new List<Effect>();
                    foreach (Effect effect in Effects)
                    {
                        if (effect.Finished)
                        {
                            DeadEffects.Add(effect);
                        }
                        else
                        {
                            effect.Logic();
                        }
                    }
                    // Remove dead effects
                    foreach (Effect effect in DeadEffects)
                    {
                        Effects.Remove(effect);
                    }
                    // Clear and null the dead effects array
                    DeadEffects.Clear();
                    DeadEffects = null;
                    // Texture logic
                    _TexturesArray.Logic();
                    // Respawn dead entities
                    _SpawnManager.Logic();
                    // Map logic
                    _Map.Logic();
                    // Score logic
                    _ScoreManager.Logic();
                    // Clear destroyed entities
                    ClearDestroyed();
                }
            }
            public virtual void LogicOverride(GameTime gameTime)
            {
                // A virtual function intended to be overriden by an inheriting gamemode.
            }
            /// <summary>
            /// Destroys the gamemode safely, releasing all the resources used (to clear memory).
            /// </summary>
            public override void Destroying()
            {
                foreach (Entity ent in _Entities)
                {
                    ent.Dispose();
                }
                _Entities.Clear();
                _Entities = null;
                foreach (Player ply in _Players)
                {
                    ply.Destroy();
                }
                _Players = null;
                _Map.Destroy();
                _Map = null;
                _Physics = null;
                _RemoveEnts = null;
                _SpawnManager = null;
                _TexturesArray.DestroyArray();
                _TexturesArray = null;
                _Main.Gamemode = null;
                _Main = null;
                Audio_Engine.Dispose();
                Audio_Engine = null;
                Audio_Wave.Dispose();
                Audio_Wave = null;
                Audio_Control.Dispose();
                Audio_Control = null;
            }
        #endregion

        #region "Functions - Misc (random num, load level, restart level)"
                /// <summary>
                /// Generates a random number from 0 to the specified number.
                /// </summary>
                /// <param name="max"></param>
                public int RNumber(int max)
                {
                    if (max <= 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return r.Next(0, max);
                    }
                }
                /// <summary>
                /// Generates a random number between the values specified.
                /// </summary>
                /// <param name="min"></param>
                /// <param name="max"></param>
                /// <returns></returns>
                public int RNumber2(int min, int max)
                {
                    return r.Next(min, max);
                }
                /// <summary>
                /// Loads a new map safely.
                /// </summary>
                /// <param name="path"></param>
                public void LoadLevel(string path)
                {
                    // Pauses the game to stop any logic executing other than this function
                    Paused = true;
                    // Close the scoreboard if its open
                    foreach(Window win in _Main.WindowManager.Windows)
                    {
                        if (win is Scoreboard)
                        {
                            _Main.WindowManager.Remove.Add(win);
                        }
                    }
                    // Reset score
                    _ScoreManager.Reset();
                    // Reload spawn manager
                    _SpawnManager = new SpawnManager(this);
                    // Reload physics
                    _Physics = new Physics(this);
                    // Dispose ents
                    foreach (Entity ent in _Entities)
                    {
                        if(ent.Player == null)
                        {
                            ent.Dispose();
                        }
                        else if (ent.Player.Entity != ent)
                        {
                            ent.Dispose();
                        }
                    }
                    // Clear ents
                    _Entities.Clear();
                    // Load map
                    _Map = new Map(this);
                    _Map.LoadMapFromFile(path);
                    // Load score data
                    if (_Map.Info.Keys.Contains("ScoreMultiplier"))
                    {
                        _ScoreManager.Multiplier = float.Parse(_Map.Info.Keys["ScoreMultiplier"].ToString());
                    }
                    if (_Map.Info.Keys.Contains("ScorePerSecond"))
                    {
                        _ScoreManager.ScorePerSecond = float.Parse(_Map.Info.Keys["ScorePerSecond"].ToString());
                    }
                    // Load and spawn players
                    foreach (Player ply in _Players)
                    {
                        // Readds the entity and resets it
                        ply.AddEntityToGamemode();
                    }
                    // Raise ent loading event since the gamemode has loaded
                    foreach (Entity ent in _Entities)
                    {
                        ent.Loaded();
                    }
                    // Raise the new level event
                    if (New_Level != null)
                    {
                        New_Level();
                    }
                    Paused = false;
                    // Check if theres a brief to display
                    if (_Map.Info.Keys.ContainsKey("Brief"))
                    {
                        // Display the brief
                        _Main.WindowManager.AddWindow(new Brief(this, new Core.Texture(_Main, _Map.Info.Keys["Brief"].ToString(), 100, this)));
                    }
                    else
                    {
                        // Unpauses the game to allow other game logic to be executed
                        Paused = false;
                    }
                }
                /// <summary>
                /// Restarts a level safely; protected against null maps too.
                /// </summary>
                public void RestartLevel()
                {
                    // Check the map isnt null
                    if (_Map != null)
                    {
                        // Check a map has been previously loaded
                        if (_Map.Root != "")
                        {
                            // Reloads the previously loaded map
                            LoadLevel(_Map.Root + "\\Raw.map");
                        }
                    }
                }
        #endregion

        #region "Functions - Gamemode state"
            /// <summary>
            /// Toggles pausing the game - for inheriting gamemodes.
            /// </summary>
            public void TogglePause()
            {
                if (Paused)
                {
                    Paused = false;
                }
                else
                {
                    Paused = true;
                }
            }
        #endregion

        #region "Functions - Entity Addition & Removal"
            /// <summary>
            /// Safely adds an entity to the gamemode.
            /// </summary>
            /// <param name="ent"></param>
            public void AddEntity(Entity ent)
            {
                // Add the entity to the array which will be added to the main entity array
                _AddEnts.Add(ent);
            }
            /// <summary>
            /// Adds all the entities waiting to be added to the gamemode (safely
            /// to avoid system.collection modification error).
            /// </summary>
            public void AddAdditionalEntities()
            {
                _Entities.AddRange(_AddEnts);
                _AddEnts.Clear();
            }
            /// <summary>
            /// Removes an entity from the game.
            /// </summary>
            /// <param name="ent"></param>
            public void DestroyEntity(Entity ent)
            {
                // Adds an entity to the remove entity array to be removed safely to
                // avoid system.collection modified error
                _RemoveEnts.Add(ent);
            }
            /// <summary>
            /// [!INTERNAL!] Clears all the destroyed entities from the game. This shouldn't be called
            /// since its already internally used and can crash the game if mis-used!
            /// </summary>
            public void ClearDestroyed()
            {
                foreach (Entity ent in _RemoveEnts)
                {
                    ent.Dispose();
                    _Entities.Remove(ent);
                }
                _RemoveEnts.Clear();
            }
            #endregion

        #region "Effects"
            /// <summary>
            /// Uses the following textures from the texturearray:
            /// - Explosion_Main
            /// - Explosion_Flare
            /// 
            /// These textures have to be added by inheriting gamemodes or they'll be errors.
            /// </summary>
            /// <param name="position"></param>
            public void CreateGenericExplosion(Vector2 position)
            {
                Core.Effect.CreateExplosion(this, position, 1.0F, 0.5F, 2.0F, 6, _TexturesArray.GetTexture("Explosion_Main"), _TexturesArray.GetTexture("Explosion_Flare"), "Explosion");
            }
        #endregion
    }
}