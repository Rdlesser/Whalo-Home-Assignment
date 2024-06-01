using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace {

    public class PopupView : MonoBehaviour {
        
        [SerializeField] private Image _popupImage;
        [SerializeField] private Animator _popupAnimator;
        [SerializeField] private Button _popupButton;

        private Sprite[] _popupSprites;

        private bool _wasPopupPressed;
        
        private static readonly int In = Animator.StringToHash("In");
        private static readonly int Out = Animator.StringToHash("Out");

        public void Initialize(Sprite[] popupsToShow) {

            _popupSprites = popupsToShow;
            _popupButton.onClick.AddListener(ReactToPopupClick);
        }

        private void ReactToPopupClick() {

            _wasPopupPressed = true;
        }

        public async void ShowPopups() {
            
            foreach (var popupSprite in _popupSprites) {

                _wasPopupPressed = false;
                _popupImage.sprite = popupSprite;
                _popupAnimator.SetTrigger(In);
                await UniTask.WaitUntil(() => _wasPopupPressed);
                _popupAnimator.SetTrigger(Out);
            }
        }
    }

}
