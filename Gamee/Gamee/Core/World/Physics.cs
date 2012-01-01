using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class Physics
    {
        // Purpose: manages the games physics.
        #region "Variables"
            public Gamemode Gamemode;
        #endregion

        #region "Core"
            public Physics(Gamemode _gamemode)
            {
                Gamemode = _gamemode;
            }
        #endregion

        #region "Functions - collisions"
            /// <summary>
            /// Checks the game for collisions.
            /// </summary>
            /// <param name="collider"></param>
            public void CheckCollisions(Entity collider)
            {
                Rectangle r = new Rectangle((int)collider.Position.X, (int)collider.Position.Y, collider.Width, collider.Height);
                Rectangle t;
                foreach (Entity victim in Gamemode._Entities)
                {
                    t = new Rectangle((int)victim.Position.X, (int)victim.Position.Y, victim.Width, victim.Height);
                    if(victim.Alive && r.Intersects(t))
                    {
                        CollisionHandler(collider, victim);
                    }
                }
            }
            /// <summary>
            /// Handles the collision between two entities.
            /// </summary>
            /// <param name="collider"></param>
            /// <param name="Victim"></param>
            public void CollisionHandler(Entity collider, Entity victim)
            {
                // If either entity is still, they will not collide therefore
                // raise the event and return.
                if (victim.PhySolid && collider.PhySolid)
                {
                    // Calculate result of collision - this shares out the total energy based on mass ratio
                    float totalmass = collider.PhyMass + victim.PhyMass;
                    float totalx = (collider.Velocity.X + victim.Velocity.X) / totalmass;
                    float totaly = (collider.Velocity.Y + victim.Velocity.Y) / totalmass;
                    // Gives the new velocity - but also ensures each entity is not still
                    if (!victim.PhyStill)
                    {
                        victim.Velocity = new Vector2(totalx * victim.PhyMass, totaly * victim.PhyMass);
                    }
                    if (!collider.PhyStill)
                    {
                        collider.Velocity = new Vector2(totalx * collider.PhyMass, totaly * collider.PhyMass);
                    }
                    // Move the collider outside the victim
                    if (collider.Position.Y + collider.Height > victim.Position.Y
                        && collider.Position.Y + collider.Height <= victim.Position.Y + (victim.Height / 2))
                    {
                        collider.Position.Y = victim.Position.Y - collider.Height;
                    }
                    else if (collider.Position.Y < victim.Position.Y + victim.Height
                        && collider.Position.Y > victim.Position.Y + (collider.Height / 2))
                    {
                        collider.Position.Y = victim.Position.Y + victim.Height;
                    }
                    else if (collider.Position.X + collider.Width > victim.Position.X
                        && collider.Position.X + collider.Width < victim.Position.X + (victim.Width / 2))
                    {
                        collider.Position.X = victim.Position.X - collider.Width;
                    }
                    else if (collider.Position.X < victim.Position.X + victim.Width
                        && collider.Position.X > victim.Position.X + (victim.Width / 2))
                    {
                        collider.Position.X = victim.Position.X + victim.Width;
                    }
                    // Check the bounds (that neither ents are outside the map)
                    collider.BoundsCheck();
                    victim.BoundsCheck();
                }
                // Raise collision
                victim.CauseCollision(victim, collider);
                collider.CauseCollision(collider, victim);
            }
        #endregion  

        #region "Functions - ents in radius"
            /// <summary>
            /// Returns a list of all the entities within a radius of a point.
            /// </summary>
            /// <param name="centre"></param>
            /// <param name="radius"></param>
            public List<Entity> EntsInRadius(Vector2 centre, float radius)
            {
                // The array of entities to be returned to the invoker
                List<Entity> Ents = new List<Entity>();
                // Loop all the gamemodes entities to find the entities within the radius
                foreach (Entity ent in Gamemode._Entities)
                {
                    if (ent.Alive && Vector2.Distance(ent.Position + ent.Centre, centre) < radius + Common.Max(ent.Width, ent.Height))
                    {
                        Ents.Add(ent);
                    }
                }
                return Ents;
            }
        #endregion

        #region "Functions - damage"
            /// <summary>
            /// Causes health gain/reduction within a radius of a point.
            /// </summary>
            /// <param name="centre"></param>
            /// <param name="radius"></param>
            /// <param name="amount"></param>
            public void HealthRadius(Entity inflicter, Vector2 centre, float radius, float amount)
            {
                // Loop and find all the entities within the radius
                foreach (Entity ent in Gamemode._Entities)
                {
                    // Check the entity is within the radius, alive and doesn't have godmode
                    if (ent.Alive && !ent.Godmode && Vector2.Distance(centre, ent.Position + ent.Centre) < radius)
                    {
                        // Give health
                        ent.GiveHealth(amount, inflicter);
                    }
                }
            }
    #endregion
    }
}