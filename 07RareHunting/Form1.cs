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
using System.Data;
using System.Reflection;
using _07RareHunting.Properties;

namespace _07RareHunting
{
    using System;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {

        private Game GameInstance;
        private LobbyHandler LobbyHandlerInstance;

        public string messages;
        private PopulatePlayerDB playerDB;

        DateTime EndOfTime;


        public Form1()
        {
            InitializeComponent();

            GameInstance = new Game(this.DebugReturn, true);
            GameInstance.Connect();
            GameInstance.form1 = this;           

            nameBox.Text = Settings.Default.permName;
            TopMost = Settings.Default.alwaysOnTop;            


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
            nameBox.Text = Settings.Default.permName;
            TopMost = Settings.Default.alwaysOnTop;
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            //If the person is marked as active, update to the server with the supplied information.
            if (activeCheck.Checked && !nameBox.Text.Equals(""))
            {
                GameInstance.NumberSpawn = Convert.ToInt32(spawnCombo.Text);
                GameInstance.NameSpawn = nameBox.Text;
                EndOfTime = DateTime.Now.AddMinutes(5);
                activeTimer.Enabled = true;
                activeTimer.Start();
                dbUpdateTimer.Enabled = true;
                dbUpdateTimer.Start();
                stripLabel.Text = "Active";
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

        //Runs on closing the program
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GameInstance.Disconnect();
            Settings.Default.Save();
        }

        //Publishes the playerDB data to the Datagrid view
        private void update_table()
        {
            var source = new BindingSource();
            source.DataSource = GameInstance.playerDB.GetPlayerDB();
            spawnDGV.DataSource = source;

            //for (int i = 0; i < spawnDGV.Rows.Count; i++)
            //{
            //    for (int j = 0; j < GameInstance.playerDB.playerDB.Count; j++)
            //    {
            //        if (spawnDGV.Rows[i].Cells[0].Value.Equals(pDB.GetSpawn()))
            //        {
            //            spawnDGV.Rows[i].Cells[2].Value += pDB.GetPlayerName() + ", ";
            //        }
            //    }
        
            
            // Console.WriteLine("Value: " + spawnDGV.Rows[0].Cells[0].Value);
            // Console.WriteLine("Game: " + GameInstance.IncomingData[0].Spawn);
        }


        private void button1_Click(object sender, EventArgs e)
        {            
            update_table();
        }

        //Ticks every second, call necesarry constant updates within
        private void activeTimer_Tick(object sender, EventArgs e)
        {
            UpdateTimer();
            if (statusIDLabel.Text.Equals("") || statusIDLabel.Text.Equals("-  Client: 0"))
            {
                statusIDLabel.Text = "-  Client: " + GameInstance.LocalPlayer.playerID.ToString();
            }

            //while (toolStripConnection.Text.Equals("Connected"))
            //{
            //    update_table();
            //}
        }

        //Updates the 5-minute timer
        private void UpdateTimer()
        {                
            TimeSpan TimeString = (EndOfTime - DateTime.Now);
            //If the timer is above zero, format and update it
            if (TimeString.Seconds != -01)
            {
                timerLabel.Text = String.Format("{0:m\\:ss}", TimeString);
            }
            else
            {
                //Timer has expired, stop it, reset the string and flash the window alert
                activeTimer.Stop();
                activeCheck.Checked = false;
                timerLabel.Text = "0:00";
                ActiveAlert();
            }
        }

        //Handles the taskbar flashing when the timer has reached 00:00
        private static void ActiveAlert()
        {
            var helper = new FlashWindowHelper();
            helper.FlashApplicationWindow(); 
        }

        // This updates the database with the local clients details, this way we can publish them to the DGV and keep them from being removed.
        private void dbUpdateTimer_Tick(object sender, EventArgs e)
        {            
            if (GameInstance.LocalPlayer.playerID != 0)
            {
                GameInstance.playerDB.Update(new PlayerDB(GameInstance.LocalPlayer.playerID, spawnCombo.Text, nameBox.Text));
            }
        }

        //Saves the input on the options tab
        private void saveButton_Click_1(object sender, EventArgs e)
        {           
            if (ontopCheck.Checked)
            {
                TopMost = true;
            }
            else
            {
                TopMost = false;
            }
            Settings.Default.Save();
        }

        //Hyperlink to the spawn details page
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://services.runescape.com/m=forum/forums.ws?317,318,59,65016317,goto,1");
        }
    }
}