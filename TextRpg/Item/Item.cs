using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TextRpg
{


    public enum ItemType
    {
        None = 0,
        EquipmentItem = 1,
        ConsumableItem =2,
    }



    public class Item
    {
        public Item() { }
        protected static Random _globalRandom = new Random();
        public static int _nextId = 0;
        public int _price = 500;

        public ItemType itemType {  get; set; }

        protected int _itemCount = 0;
        public int ItemId = 0;
        public string _itemName {  get; set; }
        public string _iteminfo { get; set; }

        public Item(ItemType itemType)
        {
            ItemId = _nextId++;
            this.itemType = itemType;
        }
        
        public virtual void PrintInfo()
        {
        }

        ItemType GetItemType() 
        { 
            return itemType; 
        }
    }
}