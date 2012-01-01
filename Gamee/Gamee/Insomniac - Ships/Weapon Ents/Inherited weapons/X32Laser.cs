using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class X32Laser:Bullet
    {
        public X32Laser(Core.Gamemode gm, Core.Player ply, Core.Entity parent):base(gm, gm._TexturesArray.GetTexture("X32Laser"), 10, 10, parent.Enemy)
        {
            // Set properties
            DisplayName = "X32 Laser";
            BulletDamage = -10.0F;
            BulletRadius = 0.0F;
            BulletLifespan = 1800.0F;
            Player = ply;
            Fire(parent, 10.0F);
            // Play sound effect
            Gamemode.Audio_Control.PlayCue("X32Laser");
        }
    }
}