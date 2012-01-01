using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Gamee
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        #region "Variables"
            public Core.Sound Churp;
            public Core.Sound ChurpSelect;
            public string Profile = "";
            public string Root = StorageContainer.TitleLocation;
            public GraphicsDeviceManager graphics;
            public SpriteBatch spriteBatch;
            public Core.WindowManager WindowManager = new Core.WindowManager();
            public Core.Gamemode Gamemode;
            public Core.Texture ErrorTexture;
        #endregion

        #region "Core"
            public Main()
            {
                // Game window title
                Window.Title = "Insomniac - v1.1 - by Ratboy - UberMeat.co.uk";
                // Create the graphics device
                graphics = new GraphicsDeviceManager(this);
                // Reapply graphic settings
                Core.Resolution.ApplySettings(graphics);
                Core.Resolution.ReadConfig(graphics, Root + "\\Content\\Settings\\Default Profile\\Config.icf");
                // Set default profile as the current profile
                Profile = Root + "\\Content\\Settings\\Default Profile";
                // Sets the content root of the project
                Content.RootDirectory = "Content";
            }
            protected override void Initialize()
            {
                base.Initialize();
            }
            protected override void LoadContent()
            {
                // Load errror texture
                ErrorTexture = new Core.Texture(this, "%ERROR%", 100, null);
                ErrorTexture.Disposable = false;
                // Sound emitters
                Churp = new Core.Sound(this, "%MAIN%\\MenuChange.mp3", false, false, null);
                ChurpSelect = new Core.Sound(this, "%MAIN%\\MenuSelect.mp3", false, false, null);
                // Other things
                spriteBatch = new SpriteBatch(GraphicsDevice);
                Core.Splashscreen Splash = new Core.Splashscreen(this);
                Splash.LoadScenes(Root + "\\Content\\Textures\\Startup");
                WindowManager.AddWindow(Splash);
            }
            protected override void UnloadContent()
            {
                // Sound emitters
                Core.Sound.Dispose(Churp);
                Churp = null;
                Core.Sound.Dispose(ChurpSelect);
                ChurpSelect = null;
                // Releases all the content
                this.Content.Unload();
            }
            protected override void Update(GameTime gameTime)
            {
                // Update gamepad and keyboard input state
                Core.Input.Logic();
                // Execute gamemode logic/calculations
                WindowManager.Logic(gameTime);
                base.Update(gameTime);
            }
            protected override void Draw(GameTime gameTime)
            {
                GraphicsDevice.Clear(Color.Black);
                WindowManager.Draw(spriteBatch);
                // Draw main
                base.Draw(gameTime);
            }
            /// <summary>
            /// Plays the menu churp noise.
            /// </summary>
            public void PlayChurp()
            {
                Churp._EMITTER.Play();
                Churp._EMITTER.CurrentPosition = 0;
                Core.Input.Vibrate(PlayerIndex.One, 0.15F, 0.15F, 100.0F);
            }
            /// <summary>
            /// Plays the menu selection churp noise.
            /// </summary>
            public void PlayChurpSelect()
            {
                ChurpSelect._EMITTER.Play();
                ChurpSelect._EMITTER.CurrentPosition = 0;
                Core.Input.Vibrate(PlayerIndex.One, 0.4F, 0.4F, 200.0F);
            }
            public void CreateGame(string gamemode, string rootname, string map)
            {
                // Create gamemode
                Gamemode = Core.Common.GetGamemodeByClass(gamemode, this, rootname);
                // Add gamemode to window system
                WindowManager.AddWindow2(Gamemode);
                // Load level
                Gamemode.LoadLevel(map);
            }
            /// <summary>
            /// The correct procedure to exit the game safely.
            /// </summary>
            public void QuitGame()
            {
                Exit();
            }
        #endregion
    }
}