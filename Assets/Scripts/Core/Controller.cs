using Cysharp.Threading.Tasks;
using General;
using UnityEngine;

namespace Core {

    public abstract class Controller : MonoBehaviour {
        
        private Semaphore _initializationSemaphore = new Semaphore(0);

        private async void Awake() {

            ServiceManager.Inject(this);
            
            try {
                await Initialize();
            } 
            finally {
                _initializationSemaphore.Release();
            }
        }
        
        protected abstract UniTask Initialize();
        protected abstract void Clean();

    }

}