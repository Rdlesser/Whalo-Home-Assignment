using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Scripts;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : Controller{

    [SerializeField] private GameView _gameView;
    [SerializeField] private UIView _uiView;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private GameObject _endScreenPanel;
    [SerializeField] private EndScreenView _endScreenView;

    private GameModel _gameModel;

    private Texture2D _coinTexture;
    private Texture2D _energyTexture;
    private Texture2D _keyTexture;

    private ServiceReceiver<ISceneService> _sceneService = new();
    private ServiceReceiver<IAssetService> _assetService = new();

    private const string COIN_STRING = "Coin";
    private const string ENERGY_STRING = "Energy";
    private const string KEY_STRING = "Key";

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

        foreach (var prize in prizes) {

            prize.Texture = _assetService.Get().GetAsset(prize.SpriteName);
        }
        
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
        _coinTexture = _assetService.Get().GetAsset(COIN_STRING);
        _energyTexture = _assetService.Get().GetAsset(ENERGY_STRING);
        _keyTexture = _assetService.Get().GetAsset(KEY_STRING);
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

    private async void EndGame() {

        _gameView.AreBoxesClickable = false;
        DeregisterGameViewEvents();
        await _gameView.RevealAllPrizes();
        DisplayEndGamePopup();
    }

    private void DisplayEndGamePopup() {

        _endScreenPanel.SetActive(true);
        var coinSprite = Sprite.Create(_coinTexture,  new Rect(0f, 0f, _coinTexture.width, _coinTexture.height), Vector2.zero);
        var energySprite = Sprite.Create(_energyTexture,  new Rect(0f, 0f, _energyTexture.width, _energyTexture.height), Vector2.zero);
        _endScreenView.OnCollectClicked += HandleCollectClicked;
        _endScreenView.Initialize(_coinTexture, _gameModel.AccumulatedCoins, _energyTexture, _gameModel.AccumulatedEnergy);
    }

    private void HandleCollectClicked() {

        _endScreenView.OnCollectClicked -= HandleCollectClicked;
        _sceneService.Get().MoveToScene(SceneName.MainMenu);
    }

    private void OnDestroy() {

        _gameView.OnBoxClicked -= HandleBoxClick;
    }

}