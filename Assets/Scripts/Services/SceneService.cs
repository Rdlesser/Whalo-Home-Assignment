using Configs;
using Cysharp.Threading.Tasks;
using General;
using UnityEngine.SceneManagement;

namespace Services {

    public class SceneService : Service<SceneServiceConfiguration>, ISceneService {

        private SceneName _currentScene;
        
        public SceneService(string config) : base(config) {
        }
        
        public override UniTask Initialize() {

            return default;
        }

        public override void Clean() {
            
            
        }

        public void MoveToScene(SceneName scene) {

            if (GetCurrentScene() == scene) {
                return;
            }

            _currentScene = scene;
            SceneManager.LoadScene((int) scene);
        }

        public SceneName GetCurrentScene() {
            
            return _currentScene;
        }

    }

}