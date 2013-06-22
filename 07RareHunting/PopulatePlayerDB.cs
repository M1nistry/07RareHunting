using System;
using System.Collections.Generic;
using System.Linq;

namespace _07RareHunting
{
    public class PopulatePlayerDB
    {
        public List<PlayerDB> playerDB;

        public PopulatePlayerDB()
        {
            playerDB = new List<PlayerDB>();
        }

        //Update DB with current JSON data
        public void Add(PlayerDB newPlayerData)
        {
            Console.WriteLine("--------- Adding a new player! ----------");
            playerDB.Add(newPlayerData);
        }

        public void Update(PlayerDB PlayerData)
        {
            bool matchfound = false;
            Console.WriteLine("----Count: " + playerDB.Count + "------");
            for (int i = 0; i < playerDB.Count; i++)
            {
                if (playerDB[i].GetPlayerID().Equals(PlayerData.GetPlayerID()))
                {
                    Console.WriteLine("--- Player is in the table, updating them ---");
                    playerDB[i].Update(PlayerData.GetSpawn(), PlayerData.GetPlayerName());
                    matchfound = true;
                    return;
                }

                if (!matchfound)
                {
                    playerDB.Add(PlayerData);
                }

                if (matchfound)
                {
                    matchfound = false;
                }

                //playerDB = playerDB.OrderBy(q => q.GetPlayerID()).ToList();           
            }            
        }


        public void Delete(PlayerDB newPlayerData)
        {
            for (int i = 0; i < playerDB.Count; i++)
            {
                if ( playerDB[i].GetPlayerID().Equals(newPlayerData.GetPlayerID()) )
                {
                    playerDB.RemoveAt(i);
                }
            }
        }

        public List<PlayerDB> GetPlayerDB()
        {
            return this.playerDB;
        }

        public void ClearPlayerDB()
        {
            playerDB = null;
        }
    }
}
