using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public enum CreatureType
    {
        None = 0,
        Player =1,
        Monster =2,
    }

    internal class Creature
    {
        public CreatureType _type {  get; set; } = CreatureType.None;

        //기본 스탯
        public virtual int _level { get;  set; }
        public virtual int _atk {  get;  set; }
        public virtual int _def {  get;  set; }
        public virtual int _gold {  get; set; }


        //아이템 장착 스텟
        public virtual int _itemAtk { get;  set; }
        public virtual int _itemDef { get;  set; }

        [JsonProperty]
        public virtual int _hp { get; set; }
        public virtual int _initHp { get; set; }

        public Creature()
        { }

        public Creature(CreatureType creatureType)
        {
            _type = creatureType;

        }

        public void OnDamaged(int damage) 
        {
            _hp -= damage;
            if (_hp <= 0)
                _hp = 0;
        }

        public int GetHp()
        {
            return _hp; 
        }
        public void RecoveryHp(int hp)
        {
            if(hp>=_initHp)
                _hp = _initHp;
            else
                _hp += hp;
        }

        public bool isDead()
        {
            return _hp <= 0;
        }

        public void SetInfo(int level, int atk, int def, int hp)
        {
            _level = level;
            _atk = atk;
            _def = def;
            _initHp = hp;
        }

        virtual public void PrintInfo()
        {}

        virtual public void LevelUp()
        {
            _level++;
        }

    }
}
