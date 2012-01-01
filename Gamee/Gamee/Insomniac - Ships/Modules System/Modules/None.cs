using System;
using System.Collections.Generic;
using System.Text;

namespace Spacegame.Module
{
    public class None:Module
    {
        public None(Core.Player ply, Core.Gamemode gm):base(ply, gm)
        {
            Icon = gm._TexturesArray.GetTexture("Module_None");
            CurtainIcon = null;
            RechargePerCycle = CalculateRechargePerCycle(1000);
        }
        public override void Clicked()
        {
            if (Percent >= 1.0F && Ply.Entity.Alive)
            {
                Gamemode.Audio_Control.PlayCue("Beep");
                Percent = 0.0F;
            }
        }
    }
}