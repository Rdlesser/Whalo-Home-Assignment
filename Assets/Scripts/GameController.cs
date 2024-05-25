using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Scripts {

    public class GameController : MonoBehaviour {

        [SerializeField] private GameView _gameView;
        [SerializeField] private UIView _uiView;
        [SerializeField] private GameConfig _gameConfig;

        private GameModel _gameModel;
        
        private ServiceReceiver<IAssetService> _assetService = new();

        private void Start() {

            InitGameModel();
            InitGameView();
            InitUIView();
            // InitUISprites();
            // InitPrizeQueue();
            // RegisterToBoxViewEvents();
        }

        private void InitGameModel() {

            //TODO: get number of coins/energy/keys from config file
            var prizeQueue = CreatePrizeQueue(_gameConfig.Prizes.PrizeList);
            _gameModel = new GameModel(_gameConfig.Coins, _gameConfig.Energy, _gameConfig.Keys, prizeQueue);
        }

        private Queue<Prize> CreatePrizeQueue(List<Prize> prizesScriptablePrizes) {

            var prizes = new List<Prize>(prizesScriptablePrizes);
            prizes.Shuffle();

            return new Queue<Prize>(prizes);
        }

        private void InitGameView() {

            RegisterToGameViewEvents();
            _gameView.Initialize(_gameModel);
        }

        private void RegisterToGameViewEvents() {
            
            _gameView.OnBoxClicked += HandleBoxClick;
            _gameView.OnPrizeDisplayed += HandlePrizeDisplayed;
        }

        private void InitUIView() {

            _uiView.Initialize(_gameModel);
        }

        private void HandleBoxClick(int boxId) {

            PayKey(1);
            _gameView.AreBoxesClickable = false;
            _gameView.OpenBox(boxId);
        }

        private void HandlePrizeDisplayed(int boxId, Prize prize) {

            switch (prize.PrizeType) {
                
                case PrizeType.Coins:
                    _gameModel.Coins += prize.Amount;
                    _uiView.UpdateCoinAmount(_gameModel.Coins - prize.Amount, _gameModel.Coins, true);
                    break;
                
                case PrizeType.Energy:
                    _gameModel.Energy += prize.Amount;
                    _uiView.UpdateEnergyAmount(_gameModel.Energy - prize.Amount, _gameModel.Energy, true);
                    break;
                
                case PrizeType.Keys:
                    _gameModel.Keys += prize.Amount;
                    _uiView.UpdateKeysAmount(_gameModel.Keys - prize.Amount, _gameModel.Keys, true);
                    break;
            }

            if (_gameModel.Keys <= 0 || AreAllBoxesOpen()) {

                EndGame();
                return;
            }

            _gameView.AreBoxesClickable = true;
        }

        private bool AreAllBoxesOpen() {

            return _gameModel.WereAllPrizesCollected();
        }

        private void PayKey(int numberOfKeys) {

            _gameModel.Keys-= numberOfKeys;
            _uiView.UpdateKeysAmount(_gameModel.Keys + numberOfKeys, _gameModel.Keys, true);
        }

        private void EndGame() {

            DisplayEndGamePopup();
        }

        private void DisplayEndGamePopup() {
            
            
        }

        private void OnDestroy() {

            _gameView.OnBoxClicked -= HandleBoxClick;
        }

    }

}
