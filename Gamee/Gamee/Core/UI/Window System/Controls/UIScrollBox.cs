using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class UIScrollBox:Control
    {
        #region "Events"
            public delegate void ScrollerPressed(UIScrollBox sender);
            public event ScrollerPressed ScrollBox_Pressed;
        #endregion

        #region "Variables"
            public int SelectedIndex = 0;
            public List<Texture> Items = new List<Texture>();
        #endregion

        #region "Core"
            public UIScrollBox(UIWindow parent, string name, int x, int y, int width, int height):base(parent, name, null, null, x, y, width, height)
            {
            }
            public override void Logic()
            {
                // Logic for the current texture
                if (Items.Count > 0)
                {
                    Items[SelectedIndex].Logic();
                }
                // Base logic
                base.Logic();
            }
            public override void Draw(SpriteBatch sb)
            {
                if (Items.Count > 0)
                {
                    // Draw texture (no on-base since we can do it)
                    if (Selected)
                    {
                        sb.Draw(Items[SelectedIndex]._Texture, Area, Color.White);
                    }
                    else
                    {
                        sb.Draw(Items[SelectedIndex]._Texture, Area, Color.Gray);
                    }
                }
            }
            public override void Destroy()
            {
                // Dispose and null the textures
                foreach (Texture txt in Items)
                {
                    Core.Texture.Dispose(txt);
                }
                // Destroy the items list
                Items.Clear();
                Items = null;
            }
            public override void Control_OnKeyDown(Keys Key)
            {
                if (Key == Input.MENU_KEY_RIGHT)
                {
                    Increment();
                }
                else if (Key == Input.MENU_KEY_LEFT)
                {
                    Decrement();
                }
                else if (Key == Input.MENU_KEY_SELECT)
                {
                    if (ScrollBox_Pressed != null)
                    {
                        ScrollBox_Pressed(this);
                    }
                }
            }
            public override void Control_OnGamepadDown(Buttons Button)
            {
                if (Button == Input.MENU_BUTTON_RIGHT)
                {
                    Increment();
                }
                else if (Button == Input.MENU_BUTTON_LEFT)
                {
                    Decrement();
                }
                else if (Button == Input.MENU_BUTTON_SELECT)
                {
                    ScrollBox_Pressed(this);
                }
            }
        #endregion

        #region "Functions - adding, removing"
            /// <summary>
            /// Safely adds an item.
            /// </summary>
            /// <param name="txt"></param>
            public void AddItem(Texture txt)
            {
                Items.Add(txt);
            }
            /// <summary>
            /// Safety removes an item.
            /// </summary>
            /// <param name="txt"></param>
            public void RemoveItem(Texture txt)
            {
                // Find item index
                int count = -1;
                int index = -1;
                foreach (Texture t in Items)
                {
                    count += 1;
                    if (t == txt)
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
                    if(SelectedIndex < 0 || Items.Count == 0)
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