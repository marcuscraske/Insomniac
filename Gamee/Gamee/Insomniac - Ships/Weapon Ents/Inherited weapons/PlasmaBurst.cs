using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class PlasmaBurst:Bullet
    {
        #region "Variables"
            public Core.Trail Trail;
        #endregion

        public PlasmaBurst(Core.Gamemode gm, Core.Player ply, Core.Entity parent):base(gm, gm._TexturesArray.GetTexture("LaserBullet"), 25, 25, parent.Enemy)
        {
            // Create trail
            Trail = new Core.Trail(this, Gamemode._TexturesArray.GetTexture("LaserBulletTrail"), 1.0F, 1.0F, 40, new Vector2(Centre.X, Height));
            // Set bullet properties
            BulletDamage = -20.0F;
            BulletRadius = 0.0F;
            BulletLifespan = 2500.0F;
            Player = ply;
            Fire(parent, 10.0F);
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