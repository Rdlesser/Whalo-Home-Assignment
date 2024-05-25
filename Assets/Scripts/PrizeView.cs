using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts {

    public class PrizeView : MonoBehaviour {

        [SerializeField] private Image _prizeImage;
        [SerializeField] private TMP_Text _prizeAmount;
        [SerializeField] private Animator _prizeAnimator;
        
        private static readonly int In = Animator.StringToHash("In");

        public void Initialize(Sprite prizeImage, int prizeAmount) {

            _prizeImage.sprite = prizeImage;
            _prizeAmount.text = prizeAmount.ToString();
        }

        public void Initialize(Prize prize) {
            
            Initialize(prize.Sprite, prize.Amount);
            _prizeAnimator.SetTrigger(In);
        }

    }

}
