using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace DefaultNamespace {

    public interface IService {
        UniTask Initialize();
        void Clean();
    }

}
