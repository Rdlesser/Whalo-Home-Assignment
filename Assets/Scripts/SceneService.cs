using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine.SceneManagement;

namespace Scripts {

    public class SceneService : Service, ISceneService {

        private SceneName _currentScene;
        
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
