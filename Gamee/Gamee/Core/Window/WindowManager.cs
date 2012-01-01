using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class WindowManager
    {
        // Purpose: to handle the windows system.
        #region "Variables"
            // An array of active windows.
            public List<Window> Windows = new List<Window>();
            // An array of windows to be added - to avoid system.collection modified.
            public List<Window> Add = new List<Window>();
            // An array of windows to be removed - to avoid system.collection modified.
            public List<Window> Remove = new List<Window>();
        #endregion

        #region "Core"
            /// <summary>
            /// Draws each window in the collection.
            /// </summary>
            /// <param name="spriteBatch"></param>
            public void Draw(SpriteBatch spriteBatch)
            {
                foreach (Window win in Windows)
                {
                    if (win.State == Window.WindowState.Active)
                    {
                        win.Draw(spriteBatch);
                    }
                }
            }
            /// <summary>
            /// Runs the window manager logic for removal and input etc of windows.
            /// </summary>
            /// <param name="gameTime"></param>
            public virtual void Logic(GameTime gameTime)
            {
                if (Add.Count != 0)
                {
                    // Add any new windows
                    foreach (Window win in Add)
                    {
                        Windows.Add(win);
                    }
                    Add.Clear();
                }
                // Remove finished windows and reset input
                foreach (Window win in Windows)
                {
                    if (win.State == Window.WindowState.Finished)
                    {
                        Remove.Add(win);
                    }
                    else
                    {
                        win.HasInput = false;
                    }
                }
                // Remove the finished windows
                foreach (Window win in Remove)
                {
                    win.Destroying();
                    Windows.Remove(win);
                    GC.SuppressFinalize(win);
                }
                Remove.Clear();
                if (Windows.Count != 0)
                {
                    // Set the window thats of highest index
                    Windows[Windows.Count - 1].HasInput = true;
                    // Run each windows logic
                    foreach (Window win in Windows)
                    {
                        win.Logic(gameTime);
                    }
                }
            }
        #endregion

        #region "Functions"
            /// <summary>
            /// Adds a new window safely and removes all other windows.
            /// </summary>
            /// <param name="window"></param>
            public void AddWindow2(Window window)
            {
                // Remove existing windows
                RemoveAll();
                // Add window
                Add.Add(window);
            }
            /// <summary>
            /// Adds a window to the window manager safely.
            /// </summary>
            /// <param name="Window"></param>
            public void AddWindow(Window Window)
            {
                Add.Add(Window);
            }
            /// <summary>
            /// Removes all the windows from the collection.
            /// </summary>
            public void RemoveAll()
            {
                foreach (Window win in Windows)
                {
                    Remove.Add(win);
                }
            }
        #endregion
    }
}