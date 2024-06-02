using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Core {

    public class PopupView : MonoBehaviour {
        
        [SerializeField] private Image _popupImage;
        [SerializeField] private Animator _popupAnimator;
        [SerializeField] private Button _popupButton;

        private Sprite[] _popupSprites;
        private FinishState _finishState;

        private bool _wasPopupPressed;
        private bool _isPopupExiting = false;
        
        private static readonly int In = Animator.StringToHash("In");
        private static readonly int Out = Animator.StringToHash("Out");

        public void Initialize(Sprite[] popupsToShow) {

            _popupSprites = popupsToShow;
            _popupButton.onClick.AddListener(ReactToPopupClick);
        }

        private void ReactToPopupClick() {

            if (_isPopupExiting) {
                return;
            }
        
            _wasPopupPressed = true;
        }

        public async UniTask ShowPopups() {
            
            foreach (var popupSprite in _popupSprites) {

                _wasPopupPressed = false;
                _popupImage.sprite = popupSprite;
                _popupAnimator.SetTrigger(In);
            
                await UniTask.WaitUntil(() => _wasPopupPressed);
            
                _isPopupExiting = true;
                _finishState = _popupAnimator.GetBehaviour<FinishState>();
                _finishState.OnEnter += HandlePopupExit;
                _popupAnimator.SetTrigger(Out);
            
                await UniTask.WaitUntil(() => !_isPopupExiting);
            }
        }

        private void HandlePopupExit() {

            _finishState.OnEnter -= HandlePopupExit;
            _isPopupExiting = false;
        }

    }

}