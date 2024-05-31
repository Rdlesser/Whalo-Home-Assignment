﻿using System;
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
        [SerializeField] private Transform _coinIcon;
        [SerializeField] private Transform _energyIcon;
        [SerializeField] private Transform _keyIcon;

        private GameModel _gameModel;
        
        
        public Action<int> OnBoxClicked;
        public Action<int, Prize> OnPrizeDisplayed;

        public bool AreBoxesClickable {
            get;
            set;
        }

        public void Initialize(GameModel gameModel) {

            _gameModel = gameModel;
            for (int i = 0; i < _boxButtons.Count; i++) {

                var boxId = i;
                _boxButtons[i].onClick.AddListener(() => ReactToBoxClick(boxId));
            }

            AreBoxesClickable = true;
        }

        private void ReactToBoxClick(int boxId) {

            if (!AreBoxesClickable) {
                return;
            }
            
            _boxButtons[boxId].onClick.RemoveAllListeners();
            OnBoxClicked?.Invoke(boxId);
        }

        public void OpenBox(int boxId) {
            
            AnimateBoxOpening(boxId);
        }

        private void SetBoxPrize(int boxId, Prize prize) {

            var prizeContainerTransform = _prizeContainers[boxId].transform;
            var prizeObject = Instantiate(_prizeView, prizeContainerTransform);
            var prizeView = prizeObject.GetComponent<PrizeView>();
            prizeView.OnPrizeDisplayed += HandlePrizeDisplayed;
            prizeView.Initialize(boxId, prize, prizeContainerTransform, GetIconPosition(prize.PrizeType));
        }

        private Transform GetIconPosition(PrizeType prize) {

            switch (prize) {


                case PrizeType.Keys:
                    return _keyIcon;
                    break;

                case PrizeType.Coins:
                    return _coinIcon;
                    break;

                case PrizeType.Energy:
                    return _energyIcon;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(prize), prize, null);
            }
        }

        private void HandlePrizeDisplayed(int boxId, Prize prize) {

            OnPrizeDisplayed?.Invoke(boxId, prize);
        }

        private void AnimateBoxOpening(int boxId) {

            var boxView = _boxViews[boxId];
            boxView.OnLidRemoved += () => AnimateSmokePoof(boxId);
            _boxViews[boxId].AnimateLidRemoval();
        }

        private void AnimateSmokePoof(int boxId) {
            
            var boxView = _boxViews[boxId];
            boxView.OnLidRemoved = null;
            SetBoxPrize(boxId, _gameModel.GetNextPrize());
            SetSmokeParticles(boxId);
        }

        private void SetSmokeParticles(int boxId) {

            _smokeParticles.transform.position = _prizeContainers[boxId].transform.position;
            _smokeParticles.Stop();
            _smokeParticles.Play();
        }

        private void OnDestroy() {

            foreach (var boxButton in _boxButtons) {
                
                boxButton.onClick.RemoveAllListeners();
            }
        }

    }

}
