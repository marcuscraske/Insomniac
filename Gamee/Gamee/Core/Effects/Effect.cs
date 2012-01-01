using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class Effect
    {
        // Purpose: an effect is a sprite/texture thats drawn, its better for performance than creating an entity; plus it can also have a velocity if defined.

        #region "Variables"
            /// <summary>
            /// If this is true, the effect should be removed.
            /// </summary>
            public bool Finished = false;
            /// <summary>
            /// Calculates alpha based on lifespan percentage.
            /// </summary>
            public bool ApplyAlpha = true;
            public float Alpha = 1.0F;
            /// <summary>
            /// The lifespan of the effect in milliseconds.
            /// </summary>
            public float Lifespan = 1000;
            private float StartedLife;
            public float Rotation = 0;
            public Vector2 Position = Vector2.Zero;
            public Texture Texture;
            /// <summary>
            /// For external use.
            /// </summary>
            public bool Hidden = false;
            public float StartScale = 1.0F;
            public float EndScale = 1.0F;
            public float TempScale = 1.0F;
            public Vector2 Velocity;
        #endregion
        
        public Effect(Texture txt, Vector2 position, float rotation, float _StartScale, float _EndScale, int lifespan, Vector2 velocity)
        {
            // If effects are disabled, the effect will be disposed and not drawn
            if (!Core.Resolution.Effects)
            {
                Finished = true;
                return;
            }
            Texture = txt;
            Position = position;
            Rotation = rotation;
            Lifespan = lifespan;
            StartScale = _StartScale;
            EndScale = _EndScale;
            StartedLife = Environment.TickCount;
            Velocity = velocity;
        }
        public void Logic()
        {
            if (!Finished)
            {
                float temp = Environment.TickCount - StartedLife;
                // Check if finished
                if (temp > Lifespan)
                {
                    Finished = true;
                }
                else if (ApplyAlpha)
                {
                    // Calculate alpha based on lifespan
                    Alpha = ((Lifespan - temp) / Lifespan);
                    // Work out scale
                    TempScale = ((StartScale - EndScale) * Alpha) + EndScale;
                    // Add velocity
                    Position += Velocity;
                }
                // Execute texture logic
                Texture.Logic();
            }
        }
        public void Draw(SpriteBatch sb)
        {
            if (!Finished)
            {
                sb.Draw(Texture._Texture, new Rectangle((int)(Position.X + Texture.Centre.X), (int)(Position.Y + Texture.Centre.Y), (int)(Texture._Texture.Width * TempScale), (int)(Texture._Texture.Height * TempScale)), null, new Color(255, 255, 255, Alpha), Rotation, Texture.Centre, SpriteEffects.None, 0.0F);
            }
        }
        #region "Static Functions - CreateExplosion"
            /// <summary>
            /// Creates an explosion with flares (optional via num_flares set to 0); effects are added to gamemode effects array.
            /// </summary>
            /// <param name="gm"></param>
            /// <param name="position"></param>
            /// <param name="flare_velocity"></param>
            /// <param name="num_flares"></param>
            /// <param name="explosion_main"></param>
            /// <param name="explosion_flare"></param>
            public static void CreateExplosion(Core.Gamemode gm, Vector2 position, float start_scale, float end_scale, float flare_speed, float num_flares, Texture explosion_main, Texture explosion_flare, string soundeffect)
            {
                // Set the explosion offset (to make the texture centre to the point)
                position -= explosion_main.Centre;
                // Create main explosion
                gm.Effects.Add(new Effect(explosion_main, position, 0.0F, start_scale, end_scale, 2000, Vector2.Zero));
                // Create explosion flares
                if(num_flares > 0)
                {
                    int temp;
                    for(int i = 0; i <= num_flares; i++)
                    {
                        temp = gm.RNumber2(0, 360);
                        gm.Effects.Add(new Effect(explosion_flare, position - explosion_flare.Centre + explosion_main.Centre, MathHelper.ToRadians((float)temp), 1.0F, 0.5F, 2000, new Vector2(flare_speed * (float)Math.Sin(2.0 * Math.PI * (temp) / 360.0), flare_speed * -(float)Math.Cos(2.0 * Math.PI * (temp) / 360.0))));
                    }
                }
                // Play sound
                if (soundeffect != null)
                {
                    gm.Audio_Control.PlayCue(soundeffect);
                }
            }
        #endregion
    }
}