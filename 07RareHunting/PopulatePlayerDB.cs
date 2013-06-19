using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _07RareHunting
{
    public static class PopulatePlayerDB
    {
        private PlayerDB playerDB;

        //Update DB with current JSON data
        private void Add()
        {
            if (LadderData.entries.Count > 1 && !LadderData.entries.Count.Equals(null))
            {
                bool matchfound = false;

                for (int i = 0; i < LadderData.entries.Count; i++)
                {
                    for (int j = 0; j < playerDB.Count; j++)
                    {
                        //This account from the JSON was not found in the DB. Add to pending list.
                        if (LadderData.entries[i].account.name.Equals(playerDB[j].GetAccount()) &&
                            LadderData.entries[i].character.name.Equals(playerDB[j].GetCharacter()))
                        {
                            matchfound = true;
                            break;
                        }
                    }

                    if (!matchfound)
                    {
                        PlayerDB NewPlayer = new PlayerDB(
                            LadderData.entries[i].account.name,
                            LadderData.entries[i].character.name,
                            LadderData.entries[i].character.@class);

                        playerDB.Add(NewPlayer);
                    }

                    if (matchfound)
                    {
                        matchfound = false;
                    }
                }
            }
        }

        public void Update()
        {
            //Update all accounts in the DB.
            if (LadderData.entries.Count > 1)
            {
                //Add the Ladder JSON Data to the PlayerDB
                for (int i = 0; i < LadderData.entries.Count; i++)
                {
                    for (int j = 0; j < playerDB.Count; j++)
                    {
                        //Player already exist. Update with current information.
                        if (LadderData.entries[i].account.name.Equals(playerDB[j].GetAccount()) &&
                            LadderData.entries[i].character.name.Equals(playerDB[j].GetCharacter()))
                        {
                            playerDB[j].Update(
                                LadderData.entries[i].online,
                                LadderData.entries[i].rank,
                                LadderData.entries[i].character.level,
                                LadderData.entries[i].character.experience,
                                DateTime.UtcNow);
                        }
                    }
                }

                LastUpdateCache = DateTime.UtcNow;
            }
        }

        public void Delete()
        {
            //Remove accounts from the DB that are no longer in the JSON
            for (int i = 0; i < playerDB.Count; i++)
            {
                //Account was not updated in the last 15 seconds.
                if (playerDB[i].GetLastUpdateTime() < LastUpdateCache.AddSeconds(-0.9))
                {
                    playerDB.RemoveAt(i);
                    i--;
                }
            }

            //Sort the list:
            //playerDB = playerDB.OrderBy(q => q.GetRank()).ToList();
        }
    }
}
