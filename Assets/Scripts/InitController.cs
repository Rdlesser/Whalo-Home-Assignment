using Cysharp.Threading.Tasks;
using Scripts;
using UnityEngine;

namespace DefaultNamespace {

    public class InitController : Controller {

        [SerializeField] private AssetServiceConfig _assetServiceConfig; 

        private ServiceConfiguration _serviceConfiguration;
        private ServiceReceiver<ISceneService> _sceneService = new();
        private ServiceReceiver<IAssetService> _assetService = new();

        protected override async UniTask Initialize() {
            
            _serviceConfiguration = new ServiceConfiguration();
            await _serviceConfiguration.Initialize();
            
            UniTask.WhenAll(new[] {
                _sceneService.ServiceArrived(),
                _assetService.ServiceArrived(),
            });
            
            
            Debug.LogError("Moving to main menu scene");
            _sceneService.Get().MoveToScene(SceneName.MainMenu);
        }

        protected override void Clean() {
            
            
        }

    }

}
