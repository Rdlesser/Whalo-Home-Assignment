using System;

namespace Scripts {

    [Serializable]
    public class Prize {

        public PrizeType PrizeType;
        public int Amount;

    }

    public enum PrizeType {

        Keys,
        Coins,
        Energy,
    }

}
