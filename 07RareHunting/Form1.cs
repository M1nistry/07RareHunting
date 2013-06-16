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

        public List<string> ClientIDs = new List<string>();
        //public List<KeyValuePair<int, string>> ClientIDs = new List<KeyValuePair<int, string>>();

        DateTime EndOfTime;

        public Form1()
        {
            this.InitializeComponent();

            this.GameInstance = new Game(this.DebugReturn, true); // supply a delegate to get debug output

            this.GameInstance.Connect();

            this.GameInstance.form1 = this;

            nameBox.Text = Properties.Settings.Default.permName;
            TopMost = Properties.Settings.Default.alwaysOnTop;

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
            //If the person is marked as active, update to the server with the supplied information.
            if (activeCheck.Checked && !nameBox.Text.Equals(""))
            {
                GameInstance.NumberSpawn = spawnCombo.Text;
                GameInstance.NameSpawn = nameBox.Text;
                EndOfTime = DateTime.Now.AddMinutes(5);
                activeTimer.Enabled = true;
                activeTimer.Start();
                stripLabel.Text = "Active";
            }
            else
            {
                stripLabel.Text = "In-Active";
                MessageBox.Show("Please fill out your character name and tick the active box");
            }

         
        }

        public void DebugReturn(string debug)
        {
            Console.WriteLine(debug);
        }



        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            GameInstance.Disconnect();
            Properties.Settings.Default.Save();
        }

        private void update_table()
        {
            if (!GameInstance.IncomingData[0].Equals(null))
            {
                Console.WriteLine("Value: " + spawnDGV.Rows[0].Cells[0].Value);
                Console.WriteLine("Game: " + GameInstance.IncomingData[0].Spawn);
                for (int i = 0; i < spawnDGV.Rows.Count; i++)
                {
                    if (spawnDGV.Rows[i].Cells[0].Value.ToString().Equals(GameInstance.IncomingData[0].Spawn) && !ClientIDs.Contains(GameInstance.IncomingData[0].PlayerID))
                    {
                        Console.WriteLine("We're in!");
                        spawnDGV.Rows[i].Cells[2].Value += GameInstance.IncomingData[0].Name + ", ";
                        spawnDGV.Rows[Convert.ToInt32(spawnCombo.Text)].Cells[2].Value += nameBox.Text + ", ";
                        ClientIDs.Add(GameInstance.IncomingData[0].PlayerID);
                    }
                    else
                        if (ClientIDs.Contains(GameInstance.IncomingData[0].PlayerID))
                        {
                            if (spawnDGV.Rows[i].Cells[0].Value.ToString().Contains(GameInstance.IncomingData[0].Name + ", "))
                            {
                                spawnDGV.Rows[i].Cells[0].Value.Equals("");
                                ClientIDs.Remove(GameInstance.IncomingData[0].PlayerID);
                            }
                        }

                    //Sets the checkbox to checked if there is a name in that row.
                    //if (!spawnDGV.Rows[i].Cells[2].Value.Equals(""))
                    //{
                    //    spawnDGV.Rows[i].Cells[1].Value.Equals(true);
                    //}
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            update_table();
        }

        private void optionsButton_Click(object sender, EventArgs e)
        {
            Options frmOptions = new Options();
            frmOptions.Show();
        }

        private void activeTimer_Tick(object sender, EventArgs e)
        {
            UpdateTimer();       
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

        private void ActiveAlert()
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