using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{

    public enum MonsterType
    {
        None = 0,
        Slime = 1,
        Orc = 2,
        Skeleton = 3,

    }
    class Monster : Creature
    {

        protected MonsterType _monsterType = MonsterType.None;

        protected Monster(MonsterType type): base(CreatureType.Monster)
        {
            _monsterType = type;
        }

        public MonsterType GetMonsterType()
        {
            return _monsterType;
        }
    }


    class Slime : Monster
    {
        public Slime(): base(MonsterType.Slime)
        {
            SetInfo(1, 5, 3, 40);
        }
    }

    class Orc: Monster
    {
        public Orc() : base(MonsterType.Orc)
        {
            SetInfo(1, 7, 5, 50);
        }
    }

    class Skeleton : Monster
    {
        public Skeleton() : base(MonsterType.Skeleton)
        {
            SetInfo(1, 9, 6, 60);
        }
    }
}
