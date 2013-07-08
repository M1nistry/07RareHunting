// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This class encapsulates a "player". It has members for position, name and color and provides methods to
//   change these values and to send them to Photon and other players.
//   Server side, Photon with the default Lite Application is used.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Windows.Forms;

namespace _07RareHunting
{
    using System;
    using System.Collections;
    using System.Reflection;

    using ExitGames.Client.Photon;
    using ExitGames.Client.Photon.Lite;

    public class Player
    {

        #region Variables

        public string Name { get; set; }
        public string Spawn { get; set; }

        public Form1 form1;

        /// <summary>
        /// This player's name. In the demo, we just set the platform's name and 
        /// use it only to display. Technically, the player is identified by her actorNr.
        /// </summary>
        /// <remarks>This is send to others in this.SendPlayerInfo()</remarks>
        public string name = string.Empty;

        public int playerID = -1;

        public static bool isSendReliable;

        

        #endregion

        #region Constants

        /// <summary>For this demo, we use a 16x16 tiles "game board" on which players can move.</summary>
        /// <remarks>The grid size is shared by realtime demos on other platforms, so you can "play" against a C++ Windows client.</remarks>
        public const byte GRIDSIZE = 16;

        /// <summary>
        /// Using the Lite logic on the server, this demo can send just about any key-value pair in an event.
        /// To keep things lean, we use event keys of type byte.
        /// </summary>
        /// <remarks>
        /// We don't define a type for the value anywhere. This is done "by convention of this demo". Again, the client
        /// is open to send whatever it likes but we make sure to send the same type as we expect in Game.OnEvent().
        /// </remarks>
        public enum DemoEventKey : byte { PlayerPositionX = 0, PlayerPositionY = 1, PlayerName = 2, PlayerColor = 3 }

        /// <summary>
        /// Using the Lite logic on the server, this demo can send just about any event it wants to.
        /// The event and event-content is purely defined client side. The server does not understand or know this, 
        /// which is ok. We don't want to check for cheating and don't persist data in this demo.
        /// </summary>
        public enum DemoEventCode : byte { PlayerInfo = 0, PlayerMove = 1 }
        
        #endregion

        #region Player Control

        public Player(int actorNr)
        {
            playerID = actorNr;

            
            // we create the local player as actornumber 0. this is not used (ever) in a room. random init is only for "our" player:
            if (actorNr < 1)
            {
                name = "dotnet";
            }
        }

        internal void SendPlayerInfo(LitePeer peer)
        {
            if (peer == null)
            {
                return;
            }

            // Setting up the content of the event. Here we want to send a player's info: name and color.
            Hashtable evInfo = new Hashtable();
            evInfo.Add((byte)DemoEventKey.PlayerName, name);

            // The event's code must be of type byte, so we have to cast it. We do this above as well, to get routine ;)
            peer.OpRaiseEvent((byte)DemoEventCode.PlayerInfo, evInfo, true);
        }

        internal void SetInfo(Hashtable customEventContent)
        {
            name = (string)customEventContent[(byte)DemoEventKey.PlayerName];
        }

        public void SendEvMove(LitePeer peer, int spawnNumber, string spawnName, bool IsActive)
        {
            if (peer == null) return;

            var updateStatus = new Hashtable();

                updateStatus.Add(1, playerID);
                updateStatus.Add(2, spawnNumber);
                updateStatus.Add(3, spawnName);
                updateStatus.Add(4, IsActive);

                peer.OpRaiseEvent(1, updateStatus, isSendReliable);
        }        
        #endregion
    }
}