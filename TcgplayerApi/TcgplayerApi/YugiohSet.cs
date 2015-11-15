using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcgplayerApi
{
    public struct Rarity
    {
        public string[] RarityList;
        public int[] Rarities;
    }

    public class YugiohSet
    {
        public string Name { get; set; } // done
        public Rarity Rarities { get; set; }

        public string Url { get; set; }
        public Database CardListSource { get; set; }
        public DataTable CardList { get; set; }
        public int CardNum { get; set; }
        public DateTime LastUpdate { get; set; }

        public double SetExpectedValue { get; set; }

        public YugiohSet(string new_name, string new_url, DateTime last_update)
        {
            Name = new_name;

            LastUpdate = last_update;

        }

        public void Update()
        {
            LastUpdate = DateTime.Now;
        }


    }
}
