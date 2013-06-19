using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _07RareHunting
{
    public class PopulatePlayerDB
    {
        private List<PlayerDB> playerDB;

//Is this even needed for this app?
        private DateTime LastUpdateCache;

        public PopulatePlayerDB()
        {
            playerDB = new List<PlayerDB>();
        }

        //Update DB with current JSON data
        private void Add(PlayerDB newPlayerData)
        {
            //Iterate through the current PlayerDB to see if newPlayerData is found

            //If found call update or disregard?

            //If not add player
        }

        public void Update(PlayerDB newPlayerData)
        {
            //Iterate through playerDB.

            //If the newPlayerData maches an account already in the database update its information

            //If its not found add it?
        }

        public void Delete()
        {
            //Iterate through the playerDB.

            //If the newPlayerData is found it should be .Remove()d

            //Sort the DB after removal.
            playerDB = playerDB.OrderBy(q => q.GetPlayerID()).ToList();
        }
    }
}
