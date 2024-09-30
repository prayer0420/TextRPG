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
        public GameMode mode = GameMode.Lobby;
        public Inventory inventory = Inventory.GetInstance();
        public Shop shop = new Shop();
        public Player player = null;
        public Monster monster = null;
        public GameData gameData;
        public string PlayerName;
        public static Random globalRandom = new Random();
        private static int dungeonLevel = 1;
        private static Game instance = new Game();


        public TimeSpan TotalPlayTime { get; private set; }  // 총 플레이 시간
        public Stopwatch stopwatch { get; set; }  // 게임 실행 중 시간 추적

        public static Game GetInstance()
        {
            if (instance == null)
                return instance;
            return instance;
        }

        //게임데이터 저장
        public void SaveGameData()
        {
            Console.Clear();

            GameData gameData = new GameData(player, inventory, shop, mode);
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
            Console.Clear();

            string filePath = "gameData.json";

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                GameData gameData = JsonConvert.DeserializeObject<GameData>(jsonData, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

                // 불러온 데이터 적용
                player = gameData.Player;

                inventory = gameData.Inventory;
                Inventory.GetInstance();

                shop = gameData.Shop;
                mode = gameData.Mode;

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
            Console.Clear();

            if (!LoadGameData())
            {
                mode = GameMode.Lobby;
                //처음 시작한거라면 시간 초기셋팅
                TotalPlayTime = TimeSpan.Zero;
                stopwatch= new Stopwatch();
                stopwatch.Start();
                SetPlayTime(0,15,0);


                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.Write("원하시는 이름을 설정해주세요.\n>> ");
                PlayerName = Console.ReadLine();

                MainProcess(PlayerName);
            }

            while (true)
            {
                mode = GameMode.Town;
                MainProcess(PlayerName);
            }
        }


        public void Exit()
        {
            Console.Clear();

            Console.WriteLine("게임을 종료합니다");
            // 게임 종료 시 데이터 저장
            //SaveGameData(_gameData);
            Environment.Exit(0);
        }

        public void MainProcess(string PlayerName)
        {
            Console.Clear();

            switch (mode)
            {
                case GameMode.Lobby:
                    ProcessLobby(PlayerName);
                    break;
                case GameMode.Town:
                    ProcessTown();
                    break;
                case GameMode.Dungeon:
                    ProcessDungeon(player);
                    break;
            }
        }


        public void ProcessLobby(string playerName)
        {
            Console.Clear();

            Console.WriteLine($"{playerName}님! 세계가 당신을 기다립니다. 직업을 선택하세요");
            Console.WriteLine("[1] 기사");
            Console.WriteLine("[2] 궁수");
            Console.WriteLine("[3] 법사");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    player = new Knight();
                    player.Initialize();
                    mode = GameMode.Town;
                    break;
                case "2":
                    player = new Archor();
                    mode = GameMode.Town;
                    player.Initialize();
                    break;
                case "3":
                    player = new Mage();
                    mode = GameMode.Town;
                    player.Initialize();
                    break;
            }
        }


        public void ProcessTown()
        {
            Console.Clear();

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
                    player.ShowInfo();
                    break;
                //인벤토리
                case "2":
                    Inventory.GetInstance().ShowInven(player);
                    break;
                //상점
                case "3":
                    shop.ShowShop(player);
                    break;
                //던전 입장
                case "4":
                    mode = GameMode.Dungeon;
                    ProcessDungeon(player);
                    break;
                //퀘스트 의뢰소 가기
                case "5":
                    AddQuests();
                    ProcessQuest(player);
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
            Console.Clear();

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

        private void AddQuests()
        {
            Console.Clear();

            // 독립적인 퀘스트 1
            Quest quest1 = new Quest
            (
                "마을을 위협하는 미니언 처치",
                "마을 주민:\n이봐요! 마을 근처에 몬스터들이 너무 많아졌어요.\n마을의 안전을 위해 몬스터 5마리를 처치해 주세요!",
                new Dictionary<string, int> { { "몬스터 처치", 5 } },
                new List<Item> { new EquipmentItem(EquipmentType.Weapon) },
                100,
                QuestStatus.NotStarted,
                1 // 필요한 최소 레벨
            );

            // 독립적인 퀘스트 2
            Quest quest2 = new Quest
                (
                "잃어버린 반지 찾기",
                "노부인:\n젊은이, 내 소중한 반지를 잃어버렸어요. 숲 속 어딘가에 있을 텐데, 찾아주겠나요?",
                new Dictionary<string, int> { { "반지 찾기", 1 } },
                new List<Item> { new ConsumableItem(ConsumalbeType.HpPortion) },
                50,
                QuestStatus.NotStarted,
                1
            );

            // 독립적인 퀘스트 3 (연계 퀘스트의 시작)
            Quest quest3 = new Quest
                (
                "숲 속의 이상한 소문 조사",
                "마을 장로:\n최근 숲 속에서 이상한 일이 일어나고 있다는 소문이 있어요. 확인해 주실 수 있나요?",
                new Dictionary<string, int> { { "숲 조사", 1 } },
                new List<Item> { new EquipmentItem(EquipmentType.Armor) },
                150,
                QuestStatus.NotStarted,
                2
            );

            // 연계 퀘스트 1
            Quest quest4 = new Quest
                (
                "숲 속의 고블린 퇴치",
                "숲 속에서 고블린이 나타났습니다! 마을을 지키기 위해 고블린 3마리를 처치해 주세요.",
                new Dictionary<string, int> { { "고블린 처치", 3 } },
                new List<Item> { new EquipmentItem(EquipmentType.Weapon) },
                200,
                QuestStatus.NotStarted,
                3
            );

            // 연계 퀘스트 2
            Quest quest5 = new Quest
                (
                "고블린 두목의 정체",
                "마을 장로:\n고블린들의 배후에 누군가 있는 것 같아요. 고블린 두목을 찾아 처치해 주세요.",
                new Dictionary<string, int> { { "고블린 두목 처치", 1 } },
                new List<Item> { new EquipmentItem(EquipmentType.Armor) },
                300,
                QuestStatus.NotStarted,
                4
            );

            // 최종 퀘스트
            Quest quest6 = new Quest
                (
                "마왕 토벌",
                "마을 장로:\n모든 문제의 원흉인 마왕이 모습을 드러냈습니다. 마왕을 처치하고 세계를 구해주세요!",
                new Dictionary<string, int> { { "마왕 처치", 1 } },
                new List<Item> { new EquipmentItem(EquipmentType.Weapon) },
                1000,
                QuestStatus.NotStarted,
                5
            );

            // 퀘스트 간의 연계 설정
            quest4.RequiredQuest = quest3; // 퀘스트 3을 완료해야 퀘스트 4 진행 가능
            quest5.RequiredQuest = quest4; 
            quest6.RequiredQuest = quest5; 

            QuestManager questManager = QuestManager.GetInstance();

            // 퀘스트 매니저에 퀘스트 추가
            questManager.AddQuest(quest1);
            questManager.AddQuest(quest2);
            questManager.AddQuest(quest3);
            questManager.AddQuest(quest4);
            questManager.AddQuest(quest5);
            questManager.AddQuest(quest6);
        }
        
        public void ProcessDungeon(Player _player)
        {
            Console.Clear();

            //체력이 0이하라면 자동으로 마을로 가기
            if (_player.GetHp() <= 0)
            {
                mode = GameMode.Town;
                Console.WriteLine("HP가 0이므로 던전에 들어갈 수 없습니다. 마을로 이동합니다");
                return;
            }

            Console.WriteLine($"던전 {dungeonLevel}층으로 이동합니다...");
            List<Monster> monsters = GenerateMonsters();
            Battle battle = new Battle(_player, monsters);
            battle.Start();

            if (!player.isDead())
            {
                dungeonLevel++;
                player.RecoveryMp(10);
                Console.WriteLine("MP가 10 회복되었습니다.");
            }
            else
            {
                Console.WriteLine("마을로 돌아갑니다.");
                dungeonLevel = 1;
            }

            Console.WriteLine("0. 나가기\n");

            Console.WriteLine("원하시는 행동을 입력해주세요");
            string input = Console.ReadLine();

        }

        private List<Monster> GenerateMonsters()
        {

            List<Monster> monsters = new List<Monster>();
            int monsterCount = new Random().Next(1, 5);

            for (int i = 0; i < monsterCount; i++)
            {
                int monsterType = new Random().Next(1, 3);
                Monster monster = null;
                switch (monsterType)
                {
                    case 1:
                        monster = new Slime();
                        monster.GenerateMonster(
                            $"Lv.{2 * dungeonLevel} 미니언",
                            2 * dungeonLevel,
                            15 + (dungeonLevel * 5),
                            5 + (dungeonLevel * 2),
                            2 + dungeonLevel,
                            0.1,
                            0.05,
                            2 * dungeonLevel,
                            new List<Item> { new HpPortion() },
                            100
                        );
                        break;
                    case 2:
                        monster = new Orc();
                        monster.GenerateMonster(
                            $"Lv.{3 * dungeonLevel} 오크",
                            3 * dungeonLevel,
                            10 + (dungeonLevel * 5),
                            9 + (dungeonLevel * 3),
                            3 + dungeonLevel,
                            0.1,
                            0.05,
                            3 * dungeonLevel,
                            new List<Item> { new MpPortion() },
                            200
                        );
                        break;
                    case 3:
                        monster = new Skeleton();
                        monster.GenerateMonster(
                            $"Lv.{5 * dungeonLevel} 스켈레톤",
                            5 * dungeonLevel,
                            25 + (dungeonLevel * 10),
                            8 + (dungeonLevel * 4),
                            5 + dungeonLevel,
                            0.1,
                            0.05,
                            5 * dungeonLevel,
                            new List<Item> { new Weapon()},
                            400
                        );
                        break;
                }
                monsters.Add(monster);
            }
            return monsters;
        }


        public void CheckLevelUp(Player _player)
        {
            Console.Clear();

            Console.Write($"Level Up! {_player.Level} -> ");
            _player.LevelUp();
            Console.WriteLine($"{_player.Level}으로 레벨업 하였습니다! ");
        }


        public TimeSpan GetElapsedTime()
        {
            return TotalPlayTime + stopwatch.Elapsed;
        }

        public void SetPlayTime(int hours, int minutes, int seconds)
        {
            // 기존 플레이 시간과 타이머 리셋
            stopwatch.Reset();
            TotalPlayTime = new TimeSpan(hours, minutes, seconds);
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
}