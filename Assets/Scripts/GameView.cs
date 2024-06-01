using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scripts;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour{

    [SerializeField] private List<Button> _boxButtons;
    [SerializeField] private List<GameObject> _prizeContainers;
    [SerializeField] private List<BoxView> _boxViews;
    [SerializeField] private GameObject _prizeView;
    [SerializeField] private ParticleSystem _smokeParticles;
    [SerializeField] private Transform _coinIcon;
    [SerializeField] private Transform _energyIcon;
    [SerializeField] private Transform _keyIcon;

    private GameModel _gameModel;

    private bool _isOpeningBox = false;

    public Action<int> OnBoxClicked;
    public Action<int, Prize> OnPrizeDisplayed;

    public bool AreBoxesClickable {
        get;
        set;
    }

    public void Initialize(GameModel gameModel) {

        _gameModel = gameModel;
        for (int i = 0; i < _boxButtons.Count; i++) {

            var boxId = i;
            _boxButtons[i].onClick.AddListener(() => ReactToBoxClick(boxId));
        }

        AreBoxesClickable = true;
    }

    private void ReactToBoxClick(int boxId) {

        if (!AreBoxesClickable) {
            return;
        }
            
        _boxButtons[boxId].onClick.RemoveAllListeners();
        OnBoxClicked?.Invoke(boxId);
    }

    public void OpenBox(int boxId) {
            
        AnimateBoxOpening(boxId);
    }

    private void SetBoxPrize(int boxId, Prize prize, bool isCollectable) {

        var prizeContainerTransform = _prizeContainers[boxId].transform;
        var prizeObject = Instantiate(_prizeView, prizeContainerTransform);
        var prizeView = prizeObject.GetComponent<PrizeView>();
        prizeView.OnPrizeDisplayed += HandlePrizeDisplayed;
        prizeView.Initialize(boxId, prize, prizeContainerTransform, GetIconPosition(prize.PrizeType));

        if (isCollectable) {
                
            prizeView.PlayCollectAnimation();
        }

        else {
                
            prizeView.DimPrize();
        }
    }

    private Transform GetIconPosition(PrizeType prize) {

        switch (prize) {


            case PrizeType.Keys:
                return _keyIcon;

            case PrizeType.Coins:
                return _coinIcon;

            case PrizeType.Energy:
                return _energyIcon;

            default:
                throw new ArgumentOutOfRangeException(nameof(prize), prize, null);
        }
    }

    private void HandlePrizeDisplayed(int boxId, Prize prize) {
        
        _isOpeningBox = false;
        OnPrizeDisplayed?.Invoke(boxId, prize);
    }

    private void AnimateBoxOpening(int boxId, bool isCollectable = true) {
        
        _isOpeningBox = true;
        var boxView = _boxViews[boxId];
        boxView.OnLidRemoved += () => AnimateSmokePoof(boxId, isCollectable);
        _boxViews[boxId].AnimateLidRemoval();
    }

    private void AnimateSmokePoof(int boxId, bool isCollectable) {
        
        var boxView = _boxViews[boxId];
        boxView.OnLidRemoved = null;
        _gameModel.TryGetNextPrize(out var prize);
        SetBoxPrize(boxId, prize, isCollectable);

        if (isCollectable) {
                
            SetSmokeParticles(boxId);
        }
    }

    private void SetSmokeParticles(int boxId) {

        _smokeParticles.transform.position = _prizeContainers[boxId].transform.position;
        _smokeParticles.Stop();
        _smokeParticles.Play();
    }

    private void OnDestroy() {

        foreach (var boxButton in _boxButtons) {
                
            boxButton.onClick.RemoveAllListeners();
        }
    }

    public async UniTask RevealAllPrizes() {

        var closedBoxes = _gameModel.GetClosedBoxes();

        foreach (var boxId in closedBoxes) {
            
            AnimateBoxOpening(boxId, false);
            await UniTask.WaitUntil(() => !_isOpeningBox);
        }
    }

}