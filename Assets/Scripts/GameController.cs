using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Scripts;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : Controller{

    [SerializeField] private GameView _gameView;
    [SerializeField] private UIView _uiView;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private EndScreenView _endScreenView;

    private GameModel _gameModel;

    private Texture2D _coinTexture;
    private Texture2D _energyTexture;
    private Texture2D _keyTexture;
        
    // private ServiceReceiver<IAssetService> _assetService = new();

    protected override UniTask Initialize() {

        InitGameModel();
        InitGameView();
        InitUIView();

        return UniTask.CompletedTask;
    }

    protected override void Clean() {
        
        DeregisterGameViewEvents();
    }
    
    private void InitGameModel() {
        
        var prizeQueue = CreatePrizeQueue(_gameConfig.Prizes.PrizeList);
        _gameModel = new GameModel(_gameConfig.StartingCoins, _gameConfig.StartingEnergy, _gameConfig.StartingKeys, prizeQueue);
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

    private void DeregisterGameViewEvents() {
        
        _gameView.OnBoxClicked -= HandleBoxClick;
        _gameView.OnPrizeDisplayed -= HandlePrizeDisplayed;
    }

    private async UniTask InitUIView() {

        _uiView.Initialize(_gameModel);
        // TODO: Move this to the asset service
        _coinTexture = await GetSprite("https://drive.usercontent.google.com/u/0/uc?id=1STe0U77LPpDHry2E-TrLwnvNxC1xY9f4&export=download");
        _energyTexture = await GetSprite("https://drive.usercontent.google.com/u/0/uc?id=1Lhcot4Wfho1SGtYrobmp1MrZ_YeJwApc&export=download");
        _keyTexture = await GetSprite("https://drive.usercontent.google.com/u/0/uc?id=1pLjE-n69_A_1mfJG_ZJtmzCsl6OvPRIU&export=download");
        _uiView.SetIcons(_coinTexture, _energyTexture, _keyTexture);
    }

    private async UniTask<Texture2D> GetSprite(string url) {

        var webRequest = UnityWebRequestTexture.GetTexture(url);
        await webRequest.SendWebRequest().WithCancellation(this.GetCancellationTokenOnDestroy());

        var texture = ((DownloadHandlerTexture) webRequest.downloadHandler).texture;

        return texture;
    }

    private void HandleBoxClick(int boxId) {

        _gameModel.SetBoxOpened(boxId);
        PayKey(_gameConfig.BoxOpeningPrice);
        _gameView.AreBoxesClickable = false;
        _gameView.OpenBox(boxId);
    }

    private void HandlePrizeDisplayed(int boxId, Prize prize) {

        switch (prize.PrizeType) {
                
            case PrizeType.Coins:
                _gameModel.Coins += prize.Amount;
                _gameModel.AccumulatedCoins += prize.Amount;
                _uiView.UpdateCoinAmount(_gameModel.Coins - prize.Amount, _gameModel.Coins, true);
                break;
                
            case PrizeType.Energy:
                _gameModel.Energy += prize.Amount;
                _gameModel.AccumulatedEnergy += prize.Amount;
                _uiView.UpdateEnergyAmount(_gameModel.Energy - prize.Amount, _gameModel.Energy, true);
                break;
                
            case PrizeType.Keys:
                _gameModel.Keys += prize.Amount;
                _uiView.UpdateKeysAmount(_gameModel.Keys - prize.Amount, _gameModel.Keys, true);
                break;

            default:
                throw new ArgumentOutOfRangeException();
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

        RevealAllPrizes();
        // DisplayEndGamePopup();
    }

    private void RevealAllPrizes() {

        _gameView.RevealAllPrizes();
    }

    private void DisplayEndGamePopup() {

        _endScreenView.gameObject.SetActive(true);
        var coinSprite = Sprite.Create(_coinTexture,  new Rect(0f, 0f, _coinTexture.width, _coinTexture.height), Vector2.zero);
        var energySprite = Sprite.Create(_energyTexture,  new Rect(0f, 0f, _energyTexture.width, _energyTexture.height), Vector2.zero);
        _endScreenView.Initialize(coinSprite, _gameModel.AccumulatedCoins, energySprite, _gameModel.AccumulatedEnergy);
    }

    private void OnDestroy() {

        _gameView.OnBoxClicked -= HandleBoxClick;
    }

}