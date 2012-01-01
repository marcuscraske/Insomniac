using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class UIScrollbox3:Control
    {
        #region "Variables"
            public int SelectedIndex = -1;
            public List<UIScrollbox3Item> Items = new List<UIScrollbox3Item>();
        #endregion

        #region "Core"
            public UIScrollbox3(UIWindow parent, string name, int x, int y, int width, int height):base(parent, name, null, null, x, y, width, height)
            {
            }
            public override void Draw(SpriteBatch sb)
            {
                // Check an item is selected
                if (SelectedIndex != -1)
                {
                    // Draw item (however if this control is not selected, its drawn with a shade of gray)
                    if (Selected)
                    {
                        sb.Draw(Items[SelectedIndex].TEXTURE._Texture, Area, Color.White);
                    }
                    else
                    {
                        sb.Draw(Items[SelectedIndex].TEXTURE._Texture, Area, Color.Gray);
                    }
                }
            }
        #endregion

        #region "Functions"
            /// <summary>
            /// Adds an item to the control.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="txt"></param>
            public void AddItem(string name, Core.Texture txt)
            {
                Items.Add(new UIScrollbox3Item(name, txt));
                // If theres no selected index, set it to the first item
                if(SelectedIndex == -1)
                {
                    SelectedIndex = 0;
                }
            }
            /// <summary>
            /// Removes an item from the control.
            /// </summary>
            /// <param name="name"></param>
            public void RemoveItem(string name)
            {
                // Used to temp store the item being found
                UIScrollbox3Item temp = null;
                // Find the item
                foreach(UIScrollbox3Item t in Items)
                {
                    if(t.NAME == name)
                    {
                        temp = t;
                        break;
                    }
                }
                // Check an item has been found
                if(temp != null)
                {
                    // Remove item
                    Core.Texture.Dispose(temp.TEXTURE);
                    Items.Remove(temp);
                    temp = null;
                }
                // Check if theres any items
                if(Items.Count < 1)
                {
                    SelectedIndex = -1;
                }
            }
            /// <summary>
            /// Sets the selected item to be drawn etc.
            /// </summary>
            /// <param name="name"></param>
            public void SetSelected(string name)
            {
                int count = 0;
                foreach (UIScrollbox3Item t in Items)
                {
                    if (t.NAME == name)
                    {
                        SelectedIndex = count;
                        return;
                    }
                    count += 1;
                }
                if (Items.Count > 0)
                {
                    // No item could be found, the first item is selected
                    SelectedIndex = 0;
                }
            }
            /// <summary>
            /// Increments the selected index by one.
            /// </summary>
            public void IncrementSelected()
            {
                if(SelectedIndex + 1 >= Items.Count)
                {
                    SelectedIndex = 0;
                }
                else
                {
                    SelectedIndex += 1;
                }
            }
            /// <summary>
            /// Decrements the selected index by one.
            /// </summary>
            public void DecrementSelected()
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
            public override void Control_OnGamepadDown(Buttons Button)
            {
                if (Button == Input.MENU_BUTTON_LEFT)
                {
                    DecrementSelected();
                }
                else if (Button == Input.MENU_BUTTON_RIGHT)
                {
                    IncrementSelected();
                }
            }
            public override void Control_OnKeyDown(Keys Key)
            {
                if (Key == Input.MENU_KEY_LEFT)
                {
                    DecrementSelected();
                }
                else if (Key == Input.MENU_KEY_RIGHT)
                {
                    IncrementSelected();
                }
            }
        #endregion
    }
    public class UIScrollbox3Item
    {
        public string NAME;
        public Core.Texture TEXTURE;
        public UIScrollbox3Item(string name, Core.Texture txt)
        {
            NAME = name;
            TEXTURE = txt;
        }
    }
}