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

            _gameView.OnBoxClicked += HandleBoxClick;
            _gameView.Initialize(_gameModel);
        }

        private void InitUIView() {

            _uiView.Initialize(_gameModel);
        }

        private void HandleBoxClick(int boxId) {

            PayKey(1);
            _gameView.AreBoxesClickable = false;
            _gameView.OpenBox(boxId);
        }

        private void PayKey(int i) {

            _gameModel.Keys--;
            _uiView.UpdateView();
        }

        private void OnDestroy() {

            _gameView.OnBoxClicked -= HandleBoxClick;
        }

    }

}
