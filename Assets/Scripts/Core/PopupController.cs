using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;

namespace Core {

        public class PopupController : Controller {
        
                [SerializeField] private PopupView _popupView;

                private ServiceReceiver<IPopupService> _popUpService = new();
                private ServiceReceiver<IAssetService> _assetService = new();

                protected override UniTask Initialize() {

                        var popupNames = _popUpService.Get().GetPopupNames();
                        var popupSprites = new Sprite[popupNames.Length];

                        for (var i = 0; i < popupNames.Length; i++) {
                        
                                var popupName = popupNames[i];
                                var texture = _assetService.Get().GetAsset(popupName);

                                var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
                                popupSprites[i] = sprite;
                        }

                        _popupView.Initialize(popupSprites);
                        return default;
                }

                public async UniTask ShowPopupScreen() {

                        await _popupView.ShowPopups();
                }

                protected override void Clean() {
                
                }

        }

}