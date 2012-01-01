using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core
{
    public class Trail
    {
        // Purpose: creates a trail of effects, useful for e.g. thruster engine smoke etc.
        #region "Variables"
            // Parent entity
            Entity _Parent;
            // Trail texture
            Texture _Texture;
            // Length of texture
            float _Length;
            // The scale of which the effect starts at e.g. 25% would be 0.25F.
            float _StartScale;
            // Same as above but this is the scale at which the effect ends.
            float _EndScale;
            // The offset of the trail, with 0,0 being the centre of the entity.
            Vector2 _Offset;
            // An array of all the active effects.
            List<Effect> Points = new List<Effect>();
            // An array of all the effects to be removed.
            List<Effect> Remove = new List<Effect>();
        #endregion

        public Trail(Entity Parent, Texture Txt, float StartScale, float EndScale, float Length, Vector2 Offset)
        {
            _Texture = Txt;
            _Offset = Offset;
            _StartScale = StartScale;
            _EndScale = EndScale;
            _Length = Length;
            _Parent = Parent;
            Parent.ENT_Moved += new Entity.Ev_Moved(Parent_ENT_Moved);
        }
        void Parent_ENT_Moved(Entity ent, Vector2 OldPosition, Vector2 NewPosition)
        {
            // Check the limit has not been hit
            if (Points.Count == _Length)
            {
                Points.RemoveAt(0);
            }
            // Add new effect
            Points.Add(new Effect(_Texture, Common.RotateVector(_Offset, NewPosition, _Texture.Centre, ent.Centre, ent.rotation), _Parent.rotation, _StartScale, _EndScale, (int)_Length * 4, Vector2.Zero));
        }
        public void Logic()
        {
            // Runlogic for each effect and see if it needs to be removed
            foreach (Effect effect in Points)
            {
                effect.Logic();
                if (effect.Finished)
                {
                    Remove.Add(effect);
                }
            }
            // Remove finished effects
            foreach (Effect effect in Remove)
            {
                Points.Remove(effect);
            }
            Remove.Clear();
        }

        public void Draw(SpriteBatch sb)
        {
            // Draws each effect
            foreach (Effect effect in Points)
            {
                effect.Draw(sb);
            }
        }
    }
}