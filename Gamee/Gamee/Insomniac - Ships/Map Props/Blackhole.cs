using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class Blackhole:Core.Entity
    {
        #region "Variables"
            public float BlackholePower = 0.15F;
            public float BlackholeDamage = -1.0F;
        #endregion

        public Blackhole(Core.Gamemode gm, Core.Texture txt, int width, int height, bool enemy):base(gm, txt, width, height, enemy)
        {
            // The name of this entity for statistics
            DisplayName = "Blackhole";
            // Set entity properties
            Attackable = false;
            PhySolid = false;
            PhyStill = true;
            Godmode = true;
            ENT_Moved += new Ev_Moved(Blackhole_ENT_Moved);
        }
        public override void Logic()
        {
            // Base entity logic
            base.Logic();
            // Suck-in entities
            float Rad = Width * 2;
            List<Core.Entity> Ents = Gamemode._Physics.EntsInRadius(Position + Centre, Rad);
            float temp;
            foreach (Core.Entity ent in Ents)
            {
                if (!ent.PhyStill && ent != this)
                {
                    temp = MathHelper.Clamp(1.0F - (Core.Common.EntityDistance(ent, this) / Rad), 0.0F, 1.0F);
                    // Accelerate relatively towards blackhole
                    ent.Accelerate3(temp * BlackholePower, Core.Common.AngleOfVectors(Position + Centre, ent.Position + ent.Centre) - ((float)Math.PI / 2));
                    // Cause damage relative to distance too
                    ent.GiveHealth(BlackholeDamage * temp, this);
                }
            }
        }
        void Blackhole_ENT_Moved(Core.Entity ent, Vector2 OldPosition, Vector2 NewPosition)
        {
            // Check if the blackhole is near going out of the map bounds, if so the mine
            // is teleported to the other side of the map (like in snake)
            Core.Common.NoBoundsCheck(this);
        }
    }
}