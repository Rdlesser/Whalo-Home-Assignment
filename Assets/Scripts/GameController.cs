using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Scripts {

    public class GameController : MonoBehaviour{

        [SerializeField] private List<BoxView> _boxes;
        [SerializeField] private UIView _uiView;
        [SerializeField] private string _coinURL;
        [SerializeField] private string _energyURL;
        [SerializeField] private string _keyURL;
        
        private ServiceReceiver<IAssetService> _assetService = new();

        private Queue<Prize> _prizes; 

        private void Start() {

            InitUISprites();
            InitPrizeQueue();
            RegisterToBoxViewEvents();
        }

        private async void InitUISprites() {

            var coinTexture = DownloadHandlerTexture.GetContent(await UnityWebRequestTexture.GetTexture(_coinURL).SendWebRequest());
            var energyTexture = DownloadHandlerTexture.GetContent(await UnityWebRequestTexture.GetTexture(_energyURL).SendWebRequest());
            var keyTexture = DownloadHandlerTexture.GetContent(await UnityWebRequestTexture.GetTexture(_keyURL).SendWebRequest());
            _uiView.SetIcons(coinTexture, energyTexture, keyTexture);
        }

        private void InitPrizeQueue() {
            
            
        }

        private void RegisterToBoxViewEvents() {

            for (int i = 0; i < _boxes.Count; i++) {

                _boxes[i].OnBoxClicked += HandleBoxClick;
            }
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
