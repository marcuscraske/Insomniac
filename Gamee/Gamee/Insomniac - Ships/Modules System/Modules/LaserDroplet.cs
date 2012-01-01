using System;
using System.Collections.Generic;
using System.Text;

namespace Spacegame.Module
{
    public class LaserDroplet:Module
    {
        public LaserDroplet(Core.Player ply, Core.Gamemode gm):base(ply, gm)
        {
            Icon = Gamemode._TexturesArray.GetTexture("Module_LaserDroplet");
            CurtainIcon = Gamemode._TexturesArray.GetTexture("Module_Curtain");
            RechargePerCycle = CalculateRechargePerCycle(200);
        }
        public override void Clicked()
        {
            if (Percent >= 1.0F && Ply.Entity.Alive)
            {
                // Reset percent
                Percent = 0.0F;
                // Fire bullet
                Gamemode.AddEntity(new Spacegame.LaserDroplet(Gamemode, Ply, Ply.Entity));
            }
        }
    }
}