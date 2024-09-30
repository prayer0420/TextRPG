using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TextRpg
{

    public enum MonsterType
    {
        None = 0,
        Slime = 1,
        Orc = 2,
        Skeleton = 3,

    }
    public class Monster : Creature
    {

        public MonsterType monsterType = MonsterType.None;
        public List<Item> DropItems { get; set; }
        public int ExperienceGiven { get; set; }
        public int DropGold { get; set; }
        public string Name { get; set; }
        public Monster(MonsterType type) : base(CreatureType.Monster)
        {
            monsterType = type;
        }
        public void GenerateMonster(string name, int level, int hp, int atk, int def, double criticalChance, double evasionChance, int expGiven, List<Item> dropItems, int dropGold)
        {
            Name = name;
            Level = level;
            InitHp = hp;
            MaxHp = hp;
            Hp = hp;
            Atk = atk;
            Def = def;
            CriticalChance = criticalChance;
            EvasionChance = evasionChance;
            ExperienceGiven = expGiven;
            DropItems = dropItems;
            DropGold = dropGold;
        }

        public MonsterType GetMonsterType()
        {
            return monsterType;
        }
        public List<Item> DropLoot()
        {
            return DropItems;
        }
    }


    public class Slime : Monster
    {
        public Slime() 
        : base(MonsterType.Slime)
        {
            monsterType = MonsterType.Slime;

        }
    }

    public class Orc: Monster
    {
        public Orc() : base(MonsterType.Orc)
        {
            monsterType = MonsterType.Orc;
        }
    }

    public class Skeleton : Monster
    {
        public Skeleton() : base(MonsterType.Skeleton)
        {
            monsterType = MonsterType.Skeleton;
        }
    }
}
