using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spacegame
{
    public class Portal : Core.Entity
    {
        // Purpose: allows entities to teleport accross the map to a connect Portal;
        // portals connect by having the same ID.
        #region "Enums"
        // The state of the portal
        public enum PortalState
        {
            Disconnected = 0,
            Connected = 1,
            Charging = 2,
            Teleporting = 3,
        }
        #endregion

        #region "Variables"
            // When portals connect, one is assigned the mother (for
            // performance and because if both portals worked independently
            // they could get out of sync).
            bool Mother = false;
            // Indicates if the portal has attempted to search for a connection.
            bool Searched = false;
            // The portal thats linked to this portal.
            Portal link = null;
            // The texture of when the portal is connected but idle.
            public Core.Texture Connected;
            // The texture of when the portal is disconnected.
            public Core.Texture Disconnected;
            // The texture of when the portal is teleporting.
            public Core.Texture Teleporting;
            // The time spent charging the portal.
            float ChargeTime = 7000;
            // The time the portal spends idle.
            float TeleportTime = 5000;
            // Used to calculate time.
            float Tick = Environment.TickCount;
            // The current state of this portal is stored here.
            PortalState State = PortalState.Disconnected;
            // This sound emitter only plays the portal charging sound effect if a player is near-by.
            Core.DistanceSoundEmitter ChargeSound;
        #endregion

        public Portal(Core.Gamemode gm, Core.Texture txt, int width, int height, bool enemy):base(gm, txt, width, height, enemy)
        {
            PhyStill = true;
            PhySolid = false;
            Godmode = true;
            Attackable = false;
            Connected = new Core.Texture(gm._Main, "%GAMEMODE%\\Props\\Portal-Connected", 200, gm);
            Disconnected = new Core.Texture(gm._Main, "%GAMEMODE%\\Props\\Portal-Disconnected", 200, gm);
            Teleporting = new Core.Texture(gm._Main, "%GAMEMODE%\\Props\\Portal-Teleporting", 200, gm);
        }
        // Responsible for disposing and releasing all the resources used by the portal.
        public override void Dispose()
        {
            if (link != null)
            {
                link = null;
            }
            Connected._Texture.Dispose();
            Connected = null;
            Disconnected._Texture.Dispose();
            Disconnected = null;
            Teleporting._Texture.Dispose();
            Teleporting = null;
            Core.Sound.Dispose(ChargeSound);
            base.Dispose();
        }
        public override void Logic()
        {
            // Execute entity logic
            base.Logic();
            // Execute sound emitter logic
            if (ChargeSound != null)
            {
                ChargeSound.Logic();
            }
            if (!Searched)
            {
                ReconnectPortal();
                Searched = true;
            }
            // Check theres a connection, else break
            if (link == null)
            {
                // Gate is disconnected
                Texture = Disconnected;
                State = PortalState.Disconnected;
                return;
            }
            if (!Mother)
            {
                // If its not the mother gate, it has no control hence return
                return;
            }
            if (State == PortalState.Connected)
            {
                // Gate is idle charging
                if (Environment.TickCount - Tick >= ChargeTime)
                {
                    Tick = Environment.TickCount;
                    State = PortalState.Teleporting;
                    Texture = Teleporting;
                    link.Texture = link.Teleporting;
                }
                else
                {
                    Texture = Connected;
                }
            }
            else
            {
                // Gate is teleporting/tele-charging
                if (Environment.TickCount - Tick >= TeleportTime)
                {
                    // Teleport any objects within the portal
                    Teleport();
                    // Reset state to connected/idle charging
                    Tick = Environment.TickCount;
                    State = PortalState.Connected;
                    Texture = Connected;
                    link.Texture = link.Connected;
                    // Reset the sound emitter
                    ChargeSound._EMITTER.CurrentPosition = 0;
                    link.ChargeSound._EMITTER.CurrentPosition = 0;
                }
                else
                {
                    Texture = Teleporting;
                    link.Texture = link.Teleporting;
                }
            }
        }
        /// <summary>
        /// The portal refinds its soul portal.
        /// </summary>
        public void ReconnectPortal()
        {
            // ID/name is invalid, therefore return
            if (ID == "")
            {
                return;
            }
            // Go through each entity until an equal ID is found
            foreach (Core.Entity ent in Gamemode._Entities)
            {
                if (ent != this && ent.ID == ID && ent.GetType().Name == "Portal")
                {
                    // Reset our tick (we're the mother and will control teleportations)
                    Tick = Environment.TickCount;
                    link = (Portal)ent;
                    ((Portal)ent).link = this;
                    ((Portal)ent).Mother = false;
                    ((Portal)ent).Searched = true;
                    // Reset the sound emitter
                    ChargeSound = new Core.DistanceSoundEmitter(Gamemode, Position + Centre, 800, "%GAMEMODE%\\Props\\Portal.mp3", true, true);
                    link.ChargeSound = new Core.DistanceSoundEmitter(Gamemode, link.Position + link.Centre, 800, "%GAMEMODE%\\Props\\Portal.mp3", true, true);
                    Mother = true;
                    return;
                }
            }
        }
        /// <summary>
        /// Teleports any nearby entities to the linked gate.
        /// </summary>
        public void Teleport()
        {
            // Check the linked gate is not null, else exit
            if (link == null)
            {
                return;
            }
            // Store the ents near the links gate into an array
            List<Core.Entity> LinkTemp = Gamemode._Physics.EntsInRadius(link.Position + link.Centre, Core.Common.Max(link.Width, link.Height) / 2);
            // Teleport our entities first
            foreach (Core.Entity ent in Gamemode._Physics.EntsInRadius(Position + Centre, Core.Common.Max(Width, Height) / 2))
            {
                if (!ent.PhyStill && ent != this && ent.GetType().Name != "Portal")
                {
                    // Teleport relative to the position of the ent at this portal to the end portal
                    ent.Position = link.Position + (ent.Position - Position);
                }
            }
            // Teleport the links entities
            foreach (Core.Entity ent in LinkTemp)
            {
                if (!ent.PhyStill && ent != link && ent.GetType().Name != "Portal")
                {
                    ent.Position = Position + (ent.Position - link.Position);
                }
            }
        }
    }
}