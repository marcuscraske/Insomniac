using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class LaserDroplet:Bullet
    {
        public LaserDroplet(Core.Gamemode gm, Core.Player ply, Core.Entity parent):base(gm, gm._TexturesArray.GetTexture("LaserDroplet"), 25, 25, parent.Enemy)
        {
            // Set properties
            BulletDamage = -10.0F;
            BulletRadius = 0.0F;
            BulletLifespan = 3000.0F;
            Player = ply;
            Fire(parent, 10.0F);
            // Play sound effect
            Gamemode.Audio_Control.PlayCue("Laser");
        }
    }
}