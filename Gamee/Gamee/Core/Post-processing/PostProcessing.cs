using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class PostProcessing
    {
        // Purpose: allows HLSL shaders to be used and applied to the game, which is managed by this class.

        #region "Variables"
            Gamee.Main Main;
            // The effect applied for post-processing.
            public Microsoft.Xna.Framework.Graphics.Effect Effect;
            // Contains the name of the current effect
            public string CurrentEffect = "None";
        #endregion

        #region "Core"
            public PostProcessing(Gamee.Main main)
            {
                Main = main;
            }
            /// <summary>
            /// Safely disposes the class's variables.
            /// </summary>
            public void Destroy()
            {
                DestroyEffect();
                Main = null;
            }
        #endregion

        #region "Functions - load, start, stop"
            public void LoadEffect(string name)
            {
                // Sets the current effect being used
                CurrentEffect = name;
                // Phases the type into an enum for loading
                if (name == "None" || name == "0" || name == "" || !File.Exists(Main.Root + "\\Content\\Effects\\" + name + ".fx"))
                {
                    return;
                }
                else
                {
                    CompiledEffect Temp = Microsoft.Xna.Framework.Graphics.Effect.CompileEffectFromFile(Main.Root + "\\Content\\Effects\\" + name + ".fx", null, null, Microsoft.Xna.Framework.Graphics.CompilerOptions.None, Microsoft.Xna.Framework.TargetPlatform.Windows);
                    Effect = new Microsoft.Xna.Framework.Graphics.Effect(Main.GraphicsDevice, Temp.GetEffectCode(), CompilerOptions.None, null);
                }
            }
            /// <summary>
            /// Destroys the currnet effect loaded.
            /// </summary>
            public void DestroyEffect()
            {
                if (Effect != null)
                {
                    Effect.Dispose();
                    Effect = null;
                }
            }
            /// <summary>
            /// Starts the post-process effect.
            /// </summary>
            public void Start()
            {
                if (Effect != null)
                {
                    Effect.Begin();
                    Effect.CurrentTechnique.Passes[0].Begin();
                }
            }
            /// <summary>
            /// Stops the post-process effect.
            /// </summary>
            public void Stop()
            {
                if (Effect != null)
                {
                    Effect.CurrentTechnique.Passes[0].End();
                    Effect.End();
                }
            }
        #endregion
    }
}