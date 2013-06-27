using System;
using System.Collections.Generic;
using System.Linq;

namespace _07RareHunting
{
    public class PopulatePlayerDB
    {
        public List<PlayerDB> playerDB;
        private TimeSpan DeleteSpan = TimeSpan.FromSeconds(20);
        private bool possibleAdd;

        public PopulatePlayerDB()
        {
            playerDB = new List<PlayerDB>();
        }

        public void Update(PlayerDB PlayerData)
        {   Console.WriteLine("Count: " + playerDB.Count);
            for (var i = 0; i < playerDB.Count; i++)
            {                              
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

                if (TimeGap.Seconds > DeleteSpan.Seconds)
                {
                    Console.WriteLine("Removing scum: " + i +" under ID: " + PlayerData.GetPlayerID());
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
            Console.WriteLine("Adding new player: " + newPlayerData.GetPlayerID());
            playerDB.Add(newPlayerData);
        }

        public void Delete()
        {
            for (int i = 0; i < playerDB.Count; i++)
            {
                //The delete method within Update() doesn't function correctly down here.
            }
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
