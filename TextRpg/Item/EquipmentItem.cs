using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{

    public enum EquipmentType
    {
        None = 0,
        Weapon = 1,
        Armor = 2,
    }

    public enum ItemRarity
    {
        None = 0,
        Normal = 1,
        Rare = 2,
        Unique = 3,
    }

    public enum ItemClass
    {
        None = 0,
        Knight = 1,
        Archer = 2,
        Mage = 3,
    }

    public class EquipmentItem: Item
    {

        public bool _Isequip { get; set; } = false;
        public EquipmentType equipmentType { get; set; }
        public PlayerType _itemClass { get; set; }
        public ItemRarity _rarity { get; set; }


        public EquipmentItem(EquipmentType type) : base(ItemType.EquipmentItem)
        {
            equipmentType = type;
            //희귀도 + 가격 선정
            int rand = _globalRandom.Next(0, 100);
            if (rand < 50)
            {
                _rarity = ItemRarity.Normal;

            }
            else if (rand < 80)
            {
                _rarity = ItemRarity.Rare;
                _price += 300;
            }
            else
            {
                _rarity = ItemRarity.Unique;
                _price += 500;
            }

            //아이템 클래스
            int random = _globalRandom.Next(1, 100);
            if (random < 30)
            {
                _itemClass = PlayerType.Knight;
            }
            else if (random < 60)
            {
                _itemClass = PlayerType.Archer;
            }
            else
                _itemClass = PlayerType.Mage;

            ItemDic itemDic = ItemDic.GetInstance();

            //추후에 중복코드 깔끔하게 만들어보자..
            if (equipmentType == EquipmentType.Weapon)
            {

                if (_itemClass == PlayerType.Knight)
                {
                    //아이템 키 리스트 생성
                    List<string> _itemIdx = new List<string>(itemDic._KnightWeaponNames.Keys);
                    int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                    //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                    _itemName = _itemIdx[randomNumber];
                    //해당 키에 맞는 값(설명) 가져오기

                    _iteminfo = itemDic._KnightWeaponNames[_itemName];
                }
                else if (_itemClass == PlayerType.Archer)
                {
                    List<string> _itemIdx = new List<string>(itemDic._ArcherWeaponNames.Keys);
                    int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                    _itemName = _itemIdx[randomNumber];
                    _iteminfo = itemDic._ArcherWeaponNames[_itemName];
                }
                else if (_itemClass == PlayerType.Mage)
                {
                    List<string> _itemIdx = new List<string>(itemDic._MageWeaponNames.Keys);
                    int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                    _itemName = _itemIdx[randomNumber];
                    _iteminfo = itemDic._MageWeaponNames[_itemName];
                }
            }

            else if(equipmentType == EquipmentType.Armor)
            {
                if (_itemClass == PlayerType.Knight)
                {
                    //아이템 키 리스트 생성
                    List<string> _itemIdx = new List<string>(itemDic._KnightArmorNames.Keys);
                    int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                    //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                    _itemName = _itemIdx[randomNumber];
                    //해당 키에 맞는 값(설명) 가져오기

                    _iteminfo = itemDic._KnightArmorNames[_itemName];
                }
                else if (_itemClass == PlayerType.Archer)
                {
                    List<string> _itemIdx = new List<string>(itemDic._ArcherArmorNames.Keys);
                    int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                    _itemName = _itemIdx[randomNumber];
                    _iteminfo = itemDic._ArcherArmorNames[_itemName];
                }
                else if (_itemClass == PlayerType.Mage)
                {
                    List<string> _itemIdx = new List<string>(itemDic._MageArmorNames.Keys);
                    int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                    _itemName = _itemIdx[randomNumber];
                    _iteminfo = itemDic._MageArmorNames[_itemName];
                }
            }

        }
        public virtual void PrintRarity()
        {
            switch (_rarity)
            {
                case ItemRarity.Normal:
                    Console.WriteLine("[희귀도] 일반");
                    break;
                case ItemRarity.Rare:
                    Console.WriteLine("[희귀도] 레어");
                    break;
                case ItemRarity.Unique:
                    Console.WriteLine("[희귀도] 유니크");
                    break;
            }
        }
    }

    class Weapon : EquipmentItem
    {
        private int _damage;

        public Weapon() : base(EquipmentType.Weapon)
        {
            equipmentType = EquipmentType.Weapon;
            //희귀도
            switch (_rarity)
            {
                case ItemRarity.Normal:
                    _damage = 1 + _globalRandom.Next(1, 10) % 5;
                    break;
                case ItemRarity.Rare:
                    _damage = 10 + _globalRandom.Next(1, 10) % 20;
                    break;
                case ItemRarity.Unique:
                    _damage = 50 + _globalRandom.Next(1, 10) % 40;
                    break;
            }
        }

        public override void PrintInfo()
        {
            Console.Write("-");
            if (_Isequip)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("[E] ");
            }
            Console.Write($"{_itemName}  | 공격력 + {_damage}  | {_iteminfo} ");
            Console.ResetColor();
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
        }

        public int GetDamage()
        {
            return _damage;
        }
    }


    class Armor : EquipmentItem
    {
        public int _defence = 0;

        public Armor() : base(EquipmentType.Armor)
        {
            equipmentType = EquipmentType.Armor;

            //희귀도
            switch (_rarity)
            {
                case ItemRarity.Normal:
                    _defence = 1 + _globalRandom.Next(1, 10) % 5;
                    break;
                case ItemRarity.Rare:
                    _defence = 10 + _globalRandom.Next(1, 10) % 20;
                    break;
                case ItemRarity.Unique:
                    _defence = 50 + _globalRandom.Next(1, 10) % 40;
                    break;
            }

            ItemDic itemDic = ItemDic.GetInstance();
        }

        public int GetDefence()
        {
            return _defence;
        }

        public void SetDefence(int defence)
        {
            _defence = defence;
        }

        public override void PrintInfo()
        {
            Console.Write("-");
            if (_Isequip)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("[E] ");
            }
            Console.Write($"{_itemName}  | 방어력 + {_defence}  | {_iteminfo} ");
            Console.ResetColor();
        }
    }
}
