using System;
using System.Collections.Generic;
using System.Linq;

namespace _07RareHunting
{
    public class PopulatePlayerDB
    {
        public List<PlayerDB> playerDB;
        public List<String> dgvNames;
        private TimeSpan DeleteSpan = TimeSpan.FromSeconds(30);
        private bool possibleAdd;

        public PopulatePlayerDB()
        {
            playerDB = new List<PlayerDB>();
        }

        public void Update(PlayerDB PlayerData)
        {   
            for (var i = 0; i < playerDB.Count; i++)
            {   
                //Checking if the player is already in the database- if so, update them.           
                if (playerDB[i].GetPlayerID().Equals(PlayerData.GetPlayerID()))
                {
                    playerDB[i].Update(PlayerData.GetPlayerID(), PlayerData.GetSpawn(), PlayerData.GetPlayerName(), DateTime.Now);
                    possibleAdd = false;                    
                    playerDB = playerDB.OrderBy(q => q.GetPlayerID()).ToList();  
                    return;
                }
                else
                {
                    possibleAdd = true;
                }
                TimeSpan TimeGap = (DateTime.Now - playerDB[i].LastUpdateTime());
                //Checking to see if a player within the databse has updated within the last 30 seconds- if not, remove them.
                if (TimeGap.Seconds > DeleteSpan.Seconds)
                {
                    //Console.WriteLine("Removing scum: " + i +" under ID: " + PlayerData.GetPlayerID());
                    playerDB.RemoveAt(i);
                    possibleAdd = false;
                }                              
            }

            if (possibleAdd)
            {
                Add(PlayerData);
            }
        }

        public void Add(PlayerDB newPlayerData)
        {
            playerDB.Add(newPlayerData);
        }

        public List<PlayerDB> GetPlayerDB()
        {
            return playerDB;
        }        

        public void ClearPlayerDB()
        {
            playerDB = null;
        }
    }
}
