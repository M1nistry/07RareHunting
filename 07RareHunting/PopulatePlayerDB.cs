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
            playerDB.Insert(0, newPlayerData);
            Console.WriteLine("----Count: " + playerDB.Count + "------");
        }

        public void Update(PlayerDB PlayerData)
        {
            if (playerDB.Count > 0)
            {
                for (int i = 0; i < playerDB.Count; i++)
                {
                    if (playerDB[i].GetPlayerID().Contains(PlayerData.GetPlayerID()))
                    {
                        playerDB[i].Update(PlayerData.GetSpawn(), PlayerData.GetPlayerName());
                    }
                    else
                    {
                        Add(PlayerData);
                    }
                }
                playerDB = playerDB.OrderBy(q => q.GetPlayerID()).ToList();
            }
            else
            {
                Add(PlayerData);
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
