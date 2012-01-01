using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame.Pickup
{
    public class Score:Core.Entity
    {
        // Purpose: when picked up, it gives score to the player who picks this up.
        #region "Variables"
            public bool Score_EitherTeam = false;
            public float Score_Amount = 1.0F;
            public bool Score_OnlyIfDamaged = true;
        #endregion

        public Score(Core.Gamemode gm, Core.Texture txt, int width, int height, bool enemy):base(gm, txt, width, height, enemy)
        {
            // Makes the pickup easy to destroy
            MaxHealth = 0.1F;
            // Ensures cannons etc do nto lock onto this entity
            Attackable = false;
            // Makes the pickup unmovable
            PhyStill = true;
            // Makes the solid non-solid (also better for performance)
            PhySolid = false;
            // When collisions occur, the below function is called to see if an entity has picked this up
            ENT_Collided += new Ev_Collided(Health_ENT_Collided);
        }
        void Health_ENT_Collided(Core.Entity Victim, Core.Entity Collider)
        {
            if (Collider.Player != null)
            {
                if (Enemy != Collider.Enemy || Score_EitherTeam)
                {
                    // Add score
                    Collider.AddScore(Score_Amount);
                    // Play sound effect
                    Gamemode.Audio_Control.PlayCue("Score");
                    // Destroy
                    Destroy();
                }
            }
        }
    }
}