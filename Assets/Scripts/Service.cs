using Cysharp.Threading.Tasks;

namespace DefaultNamespace {

    public abstract class Service : IService {

        public abstract UniTask Initialize();
        public abstract void Clean();

    }

}
