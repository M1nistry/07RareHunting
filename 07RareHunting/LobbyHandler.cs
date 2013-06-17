// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LobbyHandler.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This class shows how to handle game lobbies in Photon's Lite Lobby Application.
//   A separate LiteLobbyPeer is used to join a lobby and get it's updates. Start and 
//   stop multiple clients to trigger changes in per-game player-counts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace _07RareHunting
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ExitGames.Client.Photon;
    using ExitGames.Client.Photon.Lite;

    class LobbyHandler : IPhotonPeerListener
    {
        public LiteLobbyPeer peer;
        private Hashtable gameList = new Hashtable();
        private Form1 form;

        private string ipPort;
        private string appName;
        private string lobbyName;

        private System.Timers.Timer timer;

        public LobbyHandler(string ipPort, ConnectionProtocol protocol, string appName, string lobbyName, Form1 callingForm)
        {
            this.ipPort = ipPort;
            this.appName = appName;
            this.form = callingForm;
            this.lobbyName = lobbyName;

            this.peer = new LiteLobbyPeer(this, protocol);
            this.peer.Connect(this.ipPort, this.appName);
        }

        public void Service()
        {
            this.peer.Service();
        }

        public void Stop()
        {
            this.peer.Disconnect();
        }


        #region Implementation of IPhotonPeerListener

        public void DebugReturn(string debug)
        {
            this.form.DebugReturn("Lobby: " + debug);
        }

        public void DebugReturn(DebugLevel level, string debug)
        {
            this.form.DebugReturn("Lobby: " + debug);
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            //inside a lobby this sample doesn't use operations
            //any returns are ignored at the moment
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            DebugReturn("OnStatusChanged():" + (StatusCode)statusCode);

            //handle returnCodes for connect, disconnect and errors (non-operations)
            switch ((StatusCode)statusCode)
            {
                case StatusCode.Connect:
                    this.DebugReturn("Connect(ed)");
                    form.toolStripConnection.Text = "Connected";
                    this.peer.OpJoin(this.lobbyName);   // The LobbyHandler simply joins the lobby to get updates from it
                   
                    break;
                case StatusCode.Disconnect:
                    this.DebugReturn("Disconnect(ed) Peer.state: " + peer.PeerState);
                    form.toolStripConnection.Text = "Disconnected";
                    this.timer = null;
                    break;
                case StatusCode.ExceptionOnConnect:
                    this.DebugReturn("ExceptionOnConnect(ed) Peer.state: " + peer.PeerState);
                    
                    this.timer = null;
                    break;
                case StatusCode.Exception:
                    this.DebugReturn("Exception(ed) Peer.state: " + peer.PeerState);
                    this.timer = null;
                    break;
                default:
                    this.DebugReturn("OnStatusChanged: " + statusCode);
                    break;
            }
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                    // The Lobbies in LiteLobby will send two types of events with room-lists. 
                    // This is how you handle them:

                case (byte) LiteLobbyPeer.LiteLobbyEventCode.RoomList:
                case (byte) LiteLobbyPeer.LiteLobbyEventCode.RoomListUpdate:
                    {
                        Hashtable customContent;
                        customContent = (Hashtable) photonEvent[(byte) LiteEventKey.CustomContent];

                        foreach (string key in customContent.Keys)
                        {
                            //each key is a room name. each value of a key is the current player count
                            //we still list rooms when they are known and have 0 players. those could be removed
                            this.gameList[key] = customContent[key];
                        }

                        List<string> list = new List<string>();
                        foreach (string key in this.gameList.Keys)
                        {
                            list.Add((string) key + ": " + this.gameList[key]);
                        }
                        break;
                    }
            }

        }
        #endregion

        #region Timer Methods

        //creates a timer on demand to make the Lobby handling standalone
        public void StartService()
        {
            this.timer = new System.Timers.Timer();

            this.timer.Interval = 500;  //relative few updates as listing the rooms is not high prio
            this.timer.Elapsed += this.TimedService;
            this.timer.Start();
        }

        private void TimedService(object sender, EventArgs e)
        {
            this.Service();
        }

        #endregion
    }
}