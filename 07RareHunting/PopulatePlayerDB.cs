using System;
using System.Collections.Generic;
using System.Linq;

namespace _07RareHunting
{
    public class PopulatePlayerDB
    {
        public List<PlayerDB> playerDB;
        private TimeSpan DeleteSpan = TimeSpan.FromSeconds(15);

        public PopulatePlayerDB()
        {
            playerDB = new List<PlayerDB>();
        }

        public void Update(PlayerDB PlayerData)
        {
            Console.WriteLine("Count: " +playerDB.Count);
            bool possibleAdd = false;
            
            for (var i = 0; i < playerDB.Count; i++)
            {
                Console.WriteLine("Time: " + (DateTime.Now - playerDB[i].LastUpdateTime()));
                if (playerDB[i].GetPlayerID().Equals(PlayerData.GetPlayerID()))
                {
                    playerDB[i].Update(PlayerData.GetSpawn(), PlayerData.GetPlayerName(), DateTime.Now);           
                    possibleAdd = false;
                    if ((DateTime.Now - playerDB[i].LastUpdateTime()).Seconds > DeleteSpan.Seconds)
                    {
                        Console.WriteLine("Removing scum");
                        playerDB.RemoveAt(i);
                    }
                    playerDB = playerDB.OrderBy(q => q.GetPlayerID()).ToList();  
                    return;
                }
                
                else
                {
                    possibleAdd = true;
                }       
            }

            if (possibleAdd)
            {
                Add(PlayerData);
            }
        }

        //Update DB with current JSON data
        public void Add(PlayerDB newPlayerData)
        {
            playerDB.Add(newPlayerData);
        }

        public void Delete(PlayerDB newPlayerData)
        {
            for (int i = 0; i < playerDB.Count; i++)
            {
                if (playerDB[i].GetPlayerID().Equals(newPlayerData.GetPlayerID()))
                {
                    playerDB.RemoveAt(i);
                }
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
