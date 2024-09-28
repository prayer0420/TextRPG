﻿using Newtonsoft.Json;
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

        public void ShowInven(Player _player)
        {
            Console.WriteLine("[아이템 목록]");
            PrintIven();

            Console.WriteLine("1. 장착관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("원하시는 행동을 입력해주세요");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    EquipManager(_player);
                    break;

                case "0":
                    Game.GetInstance()._mode = GameMode.Town;
                    break;
            }
            Console.WriteLine();
        }

        public void EquipManager(Player _player)
        {
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다");
            Console.WriteLine("아이템을 장착하거나 해제하려면 해당 번호를 입력하세요");
            Console.WriteLine();

            Console.WriteLine("[아이템 목록]");

            //아이템 보여주기
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] != null)
                {
                    Console.Write($"{i + 1}. ");
                    _items[i].PrintInfo();
                    Console.WriteLine();
                }
            }

            Console.WriteLine("0. 나가기");
            Console.WriteLine("원하시는 행동을 입력해주세요");
            string input = Console.ReadLine();

            //나가기
            if (input == "0")
            {
                return;
            }

            //해당번오의 아이템을 장착 할 수 있는지 없는지 체크
            int number = int.Parse(input) - 1;
            if (_items[number]._Isequip)
            {
                _player.Unequip(_items[number]);
            }
            else
            {
                _player.Equip(_items[number]);
            }

        }

    }
}
