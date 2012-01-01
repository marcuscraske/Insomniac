using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame.Module
{
    // Purpose: a class produced to be inherited to produce a modules system;
    // modules are basically items the player can use that e.g. cause their
    // ship to shoot bullets etc.
    /// <summary>
    /// Icon and CurtainIcon need to be set in inherited modules!
    /// </summary>
    public class Module
    {
        #region "Variables"
            /// <summary>
            /// The player/parent of the module.
            /// </summary>
            public Core.Player Ply;
            /// <summary>
            /// The gamemode.
            /// </summary>
            public Core.Gamemode Gamemode;
            /// <summary>
            /// The icon representing the module.
            /// </summary>
            public Core.Texture Icon;
            /// <summary>
            /// The icon which is drawn over the icon to represent percentage.
            /// </summary>
            public Core.Texture CurtainIcon;
            /// <summary>
            /// Ranges from 0.0 to 1.0, used to draw a blank texture and to disable
            /// the module (depending on the inherited module).
            /// </summary>
            public float Percent = 1.0F;
            /// <summary>
            /// The area in-which the module-icon is drawn on the screen (screen co-ords).
            /// </summary>
            public Rectangle Area;
            /// <summary>
            /// The amount recharged every cycle (which is sixty times per a second).
            /// </summary>
            public float RechargePerCycle = 0.01F;
        #endregion

        public Module(Core.Player ply, Core.Gamemode gm)
        {
            Ply = ply;
            Gamemode = gm;
        }
        public virtual void Logic()
        {
            // Recharge the percentage
            if (Percent < 1.0F)
            {
                Percent += RechargePerCycle;
            }
            if (Icon != null)
            {
                Icon.Logic();
            }
            if (CurtainIcon != null)
            {
                CurtainIcon.Logic();
            }
        }
        /// <summary>
        /// Draws using world co-ords.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Draw icon
            if (Icon != null)
            {
                spriteBatch.Draw(Icon._Texture, Area, Color.White);
            }
            // Draw curtain
            if (CurtainIcon != null && Percent < 1.0F)
            {
                spriteBatch.Draw(CurtainIcon._Texture, new Rectangle(Area.X, Area.Y + (int)(Area.Height * Percent), Area.Width, Area.Height - (int)(Area.Height * Percent)), Color.White);
            }
        }
        /// <summary>
        /// Occurs when the player clicks/activates the module.
        /// </summary>
        public virtual void Clicked()
        {
            // Empty to be overriden by inheriting classes.
        }
        #region "Functions - static"
            /// <summary>
            /// Calculates the amount of percent to add every cycle to form 100%.
            /// </summary>
            /// <param name="main"></param>
            /// <param name="time_amount_ms"></param>
            /// <returns></returns>
            public static float CalculateRechargePerCycle(float time_amount_ms)
            {
                time_amount_ms = time_amount_ms / 1000;
                return (((time_amount_ms / 60) / time_amount_ms) / time_amount_ms);
            }
        #endregion
    }
}