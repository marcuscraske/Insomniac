using System;
using System.Collections.Generic;
using System.Text;

namespace Spacegame.Module
{
    public class Nuke:Module
    {
        public Nuke(Core.Player ply, Core.Gamemode gm):base(ply, gm)
        {
            Icon = gm._TexturesArray.GetTexture("Module_Nuke");
            CurtainIcon = gm._TexturesArray.GetTexture("Module_Curtain");
            RechargePerCycle = CalculateRechargePerCycle(2000);
        }
        public override void Clicked()
        {
            if (Percent >= 1.0F && Ply.Entity.Alive)
            {
                // Fire nuke
                Gamemode.AddEntity(new Spacegame.Nuke(Gamemode, Ply, Ply.Entity));
                // Reset percent
                Percent = 0.0F;
            }
        }
    }
}