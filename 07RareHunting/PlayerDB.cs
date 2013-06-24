using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _07RareHunting
{
    public class PlayerDB
    {
        //Player Information
        private int PlayerID;
        private string Spawn;
        private string PlayerName;
        private DateTime UpdateTime;

        public PlayerDB(int PlayerID, String Spawn, String PlayerName)
        {
            this.PlayerID = PlayerID;
            this.Spawn = Spawn;
            this.PlayerName = PlayerName;
        }

        public void Update(int PlayerID, String Spawn, String PlayerName, DateTime UpdateTime)
        {
            this.PlayerID = PlayerID;
            this.Spawn = Spawn;
            this.PlayerName = PlayerName;
            this.UpdateTime = UpdateTime;
        }

        public int GetPlayerID()
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

        public void SetPlayerID(int playerID)
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

        public DateTime LastUpdateTime()
        {
            return UpdateTime;
        }
    }
}
