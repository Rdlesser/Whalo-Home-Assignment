using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts {

    public class PrizeView : MonoBehaviour {

        [SerializeField] private Image _prizeImage;
        [SerializeField] private TMP_Text _prizeAmount;

        public void Initialize(Sprite prizeImage, int prizeAmount) {

            _prizeImage.sprite = prizeImage;
            _prizeAmount.text = prizeAmount.ToString();
        }

        public void Initialize(Prize prize) {
            
            Initialize(prize.Sprite, prize.Amount);
        }

    }

}
