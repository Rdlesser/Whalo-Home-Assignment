using System;
using Scripts;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenView : MonoBehaviour {

    [SerializeField] private GameObject _coinPrizeContainer;
    [SerializeField] private GameObject _energyPrizeContainer;
    [SerializeField] private PrizeView _coinPrizeView;
    [SerializeField] private PrizeView _energyPrizeView;
    [SerializeField] private Animator _animator;
    [SerializeField] private AToBTool _headerAnimator;
    [SerializeField] private AToBTool _coinPrizeAnimator;
    [SerializeField] private AToBTool _energyPrizeAnimator;
    [SerializeField] private Button _collectButton;

    private FinishState _finishState;

    private Prize _coinPrize;
    private Prize _energyPrize;
    
    private static readonly int In = Animator.StringToHash("In");

    public Action OnCollectClicked;

    public void Initialize(Texture2D coinTexture, long coinAmount, Texture2D energyTexture, long energyAmount) {
        
        _collectButton.onClick.AddListener(ReactToCollectClicked);

        if (coinAmount > 0) {
            
            _coinPrize = new Prize() {
                Amount =  coinAmount,
                PrizeType = PrizeType.Coins,
                Texture = coinTexture
            };
            
            _coinPrizeView.Initialize(0, _coinPrize);

        }

        if (energyAmount > 0) {
            
            _energyPrize = new Prize() {
                Amount =  energyAmount,
                PrizeType = PrizeType.Energy,
                Texture = energyTexture
            };
            
            _energyPrizeView.Initialize(0, _energyPrize);
        }

        _finishState = _animator.GetBehaviour<FinishState>();
        _finishState.OnEnter += ShowHeaderAndPrizes;
        _animator.SetTrigger(In);
    }

    private void ReactToCollectClicked() {

        OnCollectClicked?.Invoke();
    }

    private void ShowHeaderAndPrizes() {

        _finishState.OnEnter -= ShowHeaderAndPrizes;
        _headerAnimator.OnMoveComplete += DisplayPrizes;
        _headerAnimator.Play();
    }

    private void DisplayPrizes() {

        if (_coinPrize.Amount > 0) {

            _coinPrizeContainer.SetActive(true);
            _coinPrizeAnimator.Play();
        }

        if (_energyPrize.Amount > 0) {

            _energyPrizeContainer.SetActive(true);
            _energyPrizeAnimator.Play();
        }
    }

}