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

using System.Collections.Generic;

namespace _07RareHunting
{
    using System;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {

        private Game GameInstance;
        private LobbyHandler LobbyHandlerInstance;

        public string messages;
        public List<string> ClientIDs = new List<string>();
        //public List<KeyValuePair<int, string>> ClientIDs = new List<KeyValuePair<int, string>>();

        DateTime EndOfTime;

        readonly Options frmOptions = new Options();

        public Form1()
        {
            InitializeComponent();

            GameInstance = new Game(this.DebugReturn, true);
            GameInstance.Connect();
            GameInstance.form1 = this;


            nameBox.Text = Properties.Settings.Default.permName;
            TopMost = Properties.Settings.Default.alwaysOnTop;            


            for (var i = 0; i < spawnCombo.Items.Count; i++)
            {
                spawnDGV.Rows.Add(1);
                spawnDGV.Rows[i].SetValues((i+1));
            }

            LobbyHandlerInstance = new LobbyHandler(this.GameInstance.ServerAddress, GameInstance.protocol, Game.AppName, Game.LobbyName, this);
            LobbyHandlerInstance.StartService();
        }

        protected void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            //If the person is marked as active, update to the server with the supplied information.
            if (activeCheck.Checked && !nameBox.Text.Equals(""))
            {
                GameInstance.NumberSpawn = spawnCombo.Text;
                GameInstance.NameSpawn = nameBox.Text;
                EndOfTime = DateTime.Now.AddMinutes(5);
                activeTimer.Enabled = true;
                activeTimer.Start();
                stripLabel.Text = "Active";
                frmOptions.clientIDLabel.Text = Properties.Settings.Default.clientID;
            }
            else
            {
                stripLabel.Text = "In-Active";
                MessageBox.Show("Please fill out your character name and tick the active box.");
            }        
        }

        public void DebugReturn(string debug)
        {
            Console.WriteLine(debug);
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GameInstance.Disconnect();
            Properties.Settings.Default.Save();
        }

        private void update_table()
        {
            if (GameInstance.IncomingData != (null) && ClientIDs != (null))
            {
                Console.WriteLine("ClientIDs.Count " + ClientIDs.Count);
                if (ClientIDs.Count > 0) Console.WriteLine("CC " + ClientIDs[0]);
                for (int i = 0; i < spawnDGV.Rows.Count; i++)
                {
                    //Adds the clients names to the DGV and puts them on the list of ClientIDs
                    if (spawnDGV.Rows[i].Cells[0].Value.ToString().Equals(GameInstance.IncomingData[0].Spawn) &&
                            !ClientIDs.Contains(GameInstance.IncomingData[0].PlayerID))
                    {
                        Console.WriteLine("Added the name: " + GameInstance.IncomingData[0].Name);
                        
                        spawnDGV.Rows[i].Cells[2].Value += GameInstance.IncomingData[0].Name + ", ";
                        spawnDGV.Rows[(Convert.ToInt32(spawnCombo.Text) - 1)].Cells[2].Value += nameBox.Text + ", ";
                        ClientIDs.Add(GameInstance.IncomingData[0].PlayerID);
                    }
                    else
                    {
                        //If the player is already listed somewhere within the table, remove them and rerun the update.
                        if (ClientIDs.Contains(GameInstance.IncomingData[0].PlayerID) &&
                            spawnDGV.Rows[i].Cells[0].Value.ToString().Contains(GameInstance.IncomingData[0].Name + ", "))
                        {   
                            Console.WriteLine("We're in, we can remove!");                            
                            spawnDGV.Rows[i].Cells[0].Value.ToString().Replace(GameInstance.IncomingData[0].Name + ", ", "");
                            ClientIDs.Remove(GameInstance.IncomingData[0].PlayerID);
                            update_table();
                            Console.WriteLine("Removed the name: " + GameInstance.IncomingData[0].Name);
                        }
                    }
                }
            }
            // Console.WriteLine("Value: " + spawnDGV.Rows[0].Cells[0].Value);
            // Console.WriteLine("Game: " + GameInstance.IncomingData[0].Spawn);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            update_table();
        }

        private void optionsButton_Click(object sender, EventArgs e)
        {            
            frmOptions.Show();            
        }

        private void activeTimer_Tick(object sender, EventArgs e)
        {
            UpdateTimer();
            while (toolStripConnection.Text.Equals("Connected"))
            {
                //update_table();
            }
        }

        private void UpdateTimer()
        {                
            TimeSpan TimeString = (EndOfTime - DateTime.Now);
            if (TimeString.Seconds != -01)
            {
                timerLabel.Text = String.Format("{0:m\\:ss}", TimeString);
            }
            else
            {
                activeTimer.Stop();
                activeCheck.Checked = false;
                timerLabel.Text = "0:00";
                ActiveAlert();
            }
        }

        private static void ActiveAlert()
        {
            var helper = new FlashWindowHelper();
            helper.FlashApplicationWindow(); 
        }

        public void UpdateStatus()
        {
            nameBox.Text = Properties.Settings.Default.permName;
            TopMost = Properties.Settings.Default.alwaysOnTop;
        }      
    }
}