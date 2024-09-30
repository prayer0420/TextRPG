using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public enum ConsumalbeType
    {
        None = 0,
        HpPortion = 1,
        MpPortion = 2,
    }

    class ConsumableItem : Item
    {
        public int recoveryAmount = 50;
        public ConsumalbeType consumalbeType = ConsumalbeType.None;
        public static Random GlobalRandom = new Random();

        public ConsumableItem(ConsumalbeType consumalbeType)  : base(ItemType.ConsumableItem)
        {

            itemType = ItemType.ConsumableItem;
        }

        public void Recovery(ConsumalbeType type, Player player)
        {

            if(type == ConsumalbeType.HpPortion)
            {
                player.RecoveryHp(recoveryAmount);
                Console.WriteLine("플레이의 체력을 50 회복하였습니다!");
            }
            else if (type == ConsumalbeType.MpPortion)
            {
                player.RecoveryMp(recoveryAmount);
                Console.WriteLine("플레이의 마나를 50 회복하였습니다!");
            }
        }

    }

    class HpPortion : ConsumableItem
    {
        public HpPortion() : base(ConsumalbeType.HpPortion)
        {
            consumalbeType = ConsumalbeType.HpPortion;
        }
        
    }

    class MpPortion : ConsumableItem
    {
        public MpPortion() : base(ConsumalbeType.MpPortion)
        {

            consumalbeType = ConsumalbeType.MpPortion;
        }
    }
}
