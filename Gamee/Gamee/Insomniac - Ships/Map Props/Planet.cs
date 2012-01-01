using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class Planet:Core.Entity
    {
        // Purpose: to place entities in orbit and to allow e.g. ships to sling-shot them
        // like a real planet.
        #region "Variables"
            // The amount an entity is accelerated at, which is relative to an entities distance
            // e.g. if the planets radius is 300 pixels and an entity is 20 pixels within the planet,
            // they would be accelerated at 1 - (20 / 300) which is 0.999. I have decided to use
            // this calculation because it works best at sling-shotting and orbiting entities.
            public float OrbitalPower = 5.0F;
        #endregion

        public Planet(Core.Gamemode gm, Core.Texture txt, int width, int height, bool enemy):base(gm, txt, width, height, enemy)
        {
            // Set attributes
            Godmode = true;
            PhyStill = true;
            PhySolid = false;
            Attackable = false;
        }
        public override void Logic()
        {
            // Execute base entity logic
            base.Logic();
            // The radius is incorrect however this is simply to make the planets coverage area
            // four times as big as it usually would
            float radius = Width * 2;
            float distance_unit;
            double ent_angle;
            foreach(Core.Entity ent in Gamemode._Physics.EntsInRadius(Position + Centre, radius))
            {
                if (ent != this && !ent.PhyStill)
                {
                    // Calculate the distance unit (basically the percentage of the distance relative to the radius from 0.0 to 1.0)
                    distance_unit = MathHelper.Clamp(1.0F - (Core.Common.EntityDistance(ent, this) / radius), 0.0F, 1.0F);
                    // Calculate the entity angle
                    ent_angle = MathHelper.ToDegrees(Core.Common.AngleOfVectors(Position + Centre, ent.Position + ent.Centre));
                    // Add to position (else the velocity gets messy)
                    ent.Position += new Vector2((OrbitalPower * distance_unit) * (float)Math.Sin(2.0 * Math.PI * (ent_angle) / 360.0),
                        (OrbitalPower * distance_unit) * -(float)Math.Cos(2.0 * Math.PI * (ent_angle) / 360.0));
                    // Decrease velocity
                    ent.Velocity *= 0.999F;
                }
            }
        }
    }
}