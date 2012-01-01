using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public static class Input
    {
        // Purpose: handles game input and contains a list of variables of globally shared keys for the game (standardizing controls).

        #region "Variables"
            // Specifies if gamepad 1 is to be gamepad 2 (rather than gamepad 1)
            public static bool UseGamepadPly1ForPly2 = false;
            // The rest of these variables store previous and current keyboard and gamepad state(s).
            public static KeyboardState K_PLY1_NEW;
            public static KeyboardState K_PLY1_OLD;
            public static GamePadState G_PLY1_NEW;
            public static GamePadState G_PLY1_OLD;
            public static GamePadState G_PLY2_NEW;
            public static GamePadState G_PLY2_OLD;
            public static GamePadState G_PLY3_NEW;
            public static GamePadState G_PLY3_OLD;
            public static GamePadState G_PLY4_NEW;
            public static GamePadState G_PLY4_OLD;
        #endregion

        #region "Functions - Gamepad vibration"
            // Stores when vibration for player controllers end (these will be set a tick-count
            // for when the vibration ends.
            private static float V_PLY1_END = 0.0F;
            private static float V_PLY2_END = 0.0F;
            private static float V_PLY3_END = 0.0F;
            private static float V_PLY4_END = 0.0F;
            /// <summary>
            /// Causes the controller to vibrate; rates are 0.0 to 1.0.
            /// </summary>
            /// <param name="lrate"></param>
            /// <param name="rrate"></param>
            /// <param name="milliseconds"></param>
            public static void Vibrate(PlayerIndex ply, float left_rate, float right_rate, float milliseconds)
            {
                GamePad.SetVibration(ply, left_rate, right_rate);
                if (ply == PlayerIndex.One)
                {
                    V_PLY1_END = Environment.TickCount + milliseconds;
                }
                else if (ply == PlayerIndex.Two)
                {
                    V_PLY2_END = Environment.TickCount + milliseconds;
                }
                else if (ply == PlayerIndex.Three)
                {
                    V_PLY3_END = Environment.TickCount + milliseconds;
                }
                else if (ply == PlayerIndex.Four)
                {
                    V_PLY4_END = Environment.TickCount + milliseconds;
                }
                else
                {
                    throw new Exception("Invalid PlayerIndex (not 1-4) selected?");
                }
            }
        #endregion

        #region "Functions - Logic"
            public static void Logic()
            {
                // Set the last inputs as the previous rather than latest inpuits
                K_PLY1_OLD = K_PLY1_NEW;
                G_PLY1_OLD = G_PLY1_NEW;
                G_PLY2_OLD = G_PLY2_NEW;
                G_PLY3_OLD = G_PLY3_NEW;
                G_PLY4_OLD = G_PLY4_NEW;
                // Update new input devices
                K_PLY1_NEW = Keyboard.GetState(PlayerIndex.One);
                G_PLY1_NEW = GamePad.GetState(PlayerIndex.One);
                G_PLY2_NEW = GamePad.GetState(PlayerIndex.Two);
                G_PLY3_NEW = GamePad.GetState(PlayerIndex.Three);
                G_PLY4_NEW = GamePad.GetState(PlayerIndex.Four);
                // Check gamepad vibration
                if (V_PLY1_END != 0.0F && Environment.TickCount > V_PLY1_END)
                {
                    GamePad.SetVibration(PlayerIndex.One, 0.0F, 0.0F);
                    V_PLY1_END = 0.0F;
                }
                if (V_PLY2_END != 0.0F && Environment.TickCount > V_PLY2_END)
                {
                    GamePad.SetVibration(PlayerIndex.Two, 0.0F, 0.0F);
                    V_PLY2_END = 0.0F;
                }
                if (V_PLY3_END != 0.0F && Environment.TickCount > V_PLY3_END)
                {
                    GamePad.SetVibration(PlayerIndex.Three, 0.0F, 0.0F);
                    V_PLY3_END = 0.0F;
                }
                if (V_PLY4_END != 0.0F && Environment.TickCount > V_PLY4_END)
                {
                    GamePad.SetVibration(PlayerIndex.Four, 0.0F, 0.0F);
                    V_PLY4_END = 0.0F;
                }
            }
        #endregion

        #region "Functions - keys/buttons held down universal/multiplayer"
            /// <summary>
            /// Checks if a gamepad button has been pressed (with input currently down but wasnt previous).
            /// </summary>
            /// <param name="ply"></param>
            /// <param name="button"></param>
            public static bool Gamepad_pressed(PlayerIndex ply, Buttons button)
            {
                switch (ply)
                {
                    case PlayerIndex.One:
                    {
                        if (UseGamepadPly1ForPly2)
                        {
                            return false;
                        }
                        else
                        {
                            return (G_PLY1_NEW.IsButtonDown(button) && !G_PLY1_OLD.IsButtonDown(button));
                        }
                    }
                    case PlayerIndex.Two:
                    {
                        if (UseGamepadPly1ForPly2)
                        {
                            return G_PLY1_NEW.IsConnected && (G_PLY1_NEW.IsButtonDown(button) && !G_PLY1_OLD.IsButtonDown(button));
                        }
                        else
                        {
                            return G_PLY2_NEW.IsConnected && (G_PLY2_NEW.IsButtonDown(button) && !G_PLY2_OLD.IsButtonDown(button));
                        }
                    }
                    case PlayerIndex.Three:
                    {
                        if (UseGamepadPly1ForPly2)
                        {
                            return G_PLY2_NEW.IsConnected && (G_PLY2_NEW.IsButtonDown(button) && !G_PLY2_OLD.IsButtonDown(button));
                        }
                        else
                        {
                            return G_PLY3_NEW.IsConnected && (G_PLY3_NEW.IsButtonDown(button) && !G_PLY3_OLD.IsButtonDown(button));
                        }
                    }
                    case PlayerIndex.Four:
                    {
                        if(UseGamepadPly1ForPly2)
                        {
                            return G_PLY3_NEW.IsConnected && (G_PLY3_NEW.IsButtonDown(button) && !G_PLY4_OLD.IsButtonDown(button));
                        }
                        else
                        {
                            return G_PLY4_NEW.IsConnected && (G_PLY4_NEW.IsButtonDown(button) && !G_PLY4_OLD.IsButtonDown(button));
                        }
                    }
                }
                return false;
            }
            /// <summary>
            /// Checks if a key has been pressed (currently down but previously not).
            /// Note: this has been disabled for more than one player, XNA doesnt support multiple keyboards.
            /// </summary>
            /// <param name="ply"></param>
            /// <param name="key"></param>
            public static bool Keyboard_pressed(PlayerIndex ply, Keys key)
            {
                switch (ply)
                {
                    case PlayerIndex.One:
                    {
                        return (K_PLY1_NEW.IsKeyDown(key) && !K_PLY1_OLD.IsKeyDown(key));
                    }
                    case PlayerIndex.Two:
                    {
                        return false;
                    }
                    case PlayerIndex.Three:
                    {
                        return false;
                    }
                    case PlayerIndex.Four:
                    {
                        return false;
                    }
                }
                return false;
            }
            /// <summary>
            /// Checks if a gamepad button is currently held down.
            /// </summary>
            /// <param name="ply"></param>
            /// <param name="button"></param>
            public static bool Gamepad_isdown(PlayerIndex ply, Buttons button)
            {
                switch (ply)
                {
                    case PlayerIndex.One:
                    {
                        if (UseGamepadPly1ForPly2)
                        {
                            return false;
                        }
                        else
                        {
                            return G_PLY1_NEW.IsButtonDown(button);
                        }
                    }
                    case PlayerIndex.Two:
                    {
                        if (UseGamepadPly1ForPly2)
                        {
                            return G_PLY1_NEW.IsButtonDown(button);
                        }
                        else
                        {
                            return G_PLY2_NEW.IsButtonDown(button);
                        }
                    }
                    case PlayerIndex.Three:
                    {
                        if (UseGamepadPly1ForPly2)
                        {
                            return G_PLY2_NEW.IsButtonDown(button);
                        }
                        else
                        {
                            return G_PLY3_NEW.IsButtonDown(button);
                        }
                    }
                    case PlayerIndex.Four:
                    {
                        if (UseGamepadPly1ForPly2)
                        {
                            return G_PLY3_NEW.IsButtonDown(button);
                        }
                        else
                        {
                            return G_PLY4_NEW.IsButtonDown(button);
                        }
                    }
                }
                return false;
            }
            /// <summary>
            /// Checks if a key is currently held down.
            /// Note: this has been disabled for more than one player, XNA doesnt support multiple keyboards.
            /// </summary>
            /// <param name="ply"></param>
            /// <param name="key"></param>
            public static bool Keyboard_isdown(PlayerIndex ply, Keys key)
            {
                switch (ply)
                {
                    case PlayerIndex.One:
                    {
                        return K_PLY1_NEW.IsKeyDown(key);
                    }
                    case PlayerIndex.Two:
                    {
                        return false;
                    }
                    case PlayerIndex.Three:
                    {
                        return false;
                    }
                    case PlayerIndex.Four:
                    {
                        return false;
                    }
                }
                return false;
            }
        #endregion

        #region "Functions - keys/buttons held down for player one"
            /// <summary>
            /// Checks if an Xbox controller button has been pressed (with previous unpressed).
            /// </summary>
            /// <param name="gps_prev"></param>
            /// <param name="gps"></param>
            /// <param name="button"></param>
            /// <returns></returns>
            public static bool IsButtonDown(Buttons button)
            {
                return (G_PLY1_NEW.IsButtonDown(button) && !G_PLY1_OLD.IsButtonDown(button));
            }
            /// <summary>
            /// Checks if an Xbox controller button is down (multiple).
            /// </summary>
            /// <param name="button"></param>
            /// <returns></returns>
            public static bool IsButtonDown2(Buttons button)
            {
                return G_PLY1_NEW.IsButtonDown(button);
            }
            /// <summary>
            /// Checks if a key is held down (multiple).
            /// </summary>
            /// <param name="button"></param>
            /// <returns></returns>
            public static bool IsKeyDown2(Keys button)
            {
                return K_PLY1_NEW.IsKeyDown(button);
            }
            /// <summary>
            /// Checks if a key has been pressed (with previous unpressed).
            /// </summary>
            /// <param name="gps_prev"></param>
            /// <param name="gps"></param>
            /// <param name="button"></param>
            /// <returns></returns>
            public static bool IsKeyDown(Keys key)
            {
                return (K_PLY1_NEW.IsKeyDown(key) && !K_PLY1_OLD.IsKeyDown(key));
            }
        #endregion

        #region "Variable Keys"
            // These keys are used globally for interface and gamemodes.

            #region "Variables - menu keys"
                // Select key
                public static Keys MENU_KEY_SELECT = Keys.Enter;
                public static Buttons MENU_BUTTON_SELECT = Buttons.A;
                // Up
                public static Keys MENU_KEY_UP = Keys.Up;
                public static Buttons MENU_BUTTON_UP = Buttons.LeftThumbstickUp;
                // Down
                public static Keys MENU_KEY_DOWN = Keys.Down;
                public static Buttons MENU_BUTTON_DOWN = Buttons.LeftThumbstickDown;
                // Left
                public static Keys MENU_KEY_LEFT = Keys.Left;
                public static Buttons MENU_BUTTON_LEFT = Buttons.LeftThumbstickLeft;
                // Right
                public static Keys MENU_KEY_RIGHT = Keys.Right;
                public static Buttons MENU_BUTTON_RIGHT = Buttons.LeftThumbstickRight;
                // Back
                public static Keys MENU_KEY_BACK = Keys.Escape;
                public static Buttons MENU_BUTTON_BACK = Buttons.B;
            #endregion

            #region "Variables - game keys"
                // Zoom in
                public static Keys GAME_KEY_ZOOM_IN = Keys.Up;
                public static Buttons GAME_BUTTON_ZOOM_IN = Buttons.RightThumbstickUp;
                // Zoom out
                public static Keys GAME_KEY_ZOOM_OUT = Keys.Down;
                public static Buttons GAME_BUTTON_ZOOM_OUT = Buttons.RightThumbstickDown;
                // Rotate -
                public static Keys GAME_KEY_ROTATE_MINUS = Keys.Left;
                public static Buttons GAME_BUTTON_ROTATE_MINUS = Buttons.DPadLeft;
                // Rotate +
                public static Keys GAME_KEY_ROTATE_PLUS = Keys.Right;
                public static Buttons GAME_BUTTON_ROTATE_PLUS = Buttons.DPadRight;
                // Reset view
                public static Keys GAME_KEY_VIEW_RESET = Keys.R;
                public static Buttons GAME_BUTTON_VIEW_RESET = Buttons.DPadDown;
                // Forward
                public static Keys GAME_KEY_FORWARD = Keys.W;
                public static Buttons GAME_BUTTON_FORWARD = Buttons.RightTrigger;
                // Backward
                public static Keys GAME_KEY_BACKWARD = Keys.S;
                public static Buttons GAME_BUTTON_BACKWARD = Buttons.LeftTrigger;
                // Left
                public static Keys GAME_KEY_LEFT = Keys.A;
                public static Buttons GAME_BUTTON_LEFT = Buttons.LeftThumbstickLeft;
                // Right
                public static Keys GAME_KEY_RIGHT = Keys.D;
                public static Buttons GAME_BUTTON_RIGHT = Buttons.LeftThumbstickRight;
                // Down
                public static Keys GAME_KEY_DOWN = Keys.S;
                public static Buttons GAME_BUTTON_DOWN = Buttons.LeftThumbstickDown;
                // 1
                public static Keys GAME_KEY_1 = Keys.D1;
                public static Buttons GAME_BUTTON_1 = Buttons.A;
                // 2
                public static Keys GAME_KEY_2 = Keys.D2;
                public static Buttons GAME_BUTTON_2 = Buttons.X;
                // 3
                public static Keys GAME_KEY_3 = Keys.D3;
                public static Buttons GAME_BUTTON_3 = Buttons.Y;
                // 4
                public static Keys GAME_KEY_4 = Keys.D4;
                public static Buttons GAME_BUTTON_4 = Buttons.B;
                // 5
                public static Keys GAME_KEY_5 = Keys.D5;
                public static Buttons GAME_BUTTON_5 = Buttons.LeftShoulder;
                // 6
                public static Keys GAME_KEY_6 = Keys.D6;
                public static Buttons GAME_BUTTON_6 = Buttons.RightShoulder;
                // Pause/in-game menu
                public static Keys GAME_KEY_PAUSE = Keys.Escape;
                public static Buttons GAME_BUTTON_PAUSE = Buttons.Start;
                // Scoreboard
                public static Keys GAME_KEY_SCOREBOARD = Keys.Tab;
                public static Buttons GAME_BUTTON_SCOREBOARD = Buttons.BigButton;
        #endregion
        #endregion
    }
}