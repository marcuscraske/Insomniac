using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Core
{
    public class Textures
    {
        #region "Variables"
            // Stores the gamemode.
            Gamee.Main Main;
            // Stores the textures in a hashtable allowing name-indexing.
            Hashtable ITEMS = new Hashtable();
        #endregion

        #region "Core"
            public Textures(Gamee.Main main)
            {
                Main = main;
            }

            public IDictionaryEnumerator GetEnumerator()
            {
                return ITEMS.GetEnumerator();
            }
        #endregion

        #region "Functions"
            /// <summary>
            /// Adds a folder of non-animation textures.
            /// </summary>
            /// <param name="path"></param>
            /// <param name="at"></param>
            public void AddFolder(string path)
            {
                foreach (DirectoryInfo di in new DirectoryInfo(path).GetDirectories("*"))
                {
                        Add(di.Name, new Texture(di.FullName, Main, null));
                }
            }
            /// <summary>
            /// Adds a texture.
            /// </summary>
            /// <param name="ID"></param>
            /// <param name="texture"></param>
            public void Add(string ID, Texture texture)
            {
                ITEMS.Add(ID, texture);
            }
            /// <summary>
            /// Removes a texture.
            /// </summary>
            /// <param name="ID"></param>
            public void Remove(string ID)
            {
                ITEMS.Remove(ID);
            }
            /// <summary>
            /// Removes all the textures.
            /// </summary>
            public void RemoveAll()
            {
                ITEMS.Clear();
            }
            /// <summary>
            /// Gets a texture or returns an error texture.
            /// </summary>
            /// <param name="ID"></param>
            /// <returns></returns>
            public Texture Get(string ID)
            {
                if (ITEMS.ContainsKey(ID))
                {
                    return (Texture)ITEMS[ID];
                }
                else
                {
                    return new Texture("MISSING_TEXTURE", Main, null);
                }
            }
        #endregion

        #region "Functions - Static"
            public static Texture Create(Gamee.Main main, string path, int framerate, Gamemode gm)
            {
                Texture txt = new Texture(path, main, gm);
                txt.Framerate = framerate;
                return txt;
            }
        #endregion
    }

    public class Texture
    {
        #region "Variables"
            public bool Disposable = true;
            public List<Texture2D> Frames = new List<Texture2D>();
            public int CurrentFrame = 0;
            public int Width = 0;
            public int Height = 0;
            public int Framerate = 100;
            private int LastTick = Environment.TickCount;
            public Vector2 Centre = Vector2.Zero;
            public Texture2D _Texture
            {
                get
                {
                    return Frames[CurrentFrame];
                }
                set
                {
                    Frames[CurrentFrame] = value;
                }
            }
        #endregion

        #region "Core"
            public Texture(string path, Gamee.Main main, Gamemode gm)
            {
                path = Common.Path(path, Common.PathType.Textures, main, gm);
                if (Directory.Exists(path))
                {
                    foreach (FileInfo fi in new DirectoryInfo(path).GetFiles("*"))
                    {
                        if (ValidFile(fi.Extension))
                        {
                            Frames.Add(Texture2D.FromFile(main.graphics.GraphicsDevice, fi.FullName));
                        }
                        System.Diagnostics.Debug.WriteLine(fi.FullName);
                    }
                }
                if(Frames.Count == 0)
                {
                    Frames.Add(main.Content.Load<Texture2D>("missing_texture"));
                }
                UpdateCentre();
            }

            private bool ValidFile(string extension)
            {
                extension = extension.ToLower();
                if (extension == ".png")
                {
                    return true;
                }
                else if(extension == ".jpg")
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
            /// Checks if the frame needs updating.
            /// </summary>
            public virtual void Logic()
            {
                if(Frames.Count > 1 && Environment.TickCount - LastTick > Framerate)
                {
                    NextFrame();
                }
            }
            /// <summary>
            /// Moves to the next frame.
            /// </summary>
            public void NextFrame()
            {
                if(CurrentFrame + 1 >= Frames.Count)
                {
                    CurrentFrame = 0;
                }
                else
                {
                    CurrentFrame +=1;
                }
                LastTick = Environment.TickCount;
                UpdateCentre();
            }
            /// <summary>
            /// Updates the centre position of the current texture.
            /// </summary>
            public void UpdateCentre()
            {
                Centre = new Vector2(_Texture.Width / 2, _Texture.Height / 2);
            }
            /// <summary>
            /// Disposes the texture object.
            /// </summary>
            public static void Dispose(Texture txt)
            {
                if (txt != null && txt.Disposable)
                {
                    if (txt._Texture != null)
                    {
                        txt._Texture.Dispose();
                        txt._Texture = null;
                    }
                    txt = null;
                }
            }
        #endregion
    }
}