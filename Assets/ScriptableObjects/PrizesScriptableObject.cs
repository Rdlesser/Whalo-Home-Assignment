using System;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

namespace ScriptableObjects {

    [CreateAssetMenu(fileName = "Prizes", menuName = "ScriptableObjects/Prizes Object", order = 2)]
    [Serializable]
    public class PrizesScriptableObject : ScriptableObject {

        [SerializeField] public List<Prize> PrizeList;
    }

}
