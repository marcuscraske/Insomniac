using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class Asteroid:Core.Entity
    {
        // Purpose: an entity that randomly moves around the map and behaves like an asteroid.
        #region "Variables"
            // Maximum number of sub-children
            public float MaxChildLine = 4.0F;
            // The number of asteroids to be replicated upon an asteroid death
            public float DeathReplication = 4.0F;
            // The minimum speed an asteroid can randomly fly
            public float AsteroidSpeedMin = 5.0F;
            // The maximum speed an asteroid can randomly fly
            public float AsteroidSpeedMax = 20.0F;
            // The score awarded to an entity that kills an asteroid
            public float ScorePerKill = 2000.0F;
        #endregion

        #region "Core"
            public Asteroid(Core.Gamemode gm, Core.Texture txt, int width, int height, bool enemy):base(gm, txt, width, height, enemy)
            {
                // Ensure e.g. turrets do not lock onto this
                Attackable = false;
                // Catch collisions
                ENT_Collided += new Ev_Collided(Asteroid_ENT_Collided);
                // Catch when the entity has been respawned
                ENT_Respawned += new Ev_Respawned(Asteroid_ENT_Respawned);
            }
            public override void Logic()
            {
                // If the asteroidis not moving, its forced to randomly move around
                if (Velocity == Vector2.Zero)
                {
                    RandomlyMove();
                }
               // Base (entity) logic
               base.Logic();
            }
            void Asteroid_ENT_Respawned(Vector2 position)
            {
                // Causes the asteroid to randomly move around
                RandomlyMove();
            }
            void Asteroid_ENT_Loaded()
            {
                RandomlyMove();
                // Ensure the object can die - anti-cheat as well since the map has already been loaded
                Godmode = false;
                UseLives = false;
            }
            void Asteroid_ENT_Collided(Core.Entity Victim, Core.Entity Collider)
            {
                if (Collider is Bullet)
                {
                    // Give score
                    if (Collider.Player != null)
                    {
                        Collider.AddScore(ScorePerKill);
                    }
                    // Destroy this entity
                    Destroy();
                    // Replicate child asteroids
                    Replicate();
                }
                else if (!Collider.PhyStill && Collider.PhySolid && !Collider.Godmode && !(Collider is Asteroid))
                {
                    // The collider was not a bullet nor had godmode etc, therefore its smashed into this asteroid
                    // and destroyed
                    Collider.Destroy();
                }
                else
                {
                    // The collider has godmode, a still entity or another asteroid - therefore bounce
                    Velocity = -Velocity;
                }
            }
        #endregion

        #region "Functions"
            /// <summary>
            /// Create clone asteroids.
            /// </summary>
            public void Replicate()
            {
                if (DeathReplication > 0.0F && MaxChildLine > 0.0F)
                {
                    // Used to temp store the new height and width (which is this objects size divided by two)
                    int width = Width / 2;
                    int height = Height / 2;
                    // Ensure the new width and height is greater than 5 pixels
                    if (width < 5)
                    {
                        width = 5;
                    }
                    if (height < 5)
                    {
                        height = 5;
                    }
                    // Create and add the new asteroids
                    Asteroid temp;
                    for (int i = 0; i <= DeathReplication; i++)
                    {
                        // Subtract one from the total number of child-asteroids allowed to be produced
                        DeathReplication -= 1;
                        // Define new asteroid
                        temp = new Asteroid(Gamemode, Texture, width, height, Enemy);
                        temp.SpawnPosition = SpawnPosition;
                        temp.Position = Position + Centre - new Vector2(Gamemode.RNumber2(Width, Width * 2), Gamemode.RNumber2(Height, Height * 2));
                        temp.DeathReplication = DeathReplication;
                        temp.AsteroidSpeedMin = AsteroidSpeedMin;
                        temp.AsteroidSpeedMax = AsteroidSpeedMax;
                        temp.MaxChildLine = MaxChildLine - 1;
                        temp.ScorePerKill = ScorePerKill;
                        temp.Loaded();
                        // Add new asteroid to the game
                        Gamemode.AddEntity(temp);
                    }
                }
            }
            /// <summary>
            /// Causes the asteroid to move in a random direction at a random speed.
            /// </summary>
            public void RandomlyMove()
            {
                // Random rotation and acceleration
                Rotation = Gamemode.RNumber2(0, 360);
                Accelerate(Gamemode.RNumber2(Convert.ToInt32(AsteroidSpeedMin), Convert.ToInt32(AsteroidSpeedMax)));
            }
        #endregion
    }
}