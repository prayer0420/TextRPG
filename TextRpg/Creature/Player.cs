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

    public class Player : Creature
    {
        protected bool IsInitialized { get; set; } = false;

        public PlayerType _playerType = PlayerType.None;
        public EquipmentItem EquippedWeapon { get; private set; }
        public EquipmentItem EquippedArmor { get; private set; }

        public bool IsEquippedWeapon { get; private set; } = false;
        public bool IsEquippedArmor { get; private set; } = false ;
        public int ItemAtk { get; set; }
        public int _itemDef { get; set; }
        public double NextLevelExp { get; set; }

        public double Experience { get; set; }  
        //인벤
        public List<Player> players = new List<Player>();

        public virtual void Initialize()
        {
            Hp = InitHp;
            MaxHp = InitHp;
            
            Mp = InitMp;
            MaxMp = InitMp;

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
            MaxHp = hp;
            Hp = hp;
            InitMp = mp;
            MaxMp = mp;
            Level = level;
        }

        public PlayerType GetPlayerType()
        {
            return _playerType;
        }

        public void GainExperience(double exp)
        {
            Experience += exp;
            while(Experience >= NextLevelExp)
            {
                LevelUp();
            }
        }

        public bool CanEquip(EquipmentItem item)
        {
            if(item == null) 
                return false;
            return item._itemClass == _playerType;
        }
        public void ToggleEquip(EquipmentItem item)
        {
                //해제하기
                if (item._Isequip == true)
                {
                    Unequip(item);
                }
                //장착하기
                else
                {
                    if (item.equipmentType == EquipmentType.Weapon)
                    {
                        //이미 무기를 착용하고 있다면
                        if (IsEquippedWeapon == true)
                        {
                            //기존에 있는 무기를 해제하기
                            Unequip(EquippedWeapon);
                        }
                        IsEquippedWeapon = true;
                        //장착된 무기를 현재아이템으로 설정
                        EquippedWeapon = item;

                        item._Isequip = true;
                        Console.WriteLine($"{item._itemName}이(가) 무기로 장착되었습니다.");
                        //스탯 변경
                        Weapon weapon = (Weapon)item;
                        ItemAtk += weapon.GetDamage();
                    }
                    else if ((item.equipmentType == EquipmentType.Armor))
                    {
                        if (IsEquippedArmor == true)
                        {
                            Unequip(EquippedArmor);
                        }
                        IsEquippedArmor = true;
                        EquippedArmor = item;


                        item._Isequip = true;
                        Console.WriteLine($"{item._itemName}이(가) 방어구로 장착되었습니다.");
                        Armor armor = item as Armor;
                        _itemDef += armor.GetDefence();
                    }
                }
            
        }
        public void Unequip(EquipmentItem item)
        {
            if (CanEquip(item))
            {
                if ((item.equipmentType == EquipmentType.Weapon))
                {
                    //무기 장착되어있지 않음으로 변경
                    IsEquippedWeapon = false;
                    //장착된무기 없음으로 변경
                    EquippedWeapon = null;
                    //이 item은 장착되어있지 않음으로 변경
                    item._Isequip = false;

                    Console.WriteLine($"{item._itemName}이(가) 해제되었습니다.");
                    Weapon weapon = (Weapon)item;
                    ItemAtk -= weapon.GetDamage();
                }
                else if ((item.equipmentType == EquipmentType.Armor))
                {
                    IsEquippedArmor = false;
                    EquippedArmor = null;
                    item._Isequip = false;

                    Console.WriteLine($"{item._itemName}이(가) 해제되었습니다.");
                    Armor armor = item as Armor;
                    _itemDef -= armor.GetDefence();
                }
            }
        }
        public virtual void LevelUp()
        {
            Level++;
            Experience -= NextLevelExp;
            NextLevelExp = Level * Level * 5 + 10;
            MaxHp+= 10;
            Atk += 1;
            Def += 1;
            Hp = MaxHp;
            Console.WriteLine($"레벨 업! 현재 레벨: {Level}");
        }

        public void ShowInfo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[플레이어 상태 창]");
            Console.ResetColor();
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
                Game.GetInstance().ProcessTown();
            }
            Console.WriteLine();
        }
    }

    public class Knight : Player
    {
        public Knight() : base(PlayerType.Knight)
        {
          
        }
        public override void Initialize()
        {
            base.Initialize();
            SetInfo(10, 10, 90, 100, 1);  //공격력 10, 방어력 10, 체력 90으로 초기화
        }
    }

    public class Archor : Player
    {
        public Archor() : base(PlayerType.Archer)
        {
        
        }
        public override void Initialize()
        {
            SetInfo(13, 7, 90, 120, 1);  //공격력 10, 방어력 10, 체력 90으로 초기화
        }
        public override void LevelUp()
        {
            base.LevelUp();
        }
    }

    public class Mage : Player
    {
        public Mage() : base(PlayerType.Mage)
        {
           
        }

        public override void Initialize()
        {
            SetInfo(15, 5, 90, 150, 1);  //공격력 10, 방어력 10, 체력 90으로 초기화
        }

        public override void LevelUp()
        {
            base.LevelUp();
        }
    }
}
