using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine.SceneManagement;

namespace Scripts {

    public class SceneService : Service, ISceneService {

        public override UniTask Initialize() {

            return default;
        }

        public override void Clean() {
            
        }

        public void MoveToScene(SceneName scene) {

            if (GetCurrentScene() == scene) {
                return;
            }

            SceneManager.LoadScene((int) scene);
        }

        public SceneName GetCurrentScene() {
            throw new System.NotImplementedException();
        }

    }

}
