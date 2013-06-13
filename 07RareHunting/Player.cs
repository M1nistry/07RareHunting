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

        private Form1 form1;

        /// <summary>
        /// This player's name. In the demo, we just set the platform's name and 
        /// use it only to display. Technically, the player is identified by her actorNr.
        /// </summary>
        /// <remarks>This is send to others in this.SendPlayerInfo()</remarks>
        public string name = string.Empty;

        /// <summary>
        /// This player's actorNumber.
        /// Per room, the actorNr is unique for a player / user and is used as identifier!
        /// The Lite server logic will add a user's actorNr to events that are raised.
        /// </summary>
        /// <remarks>As the server adds this number to events automatically, it is never actually send by this client.</remarks>
        public int playerID = -1;

        /// <summary>Position x-value.</summary>
        /// <remarks>This is send to others in this.SendEvMove()</remarks>
        public int posX;

        /// <summary>Position y-value.</summary>
        /// <remarks>This is send to others in this.SendEvMove()</remarks>
        public int posY;

        /// <summary>This player's color (as RGB integer).</summary>
        /// <remarks>This is send to others in this.SendPlayerInfo()</remarks>
        public int color;

        /// <summary>
        /// Controls if the local player will send his position updates reliable or unreliable.
        /// In a real game, you would send some events reliable and some unreliable, depending
        /// on the type of info you send. See SendEvMove().
        /// Position data is a case for unreliable updates: Each position is outdated soon and 
        /// completely replaced with the next event. A missing update won't matter.
        /// The Player info (see SendPlayerInfo) is a sample for data you should send reliable
        /// in a real game: The username is not sent every update and it's bad if this would be
        /// lost.
        /// </summary>
        /// <remarks>
        /// Sending events reliable make sure they arrive on the other side but in case of
        /// packet loss, this might induce a delay (until the reliable package is finally arriving).
        /// </remarks>
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

        /// <summary>
        /// A player represents some user's client in the room we're in. 
        /// </summary>
        /// <param name="actorNr">In a room, each player has a number to be identified. We initialize out local Player before we get a actor number (with -1 then).</param>
        public Player(int actorNr)
        {
            this.playerID = actorNr;

            this.form1 = form1;
            
            // we create the local player as actornumber 0. this is not used (ever) in a room. random init is only for "our" player:
            if (actorNr < 1)
            {
                this.posX = SupportClass.ThreadSafeRandom.Next() % GRIDSIZE;
                this.posY = SupportClass.ThreadSafeRandom.Next() % GRIDSIZE;
                this.color = (int)SupportClass.ThreadSafeRandom.Next();
                this.name = "dotnet";
            }
        }

        /// <summary>
        /// This method puts the "player info" into a Hashtable (a serializable Datatype for Photon)
        /// and calls LitePeer.OpRaiseEvent() to make the server send an event to others in the room.
        /// </summary>
        /// <remarks>The event we send here is handled by each client's Game.OnEvent().</remarks>
        /// <param name="peer"></param>
        internal void SendPlayerInfo(LitePeer peer)
        {
            if (peer == null)
            {
                return;
            }

            // Setting up the content of the event. Here we want to send a player's info: name and color.
            Hashtable evInfo = new Hashtable();
            evInfo.Add((byte)DemoEventKey.PlayerName, this.name);
            evInfo.Add((byte)DemoEventKey.PlayerColor, this.color);

            // The event's code must be of type byte, so we have to cast it. We do this above as well, to get routine ;)
            peer.OpRaiseEvent((byte)DemoEventCode.PlayerInfo, evInfo, true);
        }

        /// <summary>
        /// When Game.OnEvent() receives a info event (sent by SendPlayerInfo), it's content is used here.
        /// </summary>
        /// <param name="customEventContent"></param>
        internal void SetInfo(Hashtable customEventContent)
        {
            this.name = (string)customEventContent[(byte)DemoEventKey.PlayerName];
            this.color = (Int32)customEventContent[(byte)DemoEventKey.PlayerColor];
        }

        /// <summary>
        /// Raises an event with the player's position data. 
        /// "isSendReliable"        decides if these events are reliable or unreliable.
        /// "Game.RaiseEncrypted"   decided if the event is encrypted.
        /// 
        /// Once more: Neither sending reliable nor encrypting this event makes sense in a game.
        /// Both is just done as showcase!
        /// </summary>
        /// <remarks>
        /// Each running client will know many players, but only one is the "local" one, which
        /// actually sends it's position! See Game.SendPostion(), which choses the local player only.
        /// </remarks>
        /// <param name="peer"></param>
        public void SendEvMove(LitePeer peer, string spawnNumber, string playerName)
        {

            if (peer == null)
            {
                return;
            }
            var updateStatus = new Hashtable();
            updateStatus.Add(spawnNumber, playerName);            

            // if encryption is turned off, we simply use OpRaiseEvent
            peer.OpRaiseEvent((byte)DemoEventCode.PlayerMove, updateStatus, isSendReliable, (byte)0);                                                               
        }
        
        #endregion
    }
}