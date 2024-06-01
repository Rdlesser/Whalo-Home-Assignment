using System;
using UnityEngine;

public class BoxView : MonoBehaviour {

    [SerializeField] private Animator _animator;

    private FinishState _finishState;
        
    public Action OnLidRemoved;
        
    private static readonly int RemoveLid = Animator.StringToHash("RemoveLid");

    public void AnimateLidRemoval() {

        _finishState = _animator.GetBehaviour<FinishState>();

        if (_finishState != null) {

            _finishState.OnEnter += LidRemoved;
        }
            
        _animator.SetTrigger(RemoveLid);
    }

    private void LidRemoved() {

        _finishState.OnEnter -= LidRemoved;
        OnLidRemoved?.Invoke();
    }
}