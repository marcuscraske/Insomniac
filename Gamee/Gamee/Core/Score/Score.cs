using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core
{
    public class Score
    {
        // Purpose: a class to store and manage a players score and statistics.

        #region "Variables"
            // The score of the player.
            public float SCORE = 0.0F;
            // The total kills of each type of entity by the player.
            public Hashtable TOTAL_KILLS = new Hashtable();
            // The total deaths from each type of entity by the player.
            public Hashtable TOTAL_DEATHS = new Hashtable();
            // The count of actual kills by the player.
            public float ACTUAL_TOTAL_KILLS = 0.0F;
            // The count of actual deaths by the player.
            public float ACTUAL_TOTAL_DEATHS = 0.0F;
            // The number of times the player has spawned.
            public float SPAWNCOUNT = 0.0F;
            // The longest life-span achieved by the player.
            public float LONGESTLIFESPAN = 0.0F;
        #endregion
        /// <summary>
        /// Adds a statistic.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="kill"></param>
        public void AddStat(string name, bool kill)
        {
            if (kill)
            {
                // Statistic was a kill, add it to the kills table
                if (TOTAL_KILLS.Contains(name))
                {
                    TOTAL_KILLS[name] = Convert.ToInt32(TOTAL_KILLS[name]) + 1;
                }
                else
                {
                    TOTAL_KILLS.Add(name, "1");
                }
                ACTUAL_TOTAL_KILLS += 1;
            }
            else
            {
                // Statistic was a death, add it to the deaths table
                if (TOTAL_DEATHS.Contains(name))
                {
                    TOTAL_DEATHS[name] = Convert.ToInt32(TOTAL_DEATHS[name].ToString()) + 1;
                }
                else
                {
                    TOTAL_DEATHS.Add(name, "1");
                }
                ACTUAL_TOTAL_DEATHS += 1;
            }
        }
    }
}