using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Spacegame.Module
{
    public class Hyperdrive:Module
    {
        #region "Variables"
            bool Down = false;
            float CycleCount = 0.0F;
            float MaxCycles = 800.0F;
            float DistancePerCycle = 10.0F;
            float LastTick = 0.0F;
        #endregion

        public Hyperdrive(Core.Player ply, Core.Gamemode gm):base(ply, gm)
        {
            Icon = Gamemode._TexturesArray.GetTexture("Module_Hyperdrive");
            CurtainIcon = Gamemode._TexturesArray.GetTexture("Module_Curtain");
            RechargePerCycle = CalculateRechargePerCycle(10000);
        }
        public void Jump()
        {
            float Distance = DistancePerCycle * CycleCount;
            Vector2 JumpPosition = Ply.Entity.Position + new Vector2(Distance * (float)Math.Sin(2.0 * Math.PI * (Ply.Entity.Rotation) / 360.0),
                Distance * -(float)Math.Cos(2.0 * Math.PI * (Ply.Entity.Rotation) / 360.0));
            // This part clamps the vector within the map, if it goes outside the map it continues the rest of the distance left from 0.
            // Clamp the x vector within the map
            if (JumpPosition.X < 0)
            {
                JumpPosition.X = (JumpPosition.X / Gamemode._Map.Info.ActualTileWidth);
                JumpPosition.X -= (int)JumpPosition.X;
                JumpPosition.X *= Gamemode._Map.Info.ActualTileWidth;
                JumpPosition.X = Gamemode._Map.Info.ActualTileWidth + JumpPosition.X;
            }
            else if (JumpPosition.X - Ply.Entity.Width > Gamemode._Map.Info.ActualTileWidth)
            {
                JumpPosition.X = (JumpPosition.X / Gamemode._Map.Info.ActualTileWidth);
                JumpPosition.X -= (int)JumpPosition.X;
                JumpPosition.X *= Gamemode._Map.Info.ActualTileWidth;
            }
            // Clamp the y vector within the map
            if (JumpPosition.Y < 0)
            {
                JumpPosition.Y = (JumpPosition.Y / Gamemode._Map.Info.ActualTileHeight);
                JumpPosition.Y -= (int)JumpPosition.Y;
                JumpPosition.Y *= Gamemode._Map.Info.ActualTileHeight;
                JumpPosition.Y = Gamemode._Map.Info.ActualTileHeight + JumpPosition.Y;
            }
            else if (JumpPosition.Y - Ply.Entity.Height > Gamemode._Map.Info.ActualTileHeight)
            {
                JumpPosition.Y = (JumpPosition.Y / Gamemode._Map.Info.ActualTileHeight);
                JumpPosition.Y -= (int)JumpPosition.Y;
                JumpPosition.Y *= Gamemode._Map.Info.ActualTileHeight;
            }
            // Move (the official move function isn't used in case of a collision inf loop occuring)
            // Set entity position/vector
            Ply.Entity.Position = JumpPosition;
            // Set camera position/vector
            Ply.Camera.SetPosition(JumpPosition.X, JumpPosition.Y);
            // Create effect
            Core.Effect.CreateExplosion(Gamemode, JumpPosition + Ply.Entity.Centre, 1.5F, 0.5F, 10.0F, 30.0F, Gamemode._TexturesArray.GetTexture("Hyperdrive_Explosion"), Gamemode._TexturesArray.GetTexture("Hyperdrive_Flare"), "HyperdriveJumped");
        }
        public override void Logic()
        {
            // Base logic
            base.Logic();
            if (Down && (Environment.TickCount - LastTick > 120 || CycleCount >= MaxCycles))
            {
                // Jump into hyperspace!
                Jump();
                // Reset
                Percent = 0.0F;
                LastTick = 0.0F;
                CycleCount = 0.0F;
                Down = false;
            }
        }
        public override void Clicked()
        {
            if (Percent >= 1.0F && Ply.Entity.Alive)
            {
                if (LastTick == 0.0F)
                {
                    Down = true;
                }
                CycleCount += 1.0F;
                LastTick = Environment.TickCount;
            }
        }
    }
}