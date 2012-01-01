using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Spacegame
{
    // Purpose: the actual gamemode used for Insomniac, however this inherits from the
    // engines gamemode.
    public class Gamemode : Core.Gamemode
    {
        #region "Variables"
            // Player heads-up display (HUD)
            ModulesSystem UI;
            // Ambient music
            Ambient Ambient;
        #endregion

        public Gamemode(Gamee.Main main, string rootpath):base (main, rootpath)
        {
            // Define Player UI
            UI = new ModulesSystem(main, this);
            // Define and play ambient music
            Ambient = new Ambient(this, "%GAMEMODE%\\Ambient");
            // Add static textures to array
            // Weapon textures
            _TexturesArray.AddTexture("LaserDroplet", new Core.Texture(main, "%GAMEMODE%\\Weapons\\LaserDroplet", 100, this));
            _TexturesArray.AddTexture("LaserBullet", new Core.Texture(main, "%GAMEMODE%\\Weapons\\LaserBullet", 100, this));
            _TexturesArray.AddTexture("LaserBulletTrail", new Core.Texture(main, "%GAMEMODE%\\Weapons\\LaserBulletTrail", 100, this));
            _TexturesArray.AddTexture("Nuke_Missile", new Core.Texture(main, "%GAMEMODE%\\Weapons\\Nuke_Missile", 100, this));
            _TexturesArray.AddTexture("Nuke_Trail", new Core.Texture(main, "%GAMEMODE%\\Weapons\\Nuke_Trail", 100, this));
            _TexturesArray.AddTexture("NanoCM", new Core.Texture(main, "%GAMEMODE%\\Weapons\\NanoCM", 100, this));
            _TexturesArray.AddTexture("X32Laser", new Core.Texture(main, "%GAMEMODE%\\Weapons\\X32Laser", 100, this));
            _TexturesArray.AddTexture("Flak86", new Core.Texture(main, "%GAMEMODE%\\Weapons\\Flak86", 100, this));
            _TexturesArray.AddTexture("ASM-Destroyer", new Core.Texture(main, "%GAMEMODE%\\Weapons\\ASM-Destroyer", 100, this));
            _TexturesArray.AddTexture("ASM-Destroyer_Trail", new Core.Texture(main, "%GAMEMODE%\\Weapons\\ASM-Destroyer_Trail", 100, this));
            // Explosion textures
            _TexturesArray.AddTexture("Explosion_Main", new Core.Texture(main, "%GAMEMODE%\\Effects\\Explosion_Main", 100, this));
            _TexturesArray.AddTexture("Explosion_Flare", new Core.Texture(main, "%GAMEMODE%\\Effects\\Explosion_Flare", 100, this));
            _TexturesArray.AddTexture("Hyperdrive_Explosion", new Core.Texture(main, "%GAMEMODE%\\Effects\\Hyperdrive_Explosion", 100, this));
            _TexturesArray.AddTexture("Hyperdrive_Flare", new Core.Texture(main, "%GAMEMODE%\\Effects\\Hyperdrive_Flare", 100, this));
            _TexturesArray.AddTexture("Nuclear_Shockwave", new Core.Texture(main, "%GAMEMODE%\\Effects\\Nuclear_Shockwave", 100, this));
            // Modules
            _TexturesArray.AddTexture("Module_None", new Core.Texture(main, "%GAMEMODE%\\Modules\\None", 100, this));
            _TexturesArray.AddTexture("Module_Curtain", new Core.Texture(main, "%GAMEMODE%\\Modules\\Curtain", 100, this));
            _TexturesArray.AddTexture("Module_Nuke", new Core.Texture(main, "%GAMEMODE%\\Modules\\Nuke", 100, this));
            _TexturesArray.AddTexture("Module_PlasmaBurst", new Core.Texture(main, "%GAMEMODE%\\Modules\\PlasmaBurst", 100, this));
            _TexturesArray.AddTexture("Module_LaserDroplet", new Core.Texture(main, "%GAMEMODE%\\Modules\\LaserDroplet", 100, this));
            _TexturesArray.AddTexture("Module_Hyperdrive", new Core.Texture(main, "%GAMEMODE%\\Modules\\Hyperdrive", 100, this));
            _TexturesArray.AddTexture("Module_NanoCM", new Core.Texture(main, "%GAMEMODE%\\Modules\\NanoCM", 100, this));
            _TexturesArray.AddTexture("Module_X32Laser", new Core.Texture(main, "%GAMEMODE%\\Modules\\X32Laser", 100, this));
            _TexturesArray.AddTexture("Module_Flak86", new Core.Texture(main, "%GAMEMODE%\\Modules\\Flak86", 100, this));
            _TexturesArray.AddTexture("Module_ASM-Destroyer", new Core.Texture(main, "%GAMEMODE%\\Modules\\ASM-Destroyer", 100, this));
            // Player ship
            _TexturesArray.AddTexture("ShipThrusterEffect", new Core.Texture(main, "%GAMEMODE%\\Ships\\ShipThrusterEffect", 100, this));
            // Catch gamemode deaths
            ENT_Killed += new EntKilled(Gamemode_ENT_Killed);
            // Catch new games
            New_Level += new NewLevel(Gamemode_New_Level);
            // Enable the game
            Paused = false;
        }

        void Gamemode_New_Level()
        {
            // Restart ambient
            Ambient.Restart();
        }
        public override void LoadPlayers()
        {
            // Load and define each player
            Core.ConfigFile Config;
            Core.Texture Temp;
            foreach (Core.Player ply in _Players)
            {
                // Load their config file
                Config = new Core.ConfigFile();
                Config.LoadFromFile(_Main.Root + "\\Content\\Settings\\Profiles\\" + ply.Alias + "\\Insomniac.icf");
                // Set entitys variables from the configuration file
                Temp = new Core.Texture(_Main, Config.GetKey("Ship", "Texture"), 100, this);
                ply.SetEntity(Core.Common.GetEntityByClass(Config.GetKey("Ship", "Class"), this, Temp, Temp._Texture.Width, Temp._Texture.Height, ply.Entity.Enemy));
                ply.Entity.MaxSpeed = float.Parse(Config.GetKey("Ship", "MaxSpeed"));
                ply.Entity.MaxHealth = float.Parse(Config.GetKey("Ship", "MaxHealth"));
                // Set modules
                ply.Data.Add("M1", ModulesSystem.GetModuleByClass(Config.GetKey("Ship", "Module1"), this, ply)); // Module 1
                ply.Data.Add("M2", ModulesSystem.GetModuleByClass(Config.GetKey("Ship", "Module2"), this, ply)); // Module 2
                ply.Data.Add("M3", ModulesSystem.GetModuleByClass(Config.GetKey("Ship", "Module3"), this, ply)); // Module 3
                ply.Data.Add("M4", ModulesSystem.GetModuleByClass(Config.GetKey("Ship", "Module4"), this, ply)); // Module 4
                ply.Data.Add("M5", ModulesSystem.GetModuleByClass(Config.GetKey("Ship", "Module5"), this, ply)); // Module 5
                ply.Data.Add("M6", ModulesSystem.GetModuleByClass(Config.GetKey("Ship", "Module6"), this, ply)); // Module 6
            }
            // Rebuild modules UI position
            UI.RebuildModulePositions();
        }
        void Gamemode_ENT_Killed(Core.Entity victim, Core.Entity killer)
        {
            // When an entity dies, an explosion is created
            CreateGenericExplosion(victim.Position);
        }
        public override void Gamemode_Game_Over()
        {
            // Stop ambient music
            Ambient.Stop();
            // Run base gamemode code
            base.Gamemode_Game_Over();
        }
        public override void LogicOverride(GameTime gameTime)
        {
            // Check input
            if (HasInput)
            {
                // Ambient logic
                Ambient.Logic();
                // Player logic
                foreach (Core.Player ply in _Players)
                {
                    // Forwards
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_FORWARD) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_FORWARD))
                    {
                        ply.Entity.Accelerate(0.1F);
                    }
                    // Backwards
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_BACKWARD) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_BACKWARD))
                    {
                        ply.Entity.Accelerate(-0.1F);
                    }
                    // Rotate left
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_LEFT) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_LEFT))
                    {
                        ply.Entity.Rotation -= 4.5F;
                    }
                    // Rotate right
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_RIGHT) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_RIGHT))
                    {
                        ply.Entity.Rotation += 4.5F;
                    }
                    // Zoom in
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_ZOOM_IN) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_ZOOM_IN))
                    {
                        ply.Camera.Zoom += 0.01F;
                    }
                    // Zoom out
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_ZOOM_OUT) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_ZOOM_OUT))
                    {
                        ply.Camera.Zoom -= 0.01F;
                    }
                    // Module 1
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_1) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_1))
                    {
                        ((Module.Module)ply.Data["M1"]).Clicked();
                    }
                    // Module 2
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_2) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_2))
                    {
                        ((Module.Module)ply.Data["M2"]).Clicked();
                    }
                    // Module 3
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_3) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_3))
                    {
                        ((Module.Module)ply.Data["M3"]).Clicked();
                    }
                    // Module 4
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_4) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_4))
                    {
                        ((Module.Module)ply.Data["M4"]).Clicked();
                    }
                    // Module 5
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_5) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_5))
                    {
                        ((Module.Module)ply.Data["M5"]).Clicked();
                    }
                    // Module 6
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_6) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_6))
                    {
                        ((Module.Module)ply.Data["M6"]).Clicked();
                    }
                    // Rotate left
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_ROTATE_MINUS) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_ROTATE_MINUS))
                    {
                        ply.Camera.Rotation -= 1.57079633F;
                    }
                    // Rotate right
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_ROTATE_PLUS) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_ROTATE_PLUS))
                    {
                        ply.Camera.Rotation += 1.57079633F;
                    }
                    // Reset view
                    if (Core.Input.Keyboard_isdown(ply.Index, Core.Input.GAME_KEY_VIEW_RESET) || Core.Input.Gamepad_isdown(ply.Index, Core.Input.GAME_BUTTON_VIEW_RESET))
                    {
                        ply.Camera.ResetView();
                    }
                }
            }
            UI.Logic();
        }
        public override void DrawOverride(SpriteBatch sb)
        {
            // Drawing
            // Topbar - has to be drawn last
        }
        public override void DrawOverride2(SpriteBatch sb)
        {
            // Draw player UI
            UI.Draw(sb);
        }
        /// <summary>
        /// Creates a bullet object from a string representation of the class's namespace and classname.
        /// </summary>
        /// <param name="bullet_name"></param>
        /// <param name="gm"></param>
        /// <param name="ply"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Bullet CreateBullet(string bullet_name, Core.Gamemode gm, Core.Player ply, Core.Entity parent)
        {
            return (Bullet)Activator.CreateInstance(Type.GetType(bullet_name, true, true), new object[] { gm, ply, parent });
        }
        public override void Destroying()
        {
            // Destroy ambient
            Ambient.Destroy();
            Ambient = null;
            // Destroy base gamemode
            base.Destroying();
        }
    }
}