// --------------------------------------------------------------------------------------------------------------------
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

using System.Text.RegularExpressions;
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
        public string NumberSpawn = "";

        public List<Data> IncomingData = new List<Data>(); 

        private readonly Thread updateThread;        

        public string ServerAddress = "115.64.233.52:5055";	// this must be replaced with an IP of your server  in form of: "ip:port"
        public ConnectionProtocol protocol = ConnectionProtocol.Udp;                     // Photon can use UDP (default) or TCP as underlying protocol
        public string GameId = "realtimeDemoGame000";   // Name of the room this demo joins. Used in other SDKs as well

        /// <summary>LitePeer is a PhotonPeer, which allows you to connect and call operations on the Photon Server</summary>
        public LitePeer Peer;
        
        // as this is used while drawing (in another thread than the update loop), this must be locked
        public Dictionary<int, Player> Players = new Dictionary<int, Player>();
        
        public Player LocalPlayer = new Player(0);

        // networking / timing related settings
        internal int intervalDispatch = 500; // interval between DispatchIncomingCommands() calls
        private int lastDispatch = Environment.TickCount;
        internal int intervalSend = 500; // interval between SendOutgoingCommands() calls
        private int lastSend = Environment.TickCount;
        internal int intervalMove = 500; // interval for auto-movement - each movement creates an OpRaiseEvent
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
                this.updateThread = new Thread(this.Gameloop);
                this.updateThread.IsBackground = true;
                this.updateThread.Start();
            }

            this.DebugListeners = debugDelegate;
        }

        private void Gameloop()
        {
            try
            {
                while (true)
                {
                    this.Update();
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                this.DebugReturn("Exception in Gameloop: " + ex.ToString());
            }
        }

        /// <summary>
        /// Update must be called by a gameloop (a single thread), so it can handle
        /// automatic movement and networking.
        /// </summary>
        public void Update()
        {
            if (Environment.TickCount - this.lastDispatch > this.intervalDispatch)
            {
                this.lastDispatch = Environment.TickCount;
                this.DispatchAll();
            }
            if (Environment.TickCount - this.lastSend > this.intervalSend)
            {
                this.lastSend = Environment.TickCount;
                this.SendOutgoingCommands();

            }
            if (Environment.TickCount - this.lastMove > this.intervalMove)
            {
                this.lastMove = Environment.TickCount;
                this.SendPosition();

            }           
            // Update call for windows phone UI-Thread
            if (Environment.TickCount - this.lastUiUpdate > this.intervalUiUpdate)
            {
                this.lastUiUpdate = Environment.TickCount;
                if (this.OnUpdate != null)
                {
                    this.OnUpdate();
                }
            }
        }

        #endregion

        // Here the connection to Photon is established (if not already connected).
        public bool Connect()
        {
            if (this.Peer == null)
            {
                this.Peer = new LiteLobbyPeer(this, this.protocol);
            }
            else if (this.Peer.PeerState != PeerStateValue.Disconnected)
            {
                this.DebugReturn("already connected! disconnect first.");
                return false;
            }

            // the next line would activate network simulation (only in the debug library):
            // this.Peer.NetworkSimulationSettings.IsSimulationEnabled = true;

            // The amount of debugging/logging from the Photon library can be set this way:
            this.Peer.DebugOut = DebugLevel.ERROR;

            if (!this.Peer.Connect(this.ServerAddress, AppName))
            {
                this.DebugReturn("Couldn't connect. Check debug log.");
                return false;
            }

            this.Players = new Dictionary<int, Player>();
            this.LocalPlayer = new Player(0);

            return true;
        }

        // Disconnect from the server.
        internal void Disconnect()
        {
            if (this.Peer != null)
            {
                this.Peer.Disconnect();	//this will dump all prepared outgoing data and immediately send a "disconnect"
            }
        }

        #region IPhotonPeerListener Members

        // This is the callback for the Photon library's debug output
        public void DebugReturn(DebugLevel level, string debug)
        {
            this.DebugListeners(debug); // This demo simply ignores the debug level
        }

        // Photon library callback for state changes (connect, disconnect, etc.)
        // Processed within PhotonPeer.DispatchIncomingCommands()!
        public void OnStatusChanged(StatusCode returnCode)
        {            
            // handle returnCodes for connect, disconnect and errors (non-operations)
            switch (returnCode)
            {
                case StatusCode.Connect:
                    this.DebugReturn("Connect(ed)");
                    this.JoinRandomWithLobby();
                    break;
                case StatusCode.Disconnect:
                    this.DebugReturn("Disconnect(ed) Peer.state: " + this.Peer.PeerState);
                    this.LocalPlayer.playerID = 0;
                    lock (this.Players)
                    {
                    this.Players.Clear();
                    }
                    break;
                case StatusCode.ExceptionOnConnect:
                    this.DebugReturn("ExceptionOnConnect. Peer.state: " + this.Peer.PeerState);
                    if (this.ServerAddress.StartsWith("127.0.0.1"))
                    {
                        this.DebugReturn("The server address is '127.0.0.1'. This won't work on a device. Adjust this to your server's address.");
                    }
                    this.LocalPlayer.playerID = 0;
                    lock (this.Players)
                    {
                    this.Players.Clear();
                    }
                    break;
                case StatusCode.Exception:
                    this.DebugReturn("Exception. Peer.state: " + this.Peer.PeerState);
                    this.LocalPlayer.playerID = 0;
                    lock (this.Players)
                    {
                    this.Players.Clear();
                    }
                    break;
                case StatusCode.SendError:
                    this.DebugReturn("SendError! Peer.state: " + this.Peer.PeerState);
                    this.LocalPlayer.playerID = 0;
                    lock (this.Players)
                    {
                    this.Players.Clear();
                    }
                    break;
                case StatusCode.EncryptionEstablished:
                    this.DebugReturn("Encryption now available: " + this.Peer.IsEncryptionAvailable);
                    break;
                case StatusCode.EncryptionFailedToEstablish:
                    this.DebugReturn("Encryption initialisation failed. Check previous debug output.");
                    break;
                case StatusCode.QueueIncomingReliableWarning:
                    Console.WriteLine("Status Code is working");
                    break;
                default:
                    this.DebugReturn("OnStatusChanged(): " + returnCode);
                    break;
            }
        }

        // Photon library callback to get us operation results (if our operation was executed server-side)
        // Only called for reliable commands! Anything sent unreliable will not produce a response.
        // Processed within PhotonPeer.DispatchIncomingCommands()!
        public void OnOperationResponse(OperationResponse operationResponse)
        {
            if (operationResponse.OperationCode != (byte)LiteOpCode.RaiseEvent)
            {
                this.DebugReturn("OnOperationResponse() " + operationResponse.ToStringFull());
            }

            // handle operation returns (aside from "join", this demo does not watch for returns)
            switch (operationResponse.OperationCode)
            {
                case (byte)LiteOpCode.Join:
                    // in case join fails, it won't contain the expected data. Let's print what's in there
                    if (operationResponse.ReturnCode != 0)
                    {
                        this.DebugReturn("Join failed. Response: " + operationResponse.ToStringFull());
                        break;
                    }

                    // get the local player's numer from the returnvalues, get the player from the list and colorize it:
                    int actorNrReturnedForOpJoin = (int)operationResponse[(byte)LiteOpKey.ActorNr];
                    this.LocalPlayer.playerID = actorNrReturnedForOpJoin;

                    // LocalPlayer.generateColor();
                    this.Players[this.LocalPlayer.playerID] = this.LocalPlayer;
                    this.DebugReturn("LocalPlayer: " + this.LocalPlayer);
                    break;
            }
        }

        // Called by Photon lib for each incoming event (player- and position-data in this demo, as well as joins and leaves).
        // Processed within PhotonPeer.DispatchIncomingCommands()!
        public void OnEvent(EventData photonEvent)
        {
            StatusCode statuS = new StatusCode();
            //debug output is OK for all but the most rapidly sent events (in our case: EV_MOVE)

            this.DebugReturn("OnEvent() " + photonEvent.ToStringFull());

            int actorNr = (int)photonEvent[(byte)LiteEventKey.ActorNr];

            // get the player that raised this event
            Player p;
            this.Players.TryGetValue(actorNr, out p);

            switch (photonEvent.Code)
            {
                case (byte)LiteEventCode.Join:
                    // Event is defined by Lite. A peer entered the room. It could be this peer!
                    // This event provides the current list of actors and a actorNumber of the player who is new.

                    // get the list of current players and check it against local list - create any that's not yet there
                    int[] actorsInGame = (int[])photonEvent[(byte)LiteEventKey.ActorList];
                    lock (this.Players)
                    {
                        foreach (int i in actorsInGame)
                        {
                            if (!this.Players.ContainsKey(i))
                            {
                                this.Players[i] = new Player(i);
                            }
                        }
                    }

                    this.LocalPlayer.SendPlayerInfo(this.Peer); // the new peers does not have our info, so send it again
                    break;

                case (byte)LiteEventCode.Leave:
                    // Event is defined by Lite. Someone left the room.
                    lock (this.Players)
                    {
                        this.Players.Remove(actorNr);
                    }
                    break;

                case (byte)Player.DemoEventCode.PlayerInfo:
                    // this is a custom event, which is defined by this application.
                    // if player is known (and it should be known!), update info
                    if (p != null)
                    {
                        p.SetInfo((Hashtable)photonEvent[(byte)LiteEventKey.CustomContent]);
                    }
                    else
                    {
                        this.DebugReturn("did not find player to set info: " + actorNr);
                    }

                    this.PrintPlayers();
                    break;
                case 1:
                    //This case statement receives the other clients data and saves it
                    Hashtable evData = photonEvent[(byte) LiteEventKey.Data] as Hashtable;                    
                    Data TableData = new Data
                        {
                            PlayerID = evData[1].ToString(),
                            Spawn = evData[2].ToString(),
                            Name = evData[3].ToString()                                
                        };

                    IncomingData.Insert(0,TableData);
                    
                    
                    //Console.WriteLine("PlayerID: " + IncomingData[0].PlayerID);
                    //Console.WriteLine("Spawn: " + IncomingData[0].Spawn);
                    //Console.WriteLine("Name: " + IncomingData[0].Name);
                    //Console.WriteLine("-------");
                    //Console.WriteLine("Count: " + IncomingData.Count);
                    break;
            }
        }

        #endregion

        #region Game Handling

        // new for lobby sample: joins a room/game and attachs it to a lobby
        public void JoinRandomWithLobby()
        {
            string RoomName = RoomNamePrefix;

            lock (this.Players)
            {
                // if this client is in a game already, it is now outdated: clean list of players and local actor number
                this.Players.Clear();
                this.LocalPlayer.playerID = 0;
            }

            // You can take a look at the implementation of OpJoinFromLobby in LiteLobbyPeer.cs
            ((LiteLobbyPeer) this.Peer).OpJoinFromLobby(RoomName, LobbyName, null, false);

        }

        // Will create the operation OpRaiseEvent with local player's position, calling peer.OpRaiseEvent(). 
        // This does not yet send the Operation but queue it in the peer. Sending is done by calling SendOutgoingCommands().
        public void SendPosition()
        {
            // dont move if player does not have a number or peer is not connected
            if (this.LocalPlayer == null || this.LocalPlayer.playerID == 0 || this.Peer == null)
            {
                return;
            }
           LocalPlayer.SendEvMove(Peer, NumberSpawn, NameSpawn);
        }

        // Will create and queue the operation OpRaiseEvent with local player's color and name (not position).
        // Actually sent by a call to SendOutgoingCommands().
        // At this point, we could also use properties (so we don't have to re-send this data when someone joins).
        public void SendPlayerInfo()
        {
            // dont move if player does not have a number or peer is not connected
            if (this.LocalPlayer == null || this.LocalPlayer.playerID == 0 || this.Peer == null)
            {
                return;
            }

            this.LocalPlayer.SendPlayerInfo(this.Peer);

        }

        // Actually sends the outgoing data (which was previously queued in the PhotonPeer)
        public void SendOutgoingCommands()
        {
            if (this.Peer != null)
            {
                this.Peer.SendOutgoingCommands();
            }
        }

        public void DispatchAll()
        {
            while (this.Peer != null && this.Peer.DispatchIncomingCommands())
            {
                
            }
        }

        public void PrintPlayers()
        {
            string players = "Players: ";
            lock (this.Players)
            {
                foreach (Player p in this.Players.Values)
                {
                    players += p.ToString() + ", ";
                }
            }

            this.DebugReturn(players);
        }

        // This is only used by the game / application, not by the Photon library
        public void DebugReturn(string debug)
        {
            Console.WriteLine("Debug: " + debug);
        }

        #endregion
    }
}
