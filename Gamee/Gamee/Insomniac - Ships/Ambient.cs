using System;
using System.IO;
using System.Collections.Generic;

namespace Spacegame
{
    public class Ambient
    {
        // Purpose: to play ambient music for the game.
        #region "Variables & Properties"
            // Gamemode.
            public Core.Gamemode Gamemode;
            // This will be responsible for playing our ambient music via DirectX since XNA doesn't allow dynamic audio.
            public Core.Sound Player;
            // Contains an array of music waiting to be played.
            public List<string> Ambient_Waiting = new List<string>();
            // Contains an arrayed of music that has been played already.
            public List<string> Ambient_Played = new List<string>();
            // The current path of the ambient music player
            public string CurrentPath = "";
            // Responsible for the songs available to play
            public string AmbientPath
            {
                get
                {
                    return CurrentPath;
                }
                set
                {
                    // Transform the path
                    value = Core.Common.Path(value, Core.Common.PathType.Audio, Gamemode._Main, Gamemode);
                    // Check the directory exists
                    if (!Directory.Exists(value))
                    {
                        // Doesnt exist, abort.
                        return;
                    }
                    // Set the current path
                    CurrentPath = value;
                    // Loop through the path and add .wavs and .mp3s
                    foreach(FileInfo fi in new DirectoryInfo(value).GetFiles("*.mp3"))
                    {
                        Ambient_Waiting.Add(fi.FullName);
                    }
                }
            }
        #endregion

        #region "Core"
            public Ambient(Core.Gamemode gm, string folder)
            {
                // Set gamemode object
                Gamemode = gm;
                // Set the folder of music to play
                AmbientPath = folder;
            }
            // Safely disposes the ambient array, freeing resources.
            public void Destroy()
            {
                // Dispose player
                Core.Sound.Dispose(Player);
                // Destroy arrays
                Ambient_Waiting.Clear();
                Ambient_Played.Clear();
                Ambient_Waiting = null;
                Ambient_Played = null;
                // Null gamemode object
                Gamemode = null;
            }
        #endregion

        #region "Functions"
            // Plays a random ambient song.
            public void PlayItem()
            {
                // If the waiting array is empty, copy the played array back to the waiting array
                if (Ambient_Waiting.Count == 0)
                {
                    // Add all the songs played
                    Ambient_Waiting.AddRange(Ambient_Played);
                    // Clear the played array
                    Ambient_Played.Clear();
                }
                // Check if theres any items
                if (Ambient_Waiting.Count > 0)
                {
                    // Play a random item by generating a random index between 0 and the array count - 1
                    int index = Gamemode.RNumber2(0, Ambient_Waiting.Count - 1);
                    // Get array item
                    string item = Ambient_Waiting[index];
                    // Remove array item
                    Ambient_Waiting.RemoveAt(index);
                    // Add to played array list
                    Ambient_Played.Add(item);
                    // Dispose old player
                    if (Player != null)
                    {
                        Core.Sound.Dispose(Player);
                    }
                    // Define the new player
                    Player = new Core.Sound(Gamemode._Main, item, true, false, Gamemode);
                    Player._EMITTER.Ending += new EventHandler(_EMITTER_Ending);
                    // Check if to mute it
                    if (!Core.Resolution.Ambient && Player._EMITTER.Volume == 0)
                    {
                        Player._EMITTER.Volume = -10000;
                    }
                }
            }

            void _EMITTER_Ending(object sender, EventArgs e)
            {
                // Song has ended
                PlayItem();
            }
            // For when the ambient music should be restarted e.g. at the start of a game.
            public void Restart()
            {
                // Copy the played array back to the waiting array
                Ambient_Waiting.AddRange(Ambient_Played);
                // Clear played array
                Ambient_Played.Clear();
                // Play random item
                PlayItem();
            }
            // Stops the ambient music.
            public void Stop()
            {
                if (Player != null)
                {
                    Player.Stop();
                }
            }
            public void Logic()
            {
                if (Player != null)
                {
                    // Check if ambient is enabled/disabled
                    if (Core.Resolution.Ambient && Player._EMITTER.Volume == -10000)
                    {
                        Player._EMITTER.Volume = 0;
                    }
                    else if (!Core.Resolution.Ambient && Player._EMITTER.Volume == 0)
                    {
                        Player._EMITTER.Volume = -10000;
                    }
                }
            }
        #endregion
    }
}