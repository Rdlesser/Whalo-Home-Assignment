using System;
using UnityEngine;

namespace Scripts {

    [Serializable]
    public class Prize {

        public PrizeType PrizeType;
        public Sprite Sprite;
        public int Amount;

    }

    public enum PrizeType {

        Keys,
        Coins,
        Energy,
    }

}
