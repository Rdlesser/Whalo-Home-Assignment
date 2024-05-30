using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
public class GameConfig : ScriptableObject {

    [SerializeField] public long StartingCoins = 2500;
    [SerializeField] public int StartingEnergy = 20;
    [SerializeField] public int StartingKeys = 2;
    [SerializeField] public PrizesScriptableObject Prizes;

}