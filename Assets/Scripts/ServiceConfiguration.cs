using System;
using Cysharp.Threading.Tasks;
using Scripts;

namespace DefaultNamespace {

    public class ServiceConfiguration {

        public async UniTask Initialize() {
            await Register<ISceneService, SceneService>();
        }
        
        private async UniTask Register<T, U>() where U : T, IService {
            var service = ServiceManager.GetServiceOfType<U>();
            if(service == null) {
                service = (U)Activator.CreateInstance(typeof(U));
            }
            await ServiceManager.RegisterService<T, U>(service);
        }

    }

}
