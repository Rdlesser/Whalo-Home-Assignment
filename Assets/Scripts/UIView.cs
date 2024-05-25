using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts {

    public class UIView : MonoBehaviour {

        [SerializeField] private Image _coinImage;
        [SerializeField] private TMP_Text _coinAmount;
        [SerializeField] private Image _energyImage;
        [SerializeField] private TMP_Text _energyAmount;
        [SerializeField] private Image _keyImage;
        [SerializeField] private TMP_Text _keysAmount;

        private GameModel _gameModel;

        public void SetIcons(Texture2D coinTexture, Texture2D energyTexture, Texture2D keyTexture) {

            _coinImage.sprite = Sprite.Create(coinTexture,  new Rect(0f, 0f, coinTexture.width, coinTexture.height), Vector2.zero);
            _energyImage.sprite = Sprite.Create(energyTexture, new Rect(0f, 0f, energyTexture.width, energyTexture.height), Vector2.zero);
            _keyImage.sprite = Sprite.Create(keyTexture, new Rect(0f, 0f, keyTexture.width, keyTexture.height), Vector2.zero);
        }

        public void UpdateView() {

            UpdateCoinAmount();
            UpdateEnergyAmount();
            UpdateKeysAmount();
        }

        private void UpdateCoinAmount() {

            _coinAmount.text = _gameModel.Coins.ToString();
        }

        private void UpdateEnergyAmount() {
            
            _coinAmount.text = _gameModel.Coins.ToString();
        }

        private void UpdateKeysAmount() {

            _keysAmount.text = _gameModel.Keys.ToString();
        }

        public void Initialize(GameModel gameModel) {

            _gameModel = gameModel;
        }

    }

}
