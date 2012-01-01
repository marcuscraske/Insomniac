using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class UIMapButton:Control
    {
        #region "Events"
            public delegate void ButtonPressed(UIMapButton sender);
            public event ButtonPressed Button_Pressed;
        #endregion

        #region "Variables"
            public Texture Thumbnail;
            public SpriteFont Font;
            public string Text = "";
        #endregion

        #region "Core"
            public UIMapButton(UIWindow parent, string name, Texture thumbnail, string text, int x, int y):base(parent, name, null, null, x, y, 490, 85)
            {
                // Set textures
                Thumbnail = thumbnail;
                BackgroundNormal = new Texture(Parent.Main, "%MAIN%\\MenuMaps\\Button", 100, null);
                BackgroundSelected = new Texture(Parent.Main, "%MAIN%\\MenuMaps\\ButtonSelected", 100, null);
                // Set font
                Font = parent.Main.Content.Load<SpriteFont>("MapButton");
                Text = text;
            }
            public override void Control_OnKeyDown(Keys Key)
            {
                if (Key == Input.MENU_KEY_SELECT)
                {
                    if (Button_Pressed != null)
                    {
                        Button_Pressed(this);
                    }
                }
            }
            public override void Control_OnGamepadDown(Buttons Button)
            {
                if (Button == Input.MENU_BUTTON_SELECT)
                {
                    if (Button_Pressed != null)
                    {
                        Button_Pressed(this);
                    }
                }
            }
            public override void Draw(SpriteBatch sb)
            {
                // Draw background
                base.Draw(sb);
                // Draw thumbnail
                sb.Draw(Thumbnail._Texture, new Rectangle(Area.X + 2, Area.Y + 2, 91, 81), Color.White);
                // Draw text
                if (Selected)
                {
                    sb.DrawString(Font, Text, new Vector2(Area.X + 100, Area.Y + 30), Color.White);
                }
                else
                {
                    sb.DrawString(Font, Text, new Vector2(Area.X + 100, Area.Y + 30), Color.DarkBlue);
                }
            }
            public override void Destroy()
            {
                // Destroy thumbnail and font
                Core.Texture.Dispose(Thumbnail);
                Font = null;
                // Destroy base
                base.Destroy();
            }
        #endregion
    }     
}