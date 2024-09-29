using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TextRpg
{

    public enum PlayerType
    {
        None = 0,
        Knight = 1,
        Archer = 2,
        Mage = 3,

    }

    class Player : Creature
    {
        protected bool IsInitialized { get; set; } = false;

        protected PlayerType _playerType = PlayerType.None;
        public EquipmentItem EquippedWeapon { get; private set; }
        public EquipmentItem EquippedArmor { get; private set; }

        public bool IsEquippedWeapon { get; private set; } = false;
        public bool IsEquippedArmor { get; private set; } = false ;
        public int Level { get;  set; }
        public int ItemAtk { get; set; }
        public int _itemDef { get; set; }

        //인벤
        public List<Player> players = new List<Player>();

        public virtual void Initialize()
        {
            Hp = InitHp;
            Mp = InitMp;
            Gold = 5000;
        }
        public Player(PlayerType type) : base(CreatureType.Player)
        {
            _playerType = type;
        }

        public virtual void SetInfo(int atk, int def, int hp, int mp, int level)
        {
            Atk = atk;
            Def = def;
            InitHp = hp;
            Hp = hp;
            InitMp = mp;
            Level = level;
        }

        public PlayerType GetPlayerType()
        {
            return _playerType;
        }

        public bool CanEquip(EquipmentItem item)
        {
            if(item == null) 
                return false;
            return item._itemClass == _playerType;
        }
        public void Equip(EquipmentItem item)
        {
            if(CanEquip(item))
            {
                if(item.equipmentType== EquipmentType.Weapon)
                {
                    if (IsEquippedWeapon == true)
                        return;

                    IsEquippedWeapon = true;
                    EquippedWeapon = item;
                    item._Isequip = true;
                    Console.WriteLine($"{item._itemName}이(가) 무기로 장착되었습니다.");
                    //스탯 변경
                    Weapon weapon = (Weapon)item;
                    ItemAtk += weapon.GetDamage();
                }
                else if((item.equipmentType == EquipmentType.Armor))
                {
                    if (IsEquippedArmor == true)
                        return;

                    IsEquippedArmor = true;
                    EquippedArmor = item;
                    item._Isequip = true;
                    Console.WriteLine($"{item._itemName}이(가) 방어구로 장착되었습니다.");
                    //스탯 변경
                    Armor armor = item as Armor;
                    _itemDef += armor.GetDefence();
                }
            }
            else
            {
                Console.WriteLine($"{item._itemName}은(는) {_playerType} 직업이 장착할 수 없습니다.");
            }
        }
        public void Unequip(EquipmentItem item)
        {
            if (CanEquip(item))
            {
                if ((item.equipmentType == EquipmentType.Weapon))
                {
                    IsEquippedWeapon = false;
                    EquippedWeapon = null;
                    item._Isequip = false;
                    Console.WriteLine($"{item._itemName}이(가) 해제되었습니다.");
                    //스탯 변경
                    Weapon weapon = (Weapon)item;
                    ItemAtk -= weapon.GetDamage();
                }
                else if ((item.equipmentType == EquipmentType.Armor))
                {
                    IsEquippedArmor = false;
                    EquippedArmor = null;
                    item._Isequip = false;
                    Console.WriteLine($"{item._itemName}이(가) 해제되었습니다.");
                    //스탯 변경
                    Armor armor = item as Armor;
                    _itemDef -= armor.GetDefence();
                }
            }
        }
        public virtual void LevelUp()
        {
            Atk += (int)(Atk * 0.3f);
            Def += (int)(Def * 0.3f);
            Hp += (int)(Hp * 0.3f);
        }

        public void ShowInfo()
        {
            Console.WriteLine($"레벨 : {Level}");
            Console.WriteLine($"직업: {GetPlayerType()}");
            Console.WriteLine($"공격력 : {Atk + ItemAtk}  (+{ItemAtk})");
            Console.WriteLine($"방어력 : {Def + _itemDef}  (+{_itemDef})");
            Console.WriteLine($"체력 : {GetHp()}");
            Console.WriteLine($"골드 : {Gold}");

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("0. 나가기");

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string input = Console.ReadLine();
            if (input == "0")
            {
                return;
            }
            Console.WriteLine();
        }
    }

    class Knight : Player
    {
        public Knight() : base(PlayerType.Knight)
        {
                
        }
        public override void Initialize()
        {
            base.Initialize();
            SetInfo(10, 10, 90, 100,1);  // 레벨 1, 공격력 10, 방어력 10, 체력 90으로 초기화
        }
    }

    class Archor : Player
    {
        public Archor() : base(PlayerType.Archer)
        {
        
        }
        public override void Initialize()
        {
            SetInfo(13, 7, 90, 120, 1);  // 레벨 1, 공격력 10, 방어력 10, 체력 90으로 초기화
        }
        public override void LevelUp()
        {
            base.LevelUp();
        }
    }

    class Mage : Player
    {
        public Mage() : base(PlayerType.Mage)
        {
           
        }

        public override void Initialize()
        {
            SetInfo(15, 5, 90, 150, 1);  // 레벨 1, 공격력 10, 방어력 10, 체력 90으로 초기화
        }

        public override void LevelUp()
        {
            base.LevelUp();
        }
    }
}
