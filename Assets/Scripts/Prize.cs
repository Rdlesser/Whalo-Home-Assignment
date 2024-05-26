using System;
using UnityEngine;

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