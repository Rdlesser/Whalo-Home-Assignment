﻿using System;
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
        [SerializeField] private float _rollUpTime = 1.2f; 

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
            
            UpdateCoinAmount(0, 0, false);
        }

        public async void UpdateCoinAmount(long startAmount, long endAmount, bool isAnimated = false) {

            if (isAnimated) {
                
                await StringUtils.RollNumber(_coinAmount, startAmount, endAmount, _rollUpTime);
            }
            
            _coinAmount.text = StringUtils.NumberToShortString(_gameModel.Coins);
        }

        private void UpdateEnergyAmount() {
            
            UpdateEnergyAmount(0, 0, false);
        }

        public async void UpdateEnergyAmount(int startAmount, int endAmount, bool isAnimated = false) {

            if (isAnimated) {
                
                await StringUtils.RollNumber(_energyAmount, startAmount, endAmount, _rollUpTime);
            }
            
            _energyAmount.text = StringUtils.NumberToShortString(_gameModel.Energy);
        }

        private void UpdateKeysAmount() {
            
            UpdateKeysAmount(0, 0, false);
        }

        public async void UpdateKeysAmount(int startAmount, int endAmount, bool isAnimated = false) {

            if (isAnimated) {
                
                await StringUtils.RollNumber(_keysAmount, startAmount, endAmount, _rollUpTime);
            }
            
            _keysAmount.text = StringUtils.NumberToShortString(_gameModel.Keys);
        }

        public void Initialize(GameModel gameModel) {

            _gameModel = gameModel;
            UpdateView();
        }

    }

}
