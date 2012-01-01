using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class AIShip:Core.Entity
    {
        // Purpose: a ship with artifical intelligence that moves towards enemy entities and destroys them.
        #region "Variables"
            // The ships attack radius.
            public float ShipAttackRadius = 1000.0F;
            // The firing timeout.
            public float ShipFireTimeout = 1500.0F;
            // The type of bullet entity fired.
            public string ShipBullet = "Spacegame.PlasmaBurst";
            // If the ship should only lock-on to an entity that hasn't been locked-on already.
            public bool SingleLockOn = false;
            // If this is true, a ship will stay locked onto an enemy entity until it has died (ignoring radius check)
            public bool StayLockedOn = false;
            // The entity the ship has locked on to.
            public Core.Entity LockedTarget;
            // Used to calculate the fire timeout.
            float TempFire;
            // The max size of the ships width and height.
            float TempMax;
            // The rotation of the entity when it spawned (used if theres a spawn position)
            float TempRotation;
        #endregion

        #region "Core"
            public AIShip(Core.Gamemode gm, Core.Texture txt, int width, int height, bool enemy):base(gm, txt, width, height, enemy)
            {
                // The name for statistics
                DisplayName = "A.I Ship";
                TempFire = Environment.TickCount;
                ENT_Loaded += new Ev_Loaded(AIShip_ENT_Loaded);
                // Sets the max health
                MaxHealth = 800.0F;
                // Calculate the biggest edge/side of the entity for when the ship flys back
                TempMax = Core.Common.Max(Width, Height);
                // Loose the locked target if we die
                ENT_Killed += new Ev_Killed(AIShip_ENT_Killed);
                // Attack players that give us damage
                ENT_Collided += new Ev_Collided(AIShip_ENT_Collided);
            }
            void AIShip_ENT_Collided(Core.Entity Victim, Core.Entity Collider)
            {
                // If the player thats attacked us is alive, lock-on to them
                if (Enemy != Collider.Enemy && Collider.Player != null)
                {
                    LockedTarget = Collider.Player.Entity;
                }
            }
            void AIShip_ENT_Killed(Core.Entity ent)
            {
                // If this ship dies, its locked target is lost
                if (SingleLockOn && LockedTarget != null)
                {
                    LockedTarget.LockedOn = false;
                }
            }
            void AIShip_ENT_Loaded()
            {
                SpawnPosition = Position;
                // If theres no spawn set, respawn the entity
                if (SpawnPosition == Vector2.Zero)
                {
                    // Set to dead
                    Alive = false;
                    // Give to respawn manager
                    Gamemode.RespawnEnt(this);
                }
                else
                {
                    // There is a spawn position, therefore the rotation has been set and this will be used when
                    // gliding the AI Ship back
                    TempRotation = rotation;
                }
            }
            public override void Logic()
            {
                // Ship logic
                float tc = Environment.TickCount;
                // Loop and find a target if we dont have one locked-on
                if (LockedTarget == null)
                {
                    foreach (Core.Entity ent in Gamemode._Entities)
                    {
                        // Check its alive, attackable, an enemy and not a bullet
                        if (ent.Alive && ent.Attackable && ent.Enemy != Enemy && !(ent is Bullet) && (!ent.LockedOn || !SingleLockOn) && Core.Common.EntityDistance(ent, this) < ShipAttackRadius)
                        {
                            LockedTarget = ent;
                            if (SingleLockOn)
                            {
                                ent.LockedOn = true;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    // Check the entity locked-on is valid still
                    if (!LockedTarget.Alive || (Core.Common.EntityDistance(LockedTarget, this) >= ShipAttackRadius && !StayLockedOn))
                    {
                        LockedTarget.LockedOn = false;
                        LockedTarget = null;
                    }
                }
                if (LockedTarget != null)
                {
                    // Move towards entity
                    MoveTowardsPoint2(LockedTarget.Position + LockedTarget.Centre, Vector2.Distance(LockedTarget.Position + LockedTarget.Centre, Position + Centre));
                    // Fire at entity
                    if (tc - TempFire > ShipFireTimeout)
                    {
                        TempFire = tc;
                        Gamemode.AddEntity(Spacegame.Gamemode.CreateBullet(ShipBullet, Gamemode, null, this));
                    }
                }
                else if(SpawnPosition != Vector2.Zero)
                {
                    // Go back to the ships spawn position
                    MoveTowardsPoint(SpawnPosition + Centre, Vector2.Distance(Position + Centre, SpawnPosition + Centre));
                }
                // Base logic
                base.Logic();
            }
        #endregion

        #region "Functions"
            // Used to move towards a specific point.
            public void MoveTowardsPoint(Vector2 point, float distance)
            {
                // Rotate
                rotation = (Core.Common.AngleOfVectors(Position + Centre, point) + (float)(Math.PI / 2));
                // Movement - accelerate towards the point
                if (distance > -TempMax && distance < TempMax)
                {
                    Position = point;
                    Velocity = Vector2.Zero;
                    if (SpawnPosition != Vector2.Zero)
                    {
                        rotation = TempRotation;
                    }
                }
                else if (distance > 0)
                {
                    Accelerate(MaxSpeed);
                }
                else if (distance < 0)
                {
                    Accelerate(-MaxSpeed);
                }
            }
            // Moves towards a point whilst keeping distance.
            public void MoveTowardsPoint2(Vector2 point, float distance)
            {
                // Rotate
                rotation = (Core.Common.AngleOfVectors(Position + Centre, point) + (float)(Math.PI / 2));
                // Movement
                if (distance > Width + Height)
                {
                    Accelerate(MaxSpeed);
                }
                else
                {
                    Accelerate(-MaxSpeed);
                }
            }
        #endregion 
    }
}