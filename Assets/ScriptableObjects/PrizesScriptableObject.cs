using System;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

namespace ScriptableObjects {

    [CreateAssetMenu(fileName = "Prizes", menuName = "ScriptableObjects/Prizes Object", order = 1)]
    [Serializable]
    public class PrizesScriptableObject : ScriptableObject {

        [SerializeField] public List<Prize> Prizes;
    }

}
