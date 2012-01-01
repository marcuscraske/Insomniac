using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core
{
    public static class Common
    {
        // Purpose: common functions of code used by the core and/or games.

        #region "Functions - Object types"
            /// <summary>
            /// Creates an entity  via its classname (string).
            /// </summary>
            /// <param name="classname"></param>
            /// <param name="gamemode"></param>
            /// <param name="texture"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <returns></returns>
            public static Entity GetEntityByClass(string classname, Gamemode gamemode, Texture texture, int width, int height, bool enemy)
            {
                return (Entity)Activator.CreateInstance(Type.GetType(classname, true, true), new object[] { gamemode, texture, width, height, enemy });
            }
            /// <summary>
            /// Creates a gamemode object via its classname (string).
            /// </summary>
            /// <param name="classname"></param>
            /// <param name="main"></param>
            /// <param name="rootname"></param>
            /// <returns></returns>
            public static Gamemode GetGamemodeByClass(string classname, Gamee.Main main, string rootname)
            {
                return (Gamemode)Activator.CreateInstance(Type.GetType(classname, true, true), new object[] { main, rootname });
            }
            /// <summary>
            /// Sets a public variable of a class with a specified name, type of object and the value to set.
            /// </summary>
            /// <param name="ent"></param>
            /// <param name="variable"></param>
            /// <param name="value"></param>
            public static void SetObjectVariable(object invoked_object, string variable, object value)
            {
                invoked_object.GetType().InvokeMember(variable, System.Reflection.BindingFlags.SetField | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, invoked_object, new object[] { value });
            }
        #endregion

        #region "Functions - Vectors"
            /// <summary>
            /// Gets the distance between teo entities.
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static float EntityDistance(Entity a, Entity b)
            {
                return Vector2.Distance(a.Position + a.Centre, b.Position + b.Centre);
            }
            /// <summary>
            /// Rotates a vector - created specifically for entities and effects, could be applied elsewhere.
            /// -- offset = the amount to offset the point.
            /// -- position = position of e.g. a parent entity.
            /// -- sub-centre = centre of the object being aligned e.g. texture/effect centre/half of size.
            /// -- origin = centre of e.g. parent entity.
            /// -- rotation = the rotation in radians.
            /// </summary>
            /// <param name="offset"></param>
            /// <param name="position"></param>
            /// <param name="sub_centre"></param>
            /// <param name="origin"></param>
            /// <param name="rotation"></param>
            /// <returns></returns>
            public static Vector2 RotateVector(Vector2 offset, Vector2 position, Vector2 sub_centre, Vector2 origin, float rotation)
            {
                return Vector2.Transform(-origin + offset, Matrix.CreateRotationZ(rotation)) + origin + position - sub_centre;
            }
            /// <summary>
            /// Returns the angle of two vectors with +pi/2 (90 degrees).
            /// </summary>
            /// <param name="position"></param>
            /// <param name="faceThis"></param>
            /// <returns></returns>
            public static float AngleOfVectors(Vector2 a, Vector2 b)
            {
                return (float)Math.Atan2(b.Y - a.Y, b.X - a.X);
            }
            /// <summary>
            /// This function will check if the entity is nearly out of bounds of the map and then have it come out at the opposite side of the map.
            /// </summary>
            public static void NoBoundsCheck(Entity ent)
            {
                if (ent.Position.X <= ent.Width)
                {
                    ent.Position.X = ent.Gamemode._Map.Info.ActualTileWidth - (ent.Width + 1);
                }
                if (ent.Position.Y <= ent.Height)
                {
                    ent.Position.Y = ent.Gamemode._Map.Info.ActualTileHeight - (ent.Height + 1);
                }
                if (ent.Position.X >= ent.Gamemode._Map.Info.ActualTileWidth - ent.Width)
                {
                    ent.Position.X = ent.Width + 1;
                }
                if (ent.Position.Y >= ent.Gamemode._Map.Info.ActualTileHeight - ent.Height)
                {
                    ent.Position.Y = ent.Height + 1;
                }
            }
            /// <summary>
            /// This function is similar to NoBoundsCheck except if an entity goes out of bounds of the map, its destroyed.
            /// </summary>
            /// <param name="ent"></param>
            public static void NoBoundsCheck2(Entity ent)
            {
                if (ent.Position.X <= ent.Width)
                {
                    ent.Destroy();
                }
                if (ent.Position.Y <= ent.Height)
                {
                    ent.Destroy();
                }
                if (ent.Position.X >= ent.Gamemode._Map.Info.ActualTileWidth - ent.Width)
                {
                    ent.Destroy();
                }
                if (ent.Position.Y >= ent.Gamemode._Map.Info.ActualTileHeight - ent.Height)
                {
                    ent.Destroy();
                }
            }
        #endregion

        #region "Functions - Maths"
            /// <summary>
            /// Rounds a number to the lowest nearest multiple.
            /// </summary>
            /// <param name="number"></param>
            /// <param name="multiple"></param>
            /// <returns></returns>
            public static int Round(float number, float multiple)
            {
                if (number != 0 && multiple != 0)
                {
                    return Convert.ToInt32((Math.Round(number / multiple, 0) * multiple) - multiple);
                }
                else
                {
                    return 0;
                }
            }
            /// <summary>
            /// Returns the biggest of two numbers provided.
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static float Max(float a, float b)
            {
                if (a > b)
                {
                    return a;
                }
                else
                {
                    return b;
                }
            }
            /// <summary>
            /// Calculates a K/D ratio.
            /// </summary>
            /// <param name="kills"></param>
            /// <param name="deaths"></param>
            public static float CalculateKDRatio(float kills, float deaths)
            {
                if (kills == 0.0F)
                {
                    return 0;
                }
                else if (deaths == 0.0F)
                {
                    return kills;
                }
                else
                {
                    return kills / deaths;
                }
            }
        #endregion

        #region "Functions - String manipulation"
            // Converts a score to a string representation with commas.
            public static string ConvertToScore(float score)
            {
                return score.ToString("N");
            }
            /// <summary>
            /// Convers e.g. "Vector:232#323" to Vector2(232, 323).
            /// 
            /// Allowed types:
            /// String
            /// Float
            /// Bool
            /// Int
            /// Vector
            /// Texture
            /// 
            /// Format:
            /// Type:arg0#arg1#arg2...
            /// 
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static object StringToObject(Gamemode gm, string str)
            {
                string[] temp = str.Split(':');
                if (temp.Length > 1)
                {
                    string[] temp2 = temp[1].Split('#');
                    switch (temp[0])
                    {
                        case "string":
                        {
                            return str;
                        }
                        case "float":
                        {
                            return float.Parse(temp[1]);
                        }
                        case "bool":
                        {
                            return bool.Parse(temp[1]);
                        }
                        case "int":
                        {
                            return Convert.ToInt32(temp[1]);
                        }
                        case "vector":
                        {
                            return new Vector2(float.Parse(temp2[0]), float.Parse(temp2[1]));
                        }
                        case "texture":
                        {
                            return new Core.Texture(gm._Main, temp2[0], Convert.ToInt32(temp2[1]), gm);
                        }
                    }
                    return str;
                }
                else
                {
                    return str;
                }
            }
            public enum PathType
            {
                None = 0,
                Textures = 1,
                Audio = 2,
            }
            /// <summary>
            // Converts a string with path variables:
            // %MAIN% - The main textures folder of the game.
            // %GAMEMODE% - The path of the current gamemode's texture's folder.
            // %MAP% - The path of where the current map resides.
            //
            // The gamemode object can be null.
            /// </summary>
            /// <param name="path"></param>
            public static string Path(string path, PathType PT, Gamee.Main main, Gamemode gm)
            {
                if (main != null)
                {
                    string temp = "";
                    if (PT == PathType.Audio)
                    {
                        temp = "\\Sound";
                    }
                    else if (PT == PathType.Textures)
                    {
                        temp = "\\Textures";
                    }
                    path = path.Replace("%MAIN%", main.Root + "\\Content" + temp);
                    if (gm != null)
                    {
                        path = path.Replace("%GAMEMODE%", gm.Root + temp);
                        if (gm._Map != null)
                        {
                            if(gm._Map.Root != "")
                            {
                                path = path.Replace("%MAP%", gm._Map.Root + temp);
                            }
                        }
                    }
                }
                return path;
            }
        #endregion

        #region "Functions - Filesystem"
            /// <summary>
            /// Copies a directory to a destination.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="destination"></param>
            public static void CopyFolder(string source, string destination)
            {
                // Check the directory exists, else create it
                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);
                }
                // Used to retrieve information about the source directory
                DirectoryInfo D = new DirectoryInfo(source);
                // Copy files from the source directory
                foreach (FileInfo fi in D.GetFiles())
                {
                    File.Copy(fi.FullName, destination + "\\" + fi.Name);
                }
                // Copy directories by repeating this function
                foreach (DirectoryInfo di in D.GetDirectories())
                {
                    CopyFolder(di.FullName, destination + "\\" + di.Name);
                }
            }
        #endregion
    }
}