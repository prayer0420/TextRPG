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
        public Item EquippedWeapon { get; private set; }
        public Item EquippedArmor{ get; private set; }

        public bool IsEquippedWeapon { get; private set; } = false;
        public bool IsEquippedArmor { get; private set; } = false ;
        public override int _level { get;  set; }

        public override int _atk { get;  set; } 
        public override int _def { get;  set; } 
        public override int _gold { get; set; }

        public override int _itemAtk { get; set; }
        public override int _itemDef { get; set; }

        public override int _hp { get; set; }
        public override int _initHp { get; set; }


        //인벤
        public List<Player> players = new List<Player>();

        public virtual void Initialize()
        {

        }
        public Player(PlayerType type) : base(CreatureType.Player)
        {
            _playerType = type;
        }

        public PlayerType GetPlayerType()
        {
            return _playerType;
        }

        public bool CanEquip(Item item)
        {
            if(item == null) 
                return false;
            return item._itemClass == _playerType;
        }
        public void Equip(Item item)
        {
            if(CanEquip(item))
            {
                if(item._itemType == ItemType.Weapon)
                {
                    if (IsEquippedWeapon == true)
                        return;

                    IsEquippedWeapon = true;
                    EquippedWeapon = item;
                    item._Isequip = true;
                    Console.WriteLine($"{item._itemName}이(가) 무기로 장착되었습니다.");
                    //스탯 변경
                    Weapon weapon = (Weapon)item;
                    _itemAtk += weapon.GetDamage();
                }
                else if(item._itemType == ItemType.Armor)
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
        public void Unequip(Item item)
        {
            if (CanEquip(item))
            {
                if (item._itemType == ItemType.Weapon)
                {
                    IsEquippedWeapon = false;
                    EquippedWeapon = null;
                    item._Isequip = false;
                    Console.WriteLine($"{item._itemName}이(가) 해제되었습니다.");
                    //스탯 변경
                    Weapon weapon = (Weapon)item;
                    _itemAtk -= weapon.GetDamage();
                }
                else if (item._itemType == ItemType.Armor)
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

        public override void LevelUp()
        {
            base.LevelUp();
        }

    }

    class Knight : Player
    {
        
        public Knight() : base(PlayerType.Knight)
        {
                
        }
        public override void Initialize()
        {
            SetInfo(1, 10, 10, 90);  // 레벨 1, 공격력 10, 방어력 10, 체력 90으로 초기화
            _hp = _initHp;
            _gold = 10000;
        }
        public override void LevelUp()
        {
            base.LevelUp();
            _atk += (int)(_atk * 0.3f);
            _def += (int)(_def * 0.3f);
            _hp += (int)(_hp * 0.3f);
        }
    }

    class Archor : Player
    {
        public Archor() : base(PlayerType.Archer)
        {
            //level, atk, def, hp
            //SetInfo(1, 12, 7, 75);
        }
        public override void LevelUp()
        {
            base.LevelUp();
            _atk += (int)(_atk * 0.4f);
            _def += (int)(_def * 0.2f);
            _hp += (int)(_hp * 0.3f);
        }
    }

    class Mage : Player
    {
        public Mage() : base(PlayerType.Mage)
        {
            //level, atk, def, hp
            //SetInfo(1, 15, 5, 60);
        }
        public override void LevelUp()
        {
            base.LevelUp();
            _atk += (int)(_atk * 0.45f);
            _def += (int)(_def * 0.23f);
            _hp += (int)(_hp * 0.25f);
        }
    }

}
