using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Numerics;

namespace TextRpg
{
    public enum GameMode
    {
        None = 0,
        Lobby = 1,
        Town = 2,
        Dungeon = 3,
    }


    class Game
    {
        public GameMode _mode = GameMode.Lobby;
        public Inventory _inventory = Inventory.GetInstance();
        public Shop _shop = new Shop();
        public Player _player = null;
        public Monster _monster = null;
        public GameData _gameData;
        public static Random _globalRandom = new Random();
        public static int DungeonClearCount = 0;
        public static int RequireDungeonClearCount = 1;

        //게임데이터 저장
        public void SaveGameData()
        {
            GameData gameData = new GameData(_player, _inventory, _shop, _mode, DungeonClearCount, RequireDungeonClearCount);
            string jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            File.WriteAllText("gameData.json", jsonData);
            Console.WriteLine("게임 데이터가 저장되었습니다.");
        }

        // 게임 데이터 불러오기
        public bool LoadGameData()
        {
            string filePath = "gameData.json";

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                GameData gameData = JsonConvert.DeserializeObject<GameData>(jsonData, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

                // 불러온 데이터 적용
                _player = gameData.Player;

                _inventory = gameData.Inventory;
                Inventory.SetInstance(_inventory);

                _shop = gameData.Shop;
                _mode = gameData.Mode;
                DungeonClearCount = gameData.DungeonClearCount;
                RequireDungeonClearCount = gameData.RequireDungeonClearCount;

                Console.WriteLine("게임 데이터를 불러왔습니다.");
                return true;
            }
            else
            {
                Console.WriteLine("저장된 게임 데이터가 없습니다.");
                return false;
            }
        }

        // Start 메서드 수정
        public void Start()
        {
            if (!LoadGameData())
            {
                _mode = GameMode.Lobby;
                Process();

            }

            while (true)
            {
                _mode |= GameMode.Town;
                Process();
            }
        }


        public void Exit()
        {
            Console.WriteLine("게임을 종료합니다");
            // 게임 종료 시 데이터 저장
            //SaveGameData(_gameData);
            Environment.Exit(0);
        }

        public void Process()
        {
            switch (_mode)
            {
                case GameMode.Lobby:
                    ProcessLobby();
                    break;
                case GameMode.Town:
                    ProcessTown();
                    break;
                case GameMode.Dungeon:
                    ProcessDungeon(_player);
                    break;
            }
        }


        public void ProcessLobby()
        {
            Console.WriteLine("세계가 당신을 기다립니다. 직업을 선택하세요");
            Console.WriteLine("[1] 기사");
            Console.WriteLine("[2] 궁수");
            Console.WriteLine("[3] 법사");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    _player = new Knight();
                    _player.Initialize();
                    _mode = GameMode.Town;
                    break;
                case "2":
                    _player = new Archor();
                    _mode = GameMode.Town;
                    break;
                case "3":
                    _player = new Mage();
                    _mode = GameMode.Town;
                    break;
            }
        }


        public void ProcessTown()
        {
            Console.WriteLine();
            Console.WriteLine("마을에 입장했습니다");
            Console.WriteLine("[1] 상태보기");
            Console.WriteLine("[2] 인벤토리");
            Console.WriteLine("[3] 상점");
            Console.WriteLine("[4] 던전 입장");
            Console.WriteLine("[5] 휴식하기");
            Console.WriteLine("[6] 저장하기");
            Console.WriteLine("[7] 나가기");

            string input = Console.ReadLine();
            switch (input)
            {
                //상태보기
                case "1":
                    ShowInfo(_player);
                    break;
                //인벤토리
                case "2":
                    ShowInven();
                    break;
                //상점
                case "3":
                    _shop.ShowShop(_player);
                    break;
                //던전 입장
                case "4":
                    _mode = GameMode.Dungeon;
                    break;
                //휴식하기
                case "5":
                    ProcessRest(_player);
                    break;
                case "6":
                    SaveGameData();
                    break;
                case "7":
                    Exit();
                    break;

            }
        }

        public void ShowInfo(Player _player)
        {
            Console.WriteLine($"레벨 : {_player._level}");
            Console.WriteLine($"직업: {_player.GetPlayerType()}");
            Console.WriteLine($"공격력 : {_player._atk + _player._itemAtk}  (+{_player._itemAtk})");
            Console.WriteLine($"방어력 : {_player._def + _player._itemDef}  (+{_player._itemDef})");
            Console.WriteLine($"체력 : {_player.GetHp()}");
            Console.WriteLine($"골드 : {_player._gold}");

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("0. 나가기");

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string input = Console.ReadLine();
            if (input == "0")
            {
                _mode = GameMode.Town;
            }
            Console.WriteLine();
        }

