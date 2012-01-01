using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class Spawn
    {
        // Purpose: holds spawn information and respawns an entity at a spawn
        #region "Variables"
            // Spawn position
            public Vector2 position = Vector2.Zero;
            // Spawn rotation
            public float rotation = 0;
            // Spawn launch speed
            public float launch = 0.0F;
        #endregion

        #region "Core"
                public Spawn(int pos_x, int pos_y, int pos_rotation, float launch_speed)
                {
                    launch = launch_speed;
                    position = new Vector2(pos_x, pos_y);
                    rotation = pos_rotation;
                }
            #endregion

        #region "Functions - spawning"
            public static void SpawnEntity(Entity ent, Vector2 position, float rotation, float launch_speed)
            {
                // Clear the entities velocity
                ent.Velocity = Vector2.Zero;
                // Accelerate the entity
                if (launch_speed != 0.0F)
                {
                    ent.Accelerate2(launch_speed, rotation);
                }
                // Set camera position
                if (ent.Player != null)
                {
                    ent.Player.Camera.SetPosition(position.X, position.Y);
                }
                // Set position, rotation etc
                ent.Position = position;
                ent.Rotation = rotation;
                ent.Alive = true;
                ent.Health = ent.MaxHealth;
                // Raise respawn event
                ent.Respawned(position);
            }
        #endregion
    }
}