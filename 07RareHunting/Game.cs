﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Game.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   The Game class wraps up usage of a PhotonPeer, event handling and simple game logic. To make it work,
//   it must be integrated in a "gameloop", which regularly calls Game.Update().
//   A PhotonPeer is not thread safe, so make sure Update() is only called by one thread.
//
//   This sample should show how to get a player's position across to other players in the same room.
//   Each running instance will connect to Photon (with a local player / Peer), go into the same room and
//   move around.
//   Players have positions (updated regularly), name and color (updated only when someone joins).
//   Server side, Photon with the default Lite Application is used.
//   This class encapsulates the (simple!) logic for the Realtime Demo in a way that can be used on several
//   DotNet platforms (DotNet, Unity3D and Silverlight).
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading;

namespace _07RareHunting
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using ExitGames.Client.Photon;
    using ExitGames.Client.Photon.Lite;

    public class Game : IPhotonPeerListener
    {
        #region Members
        public Form1 form1;

        public string NameSpawn = "";
        public int NumberSpawn;
        public bool IsActive;
        public PopulatePlayerDB playerDB;

        public List<Data> IncomingData = new List<Data>(); 

        private readonly Thread updateThread;        

        //public string ServerAddress = "115.64.233.52:5055";
        public string ServerAddress = "115.64.233.52:5055";
        public ConnectionProtocol protocol = ConnectionProtocol.Udp;
        public string GameId = "RSRH07";  

        // LitePeer is a PhotonPeer, which allows you to connect and call operations on the Photon Server
        public LitePeer Peer;
        
        // as this is used while drawing (in another thread than the update loop), this must be locked
        public Dictionary<int, Player> Players = new Dictionary<int, Player>();
        
        public Player LocalPlayer = new Player(0);

        // networking / timing related settings
        internal int intervalDispatch = 2000; // interval between DispatchIncomingCommands() calls
        private int lastDispatch = Environment.TickCount;
        internal int intervalSend = 2000; // interval between SendOutgoingCommands() calls
        private int lastSend = Environment.TickCount;
        internal int intervalMove = 2000; // interval for auto-movement - each movement creates an OpRaiseEvent
        private int lastMove = Environment.TickCount;

        public delegate void DebugOutputDelegate(string debug);
        public DebugOutputDelegate DebugListeners;

        // update control variables for UI
        internal int lastUiUpdate = Environment.TickCount;
        private int intervalUiUpdate = 500;  // the UI update interval. this relates loosely to the dispatch interval
        public Action OnUpdate { get; set; }

        public static readonly string AppName = "LiteLobby";
        public static readonly string RoomNamePrefix = "RSRH";
        public static readonly string LobbyName = "RSLobby";        

        // each instance of this demo joins a random room. this initializes the name:
        public int RoomNumber = Environment.TickCount % 2;

        public enum ChatEventCode : byte { Message = 50, PrivateMessage = 51 }
        public enum ChatEventKey : byte { TextLine = 1 }

        #endregion

        #region Constructor and Gameloop

        public Game(DebugOutputDelegate debugDelegate, bool createGameLoopThread)
        {
            if (createGameLoopThread)
            {
                updateThread = new Thread(Gameloop);
                updateThread.IsBackground = true;
                updateThread.Start();
            }

            DebugListeners = debugDelegate;
        }

        private void Gameloop()
        {
            try
            {
                while (true)
                {
                    Update();
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Update()
        {
            if (Environment.TickCount - lastDispatch > intervalDispatch)
            {
                this.lastDispatch = Environment.TickCount;
                this.DispatchAll();
            }
            if (Environment.TickCount - lastSend > intervalSend)
            {
                this.lastSend = Environment.TickCount;
                this.SendOutgoingCommands();

            }
            if (Environment.TickCount - lastMove > intervalMove)
            {
                lastMove = Environment.TickCount;
                SendPosition();

            }           
            // Update call for windows phone UI-Thread
            if (Environment.TickCount - lastUiUpdate > intervalUiUpdate)
            {
                lastUiUpdate = Environment.TickCount;
                if (OnUpdate != null)
                {
                    OnUpdate();
                }
            }
        }

        #endregion

        // Here the connection to Photon is established (if not already connected).
        public bool Connect()
        {
            if (Peer == null)
            {
               Peer = new LiteLobbyPeer(this, protocol);
            }
            else if (Peer.PeerState != PeerStateValue.Disconnected)
            {
                return false;
            }

            // the next line would activate network simulation (only in the debug library):
            // this.Peer.NetworkSimulationSettings.IsSimulationEnabled = true;

            // The amount of debugging/logging from the Photon library can be set this way:
            this.Peer.DebugOut = DebugLevel.ERROR;

            if (!Peer.Connect(ServerAddress, AppName))
            {
                return false;
            }

            Players = new Dictionary<int, Player>();
            LocalPlayer = new Player(0);

            return true;
        }

        // Disconnect from the server.
        internal void Disconnect()
        {
            if (Peer != null)
            {
                Peer.Disconnect();	//this will dump all prepared outgoing data and immediately send a "disconnect"
            }
        }

        #region IPhotonPeerListener Members

        // This is the callback for the Photon library's debug output
        public void DebugReturn(DebugLevel level, string debug)
        {
            DebugListeners(debug); 
        }

        // Photon library callback for state changes (connect, disconnect, etc.)
        public void OnStatusChanged(StatusCode returnCode)
        {            
            // handle returnCodes for connect, disconnect and errors (non-operations)
            switch (returnCode)
            {
                case StatusCode.Connect:               
                    JoinRandomWithLobby();

                    //Create DB Object
                    playerDB = new PopulatePlayerDB();

                    break;
                case StatusCode.Disconnect:
                    LocalPlayer.playerID = 0;
                    lock (Players)
                    {
                        Players.Clear();
                    }

                    break;
                case StatusCode.ExceptionOnConnect:
                    LocalPlayer.playerID = 0;
                    lock (Players)
                    {
                        Players.Clear();
                    }
                    break;
                case StatusCode.Exception:
                    LocalPlayer.playerID = 0;
                    lock (Players)
                    {
                        Players.Clear();
                    }
                    break;
                case StatusCode.SendError:
                    LocalPlayer.playerID = 0;
                    lock (Players)
                    {
                        Players.Clear();
                    }
                    break;;
            }
        }

        // Photon library callback to get us operation results (if our operation was executed server-side)
        // Only called for reliable commands! Anything sent unreliable will not produce a response.
        public void OnOperationResponse(OperationResponse operationResponse)
        {

            // handle operation returns (aside from "join", this demo does not watch for returns)
            switch (operationResponse.OperationCode)
            {
                case LiteOpCode.Join:
                    // in case join fails, it won't contain the expected data. Let's print what's in there
                    if (operationResponse.ReturnCode != 0)
                    {
                        break;
                    }

                    // get the local player's numer from the returnvalues, get the player from the list and colorize it:
                    int actorNrReturnedForOpJoin = (int)operationResponse[LiteOpKey.ActorNr];
                    LocalPlayer.playerID = actorNrReturnedForOpJoin;
                    Players[LocalPlayer.playerID] = LocalPlayer;
                    
                    //Add Local Player to the PlayerDB after joining the room.
                    Console.WriteLine("Joined room. Adding you to the Database.");
                    playerDB.Add(new PlayerDB(LocalPlayer.playerID, "Name", "loc", DateTime.Now, true));
                    //playerDB.Update(new PlayerDB(LocalPlayer.playerID, "loc", "name", DateTime.Now));
                    break;
            }
        }

        // Called by Photon lib for each incoming event (player- and position-data in this demo, as well as joins and leaves).
        // Processed within PhotonPeer.DispatchIncomingCommands()!
        public void OnEvent(EventData photonEvent)
        {           

            int actorNr = (int)photonEvent[LiteEventKey.ActorNr];

            Player p;
            Players.TryGetValue(actorNr, out p);

            switch (photonEvent.Code)
            {
                case LiteEventCode.Join:
                    int[] actorsInGame = (int[])photonEvent[LiteEventKey.ActorList];
                    lock (Players)
                    {
                        foreach (int i in actorsInGame)
                        {
                            if (!Players.ContainsKey(i))
                            {
                                Players[i] = new Player(i);
                            }
                        }
                    }
                    LocalPlayer.SendPlayerInfo(Peer);
                    break;

                case LiteEventCode.Leave:                  
                    lock (Players)
                    {
                        Players.Remove(actorNr);
                    }
                    break;

                case (byte)Player.DemoEventCode.PlayerInfo:
                    // this is a custom event, which is defined by this application.
                    // if player is known (and it should be known!), update info
                    if (p != null)
                    {
                        p.SetInfo((Hashtable)photonEvent[LiteEventKey.CustomContent]);                       
                    }

                    PrintPlayers();
                    break;

                case 1:
                    var evData = photonEvent[LiteEventKey.Data] as Hashtable;
                    playerDB.Update(new PlayerDB(Convert.ToInt32(evData[1]), evData[2].ToString(), evData[3].ToString(), DateTime.Now, evData[4].Equals(IsActive)));                    
                    break;

            }
        }

        #endregion

        #region Game Handling

        // new for lobby sample: joins a room/game and attachs it to a lobby
        public void JoinRandomWithLobby()
        {
            string RoomName = RoomNamePrefix;

            lock (Players)
            {
                Players.Clear();
                LocalPlayer.playerID = 0;
            }

            ((LiteLobbyPeer) Peer).OpJoinFromLobby(RoomName, LobbyName, null, false);

        }

        // Will create the operation OpRaiseEvent with local player's position, calling peer.OpRaiseEvent(). 
        // This does not yet send the Operation but queue it in the peer. Sending is done by calling SendOutgoingCommands().
        public void SendPosition()
        {
            // dont move if player does not have a number or peer is not connected
            if (LocalPlayer == null || LocalPlayer.playerID == 0 || Peer == null)
            {
                return;
            }
           LocalPlayer.SendEvMove(Peer, NumberSpawn, NameSpawn, IsActive);
        }

        // Will create and queue the operation OpRaiseEvent with local player's color and name (not position).
        // Actually sent by a call to SendOutgoingCommands().
        // At this point, we could also use properties (so we don't have to re-send this data when someone joins).
        public void SendPlayerInfo()
        {
            // dont move if player does not have a number or peer is not connected
            if (LocalPlayer == null || LocalPlayer.playerID == 0 || Peer == null)
            {
                return;
            }

            LocalPlayer.SendPlayerInfo(Peer);

        }

        // Actually sends the outgoing data (which was previously queued in the PhotonPeer)
        public void SendOutgoingCommands()
        {
            if (Peer != null)
            {
                Peer.SendOutgoingCommands();
            }
        }

        public void DispatchAll()
        {
            while (Peer != null && Peer.DispatchIncomingCommands())
            {
                
            }
        }

        public void PrintPlayers()
        {
            string players = "Players: ";
            lock (Players)
            {
                foreach (Player p in Players.Values)
                {
                    players += p + ", ";
                }
            }

        }

        #endregion
    }
}
