using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core
{
    public class UILabel:Control
    {
        // Purpose: a non-select control which displays text (like a label control in .NET).

        #region "Variables"
            public Color Color;
            public SpriteFont Font;
            public string Text;
            public bool Hidden = false;
        #endregion

        #region "Core"
            public UILabel(UIWindow parent, string name, string text, Color color, int x, int y, bool hidden):base(parent, name, null, null, x, y, 1, 1)
            {
                Hidden = hidden;
                Color = color;
                Text = text;
                Font = parent.Main.Content.Load<SpriteFont>("UILabel");
            }
            public override void Draw(SpriteBatch sb)
            {
                if (!Hidden)
                {
                    sb.DrawString(Font, Text, new Vector2(Area.X, Area.Y), Color);
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