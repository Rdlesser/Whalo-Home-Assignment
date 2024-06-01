using Scripts;
using UnityEngine;

public class EndScreenView : MonoBehaviour {

    [SerializeField] private GameObject _coinPrizeContainer;
    [SerializeField] private GameObject _energyPrizeContainer;
    [SerializeField] private PrizeView _coinPrizeView;
    [SerializeField] private PrizeView _energyPrizeView;

    public void Initialize(Sprite coinSprite, long coinAmount, Sprite energySprite, long energyAmount) {

        if (coinAmount > 0) {
            
            var coinPrize = new Prize() {
                Amount =  coinAmount,
                PrizeType = PrizeType.Coins,
                Sprite = coinSprite
            };
            
            _coinPrizeView.Initialize(0, coinPrize);
            _coinPrizeContainer.SetActive(true);

        }

        if (energyAmount > 0) {
            
            var energyPrize = new Prize() {
                Amount =  energyAmount,
                PrizeType = PrizeType.Energy,
                Sprite = energySprite
            };
            
            _energyPrizeView.Initialize(0, energyPrize);
            _energyPrizeContainer.SetActive(true);
        }
    }
}