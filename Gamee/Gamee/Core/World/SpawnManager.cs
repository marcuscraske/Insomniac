using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class SpawnManager
    {
        // Purpose: handles the (re)spawning of entities.
        #region "Variables"
            Gamemode Gamemode;
            // Array of friendly and enemy spawns.
            public List<Spawn> FriendlySpawns = new List<Spawn>();
            public List<Spawn> EnemySpawns = new List<Spawn>();
            // Array of entities that have been respawned.
            public List<Entity> Respawned = new List<Entity>();
            // Array of entities awaiting respawn
            public List<Entity> WaitingRespawn = new List<Entity>();
            // Array of entities awaiting to be placed into the awaiting respawn
            // array - this is to avoid system.collection modification error.
            public List<Entity> WaitingWaitingRespawn = new List<Entity>();
        #endregion

        #region "Core"
            public SpawnManager(Gamemode gm)
            {
                Gamemode = gm;
            }
            public void Logic()
            {
                // Safely move new entities wanting to respawn to a waiting list
                // to avoid an error if an entity spawns and dies
                foreach (Entity ent in WaitingWaitingRespawn)
                {
                    WaitingRespawn.Add(ent);
                }
                WaitingWaitingRespawn.Clear();
                // Loop all the entities waiting respawn and respawn them
                foreach(Entity ent in WaitingRespawn)
                {
                    if (ent.SpawnDelay == 0.0F || Environment.TickCount - ent.SpawnDelayTemp > ent.SpawnDelay)
                    {
                        if (ent.SpawnPosition != Vector2.Zero)
                        {
                            // Forces a respawn at the original load/spawn position
                            Spawn.SpawnEntity(ent, ent.SpawnPosition, ent.rotation, 0.0F);
                            Respawned.Add(ent);
                        }
                        else if (ent.Enemy)
                        {
                            foreach (Spawn spawn in EnemySpawns)
                            {
                                // Attempts a respawn at an enemy spawn
                                if (AttemptSpawn(spawn, ent))
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (Spawn spawn in FriendlySpawns)
                            {
                                // Attempts a respawn at a friendly spawn
                                if (AttemptSpawn(spawn, ent))
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                // Clear all the entities that have respawned from the spawn manager
                foreach (Entity ent in Respawned)
                {
                    WaitingRespawn.Remove(ent);
                }
                Respawned.Clear();
            }
        #endregion

        #region "Functions"
            /// <summary>
            /// Respawns an entity safely at a spawn (recommended method).
            /// </summary>
            /// <param name="ent"></param>
            public void RespawnEnt(Core.Entity ent)
            {
                // Set a variable used to calculate the spawn delay
                ent.SpawnDelayTemp = Environment.TickCount;
                // Add the entity to be respawned
                WaitingWaitingRespawn.Add(ent);
            }
            /// <summary>
            /// *CAUTION*: This is an internal function! However this can be used externally but it's not recommended!
            /// This attempts to spawn an object at spawn and returns a boolean if successful.
            /// </summary>
            /// <param name="spawn"></param>
            /// <param name="ent"></param>
            /// <returns></returns>
            public bool AttemptSpawn(Spawn spawn, Entity ent)
            {
                bool canspawn = true;
                foreach (Entity entity in Gamemode._Physics.EntsInRadius(spawn.position, Common.Max(ent.Width, ent.Height)))
                {
                    if (entity.Alive && !entity.PhyStill)
                    {
                        canspawn = false;
                        entity.Velocity = Vector2.Zero;
                        entity.Accelerate2(spawn.launch, spawn.rotation);
                    }
                }
                if (canspawn)
                {
                    Spawn.SpawnEntity(ent, spawn.position, spawn.rotation, spawn.launch);
                    Respawned.Add(ent);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        #endregion
    }
}