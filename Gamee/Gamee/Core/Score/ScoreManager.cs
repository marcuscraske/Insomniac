using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core
{
    public class ScoreManager
    {
        // Purpose: manages a games score.

        #region "Variables"
            // The parent game(mode).
            Gamemode Gamemode;
            // Stores the different scores available for various items, as defined in the
            // %GAMEMODE%\score_values.txt file e.g. app_start_path/content/gamemodes/Insomniac/score_values.txt.
            // Each line of that file begins with the name of the entity followed by an equals sign, then the score
            // the player achieves from killing that type of entity and then the line is then ended with a semi-colon.
            public Hashtable ScoreValues = new Hashtable();
            // The score multiplier, as defined by a map (else this is 1.0).
            public float Multiplier = 1.0F;
            // The amount of score achieved every second.
            public float ScorePerSecond = 0.0F;
            // Used to calculate when its been a second.
            int LastTick = Environment.TickCount;
        #endregion

        #region "Core
            public ScoreManager(Gamemode gm)
            {
                Gamemode = gm;
                // Load score values for gamemode
                if (File.Exists(Gamemode.Root + "\\score_values.txt"))
                {
                    string[] temp = File.ReadAllText(Gamemode.Root + "\\score_values.txt").Replace(Environment.NewLine, "").Split(';');
                    string[] temp2;
                    foreach (string str in temp)
                    {
                        if (str.Length >= 3 && str.Contains("="))
                        {
                            temp2 = str.Split('=');
                            ScoreValues.Add(temp2[0], temp2[1]);
                        }
                    }
                }
                // Add handler to catch deaths
                Gamemode.ENT_Killed += new Gamemode.EntKilled(Gamemode_ENT_Killed);
            }
            public void Logic()
            {
                // Add score per a second to each player alive (but only if there is
                // score per a second, since by default its 0.0F and will provide nothing.
                if (ScorePerSecond != 0.0F)
                {
                    if (Environment.TickCount - LastTick >= 1000)
                    {
                        foreach (Player ply in Gamemode._Players)
                        {
                            if (ply.Entity != null)
                            {
                                if (ply.Entity.Alive)
                                {
                                    ply.Score.SCORE += ScorePerSecond;
                                }
                            }
                        }
                        LastTick = Environment.TickCount;
                    }
                }
            }
            void Gamemode_ENT_Killed(Entity victim, Entity killer)
            {
                // Check the entity has a player
                if (victim.Player != null)
                {
                    // Check the victim entity is a player, else do not add a statistic (since this would
                    // count the deaths of e.g. missiles a player has sent as player deaths (which would be
                    // bad
                    if (victim == victim.Player.Entity)
                    {
                        // Add statistic
                        victim.Player.Score.AddStat(killer.DisplayName, false);
                    }
                }
                // Check if the killer is a player to add score
                if (killer.Player != null)
                {
                    // Check the victim can be counted as a kill and that its not a team kill
                    if (victim.CountAsKill && killer.Enemy != victim.Enemy)
                    {
                        // Add statistic for successful kill
                        killer.Player.Score.AddStat(victim.DisplayName, true);
                    }
                    // Add score for successful kill
                    if (ScoreValues.Contains(victim.DisplayName))
                    {
                        killer.Player.Score.SCORE += Convert.ToInt32(ScoreValues[victim.DisplayName].ToString()) * Multiplier;
                    }
                    else
                    {
                        killer.Player.Score.SCORE += 1 * Multiplier;
                    }
                }
            }
        #endregion

        #region "Functions"
            /// <summary>
            /// Saves each players score.
            /// </summary>
            public void SaveScores()
            {
                foreach (Player ply in Gamemode._Players)
                {
                    ply.SaveScore();
                }
            }
            /// <summary>
            /// Resets the score-manager to default values.
            /// </summary>
            public void Reset()
            {
                Multiplier = 1.0F;
                ScorePerSecond = 0.0F;
                WipeAllScores();
            }
            /// <summary>
            /// Wipes every players score.
            /// </summary>
            public void WipeAllScores()
            {
                foreach (Player ply in Gamemode._Players)
                {
                    ply.Score.SCORE = 0.0F;
                }
            }
        #endregion    
    }
}