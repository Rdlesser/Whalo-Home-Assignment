using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts {

    public class GameView : MonoBehaviour{

        [SerializeField] private List<Button> _boxButtons;
        [SerializeField] private List<GameObject> _prizeContainers;
        [SerializeField] private List<BoxView> _boxViews;
        [SerializeField] private GameObject _prizeView;
        [SerializeField] private ParticleSystem _smokeParticles;

        private GameModel _gameModel;
        
        
        public Action<int> OnBoxClicked;

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

            var prizeView = Instantiate(_prizeView, _prizeContainers[boxId].transform).GetComponent<PrizeView>();
            prizeView.Initialize(prize);
        }

        private void AnimateBoxOpening(int boxId) {

            var boxView = _boxViews[boxId];
            boxView.OnLidRemoved += () => AnimateSmokePoof(boxId);
            _boxViews[boxId].AnimateLidRemoval();
        }

        private void AnimateSmokePoof(int boxId) {

            var boxView = _boxViews[boxId];
            boxView.OnLidRemoved = null;
            _smokeParticles.transform.position = boxView.transform.position;
            _smokeParticles.Play();
        }

        private void OnDestroy() {

            foreach (var boxButton in _boxButtons) {
                
                boxButton.onClick.RemoveAllListeners();
            }
        }

    }

}
