using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class UIScrollbox2:Control
    {
        #region "Events"
            public delegate void ScrollerPressed(UIScrollbox2 sender);
            public event ScrollerPressed Scroller_Pressed;
        #endregion

        #region "Variables"
            public Texture Background;
            public SpriteFont Font;
            public int SelectedIndex = 0;
            public List<string> Items = new List<string>();
            public Color Focused = Color.White;
            public Color NonFocused = Color.White;
            public Vector2 TextOffset = new Vector2(50, 4);
        #endregion

        #region "Core"
            public UIScrollbox2(UIWindow parent, string name, Texture txt, int x, int y, int width, int height):base(parent, name, null, null, x, y, width, height)
            {
                // Set the baclokground
                Background = txt;
                // Load font
                Font = parent.Main.Content.Load<SpriteFont>("UIScrollbox2");
            }
            public override void Draw(SpriteBatch sb)
            {
                if (Selected)
                {
                    sb.Draw(Background._Texture, Area, Color.White);
                    sb.DrawString(Font, Items[SelectedIndex], new Vector2(Area.X + TextOffset.X, Area.Y + TextOffset.Y), Focused);
                }
                else
                {
                    sb.Draw(Background._Texture, Area, Color.Gray);
                    sb.DrawString(Font, Items[SelectedIndex], new Vector2(Area.X + TextOffset.X, Area.Y + TextOffset.Y), NonFocused);
                }
            }
            public override void Destroy()
            {
                Font = null;
                Core.Texture.Dispose(Background);
                Items = null;
            }
            public override void Control_OnGamepadDown(Buttons Button)
            {
                if (Button == Input.MENU_BUTTON_SELECT)
                {
                    Scroller_Pressed(this);
                }
                else if (Button == Input.MENU_BUTTON_LEFT)
                {
                    Decrement();
                }
                else if (Button == Input.MENU_BUTTON_RIGHT)
                {
                    Increment();
                }
            }
            public override void Control_OnKeyDown(Keys Key)
            {
                if (Key == Input.MENU_KEY_SELECT)
                {
                    if (Scroller_Pressed != null)
                    {
                        Scroller_Pressed(this);
                    }
                }
                else if (Key == Input.MENU_KEY_LEFT)
                {
                    Decrement();
                }
                else if (Key == Input.MENU_KEY_RIGHT)
                {
                    Increment();
                }
            }
        #endregion

        #region "Functions - getting and setting"
            /// <summary>
            /// Returns the selected items text; blank is returned if its empty.
            /// </summary>
            /// <returns></returns>
            public string GetSelected()
            {
                if (Items.Count == 0)
                {
                    return "";
                }
                else
                {
                    return Items[SelectedIndex];
                }
            }

            public void SetSelected(string str)
            {
                int count = 0;
                foreach (string t in Items)
                {
                    if (t == str)
                    {
                        SelectedIndex = count;
                        return;
                    }
                    count += 1;
                }
                SelectedIndex = 0;
            }
        #endregion

        #region "Functions - adding and removing items"
            /// <summary>
            /// Safety adds an item.
            /// </summary>
            /// <param name="text"></param>
            public void AddItem(string text)
            {
                Items.Add(text);
            }
            /// <summary>
            /// Safely removes an item.
            /// </summary>
            /// <param name="text"></param>
            public void RemoveItem(string text)
            {
                // Find item index
                int count = -1;
                int index = -1;
                foreach (string str in Items)
                {
                    count += 1;
                    if (str == text)
                    {
                        index = count;
                    }
                }
                // Return if nothing is found
                if (index == -1)
                {
                    return;
                }
                else
                {
                    // If its equal or less than the selected index, selectedindex -=1
                    if (index <= SelectedIndex)
                    {
                        SelectedIndex -= 1;
                    }
                    // Ensure the selected index is valid
                    if (SelectedIndex < 0 || Items.Count == 0)
                    {
                        SelectedIndex = 0;
                    }
                    else if (SelectedIndex >= Items.Count)
                    {
                        SelectedIndex = Items.Count - 1;
                    }
                }
            }
        #endregion

        #region "Functions - incrementing and decrementing"
            /// <summary>
            /// Increments the selected index safely.
            /// </summary>
            public void Increment()
            {
                if (Items.Count > 0 && SelectedIndex + 1 < Items.Count)
                {
                    SelectedIndex += 1;
                }
                else
                {
                    SelectedIndex = 0;
                }
            }
            /// <summary>
            /// Decrements the selected index safely.
            /// </summary>
            public void Decrement()
            {
                if (Items.Count > 0)
                {
                    if (SelectedIndex - 1 < 0)
                    {
                        SelectedIndex = Items.Count - 1;
                    }
                    else
                    {
                        SelectedIndex -= 1;
                    }
                }
                else
                {
                    SelectedIndex = 0;
                }
            }
        #endregion
    }
}