using System;
using System.Collections.Generic;
using System.Text;

namespace Spacegame.Module
{
    public class Flak86:Module
    {
        public Flak86(Core.Player ply, Core.Gamemode gm):base(ply, gm)
        {
            Icon = Gamemode._TexturesArray.GetTexture("Module_Flak86");
            CurtainIcon = Gamemode._TexturesArray.GetTexture("Module_Curtain");
            RechargePerCycle = CalculateRechargePerCycle(1000);
        }
        public override void Clicked()
        {
            if (Percent >= 1.0F && Ply.Entity.Alive)
            {
                // Fire entity
                Gamemode.AddEntity(new Spacegame.Flak86(Gamemode, Ply, Ply.Entity));
                // Reset percent
                Percent = 0.0F;
            }
        }
    }
}