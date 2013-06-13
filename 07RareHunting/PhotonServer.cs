using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Lite;

using EventData = ExitGames.Client.Photon.EventData;
using OperationResponse = ExitGames.Client.Photon.OperationResponse;

public class PhotonServer : IPhotonPeerListener
{
    public LitePeer Peer { get; set; }
    public string ServerStatus;
    public bool Connected;

    private readonly Thread updateThread;

    internal int intervalDispatch = 50; // interval between DispatchIncomingCommands() calls
    private int lastDispatch = Environment.TickCount;
    internal int intervalSend = 50; // interval between SendOutgoingCommands() calls
    private int lastSend = Environment.TickCount;
    internal int intervalMove = 500; // interval for auto-movement - each movement creates an OpRaiseEvent
    private int lastMove = Environment.TickCount;

    public int NextSendTickCount = 500;

    public Hashtable hashes = new Hashtable();


    public PhotonServer(bool createGameLoopThread)
    {
        if (createGameLoopThread)
        {
            this.updateThread = new Thread(Gameloop);
            this.updateThread.IsBackground = true;
            //this.updateThread.Start();
            Initialize();
            
        }

    }

    private void Gameloop()
    {
        try
        {
            while (true)
            {
                Peer.Service();
                Thread.Sleep(10);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("There's a problem here : " + ex);
            //this.DebugReturn("Exception in Gameloop: " + ex.ToString());
        }
    }

    public void Initialize()
    {
        Peer = new LitePeer(this);
        if (Peer.Connect("127.0.0.1:5055", "Lite"))
        {
            ServerStatus = "Connected";
        }
        else
        {
            ServerStatus = "Disconnected";
        }
         
        //Peer.OpCustom(255,  new Dictionary<byte, object> {{1, "HellO WorlD!"}}, false);
    }

    public void Update()
    {
        
    }
    
    public void sendMsg(String message)
    {
        Peer.OpRaiseEvent(253, hashes, false);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Console.WriteLine("String -" +message);
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        throw new NotImplementedException();
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Console.WriteLine("H: " + statusCode);
        if (statusCode.Equals("Connect"))
        {
            Connected = true;
        }
    }

    public void OnEvent(EventData eventData)
    {
        
    }
}
