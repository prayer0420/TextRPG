using Newtonsoft.Json;
using System;
using System.Collections;
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

    public class Creature
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
        public int MaxHp{ get; set; }
        public int MaxMp{ get; set; }

        public int Level { get; set; }
        public double CriticalChance { get; set; }
        public double EvasionChance { get; set; }
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
        public int CalculateDamage()
        {
            Random rand = new Random();
            int damageRange = (int)(Atk* 0.1);
            int finalDamage = Atk+ rand.Next(-damageRange, damageRange + 1);
            return finalDamage > 0 ? finalDamage : 1;
        }

        public int GetHp()
        {
            return Hp; 
        }
        public void RecoveryHp(int hp)
        {
            if(hp>= MaxHp)
                Hp = MaxHp;
            else
                Hp += hp;
        }
        public void RecoveryMp(int mp)
        {
            if (mp >= MaxMp)
                mp = MaxMp;
            else
                Mp += mp;
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
            MaxHp = hp;
            Hp  = hp;
            InitMp = mp;
            Mp = mp;
            MaxMp = mp;
        }

        virtual public void PrintInfo()
        {}


    }
}
