using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            for (int i = 0; i < playerDB.Count; i++)
            {
                if (playerDB[i].GetPlayerID().Contains(newPlayerData.GetPlayerID()) )
                {
                    return;
                }                            
            }

            playerDB.Add(newPlayerData);
        }

        public void Update(PlayerDB newPlayerData)
        {
            for (int i = 0; i < playerDB.Count; i++)
            {
                if (playerDB[i].GetPlayerID().Contains(newPlayerData.GetPlayerID()))
                {
                    playerDB[i].GetPlayerName().Equals(newPlayerData.GetPlayerID());
                    playerDB[i].GetSpawn().Equals(newPlayerData.GetSpawn());
                }
                else return;
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
            //playerDB = playerDB.OrderBy(q => q.GetPlayerID()).ToList();
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
