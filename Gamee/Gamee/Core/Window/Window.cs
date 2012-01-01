using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class Window
    {
        // Purpose: a window for a window-based system; this class is not meant to be used directly but rather
        // inherited by another class.
        #region "Variables & Enums"
            /// <summary>
            /// If the window is topmost, it will have input.
            /// </summary>
            public bool HasInput = false;
            // The current state of the window.
            public WindowState State = WindowState.Active;
            // The possible window states.
            public enum WindowState
            {
                Active = 0,
                Hidden = 1,
                Finished = 2,
            }
        #endregion
        
        /// <summary>
        /// Draws the window.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // A virtual function to be overriden by an inherting-window.
        }
        /// <summary>
        /// Runs the windows logic.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Logic(GameTime gameTime)
        {
            // A virtual function to be overriden by an inherting-window.
        }
        /// <summary>
        /// Destroys the window.
        /// </summary>
        public void Destroy()
        {
            State = WindowState.Finished;
        }
        /// <summary>
        /// Called when a window is being destroyed.
        /// </summary>
        public virtual void Destroying()
        {
            // A virtual function to be overriden by an inherting-window.
        }
    }
}