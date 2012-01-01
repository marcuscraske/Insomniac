using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class Cannon:Core.Entity
    {
        #region "Variables"
            // Indicates if the base of the cannon should be drawn
            public bool DrawBase = true;
            // The rotation of the drawn base
            public float BaseRotation = 0.0F;
            // The radius in-which to attack.
            public float Radius = 1000;
            // The overridden speed in-which to fire bullets in pixels-per-cycle (PPC).
            public float FiringSpeed = 0.0F;
            // The overridden lifespan of bullets in milliseconds.
            public float FiringLifespan = 0.0F;
            // The timeout of firing in milliseconds.
            public float FiringTimeout = 250.0F;
            // The angle in-which the cannon can fire relative to the entity.
            public float FiringAngleZone = 0.523598776F;
            // Indicates if the cannon should fire at bullets.
            public bool FireAtBullet = true;
            // Fire at either team
            public bool FireAtEitherTeam = false;
            // The timeout between rotating in milliseconds.
            public float RotationTimeout = 80.0F;
            // The amount to rotate the cannon.
            public float RotationAmount = 0.314159265F;
            float TempFiring;
            float TempAngle;
            // The type of bullet (class path) to be fired.
            public string FiringBullet = "Spacegame.LaserDroplet";
            // If the cannon should only lock-on to an entity that hasn't been locked-on already.
            public bool SingleLockOn = true;
            // The base texture (which doesnt rotate).
            public Core.Texture CannonBase;
            // The entity the cannon has locked on to.
            public Core.Entity LockedTarget;
        #endregion

        #region "Core"
            public Cannon(Core.Gamemode gm, Core.Texture txt, int width, int height, bool enemy):base(gm, txt, width, height, enemy)
            {
                // The name of the entity for statistics.
                DisplayName = "Cannon";
                // Other variable settings.
                PhyStill = true;
                TempFiring = Environment.TickCount;
                TempAngle = Environment.TickCount;
                CannonBase = new Core.Texture(Gamemode._Main, "%GAMEMODE%\\Props\\CannonBase", 100, Gamemode);
            }
            public override void Logic()
            {
                // Execute entity base logic
                base.Logic();
                // If no locked target, find a new one to lock-on
                if (LockedTarget == null)
                {
                    // Shoot the first entity within the radius
                    foreach (Core.Entity ent in Gamemode._Entities)
                    {
                        // Found entity
                        if (ent.Alive && ent.Attackable && (FireAtEitherTeam == true || ent.Enemy != Enemy) && (FireAtBullet || (!FireAtBullet && !(ent is Bullet))) && !ent.LockedOn && Core.Common.EntityDistance(ent, this) < Radius)
                        {
                            if (SingleLockOn)
                            {
                                LockedTarget = ent;
                                ent.LockedOn = true;
                            }
                            FireAtEntity(ent);
                            break;
                        }
                    }
                }
                else
                {
                    if (LockedTarget.Alive && Core.Common.EntityDistance(LockedTarget, this) < Radius)
                    {
                        FireAtEntity(LockedTarget);
                    }
                    else
                    {
                        LockedTarget.LockedOn = false;
                        LockedTarget = null;
                    }
                }
            }
            /// <summary>
            /// Causes the cannon to aim and fire at a specified entity (disregards radius).
            /// </summary>
            /// <param name="ent"></param>
            public void FireAtEntity(Core.Entity ent)
            {
                // Tick-count (number of cycles)
                float tc = Environment.TickCount;
                // Variable for getting the angle of the entity locked-on
                float entangle = Core.Common.AngleOfVectors(Position + Centre, ent.Position + ent.Centre) + (float)(Math.PI / 2);
                float temp = entangle - rotation;
                // Rotate turret/head
                if (tc - TempAngle > RotationTimeout)
                {
                    TempAngle = tc;
                    rotation += MathHelper.Clamp(temp, -RotationAmount, RotationAmount);
                }
                // Shoot bullet
                if (tc - TempFiring > FiringTimeout)
                {
                    if (temp > 0 && temp < FiringAngleZone || temp < 0 && temp > -FiringAngleZone)
                    {
                        TempFiring = tc;
                        // Fire bullet
                        Bullet bullet = Spacegame.Gamemode.CreateBullet(FiringBullet, Gamemode, null, this);;
                        if (FiringSpeed != 0.0F)
                        {
                            bullet.Velocity = Vector2.Zero;
                            bullet.Fire(this, FiringSpeed);
                        }
                        if (FiringLifespan != 0.0F)
                        {
                            bullet.BulletLifespan = FiringLifespan;
                        }
                        bullet.Enemy = !ent.Enemy;
                        Gamemode.AddEntity(bullet);
                    }
                }
                // Exit - only doing one ent
                return;
            }
            public override void Draw(SpriteBatch spriteBatch)
            {
                if (DrawBase)
                {
                    // Draw cannon base
                    spriteBatch.Draw(CannonBase._Texture, new Rectangle((int)(Position.X + Centre.X), (int)(Position.Y + Centre.Y), Width, Height), null, Color.White, BaseRotation, Texture.Centre, SpriteEffects.None, 0.0F);
                }
                // Draw base (which is the cannon head)
                base.Draw(spriteBatch);
            }
            public override void Dispose()
            {
                if (LockedTarget != null)
                {
                    LockedTarget = null;
                }
                // Dispose bullet texture
                Core.Texture.Dispose(CannonBase);
                // Dispose base entity
                base.Dispose();
            }
        #endregion
    }
}