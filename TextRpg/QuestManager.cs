using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    class QuestManager
    {
        public List<Quest> availableQuests; //진행 가능한 퀘스트 
        private List<Quest> activeQuests; //진행중인 퀘스트
        private static QuestManager instance;
        public static QuestManager GetInstance()
        {
            if(instance == null)
            {
                instance = new QuestManager();
            }
            return instance;
        }

        public QuestManager() 
        {
            availableQuests = new List<Quest>();
            activeQuests = new List<Quest>();
            AddQuests();

        }

        public void AddQuest(Quest quest)
        {
            availableQuests.Add(quest);
        }


        //진행 가능한 퀘스트 보여주기
        public void ShowAvailableQuests(Player player)
        {
            Console.WriteLine("진행 가능한 퀘스트");
            int questCount = 0;
            foreach(Quest quest in availableQuests)
            {
                if(quest != null)
                {
                   if (quest.RequiredLevel <= player.Level
                   && (quest.RequiredQuest == null
                   ||  quest.RequiredQuest.Status == QuestStatus.Completed))
                    {
                        Console.WriteLine($"{(questCount++) + 1}. {quest.Name} (레벨 {quest.RequiredLevel} 이상)");
                    }
                }
                else
                {
                    return;
                }
            }
        }

        //진행중인 퀘스트의 진행량 보여주기
        public void ShowActiveQuests()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[진행중인 퀘스트]");
            Console.ResetColor();

            int questCount = 0;
            foreach(Quest quest in activeQuests)
            { 
                if(quest != null)
                {
                    activeQuests[questCount++].ShowProgress();
                }

            }
            Console.WriteLine("\n0.나가기\n");
            string input = Console.ReadLine();
            if(input == "0")
                return;

        }

        //퀘스트 선택하기 
        public void SelectQuest(int index, Player player)
        {
            //진행 가능한 퀘스트의 범위를 벗어날때
            if (index < 1 || index > availableQuests.Count)
            {
                Console.WriteLine("잘못된 선택입니다");
                return;
            }

            //플레이어레벨이 요구레벨보다 낮으면 X
            Quest quest = availableQuests[index - 1];
            if (quest.RequiredLevel > player.Level)
            {
                Console.WriteLine($"레벨 {quest.RequiredLevel} 이상이어야 이 퀘스트를 수락할 수 있습니다.");
                return;
            }

            //선행 퀘스트가 있는데도 선행퀘스트를 완료한 상태가 아니라면 X
            if (quest.RequiredQuest != null && quest.RequiredQuest.Status != QuestStatus.Completed)
            {
                Console.WriteLine($"이 퀘스트를 수락하려면 이전 퀘스트 '{quest.RequiredQuest.Name}'를 완료해야 합니다.");
            }

            Console.WriteLine($"퀘스트 선택됨: {quest.Name}");
            Console.WriteLine(quest.Description);

            //선택 한 후 
            foreach(var objective in quest.Objectives)
            {
                Console.WriteLine($"- {objective.Key} ({quest.Progress[objective.Key]}/{objective.Value})");
            }

            Console.WriteLine();

            Console.WriteLine("1. 수락");
            Console.WriteLine("2. 거절");

            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                activeQuests.Add(quest);
                availableQuests.RemoveAt(index - 1);
                quest.Status = QuestStatus.InProgress;
                Console.WriteLine("퀘스트 수락됨.");
            }
            else
            {
                Console.WriteLine("퀘스트 거절됨.");
            }
        }

        public void CompleteQuest(int index, Player player)
        {
            //진행 중인 퀘스트의 범위를 벗어날때
            if (index < 1 || index > activeQuests.Count)
            {
                Console.WriteLine("잘못된 선택입니다.");
                return;
            }

            Quest quest = activeQuests[index-1];
            //퀘스트가 완료 상태라면
            if(quest.Status == QuestStatus.Completed)
            {
                player.Gold += quest.GoldReward;
                Console.WriteLine($"골드 {quest.GoldReward}을(를) 받았습니다.");
                foreach(var reward in quest.Rewards)
                {
                    Console.WriteLine($"아이템 획득 : {reward._itemName}");
                    Inventory.GetInstance().AddItem(reward);
                }
                //이제 진행중인 퀘스트 목록에서 제거
                activeQuests.RemoveAt(index - 1); 
            }
            else
            {
                Console.WriteLine("퀘스트가 아직 완료되지 않았습니다.");
            }
        }

        //퀘스트 상태 업데이트
        public void UpdateQuestProgress(string objective, int amount)
        {
            foreach(var quest in activeQuests)
            {
                quest.UpdateProgress(objective,amount);
            }
        }


        private void AddQuests()
        {
            // 독립적인 퀘스트 1
            Quest quest1 = new Quest
            (
                "마을을 위협하는 미니언 처치",
                "마을 주민:\n이봐요! 마을 근처에 미니언들이 너무 많아졌어요.\n마을의 안전을 위해 미니언 5마리를 처치해 주세요!",
                new Dictionary<string, int> { { "미니언 처치", 5 } },
                new List<Item> { new EquipmentItem(EquipmentType.Weapon) },
                100,
                QuestStatus.NotStarted,
                1 // 필요한 최소 레벨
            );

            // 독립적인 퀘스트 2
            Quest quest2 = new Quest
                (
                "잃어버린 반지 찾기",
                "노부인:\n젊은이, 내 소중한 반지를 잃어버렸어요. 던전 속 어딘가에 있을 텐데, 찾아주겠나요?",
                new Dictionary<string, int> { { "반지 찾기", 1 } },
                new List<Item> { new ConsumableItem(ConsumalbeType.HpPortion) },
                50,
                QuestStatus.NotStarted,
                1
            );

            // 독립적인 퀘스트 3 (연계 퀘스트의 시작)
            Quest quest3 = new Quest
                (
                "던전 속의 이상한 소문 조사",
                "마을 장로:\n최근 숲 속에서 이상한 일이 일어나고 있다는 소문이 있어요. 확인해 주실 수 있나요?",
                new Dictionary<string, int> { { "던전 조사", 1 } },
                new List<Item> { new EquipmentItem(EquipmentType.Armor) },
                150,
                QuestStatus.NotStarted,
                2
            );

            // 연계 퀘스트 1
            Quest quest4 = new Quest
                (
                "던전 속의 공허충 퇴치",
                "던전 속에서 공허충이 나타났습니다! 마을을 지키기 위해 고블린 3마리를 처치해 주세요.",
                new Dictionary<string, int> { { "공허충 처치", 3 } },
                new List<Item> { new EquipmentItem(EquipmentType.Weapon) },
                200,
                QuestStatus.NotStarted,
                3
            );

            // 연계 퀘스트 2
            Quest quest5 = new Quest
                (
                "공허충 두목의 정체",
                "마을 장로:\n공허충들의 배후에 누군가 있는 것 같아요. 공허충 두목을 찾아 처치해 주세요.",
                new Dictionary<string, int> { { "공허충 두목 처치", 1 } },
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

            // 퀘스트 매니저에 퀘스트 추가
            AddQuest(quest1);
            AddQuest(quest2);
            AddQuest(quest3);
            AddQuest(quest4);
            AddQuest(quest5);
            AddQuest(quest6);
        }
    }
}
