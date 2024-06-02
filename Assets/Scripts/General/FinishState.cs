using System;
using UnityEngine;

public class FinishState : StateMachineBehaviour {

    [field: SerializeField] public string Id { get; private set; }
    
    public Action OnEnter;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
        base.OnStateEnter(animator, stateInfo, layerIndex);
        OnEnter?.Invoke();
    }
}