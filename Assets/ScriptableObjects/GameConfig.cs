using System;
using UnityEngine;

namespace ScriptableObjects {

    [Serializable]
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject {

        [SerializeField] public long Coins = 2500;
        [SerializeField] public int Energy = 20;
        [SerializeField] public int Keys = 2;
        [SerializeField] public PrizesScriptableObject Prizes;

    }

}
