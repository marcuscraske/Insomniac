using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.AudioVideoPlayback;

namespace Core
{
    public class Sound
    {
        // Purpose: allows MP3 and other various files to be dynamically played using DirectX audio playback (which XNA does not support very well).

        #region "Variables"
            /// <summary>
            /// Auto-play content.
            /// </summary>
            public bool AutoPlay = false;
            /// <summary>
            /// Loop content.
            /// </summary>
            private bool Loop = true;
            /// <summary>
            /// Responsible for actually playing audio via DX.
            /// </summary>
            public Audio _EMITTER;
            private string _PATH = "";
            /// <summary>
            /// Path of the content being played.
            /// </summary>
            public string Path
            {
                get
                {
                    return _PATH;
                }
                set
                {
                    if (_EMITTER.State == StateFlags.Running || _EMITTER.State == StateFlags.Paused)
                    {
                        _EMITTER.Stop();
                    }
                    _PATH = value;
                    _EMITTER = new Audio(_PATH, AutoPlay);
                }
            }
            /// <summary>
            /// The volume between 0 to 100 (with 0 being silent and 100 being the loudest).
            /// </summary>
            public int Volume
            {
                get
                {
                    return (_EMITTER.Volume - 10000) / 100 * -1;
                }
                set
                {
                    _EMITTER.Volume = -10000 + (value * 100);
                }
            }
            /// <summary>
            /// Indicates if the emitter is playing or not.
            /// </summary>
            public bool IsPlaying
            {
                get
                {
                    return _EMITTER.State == StateFlags.Running;
                }
            }
        #endregion

        #region "Core"
            public Sound(Gamee.Main main, string path, bool autoplay, bool loop, Gamemode gm)
            {
                // Formats the path (e.g. replacing %GAMEMODE% with the gamemode file root etc)
                path = Common.Path(path, Common.PathType.Audio, main, gm);
                // Sets the path of the soundfile
                _PATH = path;
                // Sets other variables
                AutoPlay = autoplay;
                Loop = loop;
                // Creates a new emitter and catches when its ending
                _EMITTER = new Audio(path, autoplay);
                _EMITTER.Ending += new EventHandler(_EMITTER_Ending);
            }
            void _EMITTER_Ending(object sender, EventArgs e)
            {
                if (_EMITTER != null)
                {
                    if (Loop)
                    {
                        _EMITTER.CurrentPosition = 0;
                    }
                }
            }
            /// <summary>
            /// Destroys the emitter (releasng all its resources, clearing the memory too).
            /// </summary>
            public static void Dispose(Sound obj)
            {
                if (obj != null)
                {
                    if (obj._EMITTER != null)
                    {
                        obj._EMITTER.Stop();
                        obj._EMITTER = null;
                    }

                    obj = null;
                }
            }
        #endregion

        #region "Functions - State"
            // This allows external classes to get the state of the emitter.
            public enum StateType
            {
                Playing = 0,
                Paused = 1,
                Stopped = 2,
            }
            public StateType State
            {
                get
                {
                    if (_EMITTER != null)
                    {
                        if (_EMITTER.State == StateFlags.Running)
                        {
                            return StateType.Playing;
                        }
                        else if (_EMITTER.State == StateFlags.Paused)
                        {
                            return StateType.Paused;
                        }
                        else
                        {
                            return StateType.Stopped;
                        }
                    }
                    else
                    {
                        return StateType.Stopped;
                    }
                }
            }
        #endregion

        #region "Functions - Control"
            /// <summary>
            /// Toggles pausing or playing the emitter.
            /// </summary>
            public void TogglePlay()
            {
                if (_EMITTER != null)
                {
                    if (_EMITTER.State != StateFlags.Stopped)
                    {
                        if (_EMITTER.State == StateFlags.Running)
                        {
                            _EMITTER.Pause();
                        }
                        else
                        {
                            _EMITTER.Play();
                        }
                    }
                }
            }
            /// <summary>
            /// Stops the emitter playing the content.
            /// </summary>
            public void Stop()
            {
                if (_EMITTER != null && _EMITTER.Playing)
                {
                    _EMITTER.Stop();
                }
            }
        #endregion
    }
}