        public void ShowInven()
        {
            //Weapon weapon = new Weapon();
            //inventory.AddItem(weapon);

            Console.WriteLine("[아이템 목록]");
            _inventory.PrintIven();

            Console.WriteLine("1. 장착관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("원하시는 행동을 입력해주세요");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    EquipManager();
                    break;

                case "0":
                    _mode = GameMode.Town;
                    break;
            }
            Console.WriteLine();
        }

        public void EquipManager()
        {
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다");
            Console.WriteLine("아이템을 장착하거나 해제하려면 해당 번호를 입력하세요");
            Console.WriteLine();

            Console.WriteLine("[아이템 목록]");

            //아이템 보여주기
            for (int i = 0; i < _inventory._items.Count; i++)
            {
                if (_inventory._items[i] != null)
                {
                    Console.Write($"{i + 1}. ");
                    _inventory._items[i].PrintInfo();
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
            if (_inventory._items[number]._Isequip)
            {
                _player.Unequip(_inventory._items[number]);
            }
            else
            {
                _player.Equip(_inventory._items[number]);
            }

        }

        public void ProcessRest(Player _player)
        {
            Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. 보유 골드 : {_player._gold} ");

            Console.WriteLine();

            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string input = Console.ReadLine();
            if (input == "0")
            {
                return;
            }
            else if (input == "1")
            {
                // 돈 감소
                _player._gold -= 500;
                // 체력 회복
                _player.RecoveryHp(100);

                Console.WriteLine("체력이 회복되었습니다");
                Console.WriteLine("마을로 이동합니다");
            }
        }

        
        public void ProcessDungeon(Player _player)
        {
            //체력이 0이하라면 자동으로 마을로 가기
            if (_player.GetHp() <= 0)
            {
                _mode = GameMode.Town;
                Console.WriteLine("HP가 0이므로 던전에 들어갈 수 없습니다. 마을로 이동합니다");
                return;
            }

            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다\n");
            Console.WriteLine("1. 쉬운 던전    |   방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전    |   방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전    |   방어력 17 이상 권장");
            Console.WriteLine("0. 나가기\n");

            Console.WriteLine("원하시는 행동을 입력해주세요");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    EasyDungeon(_player);
                    break;
                case "2":
                    NormalDungeon(_player);
                    break;
                case "3":
                    HardDungeon(_player);
                    break;
                case "0":
                    _mode = GameMode.Town;
                    break;
            }
        }

        public void EasyDungeon(Player _player)
        {
            const int REQUIRE_DEF = 5;
            const int REWARD = 1000;

            //권장 방어력보다 낮으면
            if (_player._def < REQUIRE_DEF)
            {
                //40%확률로 실패
                int rand = _globalRandom.Next(0, 10);
                if (rand < 4)
                {
                    Console.WriteLine("[쉬운 던전공략 실패!]");
                    _player.OnDamaged((int)(_player.GetHp() * 0.5f));
                    Console.WriteLine($"체력이 {_player.GetHp()}으로 깎였습니다");

                    //체력이 0이하라면 자동으로 마을로 가기
                    if (_player.GetHp() <= 0)
                    {
                        _mode = GameMode.Town;
                        Console.WriteLine("HP가 0이므로 마을로 이동합니다");
                        return;
                    }
                }
                //60%확률로 성공
                else
                {
                    Console.WriteLine("[쉬운 던전공략 성공!]");

                    Console.WriteLine("[탐험결과]");
                    int rand2 = _globalRandom.Next(20, 36);
                    Console.Write($"체력 {_player.GetHp()} -> ");
                    _player.OnDamaged(rand2 - (REQUIRE_DEF - _player._def));
                    Console.WriteLine($"{_player.GetHp()}");

                    rand2 = _globalRandom.Next(_player._atk, _player._atk * 2);
                    Console.Write($"Gold {_player._gold} -> ");
                    _player._gold += REWARD + rand2;
                    Console.WriteLine($"{_player._gold}");

                    //클리어 횟수 추가
                    DungeonClearCount++;
                    CheckLevelUp(_player);

                    //체력이 0이하라면 자동으로 마을로 가기
                    if (_player.GetHp() <= 0)
                    {
                        _mode = GameMode.Town;
                        Console.WriteLine("HP가 0이므로 마을로 이동합니다");
                        return;
                    }

                    Console.WriteLine("0.나가기\n");

                    Console.WriteLine("원하시는 행동을 입력해주세요");
                    string input = Console.ReadLine();
                    if (input == "0")
                        return;

                   
                }
            }
            //권장 방어력보다 높으면 무조건 성공
            else
            {
                Console.WriteLine("[쉬운 던전공략 성공!]");

                Console.WriteLine("[탐험결과]");
                int rand2 = _globalRandom.Next(20, 36);
                Console.Write($"체력 {_player.GetHp()} -> ");
                _player.OnDamaged(rand2 - (_player._def- REQUIRE_DEF));
                Console.WriteLine($"{_player.GetHp()}");

                rand2 = _globalRandom.Next(_player._atk, _player._atk * 2);
                Console.Write($"Gold {_player._gold} -> ");
                _player._gold += REWARD + rand2;
                Console.WriteLine($"{_player._gold}");

                //클리어 횟수 추가
                DungeonClearCount++;
                CheckLevelUp(_player);

                //체력이 0이하라면 자동으로 마을로 가기
                if (_player.GetHp() <= 0)
                {
                    _mode = GameMode.Town;
                    Console.WriteLine("HP가 0이므로 마을로 이동합니다");
                    return;
                }
                Console.WriteLine("0.나가기\n");

                Console.WriteLine("원하시는 행동을 입력해주세요");
                string input = Console.ReadLine();
                if (input == "0")
                    return;
            }
        }

        public void NormalDungeon(Player _player)
        {
            const int REQUIRE_DEF = 11;
            const int REWARD = 1700;

            //권장 방어력보다 낮으면
            if (_player._def < REQUIRE_DEF)
            {
                //60%확률로 실패
                int rand = _globalRandom.Next(0, 10);
                if (rand < 6)
                {
                    Console.WriteLine("[일반 던전공략 실패!]");
                    _player.OnDamaged((int)(_player.GetHp() * 0.5f));
                    Console.WriteLine($"체력이 {_player.GetHp()}으로 깎였습니다");

                    //체력이 0이하라면 자동으로 마을로 가기
                    if (_player.GetHp() <= 0)
                    {
                        _mode = GameMode.Town;
                        Console.WriteLine("HP가 0이므로 마을로 이동합니다");
                        return;
                    }
                }
                //40%확률로 성공
                else
                {
                    Console.WriteLine("[일반 던전공략 성공!]");

                    Console.WriteLine("[탐험결과]");
                    int rand2 = _globalRandom.Next(20, 36);
                    Console.Write($"체력 {_player.GetHp()} -> ");
                    _player.OnDamaged(rand2 - (REQUIRE_DEF - _player._def));
                    Console.WriteLine($"{_player.GetHp()}");

                    rand2 = _globalRandom.Next(_player._atk, _player._atk * 2);
                    Console.Write($"Gold {_player._gold} -> ");
                    _player._gold += (REWARD + (REWARD * (rand2/100)) );
                    Console.WriteLine($"{_player._gold}");
                    //클리어 횟수 추가
                    DungeonClearCount++;
                    CheckLevelUp(_player);

                    //체력이 0이하라면 자동으로 마을로 가기
                    if (_player.GetHp() <= 0)
                    {
                        _mode = GameMode.Town;
                        Console.WriteLine("HP가 0이므로 마을로 이동합니다");
                        return;
                    }
                    Console.WriteLine("0.나가기\n");

                    Console.WriteLine("원하시는 행동을 입력해주세요");
                    string input = Console.ReadLine();
                    if (input == "0")
                        return;
                }
            }
            //권장 방어력보다 높으면
            else
            {
                Console.WriteLine("[일반 던전공략 성공!]");

                Console.WriteLine("[탐험결과]");
                int rand2 = _globalRandom.Next(20, 36);
                Console.Write($"체력 {_player.GetHp()} -> ");
                _player.OnDamaged(rand2 - (_player._def - REQUIRE_DEF));
                Console.WriteLine($"{_player.GetHp()}");
                
                rand2 = _globalRandom.Next(_player._atk, _player._atk * 2);
                Console.Write($"Gold {_player._gold} -> ");
                _player._gold += (REWARD + (REWARD * (rand2 / 100)));
                Console.WriteLine($"{_player._gold}");

                //클리어 횟수 추가
                DungeonClearCount++;
                CheckLevelUp(_player);

                //체력이 0이하라면 자동으로 마을로 가기
                if (_player.GetHp() <= 0)
                {
                    _mode = GameMode.Town;
                    Console.WriteLine("HP가 0이므로 마을로 이동합니다");
                    return;
                }

                Console.WriteLine("0.나가기\n");
                
                Console.WriteLine("원하시는 행동을 입력해주세요");
                string input = Console.ReadLine();
                if (input == "0")
                    return;
            }
        }

        public void HardDungeon(Player _player)
        {
            const int REQUIRE_DEF = 17;
            const int REWARD = 2000;

            //권장 방어력보다 낮으면
            if (_player._def < REQUIRE_DEF)
            {
                //70%확률로 실패
                int rand = _globalRandom.Next(0, 10);
                if (rand < 4)
                {
                    Console.WriteLine("[어려운 던전공략 실패!]");
                    _player.OnDamaged((int)(_player.GetHp() * 0.5f));
                    Console.WriteLine($"체력이 {_player.GetHp()}으로 깎였습니다");

                    //체력이 0이하라면 자동으로 마을로 가기
                    if (_player.GetHp() <= 0)
                    {
                        _mode = GameMode.Town;
                        Console.WriteLine("HP가 0이므로 마을로 이동합니다");
                        return;
                    }
                }
                //30%확률로 성공
                else
                {
                    Console.WriteLine("[어려운 던전공략 성공!]");

                    Console.WriteLine("[탐험결과]");
                    int rand2 = _globalRandom.Next(20, 36);
                    Console.Write($"체력 {_player.GetHp()} -> ");
                    _player.OnDamaged(rand2 - (REQUIRE_DEF - _player._def));
                    Console.WriteLine($"{_player.GetHp()}");

                    rand2 = _globalRandom.Next(_player._atk, _player._atk * 2);
                    Console.Write($"Gold {_player._gold} -> ");
                    _player._gold += (REWARD + (REWARD * (rand2 / 100)));
                    Console.WriteLine($"{_player._gold}");


                    //클리어 횟수 추가
                    DungeonClearCount++;
                    CheckLevelUp(_player);

                    //체력이 0이하라면 자동으로 마을로 가기
                    if (_player.GetHp() <= 0)
                    {
                        _mode = GameMode.Town;
                        Console.WriteLine("HP가 0이므로 마을로 이동합니다");
                        return;
                    }

                    Console.WriteLine("0.나가기\n");

                    Console.WriteLine("원하시는 행동을 입력해주세요");
                    string input = Console.ReadLine();
                    if (input == "0")
                        return;
                }
            }
            //권장 방어력보다 높으면
            else
            {
                Console.WriteLine("[어려운 던전공략 성공!]");

                Console.WriteLine("[탐험결과]");
                int rand2 = _globalRandom.Next(20, 36);
                Console.Write($"체력 {_player.GetHp()} -> ");
                _player.OnDamaged(rand2 - (_player._def - REQUIRE_DEF));
                Console.WriteLine($"{_player.GetHp()}");

                rand2 = _globalRandom.Next(_player._atk, _player._atk * 2);
                Console.Write($"Gold {_player._gold} -> ");
                _player._gold += (REWARD + (REWARD * (rand2 / 100)));
                Console.WriteLine($"{_player._gold}");

                //클리어 횟수 추가
                DungeonClearCount++;
                CheckLevelUp(_player);

                //체력이 0이하라면 자동으로 마을로 가기
                if (_player.GetHp() <= 0)
                {
                    _mode = GameMode.Town;
                    Console.WriteLine("HP가 0이므로 마을로 이동합니다");
                    return;
                }
                Console.WriteLine("0.나가기\n");

                Console.WriteLine("원하시는 행동을 입력해주세요");
                string input = Console.ReadLine();
                if (input == "0")
                    return;
            }
        }


        public void CheckLevelUp(Player _player)
        {
            if(DungeonClearCount == RequireDungeonClearCount)
            {
                Console.Write($"Level Up! {_player._level} -> ");
                _player.LevelUp();
                RequireDungeonClearCount++;
                DungeonClearCount = 0;
                Console.WriteLine($"{_player._level}으로 레벨업 하였습니다! ");
            }
        }
    }
}
