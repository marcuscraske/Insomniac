using System;
using System.Collections.Generic;
using System.Text;

namespace Spacegame.Module
{
    public class X32Laser:Module
    {
        #region "Variables"
            float TotalShots = 0.0F;
        #endregion

        public X32Laser(Core.Player ply, Core.Gamemode gm):base(ply, gm)
        {
            Icon = Gamemode._TexturesArray.GetTexture("Module_X32Laser");
            CurtainIcon = Gamemode._TexturesArray.GetTexture("Module_Curtain");
            RechargePerCycle = CalculateRechargePerCycle(10000);
        }
        public override void Logic()
        {
            // Recharge shots
            if (TotalShots > 0.0F)
            {
                TotalShots -= 0.5F;
            }
            // Base logic
            base.Logic();
        }
        public override void Clicked()
        {
            if (Percent >= 1.0F && Ply.Entity.Alive)
            {
                if (TotalShots >= 50.0F)
                {
                    Percent = 0.0F;
                    TotalShots = 0.0F;
                }
                TotalShots += 1;
                // Fire bullet
                Gamemode.AddEntity(new Spacegame.X32Laser(Gamemode, Ply, Ply.Entity));
            }
        }
    }
}