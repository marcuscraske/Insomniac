using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core
{
    public class DistanceSoundEmitter : Sound
    {
        // Purpose: inherits the sound emitter and changes the volume relative to the closest player; the closer the player to the emitter, the louder it is.
        #region "Variables"
            Gamemode Gamemode;
            float MinimumDistance;
            Vector2 Point;
        #endregion
        public DistanceSoundEmitter(Gamemode gm, Vector2 point, float minimumdistance, string path, bool autoplay, bool loop):base(gm._Main, path, autoplay, loop, gm)
        {
            // Renders the emitter silent (-10000 being the lowest volume possible)
            _EMITTER.Volume = -10000;
            // Sets other variables passed to this class
            Gamemode = gm;
            MinimumDistance = minimumdistance;
            Point = point;
        }
        public void Logic()
        {
            // Loop through each player and get the closest
            float temp = MinimumDistance;
            float temp2;
            foreach (Player ply in Gamemode._Players)
            {
                if (ply.Entity != null && !ply.Dead && ply.Entity.Alive)
                {
                    temp2 = Vector2.Distance(ply.Entity.Position + ply.Entity.Centre, Point);
                    if (temp2 < temp)
                    {
                        temp = temp2;
                    }
                }
            }
            if (temp < MinimumDistance && temp >= 0)
            {
                // Player found, calculate the volume relative to their proximity
                int lol = (int)Math.Round(-10000 * (temp / MinimumDistance), 0);
                _EMITTER.Volume = lol;
            }
            else
            {
                // No player was found, the emitter is made silent
                _EMITTER.Volume = -10000;
            }
        }
    }
}