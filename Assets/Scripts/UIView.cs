using System;
using Cysharp.Threading.Tasks;
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
        [SerializeField] private float _rollUpTime = 2f; 

        private GameModel _gameModel;

        public void SetIcons(Texture2D coinTexture, Texture2D energyTexture, Texture2D keyTexture) {

            _coinImage.sprite = Sprite.Create(coinTexture,  new Rect(0f, 0f, coinTexture.width, coinTexture.height), Vector2.zero);
            _energyImage.sprite = Sprite.Create(energyTexture, new Rect(0f, 0f, energyTexture.width, energyTexture.height), Vector2.zero);
            _keyImage.sprite = Sprite.Create(keyTexture, new Rect(0f, 0f, keyTexture.width, keyTexture.height), Vector2.zero);
        }

        public void UpdateView(bool isAnimated = false) {

            UpdateCoinAmount(isAnimated);
            UpdateEnergyAmount(isAnimated);
            UpdateKeysAmount(isAnimated);
        }

        private async void UpdateCoinAmount(bool isAnimated = false) {

            if (isAnimated) {
                
                await StringUtils.RollNumber(_coinAmount, Int64.Parse(_coinAmount.text), _gameModel.Coins, _rollUpTime);
            }
            _coinAmount.text = _gameModel.Coins.ToString();
        }

        private async void UpdateEnergyAmount(bool isAnimated = false) {
            
            await StringUtils.RollNumber(_energyAmount, Int64.Parse(_energyAmount.text), _gameModel.Energy, _rollUpTime);
            _energyAmount.text = _gameModel.Energy.ToString();
        }

        private async void UpdateKeysAmount(bool isAnimated = false) {

            await StringUtils.RollNumber(_keysAmount, Int64.Parse(_keysAmount.text), _gameModel.Keys, _rollUpTime);
            _keysAmount.text = _gameModel.Keys.ToString();
        }

        public void Initialize(GameModel gameModel) {

            _gameModel = gameModel;
            UpdateView();
        }

    }

}
