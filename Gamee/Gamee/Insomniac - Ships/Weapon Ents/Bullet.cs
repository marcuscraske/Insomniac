using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    // Purpose: a class which can be inherited to create a bullet entity.
    public class Bullet:Core.Entity
    {
        #region "Variables"
            float TempBulletTick;
            // The bullets lifespan.
            public float BulletLifespan = 2500.0F;
            // The amount of damage caused to the colliding entity.
            public float BulletDamage = -20.0F;
            // The damage radius of the bullet, 0.0 will only cause damage to the colliding ent.
            public float BulletRadius = 0.0F;
        #endregion

        #region "Core"
            public Bullet(Core.Gamemode gm, Core.Texture txt, int width, int height, bool enemy):base(gm, txt, width, height, enemy)
            {
                // Used for calculating the lifespan
                TempBulletTick = Environment.TickCount;
                // Sets necessary variables for this entity
                Destroyable = true;
                UseLives = false;
                PhySolid = false;
                CountAsKill = false;
                // Display name for statistics, this is ony overriden if the name hasn't been changed
                if (DisplayName == "Base Entity")
                {
                    DisplayName = "Bullet";
                }
                // Catch collisions to cause damage
                ENT_Collided += new Ev_Collided(Bullet_ENT_Collided);
                ENT_Moved += new Ev_Moved(Bullet_ENT_Moved);
            }

            void Bullet_ENT_Moved(Core.Entity ent, Vector2 OldPosition, Vector2 NewPosition)
            {
                // Check if the bullet has hit the edge of the map, if so it will be destroyed (improves game efficiency)
                Core.Common.NoBoundsCheck2(this);
            }
            public override void Logic()
            {
                // Check the lifespan
                if (Environment.TickCount - TempBulletTick > BulletLifespan)
                {
                    Destroy();
                    return;
                }
                // Base entity logic
                base.Logic();
            }
            /// <summary>
            /// Causes damage to an entity thats collided with this entity/bullet.
            /// </summary>
            /// <param name="ent"></param>
            /// <param name="amount"></param>
            public virtual void Bullet_ENT_Collided(Core.Entity Victim, Core.Entity Collider)
            {
                // Check the collider is alive, not on the same team and solid or a bullet.
                if (Alive && Victim.Enemy != Collider.Enemy && (Collider.PhySolid || Collider is Bullet))
                {
                    if (BulletRadius == 0.0F)
                    {
                        // Cause damage only to the collider
                        Collider.GiveHealth(BulletDamage, this);
                    }
                    else
                    {
                        // Cause damage within a radius
                        Gamemode._Physics.HealthRadius(this, Position + Centre, BulletRadius, BulletDamage);
                    }
                    // Creates a generic explosion
                    Gamemode.CreateGenericExplosion(Position);
                    // Destroys this entity
                    Destroy();
                }
            }            
        #endregion

        #region "Functions"
            // Causes the entity to be fired by calculating a rotated vector around the entity
            // the bullet was fired from (at a specified speed).
            public void Fire(Core.Entity Ent, float speed)
            {
                Position = Core.Common.RotateVector(new Vector2(Ent.Centre.X, Ent.Centre.Y - (Ent.Height / 2) - (Height * 2)), Ent.Position, Centre, Ent.Centre, Ent.rotation);
                rotation = Ent.rotation;
                Enemy = Ent.Enemy;
                MaxSpeed = speed;
                Accelerate(speed);
            }
        #endregion
    }
}