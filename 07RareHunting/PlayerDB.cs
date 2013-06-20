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

        public string GetPlayerID()
        {
            return PlayerID;
        }

        public string GetSpawn()
        {
            return Spawn;
        }

        public string GetPlayerName()
        {
            return PlayerName;
        }

        public void SetPlayerID(string playerID)
        {
            PlayerID = playerID;
        }

        public void SetSpawn(string spawn)
        {
            Spawn = spawn;
        }

        public void SetPlayerName(string playername)
        {
            this.PlayerName = playername;
        }
    }
}
