using System;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Scripts {

    public class BoxView : MonoBehaviour {

        [SerializeField] private Button _boxButton;

        public Action<BoxView> OnBoxClicked;
    
        private void OnEnable() {
            AddButtonListener();
        }

        private void AddButtonListener() {
            _boxButton.onClick.AddListener(HandleBoxClick);
        }

        private void HandleBoxClick() {
        
            OnBoxClicked?.Invoke(this);
        }

    }

}
