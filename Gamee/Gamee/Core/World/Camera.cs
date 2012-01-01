using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class Camera
    {
        // Purpose: manages the viewport and rendering of a player with capabilities such as zoom and rotation.
        #region "Variables"
            // The current game.
            public Gamemode Gamemode;
            // The entity the camera is following.
            public Entity AttachedEntity = null;
            // The position of the camera.
            public Vector2 Position = Vector2.Zero;
            // The matrix used for rendering with the cameras settings such as zoom and rotation.
            public Matrix CameraTransformation = new Matrix();
            // The area of where the camera is drawn.
            public Viewport Viewport = new Viewport();
        #endregion

        #region "Variables - Zoom"
            // Used to store the zoom; this is strictly controlled to avoid game zoom issues.
            private float zoom = 1.0F;
            /// <summary>
            /// Sets zoom of the game with limits in-place.
            /// </summary>
            public float Zoom
            {
                get
                {
                    return zoom;
                }
                set
                {
                    if (value < 0.02F)
                    {
                        // The maximum zoom-out value
                        zoom = 0.02F;
                    }
                    else if (value > 290F)
                    {
                        // The maximum zoom-in level
                        zoom = 290.0F;
                    }
                    else
                    {
                        zoom = value;
                    }
                    RebuildMatrix();
                }
            }
        #endregion

        #region "Variables - Rotation"
            // Like zoom, this is also strictly controlled to ensure it's done correctly to avoid issues.
            private float rotation = 0.0F;
            /// <summary>
            /// Sets the rotation of the game.
            /// </summary>
            public float Rotation
            {
                get
                {
                    return MathHelper.ToDegrees(rotation);
                }
                set
                {
                    rotation = MathHelper.ToRadians(value);
                    RebuildMatrix();
                }
            }
        #endregion

        #region "Core"
            public Camera(Gamemode _gamemode)
            {
                // Set gamemode
                Gamemode = _gamemode;
                // Rebuild viewing matrice
                RebuildMatrix();
            }
        #endregion

        #region "Functions - Matrices"
            /// <summary>
            /// Rebuilds the transformation matrix used for scaling,
            /// zooming and rotation.
            /// </summary>
            public void RebuildMatrix()
            {
                // Rebuilds the actual matrix
                CameraTransformation =
                Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) * // Sets the position of the camera
                Matrix.CreateRotationZ(rotation) * // Sets rotation
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) * // Sets zoom
                Matrix.CreateTranslation(new Vector3(Viewport.Width * 0.5F * (Resolution.RenderResolution.X / Resolution.GameResolution.X), Viewport.Height * 0.5F * (Resolution.RenderResolution.Y / Resolution.GameResolution.Y), 0)); // Sets resolution
            }
        #endregion

        #region "Functions - Render Settings"
            /// <summary>
            /// Only resets the view of the camera.
            /// </summary>
            public void ResetView()
            {
                zoom = 1.0F;
                rotation = 0.0F;
            }
            /// <summary>
            /// Resets the cameras position.
            /// </summary>
            public void ResetCamera()
            {
                RemoveFromEntity();
                Position = new Vector2(1, 1);
                SetPosition(0, 0);
                Zoom = 1.0F;
                rotation = 0.0F;
            }
            /// <summary>
            /// Sets the position of the camera (will be centre of camera).
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void SetPosition(float x, float y)
            {
                // Check the position has changed
                if (Position.X != x || Position.Y != y)
                {
                    float t1 = Viewport.Width * 0.5F * (Resolution.RenderResolution.X / Resolution.GameResolution.X);
                    float t2 = Viewport.Height * 0.5F * (Resolution.RenderResolution.Y / Resolution.GameResolution.Y);
                    // Check its not outside the map bounds
                    if (x < t1)
                    {
                        x = t1;
                    }
                    else if (x > Gamemode._Map.Info.ActualTileWidth - t1)
                    {
                        x = Gamemode._Map.Info.ActualTileWidth - t1;
                    }
                    if (y < t2)
                    {
                        y = t2;
                    }
                    else if (y > Gamemode._Map.Info.ActualTileHeight - t2)
                    {
                        y = Gamemode._Map.Info.ActualTileHeight - t2;
                    }
                    // Update the position
                    Position = new Vector2(x, y);
                    // Rebuild matrix
                    RebuildMatrix();
                }
            }
        #endregion

        #region "Functions - Follow Entity"
            /// <summary>
            /// Attaches the camera to an entity.
            /// </summary>
            /// <param name="ent"></param>
            public void AttachToEntity(Entity ent)
            {
                // Remove a previous chase if it exists
                RemoveFromEntity();
                // Entity moves
                ent.ENT_Moved += new Entity.Ev_Moved(ParentMoved);
                // Entity respawned
                ent.ENT_Respawned += new Entity.Ev_Respawned(ent_ENT_Respawned);
                // Set attached ent variable
                AttachedEntity = ent;
            }

            void ent_ENT_Respawned(Vector2 position)
            {
                SetPosition(position.X, position.Y);
            }
            // Called when the parent entity moves.
            void ParentMoved(Entity ent, Vector2 OldPosition, Vector2 NewPosition)
            {
                SetPosition(NewPosition.X + (ent.Width / 2), NewPosition.Y + (ent.Height / 2));
            }
            /// <summary>
            /// Removes the lock-on by the camera onto an entity.
            /// </summary>
            public void RemoveFromEntity()
            {
                if (AttachedEntity != null)
                {
                    // Catch when the entity moves so the camera can follow
                    AttachedEntity.ENT_Moved -= new Entity.Ev_Moved(ParentMoved);
                    AttachedEntity.ENT_Respawned -= new Entity.Ev_Respawned(ent_ENT_Respawned); 
                }
            }
        #endregion
    }
}