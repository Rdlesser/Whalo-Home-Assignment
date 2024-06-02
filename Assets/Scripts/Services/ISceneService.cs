using General;

namespace Services {

    public interface ISceneService {
        
        void MoveToScene(SceneName scene);
        SceneName GetCurrentScene();

    }

}