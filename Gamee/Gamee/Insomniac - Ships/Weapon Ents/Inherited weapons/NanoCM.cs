using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class NanoCM:Bullet
    {
        public NanoCM(Core.Gamemode gm, Core.Player ply, Core.Entity parent):base(gm, gm._TexturesArray.GetTexture("NanoCM"), 10, 20, parent.Enemy)
        {
            // Set bullet properties
            DisplayName = "Nano CM";
            BulletDamage = -7.0F;
            BulletRadius = 0.0F;
            BulletLifespan = 1000.0F;
            Player = ply;
            Fire(parent, 8.0F);
            // Play sound effect
            Gamemode.Audio_Control.PlayCue("NanoCM");
        }
    }
}