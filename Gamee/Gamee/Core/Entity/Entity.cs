using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class Entity
    {
        // Purpose: an entity is an object in the game such as a ship etc with a sprite (a texture), velocity and more.

        #region "Events - functions & delegates"
            // Entity Destroyed
            public delegate void Ev_Destroyed(Entity ent);
            public event Ev_Destroyed ENT_Destroyed;
            // Entity killed
            public delegate void Ev_Killed(Entity ent);
            public event Ev_Killed ENT_Killed;
            // Entity damage
            public delegate void Ev_Damage(Entity ent, float amount);
            public event Ev_Damage ENT_Damage;
            // Entity (recieves) health
            public delegate void Ev_Health(Entity ent, float amount);
            public event Ev_Health ENT_Health;
            // Entity moved
            public delegate void Ev_Moved(Entity ent, Vector2 OldPosition, Vector2 NewPosition);
            public event Ev_Moved ENT_Moved;
            // Entity rotated
            public delegate void Ev_Rotated(Entity ent, float OldAngle, float NewAngle);
            public event Ev_Rotated ENT_Rotated;
            // Entity collided
            public delegate void Ev_Collided(Entity Victim, Entity Collider);
            public event Ev_Collided ENT_Collided;
            // Entity respawned
            public delegate void Ev_Respawned(Vector2 position);
            public event Ev_Respawned ENT_Respawned;
            // Entity loaded (for e.g. from map placement or game restart)
            public delegate void Ev_Loaded();
            public event Ev_Loaded ENT_Loaded;
            /// <summary>
            /// Causes a collision.
            /// </summary>
            /// <param name="Victim"></param>
            /// <param name="Collider"></param>
            public void CauseCollision(Entity Victim, Entity Collider)
            {
                if (ENT_Collided != null)
                {
                    ENT_Collided(Victim, Collider);
                }
            }
            /// <summary>
            /// Private function for raising damage.
            /// </summary>
            /// <param name="num"></param>
            void RecieveDamage(float num)
            {
                ENT_Damage(this, num);
            }
            /// <summary>
            /// Private function for raising health gain.
            /// </summary>
            /// <param name="num"></param>
            void RecieveHealth(float num)
            {
                ENT_Health(this, num);
            }
            /// <summary>
            /// Raises the respawned event.
            /// </summary>
            /// <param name="pos"></param>
            public void Respawned(Vector2 pos)
            {
                if (ENT_Respawned != null)
                {
                    ENT_Respawned(pos);
                }    
            }
            /// <summary>
            ///  Raises that the entity has been loaded.
            /// </summary>
            public void Loaded()
            {
                if (ENT_Loaded != null)
                {
                    ENT_Loaded();
                }
            }
        #endregion

        #region "Variables"
            // Gamemode class
            public Gamemode Gamemode;
            //ID
            public string ID = "";
            // Player
            public Player Player;
            // Display name (for stats)
            public string DisplayName = "Base Entity";
            // Attackable
            public bool Attackable = true;
            // Locked-on
            public bool LockedOn = false;
            // Enemy
            public bool Enemy = false;
            // Texture
            public Texture Texture;
            // The position of the entity.
            public Vector2 Position = Vector2.Zero;
            // Velocity of the entity
            public Vector2 Velocity = Vector2.Zero;
            // Max speed of the entity
            public float MaxSpeed = 5.0F;
            // Physics properties
            public float PhyMass = 100.0F;
            public bool PhySolid = true;
            public bool PhyStill = false;
            // Size
            public int Width = 1;
            public int Height = 1;
            // The rotation of the entity.
            public float rotation = 0.0F;
            public float Rotation
            {
                get
                {
                    return MathHelper.ToDegrees(rotation);
                }
                set
                {
                    rotation = MathHelper.ToRadians(value);
                }
            }
            // Centre of the entity (without position)
            public Vector2 Centre = Vector2.Zero;
            public Vector2 SpawnPosition = Vector2.Zero;
            public float SpawnDelay = 0.0F;
            public float SpawnDelayTemp = 0.0F;
        #endregion

        #region "Logic"
            /// <summary>
            /// Executed every time the main class does an update.
            /// </summary>
            public virtual void Logic()
            {
                // If the entity has no life just abort
                if (!Alive)
                {
                    return;
                }
                if (Player != null)
                {
                    Player.PlayerEntityLogic();
                }
                // Increment the sprite frame
                if (Texture != null)
                {
                    Texture.Logic();
                }
                // Applied velocity
                Velocity += Gamemode._Map.Info.AppliedVelocity;
                // Force multiplier
                Velocity *= Gamemode._Map.Info.ForceMultiplier;
                // Set the new position
                if (Velocity != Vector2.Zero)
                {
                    Move(Position + Velocity);
                }   
            }
        #endregion

        #region "Core"
            // Called when the object is being made
            public Entity(Gamemode _gamemode, Texture _texture, int _width, int _height, bool _enemy)
            {
                // Set the main obj
                Gamemode = _gamemode;
                // Set the sprite variables
                Texture = _texture;
                SetSize(_width, _height);
                Enemy = _enemy;
            }
            /// <summary>
            /// Called when the entity is being disposed, causing all the sub-classes to be disposed to free memory.
            /// </summary>
            public virtual void Dispose()
            {
                Gamemode = null;
                Player = null;
                Core.Texture.Dispose(Texture);
            }
        #endregion

        #region "Rendering"
            /// <summary>
            /// Draws the entity, but only if its alive.
            /// </summary>
            /// <param name="spriteBatch"></param>
            public virtual void Draw(SpriteBatch spriteBatch)
            {
                if (Alive)
                {
                    spriteBatch.Draw(Texture._Texture, new Rectangle((int)(Position.X + Centre.X), (int)(Position.Y + Centre.Y), Width, Height), null, Color.White, rotation, Texture.Centre, SpriteEffects.None, 0.0F);
                }
            }
        #endregion

        #region "Functions - movement"
            /// <summary>
            /// Rotates the entity using degrees.
            /// </summary>
            /// <param name="angle"></param>
            public void Rotate(float angle)
            {
                // Ensure the rotation is 0 to 259
                if (angle == 360)
                {
                    angle = 0;
                }
                else if (angle > 360)
                {
                    angle = ((angle / 360) - (angle / 360)) * 360;
                }
                // Old rotation
                float oldangle = Rotation;
                // Set new
                Rotation = angle;
                // Raise event
                if (ENT_Rotated != null)
                {
                    ENT_Rotated(this, oldangle, angle);
                }
            }
            /// <summary>
            /// Moves the entity correctly; caution of loops that can occur from placing an entity in an object spawning
            /// in the same position!
            /// </summary>
            /// <param name="NewPosition"></param>
            public void Move(Vector2 NewPosition)
            {
                // If they're a still object, return (since they cant move)
                if (PhyStill)
                {
                    return;
                }
                // Old position for event
                Vector2 OldPosition = Position;
                // Check the entity is not outside the map
                BoundsCheck();
                // Set new position
                Position = NewPosition;
                // Raises an event that the entity has moved
                if (ENT_Moved != null)
                {
                    ENT_Moved(this, OldPosition, Position);
                }
                // Check for collisions
                Gamemode._Physics.CheckCollisions(this);
            }
            /// <summary>
            /// Bounds the entity within the map.
            /// </summary>
            public void BoundsCheck()
            {
                Map map = Gamemode._Map;
                // Check X
                if (Position.X < 0)
                {
                    Velocity.X = -Velocity.X;
                    Position.X = 0;
                }
                if (Position.X > map.Info.ActualTileWidth - Width)
                {
                    Velocity.X = -Velocity.X;
                    Position.X = map.Info.ActualTileWidth - Width;
                }
                // Check Y
                if (Position.Y < 0)
                {
                    Velocity.Y = -Velocity.Y;
                    Position.Y = 0;
                }
                if (Position.Y > map.Info.ActualTileHeight - Height)
                {
                    Velocity.Y = -Velocity.Y;
                    Position.Y = map.Info.ActualTileHeight - Height;
                }
            }
            /// <summary>
            /// Accelerates the entity by the pixel-per-a-cycle (ppc) specified; uses degrees.
            /// </summary>
            /// <param name="amount"></param>
            public void Accelerate(float amount)
            {
                // Accelerate the entity at the current angle
                Velocity.X += amount * (float)Math.Sin(2.0 * Math.PI * (Rotation) / 360.0);
                Velocity.Y += amount * -(float)Math.Cos(2.0 * Math.PI * (Rotation) / 360.0);
                // Ensure the speed isn't already exceeding
                if (Velocity.Length() > MaxSpeed)
                {
                    Velocity.Normalize();
                    Velocity *= MaxSpeed;
                }
            }
            /// <summary>
            /// Accelerates the entity at a custom angle (degrees).
            /// </summary>
            /// <param name="amount"></param>
            public void Accelerate2(float amount, float angle)
            {
                // Accelerate the entity at the current angle
                Velocity.X += amount * (float)Math.Sin(2.0 * Math.PI * (angle) / 360.0);
                Velocity.Y += amount * -(float)Math.Cos(2.0 * Math.PI * (angle) / 360.0);
                // Ensure the speed isn't already exceeding
                if (Velocity.Length() > MaxSpeed)
                {
                    Velocity.Normalize();
                    Velocity *= MaxSpeed;
                }
            }
            /// <summary>
            /// Accelerates the entity at a custom angle (radians).
            /// </summary>
            /// <param name="amount"></param>
            public void Accelerate3(float amount, float radians)
            {
                // Convert to degrees
                radians = MathHelper.ToDegrees(radians);
                // Use Accelerate2
                Accelerate2(amount, radians);
            }
        #endregion

        #region "Functions - modifications"
            /// <summary>
            /// Sets the internal variables regarding the size and centre of the entity.
            /// </summary>
            /// <param name="width"></param>
            /// <param name="height"></param>
            public void SetSize(int width, int height)
            {
                // Sets width and height of entity
                Width = width;
                Height = height;
                // Sets centre point - used for calculations e.g. ents in radius etc
                Centre = new Vector2(width / 2, height / 2);
            }
        #endregion

        #region "Health and Destruction"
            #region "Variables"
                // Specifies if the entity is alive or dead
                public bool Alive = true;
                // Specifies if the entity has godmode and should not die
                public bool Godmode = false;
                // Specifies if the entity should respawn or not
                public bool Destroyable = false;
                // Specifies the max health of the entity
                public float MaxHealth = 100.0F;
                // Specifies the current health of the entity
                public float Health = 100.0F;
                // Specifies the max lives of the entity
                public float MaxLives = 3.0F;
                // Specifies the current number of lives the entity has
                public float Lives = 3.0F;
                // Specifies if the entity should use lives or not
                public bool UseLives = false;
                // Specifies if an entity who kills this entity gains score
                public bool CountAsKill = true;
            #endregion
            /// <summary>
            /// Gives the entity health, which can also be negative (damage).
            /// </summary>
            /// <param name="amount"></param>
            /// <param name="inflicter"></param>
            public void GiveHealth(float amount, Entity inflicter)
            {
                // If the entity has godmode, just exit.
                if (Godmode || !Alive)
                {
                    return;
                }
                if (amount > 0.0F)
                {
                    // Entity is recieving health
                    Health += amount;
                    // Raise recieving health event
                    if (ENT_Health != null)
                    {
                        ENT_Health(this, amount);
                    }
                }
                else
                {
                    // Entity is recieving damage
                    Health += amount;
                    // Raise recieving damage event
                    if (ENT_Damage != null)
                    {
                        ENT_Damage(this, amount);
                    }
                    if (Health <= 0.0F)
                    {
                        // The entity has been killed, raise the event and destroy it
                        if (inflicter != null)
                        {
                            Gamemode._Event_ENT_Killed(this, inflicter);
                        }
                        Destroy();
                    }
                }
            }
            /// <summary>
            /// Destroys the entity safely and correctly.
            /// </summary>
            public virtual void Destroy()
            {
                // If the entity has godmode, just exit since it cannot die
                if (Godmode || !Alive)
                {
                    return;
                }
                // Set the entity has dead
                Alive = false;
                // Remove a life if it uses lives
                if (UseLives)
                {
                    Lives -= 1;
                }
                if (!Destroyable && (Lives > 0 || !UseLives))
                {
                    // Entity cannot be destroyed, instead it will be respawned
                    Gamemode.RespawnEnt(this);
                    if (ENT_Killed != null)
                    {
                        ENT_Killed(this);
                    }
                }
                else if (Destroyable)
                {
                    // Entity has been destroyed, remove it from the game permanently
                    Gamemode._RemoveEnts.Add(this);
                    if (ENT_Destroyed != null)
                    {
                        ENT_Destroyed(this);
                    }
                }
                else if (Player != null)
                {
                    // If the player exists, theres no lives remaining and the ent is undestroyable
                    // hence the player is then dead/game over :(
                    if (Player.Entity == this)
                    {
                        Player.Dead = true;
                        Gamemode._Event_Player_Died(Player);
                    }
                }
            }
        #endregion

        #region "Resetting the entity - also virtual"
            /// <summary>
            /// Resets an entity; allows inheriting entities to add custom code.
            /// </summary>
            public virtual void ResetEntity()
            {
                // Reset speed
                Velocity = Vector2.Zero;
                // Reset position
                Position = Vector2.Zero;
                // Reset rotation
                Rotation = 0;
                // Max the health
                Health = MaxHealth;
                // Max the lives
                Lives = MaxLives;
                // Disengage lock-on
                LockedOn = false;
            }
        #endregion

        #region "Score"
            /// <summary>
            /// If the entity is a player, they recieve score; this method is used since sub-classing is not
            /// possible.
            /// </summary>
            /// <param name="amount"></param>
            public void AddScore(float amount)
            {
                if (Player != null)
                {
                    Player.Score.SCORE += amount;
                }
            }
        #endregion
    }
}