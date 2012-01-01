using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame.Module
{
    public class NanoCM:Module
    {
        public NanoCM(Core.Player ply, Core.Gamemode gm):base(ply, gm)
        {
            Icon = Gamemode._TexturesArray.GetTexture("Module_NanoCM");
            CurtainIcon = Gamemode._TexturesArray.GetTexture("Module_Curtain");
            RechargePerCycle = CalculateRechargePerCycle(120);
        }
        public override void Clicked()
        {
            if (Percent >= 1.0F && Ply.Entity.Alive)
            {
                // Reset percent
                Percent = 0.0F;
                // Fire bullet
                Gamemode.AddEntity(new Spacegame.NanoCM(Gamemode, Ply, Ply.Entity));
            }
        }
    }
}