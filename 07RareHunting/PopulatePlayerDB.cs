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
            playerDB.Add(newPlayerData);
        }

        public void Update(PlayerDB PlayerData)
        {
            bool possibleAdd = false;
            for (int i = 0; i < playerDB.Count; i++)
            {
                if (playerDB[i].GetPlayerID().Equals(PlayerData.GetPlayerID()))
                {
                    playerDB[i].Update(PlayerData.GetSpawn(), PlayerData.GetPlayerName());
                    possibleAdd = false;
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


        public void Delete(PlayerDB newPlayerData)
        {
            for (int i = 0; i < playerDB.Count; i++)
            {
                if ( playerDB[i].GetPlayerID().Equals(newPlayerData.GetPlayerID()))
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
