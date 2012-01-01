using System;
using System.Collections.Generic;
using System.Text;

namespace Spacegame.Module
{
    public class PlasmaBurst:Module
    {
        public PlasmaBurst(Core.Player ply, Core.Gamemode gm): base(ply, gm)
        {
            Icon = Gamemode._TexturesArray.GetTexture("Module_PlasmaBurst");
            CurtainIcon = Gamemode._TexturesArray.GetTexture("Module_Curtain");
            RechargePerCycle = CalculateRechargePerCycle(500);
        }
        public override void Clicked()
        {
            if (Percent >= 1.0F && Ply.Entity.Alive)
            {
                // Reset percent
                Percent = 0.0F;
                // Fire bullet
                Gamemode.AddEntity(new Spacegame.PlasmaBurst(Gamemode, Ply, Ply.Entity));
                // Play sound effect
                Gamemode.Audio_Control.PlayCue("Laser");
            }
        }
    }
}