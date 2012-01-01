using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class Control
    {
        // Purpose: a base control, grouping all the similar variables and code used by controls (to save retyping all that code);
        // plus if theres a bug in this code, I only need to modify this control and its fixed for all controls etc.
        #region "Variables"
            // The parent window.
            public UIWindow Parent;
            // The area which holds the location and size of this control.
            public Rectangle Area;
            // The background of the control when its normal.
            public Texture BackgroundNormal;
            // The background of the control when its selected for input.
            public Texture BackgroundSelected;
            // The name of the control.
            public string Name;
            // A boolean to say if the control is currently selected for input.
            public bool Selected = false;
        #endregion

        #region "Functions - virtual"
            public Control(UIWindow parent, string name, Texture backgroundnormal, Texture backgroundselected, int x, int y, int width, int height)
            {
                // Sets variables of control
                Name = name;
                Parent = parent;
                BackgroundNormal = backgroundnormal;
                BackgroundSelected = backgroundselected;
                Area = new Rectangle(x + parent.Area.X, y + parent.Area.Y, width, height);
            }
            /// <summary>
            /// Base will determine if the control is selected!
            /// </summary>
            public virtual void Logic()
            {
                if (Parent.Controls[Parent.SelectedControl] == this)
                {
                    Selected = true;
                }
                else
                {
                    Selected = false;
                }
            }
            /// <summary>
            /// Base will draw the background based on selected!
            /// </summary>
            /// <param name="sb"></param>
            public virtual void Draw(SpriteBatch sb)
            {
                if (Selected)
                {
                    sb.Draw(BackgroundSelected._Texture, Area, Color.White);
                }
                else
                {
                    sb.Draw(BackgroundNormal._Texture, Area, Color.White);
                }
            }
            /// <summary>
            /// Base will destroy background variables!
            /// </summary>
            /// <param name="sb"></param>
            public virtual void Destroy()
            {
                Parent = null;
                Core.Texture.Dispose(BackgroundNormal);
                Core.Texture.Dispose(BackgroundSelected);
            }
            public virtual void Control_OnKeyDown(Keys Key)
            {
                // Called by a UIWindow; this is overriden by other controls with code inside
            }
            public virtual void Control_OnGamepadDown(Buttons Button)
            {
                // Called by a UIWindow; this is overriden by other controls with code inside
            }
        #endregion 
    }
}