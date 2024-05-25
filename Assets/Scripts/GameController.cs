using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Scripts {

    public class GameController : MonoBehaviour {

        [SerializeField] private GameView _gameView;
        [SerializeField] private UIView _uiView;
        [SerializeField] private string _coinURL;
        [SerializeField] private string _energyURL;
        [SerializeField] private string _keyURL;

        private GameModel _gameModel;
        
        private ServiceReceiver<IAssetService> _assetService = new();

        private Queue<Prize> _prizes; 

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
            _gameModel = new GameModel(0, 0, 2);
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
            var prize = GetNextPrize();
            _gameView.OpenBox(boxId, prize);
        }

        private void PayKey(int i) {

            _gameModel.Keys--;
            _uiView.UpdateView();
        }

        private async void InitUISprites() {

            // var coinTexture = DownloadHandlerTexture.GetContent(await UnityWebRequestTexture.GetTexture(_coinURL).SendWebRequest());
            // var energyTexture = DownloadHandlerTexture.GetContent(await UnityWebRequestTexture.GetTexture(_energyURL).SendWebRequest());
            // var keyTexture = DownloadHandlerTexture.GetContent(await UnityWebRequestTexture.GetTexture(_keyURL).SendWebRequest());
            // _uiView.SetIcons(coinTexture, energyTexture, keyTexture);
        }

        private void InitPrizeQueue() {
            
            
        }

        private void RegisterToBoxViewEvents() {

            // for (int i = 0; i < _boxes.Count; i++) {
            //
            //     _boxes[i].OnBoxClicked += HandleBoxClick;
            // }
        }

        private void HandleBoxClick(BoxView box) {

            var prize = GetNextPrize();
            
        }

        private Prize GetNextPrize() {

            var prize = _prizes.Dequeue();

            return prize;
        }

    }

}
