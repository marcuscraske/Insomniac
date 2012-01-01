using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class ASMDestroyer : Bullet
    {
        #region "Variables"
            public Core.Trail Trail;
        #endregion

        public ASMDestroyer(Core.Gamemode gm, Core.Player ply, Core.Entity parent):base(gm, gm._TexturesArray.GetTexture("ASM-Destroyer"), 30, 40, parent.Enemy)
        {
            // Create texture
            Trail = new Core.Trail(this, Gamemode._TexturesArray.GetTexture("ASM-Destroyer_Trail"), 0.1F, 4.0F, 100, new Vector2(Centre.X, Height));
            // Set bullet properties
            DisplayName = "ASM-Destroyer";
            Player = ply;
            BulletDamage = -500.0F;
            BulletRadius = 0.0F;
            BulletLifespan = 8000.0F;
            Fire(parent, 8.0F);
            // Play sound effect
            Gamemode.Audio_Control.PlayCue("Fireball");
        }
        public override void Logic()
        {
            Trail.Logic();
            base.Logic();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Trail.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
        public override void Destroy()
        {
            Trail = null;
            base.Destroy();
        }
    }
}