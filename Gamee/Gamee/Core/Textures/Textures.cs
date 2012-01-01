using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class Texture
    {
        // Purpose: provides a better wrapper for an XNA Texture2D allowing animation.
        #region "Variables & Properties"
            // Indicates if the texture can be disposed; global textures usually have this set to false
            // since e.g. multiple entities may be using the same texture.
            public bool Disposable = true;
            // An array of actual textures that represent this texture (there may be multiple
            // actual textures for an animated texture).
            public List<Texture2D> Frames = new List<Texture2D>();
            // Indicates the current index of the frame (above variable) array.
            public int CurrentFrame = 0;
            // The interval in-which to increment the frame index (for animation purposes).
            public int Framerate = 100;
            // The last tick-count of the application (used to work out when to swap the frames of the texture).
            private int LastTick = Environment.TickCount;
            // The centre-point of the texture
            public Vector2 Centre = Vector2.Zero;
            /// <summary>
            /// Returns the current actual texture selected.
            /// </summary>
            public Texture2D _Texture
            {
                get
                {
                    return Frames[CurrentFrame];
                }
            }
        #endregion

        #region "Core"
            public Texture(Gamee.Main main, string path, int framerate, Gamemode gm)
            {
                // Format the path replacing e.g. %GAMEMODE% with the actual gamemode filesystem root
                path = Common.Path(path, Common.PathType.Textures, main, gm);
                // Check the path exists
                if (Directory.Exists(path))
                {
                    // Loop through each folder in the directory
                    foreach (FileInfo fi in new DirectoryInfo(path).GetFiles("*"))
                    {
                        // Check the file is valid (via extensions)
                        if (ValidFile(fi.Extension))
                        {
                            // Add the image to the texture array
                            Frames.Add(Texture2D.FromFile(main.graphics.GraphicsDevice, fi.FullName));
                        }
                    }
                }
                // If theres no frames in the texture, the missing texture image is added
                if (Frames.Count == 0)
                {
                    Frames.Add(main.Content.Load<Texture2D>("missing_texture"));
                    Disposable = false;
                }
                // Set the frame-rate
                Framerate = framerate;
                UpdateCentre();
            }
            /// <summary>
            /// Indicates if the extension provided is valid for loading by returning true, else false for non-valid.
            /// </summary>
            /// <param name="extension"></param>
            /// <returns></returns>
            private bool ValidFile(string extension)
            {
                extension = extension.ToLower();
                if (extension == ".png")
                {
                    return true;
                }
                else if (extension == ".jpg")
                {
                    return true;
                }
                else if (extension == ".bmp")
                {
                    return true;
                }
                else if (extension == ".jpeg")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        #endregion

        #region "Functions"
            /// <summary>
            /// Checks if the frame needs updating by getting the current tick-count and sub-tracting the last tickcount (which was set
            /// when the frame last changed), giving the number of milliseconds since the last frame change.
            /// </summary>
            public virtual void Logic()
            {
                if (Frames.Count > 1 && Environment.TickCount - LastTick > Framerate)
                {
                    NextFrame();
                }
            }
            /// <summary>
            /// Moves to the next frame.
            /// </summary>
            public void NextFrame()
            {
                if (CurrentFrame + 1 >= Frames.Count)
                {
                    CurrentFrame = 0;
                }
                else
                {
                    CurrentFrame += 1;
                }
                // Set the current tick-count - used for calcuating (in the logic function) when
                // to change the frame again
                LastTick = Environment.TickCount;
                // Updates the centre-point
                UpdateCentre();
            }
            /// <summary>
            /// Updates the centre position of the current texture.
            /// </summary>
            public void UpdateCentre()
            {
                Centre = new Vector2(_Texture.Width / 2, _Texture.Height / 2);
            }
        #endregion

        #region "Functions - static (dispose)"
            /// <summary>
            /// Disposes a texture, freeing its resources.
            /// </summary>
            /// <param name="txt"></param>
            public static void Dispose(Texture txt)
            {
                if (txt != null && txt.Disposable)
                {
                    foreach (Texture2D t in txt.Frames)
                    {
                        t.Dispose();
                    }
                    txt.Frames.Clear();
                    txt = null;
                }
            }
            /// <summary>
            /// Similar to dispose, however this function does not null the parent object (to avoid collection modified issues).
            /// </summary>
            /// <param name="txt"></param>
            public static void Dispose2(Texture txt)
            {
                if (txt != null && txt.Disposable)
                {
                    foreach (Texture2D t in txt.Frames)
                    {
                        t.Dispose();
                    }
                    txt.Frames.Clear();
                }
            }
        #endregion
    }
    /// <summary>
    /// Used to store textures for e.g. an entire gamemode; the textures are then automatically destroyed.
    /// </summary>
    public class TextureArray
    {
        // Purpose: Allows Core.Textures to be stored in an array with an ID, which is great for
        // performance.
        #region "Variables"
            public Gamee.Main Main;
            // All the textures in the array with the key as the name an the value as the Core.Texture object.
            public Hashtable TEXTURES = new Hashtable();
        #endregion
        public TextureArray(Gamee.Main main)
        {
            Main = main;
        }
        /// <summary>
        /// Gets a texture safely from the texture array.
        /// </summary>
        /// <param name="id"></param>
        public Core.Texture GetTexture(string id)
        {
            // If the hashtable of textures does not contain the ID, an error texture is
            // returned to avoid null issues and to indicate a problem
            if (TEXTURES.ContainsKey(id))
            {
                return (Core.Texture)TEXTURES[id];
            }
            else
            {
                return Main.ErrorTexture;
            }
        }
        /// <summary>
        /// Adds a texture safely allowing it to not be disposed etc.
        /// </summary>
        /// <param name="txt"></param>
        public void AddTexture(string id, Core.Texture txt)
        {
            // Disallows the object to be disposed
            txt.Disposable = false;
            // Adds the texture to the array
            TEXTURES.Add(id, txt);
        }
        public void Logic()
        {
            // The texture is temp stored in temp during the loop;
            // this is better for performance because recreating temp
            // variables within loops is slow and pointless since
            // its better to define the variables outside the loop.
            Texture temp;
            foreach (DictionaryEntry di in TEXTURES)
            {
                // Sets the temp object as the texture
                temp = (Texture)di.Value;
                if (temp != null)
                {
                    temp.Logic();
                }
            }
        }
        /// <summary>
        /// Destroys the array of textures and the hashtable.
        /// </summary>
        public void DestroyArray()
        {
            // Disposes each texture
            foreach (DictionaryEntry di in TEXTURES)
            {
                Core.Texture.Dispose2((Texture)di.Value);
            }
            // Destroys the texture array
            TEXTURES.Clear();
            TEXTURES = null;
        }
    }
}