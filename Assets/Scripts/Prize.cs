using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts {

    [Serializable]
    public class Prize {

        public PrizeType PrizeType;
        [FormerlySerializedAs("PrizeSprite")] public Sprite Sprite;
        public int Amount;

    }

    public enum PrizeType {

        Keys,
        Coins,
        Energy,
    }

}
