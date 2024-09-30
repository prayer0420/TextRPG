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
        public List<Item> Inven { get; set; } = new List<Item>(MAX_SLOT);
        private int invenCount;

        public Inventory()
        {
            for (int i = 0; i < MAX_SLOT; i++)
            {
                Inven.Add(null);
            }
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

            Inven[emptySlot] = item;
            invenCount++;
            return true;
        }

        public Item GetItemAtSlot(int slot)
        {
            if(slot < 0 || slot >= MAX_SLOT)
                return null;

            return Inven[slot];
        }

        public bool RemoveItem(Item item)
        {
            if(item == null)
                return false;

            int slot = FindItemSlot(item);
            if(slot < 0) 
                return false;
            Inven.RemoveAt(slot);
            //Inven[slot] = null;
            invenCount--;
            return true;
        }

        public void PrintIven()
        {
            foreach(Item item in Inven)
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
                if (Inven[i] != null)
                {
                    Inven[i] = null;
                }
            }
        }

        private int FindEmptySlot()
        {
            for(int i = 0;i < MAX_SLOT;i++)
            {
                if( Inven[i] == null)
                    return i;
            }
            return -1;
        }

        private int FindItemSlot(Item item)
        {
            for (int i = 0; i < MAX_SLOT; i++)
            {
                if(Inven[i] == item)
                    return i;
            }
            return -1;
        }

        public void ShowInven(Player _player)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[인벤토리]");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[아이템 목록]");
            Console.ResetColor();

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
                    Game.GetInstance().mode = GameMode.Town;
                    Game.GetInstance().ProcessTown();
                    break;
            }
            Console.WriteLine();
        }

        public void EquipManager(Player _player)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[장착관리]");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[아이템 목록]");
            Console.ResetColor();

            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다");
            Console.WriteLine("아이템을 장착하거나 해제하려면 해당 번호를 입력하세요\n");

            //아이템 보여주기
            int itemCount = 0; ;
            foreach(var item in Inven)
            {
                if(item != null)
                {
                    Console.Write($"{itemCount + 1}. ");
                    Inven[itemCount++].PrintInfo();
                    Console.WriteLine();
                }
            }
            Console.WriteLine("0. 나가기");
            Console.WriteLine("원하시는 행동을 입력해주세요");
            string input = Console.ReadLine();

            //나가기
            if (input == "0")
            {
                ShowInven(_player);
                return;
            }


            //해당번호의 아이템을 장착 할 수 있는지 없는지 체크
            int number = int.Parse(input) - 1;

            if (Inven[number].itemType == ItemType.EquipmentItem)
            {
                EquipmentItem item  = (EquipmentItem)Inven[number];
                if(_player.CanEquip(item))
                {
                    _player.ToggleEquip(item);
                    EquipManager(_player);
                }
                else
                {
                    Console.WriteLine($"{item._itemName}은(는) {_player._playerType} 직업이 장착할 수 없습니다.");
                    EquipManager(_player);
                }
            }
        }
    }
}
