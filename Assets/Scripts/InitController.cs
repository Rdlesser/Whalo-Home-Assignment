using Cysharp.Threading.Tasks;
using Scripts;
using UnityEngine;

namespace DefaultNamespace {

    public class InitController : Controller {

        private ServiceConfiguration _serviceConfiguration;
        private ServiceReceiver<ISceneService> _sceneService = new();
        
        protected override async UniTask Initialize() {
            
            _serviceConfiguration = new ServiceConfiguration();
            Debug.LogError("Initializing Service Configuration");
            await _serviceConfiguration.Initialize();
            Debug.LogError("Awaiting for scene service to arrive");
            await _sceneService.ServiceArrived();
            
            Debug.LogError("Moving to main menu scene");
            _sceneService.Get().MoveToScene(SceneName.MainMenu);
        }

        protected override void Clean() {
            
            
        }

    }

}
