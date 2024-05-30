using System;
using DefaultNamespace;

namespace Scripts {

    public interface ISceneService {
        
        void MoveToScene(SceneName scene);
        SceneName GetCurrentScene();

    }

}