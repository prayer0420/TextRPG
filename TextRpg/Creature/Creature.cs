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
        public int Atk {  get;  set; }
        public int Def {  get;  set; }
        public int Gold {  get; set; }

        public int Hp { get; set; }
        public int Mp { get; set; }
        public int InitHp { get; set; }
        public int InitMp { get; set; }

        public Creature()
        { }

        public Creature(CreatureType creatureType)
        {
            _type = creatureType;

        }

        public void OnDamaged(int damage) 
        {
            Hp -= damage;
            if (Hp <= 0)
                Hp = 0;
        }

        public int GetHp()
        {
            return Hp; 
        }
        public void RecoveryHp(int hp)
        {
            if(hp>= InitHp)
                Hp = InitHp;
            else
                Hp += hp;
        }

        public bool isDead()
        {
            return Hp <= 0;
        }

        public virtual void SetInfo(int atk, int def, int hp, int mp)
        {
            Atk = atk;
            Def = def;
            InitHp = hp;
            Hp  = hp;
            InitMp = mp;
        }

        virtual public void PrintInfo()
        {}


    }
}
