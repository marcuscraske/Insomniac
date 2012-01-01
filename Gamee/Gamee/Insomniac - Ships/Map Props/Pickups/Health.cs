using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame.Pickup
{
    public class Health:Core.Entity
    {
        // Purpose: when picked up, the colliding entity recievers health.
        #region "Variables"
            public bool Health_EitherTeam = true;
            public bool Health_Max = true;
            public float Health_Amount = 100.0F;
            public bool Health_OnlyIfDamaged = true;
        #endregion

        public Health(Core.Gamemode gm, Core.Texture txt, int width, int height, bool enemy):base(gm, txt, width, height, enemy)
        {
            // Makes the pickup easy to kill
            MaxHealth = 0.1F;
            // When collisions occur, the below function is called to see if an entity has picked this up
            ENT_Collided += new Ev_Collided(Health_ENT_Collided);
            // Stops e.g. cannons etc shooting the pickup
            Attackable = false;
        }
        void Health_ENT_Collided(Core.Entity Victim, Core.Entity Collider)
        {
            if (Enemy == Collider.Enemy || Health_EitherTeam)
            {
                if (Health_OnlyIfDamaged && Collider.MaxHealth - Collider.Health >= 0.1F && !Health_Max)
                {
                    // Only give the specified amount of health
                    Collider.GiveHealth(Health_Amount, this);
                    Given();
                }
                else if (Health_Max && Collider.MaxHealth - Collider.Health >= 0.1F)
                {
                    // Give maximum health
                    Collider.GiveHealth(Collider.MaxHealth - Collider.Health, this);
                    Given();
                }
            }
        }
        void Given()
        {
            // Play the sound effect
            Gamemode.Audio_Control.PlayCue("Health");
            // Destroy this entity
            Destroy();
        }
    }
}