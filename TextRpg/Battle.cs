using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public class Battle
    {
        private Player player;
        private List<Monster> monsters;
        bool IsEscape = true;
        private static Random globalrandom = new Random();

        public Battle(Player player, List<Monster> monsters)
        {
            this.player = player;
            this.monsters = monsters;
        }  

        public void Start()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n [전투시작!] ");
            Console.ResetColor();

            ShowBattleStatus();

            while(!player.isDead() && monsters.Exists(m=>!m.isDead()))
            {
                PlayerTurn();
                if(monsters.Exists(m=>!m.isDead()))
                {
                    MonsterTurn();
                }
            }

            if (player.isDead())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[전투 결과]");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n패배하였습니다.");
                Console.ResetColor();
                Console.WriteLine($"Lv.{player.Level} {Game.GetInstance().PlayerName}");
                Console.WriteLine($"HP {player.Hp}/{player.InitHp}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[전투 결과]");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n승리하였습니다!");
                Console.ResetColor();
                Console.WriteLine($"던전에서 몬스터 {monsters.Count}마리를 처치했습니다.");

                //경험치 획득
                double totalExp = 0;
                foreach(var monster in monsters)
                {
                    totalExp += monster.ExperienceGiven;
                }

                player.GainExperience(totalExp);
                Console.WriteLine($"경험치를 {totalExp} 획득했습니다.");

                //전리품 획득
                foreach (var monster in monsters)
                {
                    if (monster.DropItems != null)
                    {
                        foreach (var item in monster.DropLoot())
                        {
                            Inventory.GetInstance().AddItem(item);
                        }
                    }
                }

                //골드 획득
                foreach (var monster in monsters)
                {
                    player.Gold += monster.DropGold;
                }
            }
        }

        private void PlayerTurn()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n [전투 중!] ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[플레이어의 차례]");
            Console.ResetColor();

            ShowBattleStatus();
            Console.WriteLine("1. 공격");
            Console.WriteLine("2. 스킬");
            Console.WriteLine("3. 아이템 사용");
            Console.WriteLine("4. 도주하기"); //확률

            Console.Write("원하시는 행동을 입력해주세요.\n>> ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Attack();
                    Console.Clear();
                    break;
                case 2:
                    UseSkill();
                    Console.Clear();
                    break;
                case 3:
                    //아이템 사용
                    break;
                case 4:
                    Escape();
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("잘못된 선택입니다. 다시 입력해주세요");
                    PlayerTurn();
                    break;
            }
        }

        private void Attack()
        {

            Console.Clear();

            ShowBattleStatus();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n [전투 중!] ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n공격할 몬스터를 선택하세요:");
            Console.ResetColor();
            for (int i = 0; i < monsters.Count; i++)
            {
                Monster monster = monsters[i];
                if (!monster.isDead())
                {
                    Console.WriteLine($"{i + 1}. {monster.Name} HP {monster.Hp}/{monster.MaxHp}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{i + 1}. {monster.Name} 사망");
                    Console.ResetColor();
                }
            }

            Console.Write(">> ");
            int targetIndex = int.Parse(Console.ReadLine()) - 1;

            if (targetIndex >= 0 && targetIndex < monsters.Count && !monsters[targetIndex].isDead())
            {
                Monster target = monsters[targetIndex];
                int damage = player.CalculateDamage();
                bool isCritical = globalrandom.NextDouble() < player.CriticalChance;
                bool isEvaded = globalrandom.NextDouble() < target.EvasionChance;

                if (isEvaded)
                {
                    Console.WriteLine($"{Game.GetInstance().PlayerName}의 공격!");
                    Console.WriteLine($"{target.Name}이(가) 공격을 회피했습니다.");
                }
                else
                {
                    if (isCritical)
                    {
                        damage = (int)(damage * 1.6);
                        Console.WriteLine("치명타 공격!!");
                    }
                    target.OnDamaged(damage);
                    Console.WriteLine($"{Game.GetInstance().PlayerName}의 공격!");
                    Console.WriteLine($"{target.Name}에게 {damage}의 피해를 입혔습니다.");

                    if (target.isDead())
                    {
                        Console.WriteLine($"{target.Name}이(가) 쓰러졌습니다.");
                        //퀘스트에 반영
                        QuestManager questManager =  QuestManager.GetInstance();
                        questManager.UpdateQuestProgress($"{target.Name} 처치", 1);

                        switch (target.Name)
                        {
                            case "Slime":
                                questManager.UpdateQuestProgress("슬라임 처치", 1);

                                break;

                            case "Orc":
                                questManager.UpdateQuestProgress("오크 처치", 1);

                                break;

                            case "Skeleton":
                                questManager.UpdateQuestProgress("스켈레톤 처치", 1);
                                break;
                        }
                        //questManager.UpdateQuestProgress("마왕 처치", 1);
                    }
                }
            }
            else
            {
                Console.WriteLine("잘못된 선택입니다.");
                Attack();
            }
        }

        private void UseSkill()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[전투 중!] ");
            Console.ResetColor();

            ShowBattleStatus();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[스킬을 선택하세요]");
            Console.ResetColor();

            Console.WriteLine("1. 알파 스트라이크 - MP 10");
            Console.WriteLine("   공격력 * 2 로 하나의 적을 공격합니다.");
            Console.WriteLine("2. 더블 스트라이크 - MP 15");
            Console.WriteLine("   공격력 * 1.5 로 2명의 적을 랜덤으로 공격합니다.");
            Console.WriteLine("0. 취소");
            Console.Write(">> ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    if (player.Mp >= 10)
                    {
                        player.Mp -= 10;
                        Console.WriteLine("\n알파 스트라이크를 사용합니다.");
                        Attack();
                    }
                    else
                    {
                        Console.WriteLine("MP가 부족합니다.");
                    }
                    break;
                case 2:
                    if (player.Mp >= 15)
                    {
                        player.Mp -= 15;
                        Console.WriteLine("\n더블 스트라이크를 사용합니다.");
                        for (int i = 0; i < 2; i++)
                        {
                            Monster target = monsters[globalrandom.Next(monsters.Count)];
                            if (!target.isDead())
                            {
                                int damage = (int)((player.CalculateDamage()));
                                target.OnDamaged(damage);
                                Console.WriteLine($"{target.Name}에게 {damage}의 피해를 입혔습니다.");
                                if (target.isDead())
                                {
                                    QuestManager questManager = QuestManager.GetInstance();
                                    Console.WriteLine($"{target.Name}이(가) 쓰러졌습니다.");
                                    switch (target.Name)
                                    {
                                        case "Slime":
                                            questManager.UpdateQuestProgress("슬라임 처치", 1);

                                            break;

                                        case "Orc":
                                            questManager.UpdateQuestProgress("오크 처치", 1);

                                            break;

                                        case "Skeleton":
                                            questManager.UpdateQuestProgress("스켈레톤 처치", 1);
                                            break;
                                    }
                                    //questManager.UpdateQuestProgress("마왕 처치", 1);
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("MP가 부족합니다.");
                    }
                    break;
                case 0:
                    break;
                default:
                    Console.WriteLine("잘못된 선택입니다.");
                    UseSkill();
                    break;
            }

            Console.WriteLine("0. 다음");
            string input = Console.ReadLine();
            if(input == "0")
            {
                return;
            }
        }

        private void MonsterTurn()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[전투 중!]");
            Console.ResetColor();

            ShowBattleStatus();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[적의 차례]");
            Console.ResetColor();

            foreach (var monster in monsters)
            {
                if (!monster.isDead())
                {
                    String PlayerName = Game.GetInstance().PlayerName;
                    int damage = monster.CalculateDamage();
                    bool isEvaded = globalrandom.NextDouble() < player.EvasionChance;

                    if (isEvaded)
                    {
                        Console.WriteLine($"{monster.Name}의 공격!");
                        Console.WriteLine($"{PlayerName}이(가) 공격을 회피했습니다!");
                    }
                    else
                    {
                        player.OnDamaged(damage - player.Def);
                        Console.WriteLine($"{monster.Name}의 공격!");
                        Console.WriteLine($"{PlayerName}에게 {damage}의 피해를 입었습니다.");
                        Console.WriteLine($"{PlayerName} HP {player.Hp}/{player.Mp}");
                        if (player.isDead())
                        {
                            Console.WriteLine($"player.isDead()이(가) 쓰러졌습니다.");
                            break;
                        }
                    }
                }
                Console.WriteLine(" 0. 다음");
                string input = Console.ReadLine();
                if(input == "0")
                {
                    IsEscape = true;
                    return;
                }
            }
            IsEscape = true;
        }

        private void ShowBattleStatus()
        {

            Console.Clear();

            Console.WriteLine("\n[몬스터 상태]");
            for (int i = 0; i < monsters.Count; i++)
            {
                Monster monster = monsters[i];
                //죽지않았으면
                if (!monster.isDead())
                {
                    Console.WriteLine($"{monster.Name}  HP {monster.Hp}");
                }
                //죽었으면 
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{monster.Name} 사망");
                    Console.ResetColor();
                }
            }
            Console.WriteLine($"\n[플레이어 상태]");
            Console.WriteLine($"Lv.{player.Level} {Game.GetInstance().PlayerName} ({player.GetPlayerType()})");
            Console.WriteLine($"HP {player.Hp}/{player.InitHp}");
            Console.WriteLine($"MP {player.Mp}/{player.InitMp}\n");
        }

        private void Escape()
        {
            Console.Clear();

            if(IsEscape)
            {
                int probability = globalrandom.Next(0, 10);
                if(probability > 5)
                {
                    //50%확률로 도주 가능
                    Console.WriteLine("도주 성공!");
                    Console.WriteLine("마을로 이동합니다");
                    Game.GetInstance().ProcessTown();

                }
                else
                {
                    Console.WriteLine("도주 실패!");
                    IsEscape = false;
                    PlayerTurn();
                }
            }
            else
            {
                Console.WriteLine("이번 턴에서는 도망칠 수 없습니다. 다음턴에서 다시 시도하세요");
                PlayerTurn();
            }
        }
    }
}
