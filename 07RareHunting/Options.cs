using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using _07RareHunting.Properties;

namespace _07RareHunting
{
    public partial class Options : Form
    {

        public Options()
        {
            InitializeComponent();

            alwaysNameText.Text = Settings.Default["permName"].ToString();
            ontopCheck.Checked = Settings.Default.alwaysOnTop;
            
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
