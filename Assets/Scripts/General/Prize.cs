using System;
using UnityEngine;

namespace General {

    [Serializable]
    public class Prize {

        public PrizeType PrizeType;
        public string SpriteName;
        public Texture2D Texture;
        public long Amount;

    }

    public enum PrizeType {

        Keys,
        Coins,
        Energy,
    }

}