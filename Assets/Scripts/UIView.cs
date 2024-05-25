using UnityEngine;
using UnityEngine.UI;

namespace Scripts {

    public class UIView : MonoBehaviour {

        [SerializeField] private Image _coinImage;
        [SerializeField] private Image _energyImage;
        [SerializeField] private Image _keyImage;

        public void SetIcons(Texture2D coinTexture, Texture2D energyTexture, Texture2D keyTexture) {

            _coinImage.sprite = Sprite.Create(coinTexture,  new Rect(0f, 0f, coinTexture.width, coinTexture.height), Vector2.zero);
            _energyImage.sprite = Sprite.Create(energyTexture, new Rect(0f, 0f, energyTexture.width, energyTexture.height), Vector2.zero);
            _keyImage.sprite = Sprite.Create(keyTexture, new Rect(0f, 0f, keyTexture.width, keyTexture.height), Vector2.zero);
        }

    }

}
