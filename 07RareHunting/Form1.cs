// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Form1.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   This Form uses a Game instance and a timers to mimic the gameloop
//   of a realtime game by sending player's positions on a 2d grid.
//   The classes Game and Player are used (nearly) identical in Unity 3d
//   and SilverLight demos.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

namespace _07RareHunting
{
    using System;
    using System.Drawing;
    using System.Timers;
    using System.Windows.Forms;

    using Timer = System.Timers.Timer;

    public partial class Form1 : Form
    {

        private int precision = 1;

        // private readonly string StartTimeProp = "startTime";
        private Game GameInstance;

        private LobbyHandler LobbyHandlerInstance;

        private int intervalUpdate = 5; // interval in which the gameloop is called

        public string messages;

        public Form1()
        {
            this.InitializeComponent();

            this.GameInstance = new Game(this.DebugReturn, true); // supply a delegate to get debug output

            this.GameInstance.Connect();

            this.GameInstance.form1 = this;

            // most of the Photon specific code is wrapped by the Game class

            for (int i = 0; i < spawnCombo.Items.Count; i++)
            {
                spawnDGV.Rows.Add(1);
                spawnDGV.Rows[i].SetValues((i+1));
            }

            this.LobbyHandlerInstance = new LobbyHandler(this.GameInstance.ServerAddress, GameInstance.protocol, Game.AppName, Game.LobbyName, this);
            this.LobbyHandlerInstance.StartService();
        }

        protected void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            
        }

        public void DebugReturn(string debug)
        {
            Console.WriteLine(debug);
        }



        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            GameInstance.Disconnect();
        }
    }
}