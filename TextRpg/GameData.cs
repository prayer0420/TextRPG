using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;



namespace TextRpg
{
    class GameData
    {
        public Player Player { get; set; }
        public Inventory Inventory { get; set; }
        public Shop Shop { get; set; }
        public GameMode Mode { get; set; }


        public GameData(Player player, Inventory inventory, Shop shop, GameMode mode)
        {
            Player = player;
            Inventory = inventory;
            Shop = shop;
            Mode = mode;
        }
    }
}
