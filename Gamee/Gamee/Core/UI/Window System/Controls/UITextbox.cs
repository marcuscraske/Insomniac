using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class UITextbox:Control
    {
        // Purpose: allows the player to type text, which can be captured and e.g. used by an options window.

        #region "Variables"
            int MaxChars = 24;
            public string Text = "Test!";
            SpriteFont Font;
        #endregion

        #region "Functions"
            public UITextbox(UIWindow parent, string name, int x, int y):base(parent, name, null, null, x, y, 250, 30)
            {
                BackgroundNormal = new Core.Texture(parent.Main, "%MAIN%\\UI\\Textbox", 100, null);
                BackgroundSelected = new Core.Texture(parent.Main, "%MAIN%\\UI\\Textbox-selected", 100, null);
                Font = Parent.Main.Content.Load<SpriteFont>("UITextbox");
            }
            public override void Control_OnKeyDown(Microsoft.Xna.Framework.Input.Keys Key)
            {
                if (Key == Keys.LeftShift || Key == Keys.RightShift)
                {
                    return;
                }
                else if (Key == Keys.Back)
                {
                    if (Text.Length > 0)
                    {
                        Text = Text.Remove(Text.Length - 1, 1);
                    }
                }
                else if (Key == Keys.Space)
                {
                    Text += " ";
                }
                else
                {
                    if (Text.Length + 1 < MaxChars)
                    {
                        string KEY = Key.ToString().Replace("OemPipe", "\\");
                        if (KEY.Length == 2 && KEY.StartsWith("D"))
                        {
                            KEY = KEY.Remove(0, 1);
                        }
                        if (KEY.Length < 2)
                        {
                            if (Input.IsKeyDown2(Keys.LeftShift) || Input.IsKeyDown2(Keys.RightShift))
                            {
                                Text += KEY.ToUpper();
                            }
                            else
                            {
                                Text += KEY.ToLower();
                            }
                        }
                    }
                }
            }
            public override void Logic()
            {
                // Texture logic
                BackgroundNormal.Logic();
                BackgroundSelected.Logic();
                // Base logic
                base.Logic();
            }
            public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
            {
                // Draw base (background etc)
                base.Draw(sb);
                // Draw text
                if (Selected)
                {
                    sb.DrawString(Font, Text, new Vector2(Area.X + 8, Area.Y + 4), Color.White);
                }
                else
                {
                    sb.DrawString(Font, Text, new Vector2(Area.X + 8, Area.Y + 4), Color.SteelBlue);
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