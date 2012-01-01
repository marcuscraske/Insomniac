using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Core
{
    public class UIWindow:Window
    {
        // Purpose: a base class for user-interface windows inheriting a window from the window system.
        #region "Events"
            // This event is raised when the selected control changes.
            public delegate void FocusChange();
            public event FocusChange Focus_Changed;
        #endregion

        #region "Variables"
            public Gamee.Main Main;
            // Stores the location and size of the window.
            public Rectangle Area;
            // The windows background texture.
            public Texture Background;
            // If false, the player will not be able to shut the window by pressing e.g. escape.
            public bool AllowClose = false;
            // All the window's controls are stored here.
            public List<Control> Controls = new List<Control>();
            // All the drawn-only controls are stored here.
            public List<Control> NonSelectControls = new List<Control>();
            // The selected index of the control array.
            public int SelectedControl = 0;
        #endregion

        #region "Core"
            public UIWindow(Gamee.Main main, Texture background, int x, int y, int width, int height)
            {
                // Set variables
                Main = main;
                Background = background;
                Area = new Rectangle(x, y, width, height);
            }
            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None, Resolution.Matrix);
                // Draw this window
                spriteBatch.Draw(Background._Texture, Area, Color.White);
                // Draw controls
                foreach (Control C in Controls)
                {
                    C.Draw(spriteBatch);
                }
                foreach (Control C in NonSelectControls)
                {
                    C.Draw(spriteBatch);
                }
                // Draw override
                DrawOverride(spriteBatch);
                spriteBatch.End();
            }
            /// <summary>
            /// Can be used to manually draw items.
            /// </summary>
            public virtual void DrawOverride(SpriteBatch spriteBatch)
            {
            }
            public override void Logic(GameTime gameTime)
            {
                // If theres a selected control and has input
                if (HasInput)
                {
                    Control selectedcontrol = null;
                    if (Controls.Count > 0)
                    {
                        selectedcontrol = (Control)Controls[SelectedControl];
                    }
                    // Keyboard
                    foreach (Keys key in Input.K_PLY1_NEW.GetPressedKeys())
                    {
                        if (Input.IsKeyDown(Input.MENU_KEY_DOWN))
                        {
                            ControlSelectNext();
                        }
                        else if (Input.IsKeyDown(Input.MENU_KEY_UP))
                        {
                            ControlSelectPrevious();
                        }
                        else if (key == Input.MENU_KEY_BACK && AllowClose)
                        {
                            Destroy();
                        }
                        else if (key == Keys.LeftShift || key == Keys.RightShift || Input.IsKeyDown(key))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnKeyDown(key);
                            }
                            UI_KeyDown(key);
                        }
                    }
                    // Gamepad
                    if (Input.G_PLY1_NEW.IsConnected)
                    {
                        if (Input.IsButtonDown(Input.MENU_BUTTON_DOWN))
                        {
                            ControlSelectNext();
                        }
                        else if (Input.IsButtonDown(Input.MENU_BUTTON_UP))
                        {
                            ControlSelectPrevious();
                        }
                        else if (Input.IsButtonDown(Input.MENU_BUTTON_BACK) && AllowClose)
                        {
                            Destroy();
                        }
                        if (Input.IsButtonDown(Buttons.A))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.A);
                            }
                            UI_ButtonDown(Buttons.A);
                        }
                        if (Input.IsButtonDown(Buttons.B))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.B);
                            }
                            UI_ButtonDown(Buttons.B);
                        }
                        if (Input.IsButtonDown(Buttons.Y))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.Y);
                            }
                            UI_ButtonDown(Buttons.Y);
                        }
                        if (Input.IsButtonDown(Buttons.X))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.X);
                            }
                            UI_ButtonDown(Buttons.X);
                        }
                        if (Input.IsButtonDown(Buttons.Start))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.Start);
                            }
                            UI_ButtonDown(Buttons.Start);
                        }
                        if (Input.IsButtonDown(Buttons.Back))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.Back);
                            }
                            UI_ButtonDown(Buttons.Back);
                        }
                        if (Input.IsButtonDown(Buttons.BigButton))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.BigButton);
                            }
                            UI_ButtonDown(Buttons.BigButton);
                        }
                        if (Input.IsButtonDown(Buttons.RightShoulder))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.RightShoulder);
                            }
                            UI_ButtonDown(Buttons.RightShoulder);
                        }
                        if (Input.IsButtonDown(Buttons.RightTrigger))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.RightTrigger);
                            }
                            UI_ButtonDown(Buttons.RightTrigger);
                        }
                        if (Input.IsButtonDown(Buttons.LeftShoulder))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.LeftShoulder);
                            }
                            UI_ButtonDown(Buttons.LeftShoulder);
                        }
                        if(Input.IsButtonDown(Buttons.LeftTrigger))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.LeftTrigger);
                            }
                            UI_ButtonDown(Buttons.LeftTrigger);
                        }
                        if(Input.IsButtonDown(Buttons.RightStick))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.RightStick);
                            }
                            UI_ButtonDown(Buttons.RightStick);
                        }
                        if(Input.IsButtonDown(Buttons.RightThumbstickDown))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.RightThumbstickDown);
                            }
                            UI_ButtonDown(Buttons.RightThumbstickDown);
                        }
                        if(Input.IsButtonDown(Buttons.RightThumbstickLeft))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.RightThumbstickLeft);
                            }
                            UI_ButtonDown(Buttons.RightThumbstickLeft);
                        }
                        if(Input.IsButtonDown(Buttons.RightThumbstickRight))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.RightThumbstickRight);
                            }
                            UI_ButtonDown(Buttons.RightThumbstickRight);
                        }
                        if(Input.IsButtonDown(Buttons.RightThumbstickUp))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.RightThumbstickUp);
                            }
                            UI_ButtonDown(Buttons.RightThumbstickUp);
                        }
                        if(Input.IsButtonDown(Buttons.DPadDown))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.DPadDown);
                            }
                            UI_ButtonDown(Buttons.DPadDown);
                        }
                        if(Input.IsButtonDown(Buttons.DPadLeft))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.DPadLeft);
                            }
                            UI_ButtonDown(Buttons.DPadLeft);
                        }
                        if(Input.IsButtonDown(Buttons.DPadRight))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.DPadRight);
                            }
                            UI_ButtonDown(Buttons.DPadRight);
                        }
                        if(Input.IsButtonDown(Buttons.DPadUp))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.DPadUp);
                            }
                            UI_ButtonDown(Buttons.DPadUp);
                        }
                        if(Input.IsButtonDown(Buttons.LeftStick))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.LeftStick);
                            }
                            UI_ButtonDown(Buttons.LeftStick);
                        }
                        if(Input.IsButtonDown(Buttons.LeftThumbstickDown))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.LeftThumbstickDown);
                            }
                            UI_ButtonDown(Buttons.LeftThumbstickDown);
                        }
                        if(Input.IsButtonDown(Buttons.LeftThumbstickLeft))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.LeftThumbstickLeft);
                            }
                            UI_ButtonDown(Buttons.LeftThumbstickLeft);
                        }
                        if(Input.IsButtonDown(Buttons.LeftThumbstickRight))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.LeftThumbstickRight);
                            }
                            UI_ButtonDown(Buttons.LeftThumbstickRight);
                        }
                        if(Input.IsButtonDown(Buttons.LeftThumbstickUp))
                        {
                            if (selectedcontrol != null)
                            {
                                selectedcontrol.Control_OnGamepadDown(Buttons.LeftThumbstickUp);
                            }
                            UI_ButtonDown(Buttons.LeftThumbstickUp);
                        }
                    } 
                }
                // Execute each controls logic
                foreach (Control C in Controls)
                {
                    C.Logic();
                }
                foreach (Control C in NonSelectControls)
                {
                    C.Logic();
                }
            }
            public virtual void UI_KeyDown(Keys key)
            {
                // A virtual function to be overriden by an inherting-window.
            }
            public virtual void UI_ButtonDown(Buttons button)
            {
                // A virtual function to be overriden by an inherting-window.
            }
            public override void Destroying()
            {
                // Destroy controls
                foreach (Control C in Controls)
                {
                    C.Destroy();
                }
                foreach (Control C in NonSelectControls)
                {
                    C.Destroy();
                }
                // Destroy background
                Core.Texture.Dispose(Background);
            }
        #endregion

        #region "Functions - Controls"
            /// <summary>
            /// Sets a control as the selected control.
            /// </summary>
            /// <param name="new_selected"></param>
            public void SetSelected(Control new_selected)
            {
                if (Controls.IndexOf(new_selected) != -1)
                {
                    SelectedControl = Controls.IndexOf(new_selected);
                }
            }
            /// <summary>
            /// Sets the next control (in terms of index) as selected.
            /// </summary>
            public void ControlSelectPrevious()
            {
                if (SelectedControl - 1 < 0)
                {
                    SelectedControl = Controls.Count - 1;
                }
                else
                {
                    SelectedControl -= 1;
                }
                if (Focus_Changed != null)
                {
                    Focus_Changed();
                }
            }
            /// <summary>
            /// Sets the previous control (in terms of index) as selected.
            /// </summary>
            public void ControlSelectNext()
            {
                if (SelectedControl + 1 >= Controls.Count)
                {
                    SelectedControl = 0;
                }
                else
                {
                    SelectedControl += 1;
                }
                if (Focus_Changed != null)
                {
                    Focus_Changed();
                }
            }
            /// <summary>
            /// Gets a control by its name from the controls array.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public Control GetByName(string name)
            {
                foreach (Control C in Controls)
                {
                    if (C.Name == name)
                    {
                        return C;
                    }
                }
                return null;
            }
        #endregion 
    }
}