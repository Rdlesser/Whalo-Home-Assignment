using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts {

    public class GameView : MonoBehaviour{

        [SerializeField] private List<Button> _boxButtons;
        [SerializeField] private List<GameObject> _prizeContainers;
        [SerializeField] private Animator _boxAnimator;
        [SerializeField] private PrizeView _prizeView;

        private GameModel _gameModel;

        public Action<int> OnBoxClicked;
        private static readonly int RemoveLid = Animator.StringToHash("RemoveLid");

        public void Initialize(GameModel gameModel) {

            _gameModel = gameModel;
            for (int i = 0; i < _boxButtons.Count; i++) {

                var boxId = i;
                _boxButtons[i].onClick.AddListener(() => ReactToBoxClick(boxId));
            }
        }

        private void ReactToBoxClick(int boxId) {

            OnBoxClicked?.Invoke(boxId);
        }

        public void OpenBox(int boxId, Prize prize) {

            SetBoxPrize(boxId, prize);
            AnimateBoxOpening(boxId);
        }

        private void SetBoxPrize(int boxId, Prize prize) {

            var prizeView = Instantiate(_prizeView, _prizeContainers[boxId].transform);
            prizeView.Initialize(prize);
        }

        private void AnimateBoxOpening(int boxId) {
            
            _boxAnimator.SetTrigger(RemoveLid);
        }

        private void OnDestroy() {

            foreach (var boxButton in _boxButtons) {
                
                boxButton.onClick.RemoveAllListeners();
            }
        }

    }

}
