using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _07RareHunting
{
    public class PlayerDB
    {
        //Player Information
        private string PlayerID;
        private string Spawn;
        private string PlayerName;

        public PlayerDB(String PlayerID, String Spawn, String PlayerName)
        {
            this.PlayerID = PlayerID;
            this.Spawn = Spawn;
            this.PlayerName = PlayerName;
        }

        public void Update(String Spawn, String PlayerName)
        {
            this.Spawn = Spawn;
            this.PlayerName = PlayerName;
        }
    }
}
