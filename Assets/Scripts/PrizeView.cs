using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts {

    public class PrizeView : MonoBehaviour {

        [SerializeField] private Image _prizeImage;
        [SerializeField] private TMP_Text _prizeAmount;
        [SerializeField] private AToBTool _prizeAnimator;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Prize _prize;
        private int _boxId;

        public Action<int, Prize> OnPrizeDisplayed;

        private void InitializeVisuals(Sprite prizeImage, long prizeAmount) {

            _prizeImage.sprite = prizeImage;
            _prizeAmount.text = prizeAmount.ToString();
        }

        public void Initialize(int boxId, Prize prize, Transform startPosition = null, Transform endPosition = null) {

            _prize = prize;
            _boxId = boxId;
            var prizeSprite = Sprite.Create(prize.Texture,  new Rect(0f, 0f, prize.Texture.width, prize.Texture.height), Vector2.zero);
            InitializeVisuals(prizeSprite, _prize.Amount);
            _prizeAnimator.OnMoveComplete += PrizeDisplayed;
            startPosition = startPosition ? startPosition : transform;
            endPosition = endPosition ? endPosition : transform;
            _prizeAnimator.ConfigureStartAndEndPosition(startPosition, endPosition);
        }

        private void PrizeDisplayed() {

            _prizeAnimator.OnMoveComplete -= PrizeDisplayed;
            OnPrizeDisplayed?.Invoke(_boxId, _prize);
        }

        public void PlayCollectAnimation() {

            _prizeAnimator.Play();
        }

        public void DimPrize() {

            _canvasGroup.alpha = 0.5f;
            PrizeDisplayed();
        }
    }

}
