using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{

    class Inventory
    {
        const int MAX_SLOT = 10;
        //아이템 모아놓는곳

        public List<Item> _items { get; private set; } = new List<Item>(MAX_SLOT);
        private int _itemCount;

        public Inventory()
        {

            for (int i = 0; i < MAX_SLOT; i++)
            {
                _items.Add(null);
            }
        }

        public static void SetInstance(Inventory inventory)
        {
            _instance = inventory;
        }

        private static Inventory _instance;
        public static Inventory GetInstance()
        {
            if(_instance == null)
                _instance = new Inventory();
            return _instance;
        }


        public bool AddItem(Item item)
        {
            if(item == null)
                return false;

            //찾아봤는데 없으면 false
            int emptySlot = FindEmptySlot();
            if(emptySlot <0)
            {
                return false;
            }

            _items[emptySlot] = item;
            _itemCount++;
            return true;
        }

        public Item GetItemAtSlot(int slot)
        {
            if(slot < 0 || slot >= MAX_SLOT)
                return null;

            return _items[slot];
        }

        public bool RemoveItem(Item item)
        {
            if(item == null)
                return false;

            int slot = FindItemSlot(item);
            if(slot < 0) 
                return false;

            _items[slot] = null;
            _itemCount--;
            return true;
        }

        public void PrintIven()
        {
            foreach(Item item in _items)
            {
                if(item != null)
                {
                    item.PrintInfo();
                    Console.WriteLine();
                }
            }
        }


        public void Clear()
        {
            for (int i = 0; i < MAX_SLOT; i++)
            {
                if (_items[i] != null)
                {
                    _items[i] = null;
                }
            }
        }

        private int FindEmptySlot()
        {
            for(int i = 0;i < MAX_SLOT;i++)
            {
                if( _items[i] == null)
                    return i;
            }
            return -1;
        }

        private int FindItemSlot(Item item)
        {
            for (int i = 0; i < MAX_SLOT; i++)
            {
                if(_items[i] == item)
                    return i;
            }
            return -1;
        }
    }
}
