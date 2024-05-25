using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Scripts {

    public class GameController : MonoBehaviour {

        [SerializeField] private GameView _gameView;
        [SerializeField] private UIView _uiView;
        [SerializeField] private PrizesScriptableObject _prizesScriptable;

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
            var prizeQueue = CreatePrizeQueue(_prizesScriptable.Prizes);
            _gameModel = new GameModel(0, 0, 2, prizeQueue);
        }

        private Queue<Prize> CreatePrizeQueue(List<Prize> prizesScriptablePrizes) {

            var prizes = new List<Prize>(prizesScriptablePrizes);
            prizes.Shuffle();

            return new Queue<Prize>(prizes);
        }

        private void InitGameView() {

            _gameView.Initialize(_gameModel);
            _gameView.OnBoxClicked += HandleBoxClick;
        }

        private void InitUIView() {

            _uiView.Initialize(_gameModel);
        }

        private void HandleBoxClick(int boxId) {

            PayKey(1);
            _gameView.OpenBox(boxId);
        }

        private void PayKey(int i) {

            _gameModel.Keys--;
            _uiView.UpdateView();
        }
    }

}
