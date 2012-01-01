using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class Flak86:Bullet
    {
        public Flak86(Core.Gamemode gm, Core.Player ply, Core.Entity parent):base(gm, gm._TexturesArray.GetTexture("Flak86"), 31, 40, parent.Enemy)
        {
            // Set properties
            DisplayName = "Flak 86";
            BulletDamage = -500.0F;
            BulletRadius = 0.0F;
            BulletLifespan = 2000.0F;
            Player = ply;
            Fire(parent, 10.0F);
            // Play sound effect
            Gamemode.Audio_Control.PlayCue("NanoCM");
        }
    }
}