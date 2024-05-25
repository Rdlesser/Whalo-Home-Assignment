using System;
using UnityEngine;

namespace Scripts {

    public class BoxView : MonoBehaviour {

        [SerializeField] private Animator _animator;

        public Action OnLidRemoved;
        
        private static readonly int RemoveLid = Animator.StringToHash("RemoveLid");

        public void AnimateLidRemoval() {
            _animator.SetTrigger(RemoveLid);
        }

        public void LidRemoved() {
            
            OnLidRemoved?.Invoke();
        }
    }

}
