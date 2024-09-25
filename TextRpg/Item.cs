using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TextRpg
{

    public enum ItemRarity
    {
        None = 0,
        Normal = 1,
        Rare = 2,
        Unique = 3,
    }

    public enum ItemType
    {
        None = 0,
        Weapon = 1,
        Armor = 2,
        Consumable = 3,
    }

    public enum ItemClass
    {
        None =0,
        Knight = 1,
        Archor = 2,
        Mage = 3,
    }

    class Item
    {
        public Item() { }
        protected static Random _globalRandom = new Random();
        public static int _nextId = 0;
        public int _price = 500;

        public ItemRarity _rarity {  get; set; }
        public ItemType _itemType {  get; set; }

        #region 아이템
        public Dictionary<string, string> _KnightWeaponNames = new Dictionary<string, string>
        { 
            // 전사 (Knight) 아이템
            { "용맹의 대검", "전사의 용맹함을 상징하는 강력한 대검, 적을 단번에 베어낼 수 있는 힘을 가진다." },
            { "격노의 전투 도끼", "전투의 격노를 담아내어 강력한 공격력을 발휘하는 양손 도끼." },
            { "전설의 창", "전사의 기백을 담아 먼 거리의 적도 제압할 수 있는 강력한 창." },
            { "불멸의 대검", "전설 속에서 불사의 힘을 얻은 대검, 전사의 힘을 극대화한다." },
            { "폭풍의 망치", "폭풍의 힘을 담아 적을 강타하는 전사의 망치." }
        };
        
        public Dictionary<string, string> _ArcherWeaponNames = new Dictionary<string, string>
        { 
            // 궁수 (Archer) 아이템
            { "정밀한 장궁", "멀리 있는 적도 정확하게 맞출 수 있는 궁수의 필수 장비." },
            { "암흑의 석궁", "어둠 속에서 은밀하게 적을 제압할 수 있는 강력한 석궁." },
            { "폭풍의 활", "폭풍의 힘을 담아 강력한 속도로 화살을 발사하는 마법 활." },
            { "불꽃의 장궁", "불꽃의 힘을 실어 적에게 화염 피해를 입히는 강력한 장궁." },
            { "얼음의 활", "얼음의 기운을 담아 적을 얼어붙게 만드는 신비한 활." }
        };
        
        public Dictionary<string, string> _MageWeaponNames = new Dictionary<string, string>
        {
            // 마법사 (Mage) 아이템
            { "지혜의 지팡이", "고대의 지혜를 담고 있어 마법사의 마법 효율을 극대화시키는 지팡이." },
            { "고대인의 구슬", "오래된 마법사들의 힘이 깃든 구슬로, 강력한 마법을 발현시킬 수 있다." },
            { "천둥의 마법봉", "천둥의 힘을 담아 적을 섬광처럼 빠르게 공격하는 마법봉." },
            { "불멸의 지팡이", "불사의 마법이 깃든 지팡이로, 마법사의 생명력을 강화시킨다." },
            { "시간의 지팡이", "시간을 조종하여 마법사의 속도를 높이는 신비한 지팡이." }
        };
        
        public Dictionary<string, string> _KnightArmorNames = new Dictionary<string, string>
        {
            // 전사 (Knight) 아이템
            { "철갑 판금 갑옷", "전투에서 최고의 방어력을 제공하는 튼튼한 판금 갑옷." },
            { "용가죽 갑옷", "전설적인 용의 가죽으로 만들어져 마법 저항력을 강화하는 갑옷." },
            { "불사의 갑옷", "전사의 생명력을 극대화시키는 고대의 마법이 깃든 갑옷." },
            { "황금 판금 갑옷", "전투에서 전사의 위엄을 드러내는 황금으로 만든 판금 갑옷." },
            { "암흑의 갑옷", "어둠의 힘을 이용해 전사의 방어력을 극대화하는 갑옷." }
        };
        
        public Dictionary<string, string> _ArcherArmorNames = new Dictionary<string, string>
        {
            // 궁수 (Archer) 아이템
            { "고요한 가죽 갑옷", "움직일 때 소음을 최소화하여 적에게 들키지 않게 해주는 가벼운 가죽 갑옷." },
            { "독수리 깃털 조끼", "독수리의 깃털로 만들어져 민첩성과 정확도를 높여주는 조끼." },
            { "숲의 망토", "숲의 신비한 힘을 담아 궁수의 은신 능력을 극대화하는 망토." },
            { "그림자의 가죽 갑옷", "어둠 속에서 궁수를 완벽히 숨길 수 있는 은밀한 가죽 갑옷." },
            { "바람의 갑옷", "바람의 속도를 담아 궁수의 기동성을 극대화하는 가벼운 갑옷." }
        };
        
        public Dictionary<string, string> _MageArmorNames = new Dictionary<string, string>
        {
            // 마법사 (Mage) 아이템
            { "신비의 로브", "마법사의 마나를 증폭시켜 강력한 주문을 사용할 수 있게 해주는 로브." },
            { "비전의 망토", "비전의 힘을 흡수하여 마법 보호막을 형성하는 신비로운 망토." },
            { "황금의 로브", "고대 왕국의 마법사들이 사용했던 마력 증폭 로브." },
            { "불사의 로브", "불사의 마법이 깃들어 마법사의 생명력을 강화시킨다." },
            { "얼음의 로브", "얼음의 힘을 통해 마법사의 방어력을 강화시키는 로브." }
        };
        #endregion

        protected int _itemCount = 0;
        public int ItemId = 0;
        public string _itemName {  get; set; }
        public string _iteminfo { get; set; }
        public PlayerType _itemClass{ get; set; }
        public bool _Isequip { get; set; } = false;
        public Item(ItemType itemType)
        {
            ItemId = _nextId++;
            _itemType = itemType;

            //희귀도 + 가격 선정
            int rand = _globalRandom.Next(0, 100);
            if(rand < 50)
            {
                _rarity = ItemRarity.Normal;
                
            }
            else if(rand < 80)
            {
                _rarity= ItemRarity.Rare;
                _price += 300;
            }
            else
            {
                _rarity= ItemRarity.Unique;
                _price += 500;
            }

            //아이템 종류에 따른 이름, 설명 설정
            if (_itemType == ItemType.Weapon)
            {
                if (_itemClass == PlayerType.Knight)
                {
                    //아이템 키 리스트 생성
                    List<string> _itemIdx = new List<string>(_KnightWeaponNames.Keys);
                    int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                    //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                    _itemName = _itemIdx[randomNumber];
                    //해당 키에 맞는 값(설명) 가져오기
                    _iteminfo = _KnightWeaponNames[_itemName];
                }
                else if (_itemClass == PlayerType.Archer)
                {
                    //아이템 키 리스트 생성
                    List<string> _itemIdx = new List<string>(_ArcherWeaponNames.Keys);
                    int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                    //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                    _itemName = _itemIdx[randomNumber];
                    //해당 키에 맞는 값(설명) 가져오기
                    _iteminfo = _ArcherWeaponNames[_itemName];
                }
                else if (_itemClass == PlayerType.Mage)
                {
                    //아이템 키 리스트 생성
                    List<string> _itemIdx = new List<string>(_MageWeaponNames.Keys);
                    int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                    //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                    _itemName = _itemIdx[randomNumber];
                    //해당 키에 맞는 값(설명) 가져오기
                    _iteminfo = _MageWeaponNames[_itemName];
                }
            }
            else if (_itemType == ItemType.Armor)
            {
                if (_itemClass == PlayerType.Knight)
                {
                    //아이템 키 리스트 생성
                    List<string> _itemIdx = new List<string>(_KnightArmorNames.Keys);
                    int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                    //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                    _itemName = _itemIdx[randomNumber];
                    //해당 키에 맞는 값(설명) 가져오기
                    _iteminfo = _KnightArmorNames[_itemName];
                }
                else if (_itemClass == PlayerType.Archer)
                {
                    //아이템 키 리스트 생성
                    List<string> _itemIdx = new List<string>(_ArcherArmorNames.Keys);
                    int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                    //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                    _itemName = _itemIdx[randomNumber];
                    //해당 키에 맞는 값(설명) 가져오기
                    _iteminfo = _ArcherArmorNames[_itemName];
                }
                else if (_itemClass == PlayerType.Mage)
                {
                    //아이템 키 리스트 생성
                    List<string> _itemIdx = new List<string>(_MageArmorNames.Keys);
                    int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                    //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                    _itemName = _itemIdx[randomNumber];
                    //해당 키에 맞는 값(설명) 가져오기
                    _iteminfo = _MageArmorNames[_itemName];
                }
            }

        }
        
        public virtual void PrintInfo()
        {
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

        ItemType GetItemType() { return _itemType; }
    }

    class Weapon : Item
    {
        private int _damage;

        public Weapon() : base(ItemType.Weapon)
        {
            //희귀도

            switch (_rarity)
            {
                case ItemRarity.Normal:
                    _damage = 1 + _globalRandom.Next(1, 10)%5;
                    break;
                case ItemRarity.Rare:
                    _damage = 10 + _globalRandom.Next(1, 10)%20;
                    break;
                case ItemRarity.Unique:
                    _damage = 50 + _globalRandom.Next(1, 10)%40;
                    break;
            }

            //아이템 클래스
            int random = _globalRandom.Next(1, 100);
            if(random <30)
            {
                _itemClass = PlayerType.Knight;
            }
            else if(random < 60)
            {
                _itemClass = PlayerType.Archer;
            }
            else
                _itemClass = PlayerType.Mage;



            //Random random2 = new Random();
            if (_itemClass == PlayerType.Knight)
            {
                //아이템 키 리스트 생성
                List<string> _itemIdx = new List<string>(_KnightWeaponNames.Keys);
                int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                _itemName = _itemIdx[randomNumber];
                //해당 키에 맞는 값(설명) 가져오기
                _iteminfo = _KnightWeaponNames[_itemName];
            }
            else if (_itemClass == PlayerType.Archer)
            {
                //아이템 키 리스트 생성
                List<string> _itemIdx = new List<string>(_ArcherWeaponNames.Keys);
                int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                _itemName = _itemIdx[randomNumber];
                //해당 키에 맞는 값(설명) 가져오기
                _iteminfo = _ArcherWeaponNames[_itemName];
            }
            else if (_itemClass == PlayerType.Mage)
            {
                //아이템 키 리스트 생성
                List<string> _itemIdx = new List<string>(_MageWeaponNames.Keys);
                int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                _itemName = _itemIdx[randomNumber];
                //해당 키에 맞는 값(설명) 가져오기
                _iteminfo = _MageWeaponNames[_itemName];
            }
        }

        public override void PrintInfo()
        {
            Console.Write("-");
            if (_Isequip)
            {
                Console.Write("[E] ");
            }
            Console.Write($"{_itemName}  | 공격력 + {_damage}  | {_iteminfo} ");
            
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

    class Armor : Item
    {
        public Armor() : base(ItemType.Armor)
        {
            //희귀도
            switch (_rarity)
            {
                case ItemRarity.Normal:
                    _defence = 1 + _globalRandom.Next(1, 10)% 3;
                    break;
                case ItemRarity.Rare:
                    _defence = 2 + _globalRandom.Next(1, 10) % 4;
                    break;
                case ItemRarity.Unique:
                    _defence = 3 + _globalRandom.Next(1, 10) % 5;
                    break;
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

            //Random random2 = new Random();
            if (_itemClass == PlayerType.Knight)
            {
                //아이템 키 리스트 생성
                List<string> _itemIdx = new List<string>(_KnightArmorNames.Keys);
                int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                _itemName = _itemIdx[randomNumber];
                //해당 키에 맞는 값(설명) 가져오기
                _iteminfo = _KnightArmorNames[_itemName];
            }
            else if (_itemClass == PlayerType.Archer)
            {
                //아이템 키 리스트 생성
                List<string> _itemIdx = new List<string>(_ArcherArmorNames.Keys);
                int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                _itemName = _itemIdx[randomNumber];
                //해당 키에 맞는 값(설명) 가져오기
                _iteminfo = _ArcherArmorNames[_itemName];
            }
            else if (_itemClass == PlayerType.Mage)
            {
                //아이템 키 리스트 생성
                List<string> _itemIdx = new List<string>(_MageArmorNames.Keys);
                int randomNumber = _globalRandom.Next(0, _itemIdx.Count);
                //랜덤 인덱스에 해당하는 키(아이템 이름) 가져오기
                _itemName = _itemIdx[randomNumber];
                //해당 키에 맞는 값(설명) 가져오기
                _iteminfo = _MageArmorNames[_itemName];
            }
        }

        public int _defence = 0;

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
                Console.Write("[E] ");
            }
            Console.Write($"{_itemName}  | 방어력 + {_defence}  | {_iteminfo} ");
        }

    }
}
