
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
          
            GameInstance = new Game(DebugReturn, true);
            GameInstance.Connect();
            GameInstance.form1 = this;

            nameBox.Text = (string)Settings.Default["permName"];
            alwaysNameText.Text = (string)Settings.Default["permName"];
            TopMost = (bool)Settings.Default["alwaysOnTop"];
            ontopCheck.Checked = (bool)Settings.Default["alwaysOnTop"];


            for (var i = 0; i < spawnCombo.Items.Count; i++)
            {
                spawnDGV.Rows.Add(1);
                spawnDGV.Rows[i].SetValues(i+1);
            }

            dbUpdateTimer.Enabled = true;
            dbUpdateTimer.Start();

            LobbyHandlerInstance = new LobbyHandler(this.GameInstance.ServerAddress, GameInstance.protocol,
                                                    Game.AppName, Game.LobbyName, this);
                LobbyHandlerInstance.StartService();
            
        }

        protected void Form1_Load(object sender, EventArgs e)
        {

        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            //If the person is marked as active, update to the server with the supplied information.
            if (activeCheck.Checked && !nameBox.Text.Equals("") && spawnCombo.Text != "" && !nameBox.Text.Equals("            ") && GameInstance.LocalPlayer.playerID != 0)
            {
                EndOfTime = DateTime.Now.AddMinutes(5);
                activeTimer.Enabled = true;
                activeTimer.Start();
                stripLabel.Text = "Active";                
            }
            else if (GameInstance.LocalPlayer.playerID == 0)
            {
                MessageBox.Show("Please wait until you have connected to update your status.");
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
            Settings.Default["permName"] = alwaysNameText.Text;
            Settings.Default["alwaysOnTop"] = activeCheck.Checked;
            Settings.Default.Save();         
        }

        //Publishes the playerDB data to the Datagrid view
        private void update_table()
        {            
            for (var j = 0; j < spawnDGV.Rows.Count; j++)
            {
                spawnDGV.Rows[j].Cells[2].Value = "";
                spawnDGV.Rows[j].Cells[1].Value = false;
                foreach (PlayerDB t in playerDB.playerDB)
                {
                    if (t.GetSpawn() != j.ToString() || j <= 0) continue;
                    spawnDGV.Rows[(j - 1)].Cells[2].Value += t.GetPlayerName() + ", ";
                    spawnDGV.Rows[(j - 1)].Cells[1].Value = true;
                }
            }            
        }

        //Ticks every second, call necesarry constant updates within
        private void activeTimer_Tick(object sender, EventArgs e)
        {
            UpdateTimer();
            playerDB = GameInstance.playerDB;
            GameInstance.NumberSpawn = Convert.ToInt32(spawnCombo.Text);
            GameInstance.NameSpawn = nameBox.Text;
            GameInstance.IsActive = activeCheck.Checked;

            if (activeCheck.Checked)
            {
                GameInstance.playerDB.Update(new PlayerDB(GameInstance.LocalPlayer.playerID, spawnCombo.Text,
                                                          nameBox.Text, DateTime.Now, activeCheck.Checked));
                update_table();
            }

            if (statusIDLabel.Text.Equals("") || statusIDLabel.Text.Equals("-  Client: 0"))
            {
                statusIDLabel.Text = "-  Client: " + GameInstance.LocalPlayer.playerID.ToString();
            }

        }

        //Updates the 5-minute timer
        private void UpdateTimer()
        {                
            var TimeString = (EndOfTime - DateTime.Now);
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
                clearDGV();
            }

            if (!activeCheck.Checked)
            {
                //Stop updating and clear the DGV, emulates a fresh open of the program.
                activeTimer.Stop();
                activeCheck.Checked = false;
                timerLabel.Text = "0:00";
                clearDGV();
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
                toolStripConnection.Text = "Connected";
            }
        }

        //Saves the input on the options tab
        private void saveButton_Click_1(object sender, EventArgs e)
        {           
            TopMost = ontopCheck.Checked;

            Settings.Default["permName"] = alwaysNameText.Text;
            Settings.Default["alwaysOnTop"] = activeCheck.Checked;
            nameBox.Text = alwaysNameText.Text;
            Settings.Default.Save();
        }

        //Hyperlink to the spawn details page
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://services.runescape.com/m=forum/forums.ws?317,318,59,65016317,goto,1");
        }

        //Restricting input into the name box so only correct names can be input.
        private void name_keypress(object sender, KeyPressEventArgs e)
        {
            const char Backspace = '\u0008';
            const char Delete = '\u007f';
            const char Space = '\u0020';

            if (!Char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != Backspace && e.KeyChar != Delete && e.KeyChar != Space)
            {
                e.Handled = true;
            }
        }

        //Method to clear the DGV.
        private void clearDGV()
        {
            for (var i = 0; i < spawnDGV.Rows.Count; i++)
            {
                spawnDGV.Rows[i].Cells[2].Value = "";
                spawnDGV.Rows[i].Cells[1].Value = false;
            }
        }
    }
}