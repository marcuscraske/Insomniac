using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class MenuProfile:UIWindow
    {
        // Purpose: The profile menu, which is shown after the splashscreens; this allows the player to create or load an existing profile.
        #region "Variables"
            public Core.Texture Controlss;
        #endregion

        #region "Core"
            public MenuProfile(Gamee.Main main):base(main, null, 0, 0, 1024, 768)
            {
                // Set the background
                Background = new Core.Texture(main, "%MAIN%\\Menu\\Background", 100, null);
                // Set controls texture
                Controlss = new Core.Texture(main, "%MAIN%\\Controls", 100, null);
                // Select profile label
                UILabel Label = new UILabel(this, "Label1", "Select Your Profile", Color.White, 402, 100, false);
                Label.Font = Main.Content.Load<SpriteFont>("Header");
                NonSelectControls.Add(Label);
                // Add profile buttons
                int totalheight = 0;
                UIButton temp;
                foreach (DirectoryInfo di in new DirectoryInfo(main.Root + "\\Content\\Settings\\Profiles").GetDirectories())
                {
                    temp = new UIButton(this, di.Name, di.Name, 387, 140 + totalheight);
                    temp.Area.Width = 250;
                    temp.Area.Height = 30;
                    temp.BackgroundNormal = new Core.Texture(main, "%MAIN%\\Menu\\ButtonProfile", 100, null);
                    temp.BackgroundSelected = new Core.Texture(main, "%MAIN%\\Menu\\ButtonProfileSelected", 100, null);
                    temp.Button_Pressed += new UIButton.ButtonPressed(ProfilePressed);
                    Controls.Add(temp);
                    totalheight += 40;
                }
                // Create button
                UIButton Create = new UIButton(this, "Create", "Create", 387, 600);
                Create.Button_Pressed += new UIButton.ButtonPressed(PCreate);
                Controls.Add(Create);
                // Exit button
                UIButton Exit = new UIButton(this, "Exit", "Exit", 537, 600);
                Exit.Button_Pressed += new UIButton.ButtonPressed(PExit);
                Controls.Add(Exit);
                // Focus index change
                Focus_Changed += new FocusChange(MenuProfile_Focus_Changed);
            }
            void MenuProfile_Focus_Changed()
            {
                Main.PlayChurp();
            }
            public override void DrawOverride(SpriteBatch spriteBatch)
            {
                // Draw controls
                spriteBatch.Draw(Controlss._Texture, new Rectangle(30, 140, 307, 333), Color.White);
                // Draw base
                base.DrawOverride(spriteBatch);
            }
            public override void Logic(GameTime gameTime)
            {
                // Check if the user wants the controls window
                if(Input.IsKeyDown(Keys.F1))
                {
                    // Show controls window
                    Main.WindowManager.AddWindow2(new MenuControls(Main));
                    Destroy();
                }
                // Base logic
                base.Logic(gameTime);
            }
        #endregion

        #region "Functions - button pressed"
            void PExit(UIButton sender)
            {
                Main.QuitGame();
            }
            void PCreate(UIButton sender)
            {
                Main.PlayChurpSelect();
                Main.WindowManager.AddWindow2(new Core.MenuCreate(Main));
            }
            void ProfilePressed(UIButton sender)
            {
                Main.PlayChurpSelect();
                // Set profile settings root
                Main.Profile = Main.Root + "\\Content\\Settings\\Profiles\\" + ((UIButton)sender).Text;
                // Reapply graphic settings
                Resolution.ReadConfig(Main.graphics, Main.Profile + "\\Config.icf");
                // Load main menu
                Main.WindowManager.AddWindow2(new Core.MenuMain(Main));
            }
        #endregion
    }
}