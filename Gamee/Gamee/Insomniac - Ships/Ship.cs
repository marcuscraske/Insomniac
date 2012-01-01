using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class Ship:Core.Entity
    {
        // Purpose: the class that handles the players entity.
        #region "Variables"
            // Trail used for the thruster
            public Core.Trail Trail;
        #endregion

        public Ship(Core.Gamemode gm, Core.Texture txt, int width, int height, bool enemy):base(gm, txt, width, height, enemy)
        {
            // Define player-based variables
            UseLives = true;
            // Define the thruster trail
            Trail = new Core.Trail(this, Gamemode._TexturesArray.GetTexture("ShipThrusterEffect"), 1.0F, 0.8F, 200, new Vector2(50, 100));
            // Used for statistics
            DisplayName = "Ship";
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw trail
            if (Alive)
            {
                Trail.Draw(spriteBatch);
            }
            // Draw base
            base.Draw(spriteBatch);
        }
        public override void Logic()
        {
            // Trail logic
            Trail.Logic();
            // Base entity logic
            base.Logic();
        }
    }
}