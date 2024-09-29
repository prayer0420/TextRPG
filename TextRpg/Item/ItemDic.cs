using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public class ItemDic
    {
        private static ItemDic _instance;
        public static ItemDic GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ItemDic();
            }
            return _instance;
        }

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
    }

}
