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
        public string PlayerName;
        public static Random _globalRandom = new Random();

        private static Game _instance = new Game();

        public static Game GetInstance()
        {
            if(_instance == null)   
                return _instance;
            return _instance;
        }

        //게임데이터 저장
        public void SaveGameData()
        {
            GameData gameData = new GameData(_player, _inventory, _shop, _mode);
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
                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.Write("원하시는 이름을 설정해주세요.\n>> ");
                PlayerName = Console.ReadLine();

                MainProcess(PlayerName);

            }

            while (true)
            {
                _mode |= GameMode.Town;
                MainProcess(PlayerName);
            }
        }


        public void Exit()
        {
            Console.WriteLine("게임을 종료합니다");
            // 게임 종료 시 데이터 저장
            //SaveGameData(_gameData);
            Environment.Exit(0);
        }

        public void MainProcess(string PlayerName)
        {
            switch (_mode)
            {
                case GameMode.Lobby:
                    ProcessLobby(PlayerName);
                    break;
                case GameMode.Town:
                    ProcessTown();
                    break;
                case GameMode.Dungeon:
                    ProcessDungeon(_player);
                    break;
            }
        }


        public void ProcessLobby(string playerName)
        {
            Console.WriteLine($"{playerName}님! 세계가 당신을 기다립니다. 직업을 선택하세요");
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
                    _player.Initialize();
                    break;
                case "3":
                    _player = new Mage();
                    _mode = GameMode.Town;
                    _player.Initialize();
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
            Console.WriteLine("[5] 퀘스트 의뢰소 가기");
            Console.WriteLine("[6] 저장하기");
            Console.WriteLine("[7] 나가기");

            string input = Console.ReadLine();
            switch (input)
            {
                //상태보기
                case "1":
                    _player.ShowInfo();
                    break;
                //인벤토리
                case "2":
                    Inventory.GetInstance().ShowInven(_player);
                    break;
                //상점
                case "3":
                    _shop.ShowShop(_player);
                    break;
                //던전 입장
                case "4":
                    _mode = GameMode.Dungeon;
                    break;
                //퀘스트 의뢰소 가기
                case "5":
                    ProcessQuest(_player);
                    break;
                case "6":
                    SaveGameData();
                    break;
                case "7":
                    Exit();
                    break;

            }
        }

        public void ProcessQuest(Player player)
        {
            Console.WriteLine("어서오세요. 여기는 퀘스트 의뢰소 입니다.\n 무엇을 하시겠나요?");
            Console.WriteLine("1. 진행 가능한 퀘스트 보기");
            Console.WriteLine("2. 진행 중인 퀘스트 보기");
            Console.WriteLine("3. 퀘스트 완료 하기");

            Console.Write(">> ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    QuestManager.GetInstance().ShowAvailableQuests(player);
                    Console.WriteLine("퀘스트를 선택하여 상세정보를 보거나 수락하세요:");
                    int questIndex = int.Parse(Console.ReadLine());
                    QuestManager.GetInstance().SelectQuest(questIndex, player);
                    break;
                case 2:
                    QuestManager.GetInstance().ShowActiveQuests();
                    break;
                case 3:
                    QuestManager.GetInstance().ShowActiveQuests();
                    Console.WriteLine("완료할 퀘스트를 선택하세요:");
                    int completeIndex = int.Parse(Console.ReadLine());
                    QuestManager.GetInstance().CompleteQuest(completeIndex, player);
                    break;
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
                    Dungeon.GetInstance().EasyDungeon(_player);
                    break;
                case "2":
                    Dungeon.GetInstance().NormalDungeon(_player);
                    break;
                case "3":
                    Dungeon.GetInstance().HardDungeon(_player);
                    break;
                case "0":
                    _mode = GameMode.Town;
                    break;
            }
        }


        public void CheckLevelUp(Player _player)
        {
                Console.Write($"Level Up! {_player._level} -> ");
                _player.LevelUp();
                Console.WriteLine($"{_player._level}으로 레벨업 하였습니다! ");
            }
        }



        //public void ProcessRest(Player _player)
        //{
        //    Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. 보유 골드 : {_player._gold} ");

        //    Console.WriteLine();

        //    Console.WriteLine("1. 휴식하기");
        //    Console.WriteLine("0. 나가기\n");
        //    Console.WriteLine("원하시는 행동을 입력해주세요.");
        //    string input = Console.ReadLine();
        //    if (input == "0")
        //    {
        //        return;
        //    }
        //    else if (input == "1")
        //    {
        //        // 돈 감소
        //        _player._gold -= 500;
        //        // 체력 회복
        //        _player.RecoveryHp(100);

        //        Console.WriteLine("체력이 회복되었습니다");
        //        Console.WriteLine("마을로 이동합니다");
        //    }
        //}
    }

