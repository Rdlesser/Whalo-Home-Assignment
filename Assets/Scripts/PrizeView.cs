using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts {

    public class PrizeView : MonoBehaviour {

        [SerializeField] private Image _prizeImage;
        [SerializeField] private TMP_Text _prizeAmount;
        [SerializeField] private Animator _prizeAnimator;

        private Prize _prize;
        private int _boxId;

        private FinishState _finishState;

        private static readonly int In = Animator.StringToHash("In");

        public Action<int, Prize> OnPrizeDisplayed;

        private void InitializeVisuals(Sprite prizeImage, int prizeAmount) {

            _prizeImage.sprite = prizeImage;
            _prizeAmount.text = prizeAmount.ToString();
        }

        public void Initialize(int boxId, Prize prize) {

            _prize = prize;
            _boxId = boxId;
            InitializeVisuals(_prize.Sprite, _prize.Amount);
            _finishState = _prizeAnimator.GetBehaviour<FinishState>();
            _finishState.OnEnter += PrizeDisplayed;
            _prizeAnimator.SetTrigger(In);
        }

        private void PrizeDisplayed() {

            _finishState.OnEnter -= PrizeDisplayed;
            OnPrizeDisplayed?.Invoke(_boxId, _prize);
        }

    }

}
