using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    class QuestManager
    {
        private List<Quest> availableQuests; //진행 가능한 퀘스트 
        private List<Quest> activeQuests; //진행중인 퀘스트
        private static QuestManager instance;
        public static QuestManager GetInstance()
        {
            if(instance == null)
                instance = new QuestManager();
            return instance;
        }

        public QuestManager() 
        {
            availableQuests = new List<Quest>();
            activeQuests = new List<Quest>();
        }

        public void AddQuest(Quest quest)
        {
            availableQuests.Add(quest);
        }

        //진행 가능한 퀘스트 보여주기
        public void ShowAvailableQuests(Player player)
        {
            Console.WriteLine("진행 가능한 퀘스트");
            for (int i = 0; i < availableQuests.Count; i++)
            {
                Quest quest = availableQuests[i];
                //퀘스트 요구레벨이 플레이어 요구레벨보다 낮으면서
                //퀘스트의 선행 퀘스트가 없거나 선행 퀘스트의 상태가 완료 상태라면
                //진행 가능한 퀘스트임!
                if(quest.RequiredLevel <= player._level 
                    && (quest.RequiredQuest == null 
                    || quest.RequiredQuest.Status == QuestStatus.Completed))
                {
                    Console.WriteLine($"{i + 1}. {quest.Name} (레벨 {quest.RequiredLevel} 이상)");
                }
            }
            

        }


        //진행중인 퀘스트의 진행량 보여주기
        public void ShowActiveQuests()
        {
            Console.WriteLine("진행중인 퀘스트");
            for(int i = 0;i < availableQuests.Count;i++)
            {
                activeQuests[i].ShowProgress();
            }
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
            if (quest.RequiredLevel > player._level)
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
                player._gold += quest.GoldReward;
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

    }
}
