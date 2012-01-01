using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class Nuke:Bullet
    {
        #region "Variables"
            public Core.Trail Trail;
        #endregion

        public Nuke(Core.Gamemode gm, Core.Player ply, Core.Entity parent):base(gm, gm._TexturesArray.GetTexture("Nuke_Missile"), 20, 20, parent.Enemy)
        {
            // Create texture
            Trail = new Core.Trail(this, Gamemode._TexturesArray.GetTexture("Nuke_Trail"), 0.5F, 1.0F, 60, new Vector2(Centre.X, Height));
            // Set bullet properties
            DisplayName = "Nuke";
            BulletLifespan = 30000;
            BulletDamage = -8000;
            BulletRadius = 1000;
            Player = ply;
            Fire(parent, 12.0F);
            // Play sound effect
            Gamemode.Audio_Control.PlayCue("Nuke");
        }
        public override void Bullet_ENT_Collided(Core.Entity Victim, Core.Entity Collider)
        {
            if (Enemy == Collider.Enemy)
            {
                if (Collider.PhySolid)
                {
                    // Bounce (we have hit a friendly)
                    Velocity = -Velocity;
                }
            }
            else if (Alive && Collider.PhySolid)
            {
                // Blow-up (we have hit an enemy)
                Core.Effect.CreateExplosion(Gamemode, Position + Centre, 5.0F, 0.5F, 6.0F, 100, Gamemode._TexturesArray.GetTexture("Explosion_Main"), Gamemode._TexturesArray.GetTexture("Explosion_Flare"), "NukeExplosion");
                Gamemode.Effects.Add(new Core.Effect(Gamemode._TexturesArray.GetTexture("Nuclear_Shockwave"), Position + Centre - new Vector2(500.0F, 500.0F), 0.0F, 0.1F, 3.0F, 5000, Vector2.Zero));
                // Cause damage
                Gamemode._Physics.HealthRadius(this, Position + Centre, BulletRadius, BulletDamage);
                // Destroy
                Destroy();
            }
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