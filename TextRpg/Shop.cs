using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    class Shop
    {
        public Shop() { }
        public int a = 0;
        //public Dictionary<Item, bool> shopItemDic = new Dictionary<Item, bool>();
        public List<string> shopItemlist = new List<string>();

        public Dictionary<int, bool> shopItemDic = new Dictionary<int, bool>(); //아이템아이디, 팔렸는지
        public List<Item> shopItems = new List<Item>();
        

        public void ShowShop(Player player)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[상점]");
            Console.ResetColor();

            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점 입니다\n");
            Console.WriteLine("[보유골드]");
            Console.WriteLine($"{player.Gold} G\n");

            Console.WriteLine("[아이템 목록]");
            if(shopItemDic.Count ==0)
            {
                Reset();
            }

            //아이템목록 보여지기
            ShowInfo();

            Console.WriteLine("\n[1] 아이템 구매");
            Console.WriteLine("[2] 아이템 판매");
            Console.WriteLine("[3] 상점리셋(10분마다)");
            Console.WriteLine("[0] 나가기");

            Console.WriteLine("\n원하시는 행동을 입력해주세요");

            string input = Console.ReadLine();

            switch (input)
            {
                case"0":
                    Game.GetInstance().ProcessTown();
                    break;

                case "1":
                    BuyItem(player);
                    break;

                case "2":
                    SellItem(player);
                    break;
                case "3":
                    CanRest(player);
                    break;
            }
            
        }

        public void Reset()
        {
            shopItemDic.Clear();
            shopItems.Clear();

            // 무기넣기
            for (int i = 0; i < 3; i++)
            {
                Weapon weapon;
                do
                {
                    weapon = new Weapon();
                } while (shopItems.Any(existingItem => existingItem._itemName == weapon._itemName));

                if (!shopItemDic.ContainsKey(weapon.ItemId))
                {
                    shopItems.Add(weapon);
                    shopItemDic.Add(weapon.ItemId, false);
                    shopItemlist.Add(weapon._itemName);
                }
                else
                {
                    // 이미 존재하는 경우 처리 로직
                    Console.WriteLine($"{weapon.ItemId}의 아이템은 이미 존재합니다");
                }
            }

            // 방어구 추가
            for (int i = 0; i < 3; i++)
            {
                Armor armor;
                do
                {
                    armor = new Armor();
                } while (shopItems.Any(existingItem => existingItem._itemName == armor._itemName));
                
                if (!shopItemDic.ContainsKey(armor.ItemId))
                {
                    shopItems.Add(armor);
                    shopItemDic.Add(armor.ItemId, false);
                    shopItemlist.Add(armor._itemName);
                }
                else
                {
                    // 이미 존재하는 경우 처리 로직
                    Console.WriteLine($" {armor.ItemId}의 아이템은 이미 존재합니다");
                }
            }

        }

        public void ShowInfo()
        {
            foreach (var item in shopItems)
            {
                bool isSold = shopItemDic[item.ItemId];

                if ((item.itemType == ItemType.EquipmentItem))
                {
                    if(item is Weapon)
                    {
                        Weapon weapon = (Weapon)item;
                        Console.Write($"- {weapon._itemName}   | 공격력 +{weapon.GetDamage()}   | {weapon._iteminfo}");

                        if (isSold)
                        {
                            Console.WriteLine($"| 구매완료");
                        }
                        else
                        {
                            Console.WriteLine($"| {weapon._price}G");
                        }
                    }
                    else if (item is Armor)
                    {
                        Armor armor = (Armor)item;
                        Console.Write($"- {armor._itemName}   | 방어력 +{armor.GetDefence()}   | {armor._iteminfo}");
                        if (isSold)
                        {
                            Console.WriteLine($"| 구매완료");
                        }
                        else
                        {
                            Console.WriteLine($"| {armor._price}G");
                        }
                    }
                }
            }
        }
        public void BuyItem(Player player)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[상점]");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("아이템 구매");
            Console.ResetColor();

            Console.WriteLine("원하는 아이템의 번호를 입력하면 구매할 수 있습니다\n");

            // 아이템 목록 표시
            for (int i = 0; i < shopItems.Count; i++)
            {
                var item = shopItems[i];
                bool IsSold = shopItemDic[item.ItemId];

                Console.Write($"{i + 1}. ");

                if (item.itemType == ItemType.EquipmentItem)
                {
                    if( item is Weapon)
                    {
                        Weapon weapon = (Weapon)item;
                        Console.Write($"- {weapon._itemName}   | 공격력 +{weapon.GetDamage()}   | {weapon._iteminfo}");

                        if (IsSold)
                        {
                            Console.WriteLine($"| 구매완료");
                        }
                        else
                        {
                            Console.WriteLine($"| {weapon._price}G");
                        }
                    }
                    else if (item is Armor)
                    {
                        Armor armor = (Armor)item;
                        Console.Write($"- {armor._itemName}   | 방어력 +{armor.GetDefence()}   | {armor._iteminfo}");

                        if (IsSold)
                        {
                            Console.WriteLine($"| 구매완료");
                        }
                        else
                        {
                            Console.WriteLine($"| {armor._price}G");
                        }
                    }
                }
                
            }

            Console.WriteLine("\n0. 나가기");
            Console.WriteLine("원하시는 행동을 입력해주세요");
            
            string input = Console.ReadLine();
            if (input == "0")
            {
                ShowShop(player);
            }

            int number = int.Parse(input) - 1;

            if (number < 0 || number >= shopItems.Count)
            {
                Console.WriteLine("잘못된 번호입니다.");
                BuyItem(player);

            }

            var selectedItem = shopItems[number];
            bool isSold = shopItemDic[selectedItem.ItemId];

            // 이미 구매한 아이템인지 확인
            if (isSold)
            {
                Console.WriteLine("이미 구매한 아이템입니다");
            }

            // 플레이어의 골드 확인
            if (player.Gold >= selectedItem._price)
            {
                // 구매 처리
                shopItemDic[selectedItem.ItemId] = true;
                Inventory.GetInstance().AddItem(selectedItem);
                player.Gold -= selectedItem._price;
                Console.WriteLine($"{selectedItem._itemName}을 구매하셨습니다!");
                Console.WriteLine("자동으로 마을로 이동합니다");
            }
            else
            {
                Console.WriteLine("Gold가 부족합니다");
            }

            BuyItem(player);

        }
        public void SellItem(Player player)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[상점]");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("아이템 판매");
            Console.ResetColor();
            Console.WriteLine("팔기 원하는 아이템의 번호를 입력하면 판매 할 수 있습니다");

            Inventory inventory = Inventory.GetInstance();

            //인벤 보여주기
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < inventory.Inven.Count; i++)
            {
                Item item = inventory.Inven[i];
                if (item != null)
                {
                    Console.Write($"{(i) + 1}. ");


                    item.PrintInfo();

                    Console.Write($"| {(int)(item._price * 0.8f)}");

                    Console.WriteLine();
                }
            }


            Console.WriteLine("\n0. 나가기");
            Console.WriteLine("원하시는 행동을 입력해주세요");
            string input = Console.ReadLine();
            int number = int.Parse(input) - 1;
            if (input == "0")
            {
                ShowShop(player);
            }

            else if (number >=0 && number < inventory.Inven.Count)
            {

                //골드 증가(기존 가치의 80%)
                Item item2 = inventory.Inven[number];
                player.Gold += (int)(item2._price * 0.8f);

                //알림
                Console.WriteLine($"{item2._itemName}을 팔았습니다! ");


                //상점에서 산 아이템을 다시 판거니까 팔렸다고 체크
                bool IsSold = shopItemDic[item2.ItemId];
                if(IsSold)
                {
                    shopItemDic[item2.ItemId] = false;
                }

                //인벤토리 창에서 없애기
                inventory.RemoveItem(inventory.Inven[number]);

                SellItem(player);
            }
            else
            {
                Console.WriteLine("범위를 벗어났습니다. 다시 입력해주세요");
            }
        }


        private TimeSpan resetInterval = TimeSpan.FromMinutes(10);  // 10분 간격으로 리셋
        private TimeSpan lastResetTime;  // 마지막 리셋 시간이 저장됨

        public void CanRest(Player player)
        {
            // 현재까지의 게임 시간 (TimeSpan 타입이어야 함)
            TimeSpan currentTime = Game.GetInstance().GetElapsedTime();

            // 얼마나 시간이 남았는지 계산 (resetInterval에서 경과된 시간 빼기)
            TimeSpan resetTime = resetInterval - (currentTime - lastResetTime);

            if (resetTime <= TimeSpan.Zero)
            {
                Reset();  // 상점 리셋
                lastResetTime = currentTime;  // 마지막 리셋 시간 갱신
                Game.GetInstance().stopwatch.Start();
                ShowShop(player);
            }
            else
            {
                // 남은 시간 형식 지정 (분:초 형식)
                string formattedTime = string.Format("{0:D2}:{1:D2}", resetTime.Minutes, resetTime.Seconds);
                Console.WriteLine($"10분이 경과하지 않았습니다. 남은 시간: {formattedTime}");

                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요");
                string input = Console.ReadLine();
                if (input == "0")
                {
                    ShowShop(player);
                }
            }
        }
    }

}
