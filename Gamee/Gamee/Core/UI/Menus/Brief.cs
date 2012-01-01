using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    // Purpose: to display a brief, which is a sprite, to the player of what to do etc.
    public class Brief:UIWindow
    {
        Gamemode Gamemode;
        public Brief(Gamemode gm, Core.Texture BriefTxt):base(gm._Main, BriefTxt, 0, 0, 1024, 768)
        {
            Gamemode = gm;
            // Ensures the game is paused
            gm.Paused = true;
        }
        public override void DrawOverride(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Background._Texture, new Rectangle(0, 0, 1024, 768), Color.White);
        }
        public override void Logic(GameTime gameTime)
        {
            HasInput = true;
            base.Logic(gameTime);
        }
        // Captures any input to close this window
        public override void UI_ButtonDown(Buttons button)
        {
            CloseBrief();
        }
        public override void UI_KeyDown(Keys key)
        {
            CloseBrief();
        }
        // Responsible for closing the window.
        public void CloseBrief()
        {
            // Unpauses the game
            Gamemode.Paused = false;
            // Destroys/closes this window
            Destroy();
        }
    }
}