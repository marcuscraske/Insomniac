using System;
using System.Collections.Generic;
using System.Text;

namespace Spacegame.Module
{
    public class ASMDestroyer: Module
    {
        public ASMDestroyer(Core.Player ply, Core.Gamemode gm):base(ply, gm)
        {
            Icon = Gamemode._TexturesArray.GetTexture("Module_ASM-Destroyer");
            CurtainIcon = Gamemode._TexturesArray.GetTexture("Module_Curtain");
            RechargePerCycle = CalculateRechargePerCycle(2500);
        }
        public override void Clicked()
        {
            if (Percent >= 1.0F && Ply.Entity.Alive)
            {
                // Fire bullet
                Gamemode.AddEntity(new Spacegame.ASMDestroyer(Gamemode, Ply, Ply.Entity));
                // Reset percent
                Percent = 0.0F;
            }
        }
    }
}