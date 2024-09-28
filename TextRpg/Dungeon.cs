using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    class Dungeon
    {
        public static Random _globalRandom = new Random();
        public static int DungeonClearCount = 0;
        public static int RequireDungeonClearCount = 1;
        private static Dungeon instance;
        public static Dungeon GetInstance()
        {
            if(instance == null)
                instance = new Dungeon();
            return instance; 
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
                        Game.GetInstance()._mode = GameMode.Town;
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
                    Game.GetInstance().CheckLevelUp(_player);

                    //체력이 0이하라면 자동으로 마을로 가기
                    if (_player.GetHp() <= 0)
                    {
                        Game.GetInstance()._mode = GameMode.Town;
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
                _player.OnDamaged(rand2 - (_player._def - REQUIRE_DEF));
                Console.WriteLine($"{_player.GetHp()}");

                rand2 = _globalRandom.Next(_player._atk, _player._atk * 2);
                Console.Write($"Gold {_player._gold} -> ");
                _player._gold += REWARD + rand2;
                Console.WriteLine($"{_player._gold}");

                //클리어 횟수 추가
                DungeonClearCount++;
                Game.GetInstance().CheckLevelUp(_player);

                //체력이 0이하라면 자동으로 마을로 가기
                if (_player.GetHp() <= 0)
                {
                    Game.GetInstance()._mode = GameMode.Town;
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
                        Game.GetInstance()._mode = GameMode.Town;
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
                    _player._gold += (REWARD + (REWARD * (rand2 / 100)));
                    Console.WriteLine($"{_player._gold}");
                    //클리어 횟수 추가
                    DungeonClearCount++;
                    Game.GetInstance().CheckLevelUp(_player);

                    //체력이 0이하라면 자동으로 마을로 가기
                    if (_player.GetHp() <= 0)
                    {
                        Game.GetInstance()._mode = GameMode.Town;
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
                Game.GetInstance().CheckLevelUp(_player);

                //체력이 0이하라면 자동으로 마을로 가기
                if (_player.GetHp() <= 0)
                {
                     Game.GetInstance()._mode = GameMode.Town;
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
                        Game.GetInstance()._mode = GameMode.Town;
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
                    Game.GetInstance().CheckLevelUp(_player);

                    //체력이 0이하라면 자동으로 마을로 가기
                    if (_player.GetHp() <= 0)
                    {
                        Game.GetInstance()._mode = GameMode.Town;
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
                Game.GetInstance().CheckLevelUp(_player);

                //체력이 0이하라면 자동으로 마을로 가기
                if (_player.GetHp() <= 0)
                {
                    Game.GetInstance()._mode = GameMode.Town;
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
    }
}
