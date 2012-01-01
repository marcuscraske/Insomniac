using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class Mine:Core.Entity
    {
        // Purpose: a mine that flys around in space causing immense damagr to any colliding
        // entities.
        #region "Variables"
            // The acceleration speed of the mine every cycle
            public float MineSpeed = 8.0F;
            // The amount of damage to cause per every collision
            public float MineDamage = -100.0F;
           float rotate_amount = 0.1F;
        #endregion

        public Mine(Core.Gamemode gm, Core.Texture txt, int width, int height, bool enemy):base(gm, txt, width, height, enemy)
        {
            // Sets the display name (for stats)
            DisplayName = "Space Mine";
            // Cannons etc cannot lock-on
            Attackable = false;
            // Sets properties for movement by randomly generating a rotation angle
            Rotation = (float)(Gamemode.RNumber2(0, 360));
            // Push the mine at a random speed between 0.1 to 0.9
            Accelerate((float)Gamemode.RNumber2(1, 9) * 0.1F);
            // A 33% probability of rotating anti-clockwise
            if (Gamemode.RNumber2(1, 3) == 2)
            {
                rotate_amount = -0.1F;
            }
            // Sets handlers
            ENT_Moved += new Ev_Moved(Mine_ENT_Moved);
            ENT_Collided += new Ev_Collided(Mine_ENT_Collided);
        }
        void Mine_ENT_Collided(Core.Entity Victim, Core.Entity Collider)
        {
            if (Collider.GetType().Name == "Ship")
            {
                // If the colliding entity is a ship, give damage and bounce off it
                Collider.GiveHealth(MineDamage, this);
                // Bounce mine
                Velocity = -Velocity;
                Rotation += 180;
            }
            else if (Collider.PhySolid)
            {
                // Bounce mine if the collider is solid
                Velocity = -Velocity;
                Rotation += 180;
            }
        }
        void Mine_ENT_Moved(Core.Entity ent, Vector2 OldPosition, Vector2 NewPosition)
        {
            // Check if the mine is near going out of the map bounds, if so the mine
            // is teleported to the other side of the map (like in snake)
            Core.Common.NoBoundsCheck(this);
        }
        public override void Logic()
        {
            // Add rotation amount (causing the mine to move a little randomly)
            Rotation += rotate_amount;
            // Accelerate slightly as well in the new rotation direction
            Accelerate(MineSpeed);
            base.Logic();
        }
    }
}