using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class UIButton:Control
    {
        // Purpose: a button control.

        #region "Events"
            // These are raised by the button when they've been pressed (and the event is hooked by an external class e.g. a window)
            public delegate void ButtonPressed(UIButton sender);
            public event ButtonPressed Button_Pressed;
        #endregion

        #region "Variables"
            // If the text should be centered or not.
            public bool Centered = true;
            // The buttons text.
            public string Text;
            // The buttons font.
            SpriteFont Font;
            // The buttons text position.
            Vector2 TextPos = Vector2.Zero;
        #endregion

        #region "Core"
            public UIButton(UIWindow parent, string name, string text, int x, int y):base(parent, name, null, null, x, y, 100, 30)
            {
                // Sets variables of the button
                Font = Parent.Main.Content.Load<SpriteFont>("UIButton");
                Text = text;
                BackgroundNormal = new Core.Texture(parent.Main, "%MAIN%\\UI\\Button", 100, null);
                BackgroundSelected = new Core.Texture(parent.Main, "%MAIN%\\UI\\Button-selected", 100, null);
            }
            public override void Control_OnKeyDown(Microsoft.Xna.Framework.Input.Keys Key)
            {
                // Checks if the button has been pressed, if so the event is raised
                if (Key == Input.MENU_KEY_SELECT)
                {
                    if (Button_Pressed != null)
                    {
                        Button_Pressed(this);
                    }
                }
            }
            public override void Control_OnGamepadDown(Microsoft.Xna.Framework.Input.Buttons Button)
            {
                // Checks if the button has been pressed, if so the event is raised
                if (Button == Input.MENU_BUTTON_SELECT)
                {
                    if (Button_Pressed != null)
                    {
                        Button_Pressed(this);
                    }
                }
            }
            public override void Logic()
            {
                // Calculate the text's position
                if (Centered)
                {
                    TextPos = new Vector2(Area.X + ((Area.Width / 2) - (Font.MeasureString(Text).X / 2)), Area.Y + ((Area.Height / 2) - (Font.MeasureString(Text).Y / 2)));
                }
                else
                {
                    TextPos = new Vector2(Area.X + 10, Area.Y + ((Area.Height / 2) - (Font.MeasureString(Text).Y / 2)));
                }
                // Texture logic
                BackgroundNormal.Logic();
                BackgroundSelected.Logic();
                // Base logic
                base.Logic();
            }
            public override void Draw(SpriteBatch sb)
            {
                // Draw background
                base.Draw(sb);
                // Draw text
                if (Selected)
                {
                    sb.DrawString(Font, Text, TextPos, Color.White);
                }
                else
                {
                    sb.DrawString(Font, Text, TextPos, Color.DarkBlue);
                }
            }
            public override void Destroy()
            {
                Font = null;
                base.Destroy();
            }
        #endregion
    }
}