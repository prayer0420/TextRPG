using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public enum QuestStatus
    {
        NotStarted,
        InProgress,
        Completed
    }

    internal class Quest
    {

        public string Name{ get; set; } //퀘스트 이름
        public string Description { get; set; } //설명
        public Dictionary<string, int> Objectives { get; set; } //퀘스트 목록
        public Dictionary<string, int> Progress { get; set; } //퀘스트의 현재 진행도만 따로
        public List<Item> Rewards {  get; set; } //퀘스트 완료 보상
        public int GoldReward { get; set; } //보상: 골드
        public QuestStatus Status { get; set; } //퀘스트 상태
        public Quest RequiredQuest { get; set; } //선행 퀘스트
        public int RequiredLevel {  get; set; } //필요레벨

        public Quest(string name, string descrtipon, Dictionary<string, int> objectives, List<Item> rewards, int goldReward, QuestStatus status, int requiredLevel)
        {
            Name = name;
            Description = descrtipon;
            Objectives = objectives;
            Progress = new Dictionary<string, int>();
            foreach (var objective in objectives)
            {
                Progress[objective.Key] = 0;
            }
            Rewards = rewards;
            GoldReward = goldReward;
            Status = status;
            RequiredLevel = requiredLevel;
        }


        //퀘스트의 목표량보다 현재 진행량이 작다면 아직 미완료
        bool IsCompleted()
        {
            foreach (var objective in Objectives)
            {
                if (Progress[objective.Key] < objective.Value)
                {
                    return false;   
                }
            }
            return true;
        }

        //현재 진행량을 업데이트함
        public void UpdateProgress(string objective, int amount)
        {
            if(Progress.ContainsKey(objective))
            {
                Progress[objective] += amount;
                if(IsCompleted())
                {
                    // 이번에 업데이트로 인하여 완료가 된것이라면
                    Status = QuestStatus.Completed;
                    Console.WriteLine($"퀘스트 '{Name}' 완료됨!");
                }
            }
        }

        //진행량 보여주기
        public void ShowProgress()
        {
            Console.WriteLine($"퀘스트:  {Name}");
            Console.WriteLine(Description);
            foreach (var objective in Objectives)
            {
                Console.WriteLine($"- {objective.Key} ({Progress[objective.Key]} / {Objectives[objective.Key]})");
            }
            Console.WriteLine(Status == QuestStatus.Completed ? "퀘스트 완료됨!" : "진행 중");
        }
    }


}
