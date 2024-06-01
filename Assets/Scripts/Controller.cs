using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scripts;
using UnityEngine;

namespace DefaultNamespace {

    public abstract class Controller : MonoBehaviour {

        private static readonly Dictionary<System.Type, Controller> _instances = new();
        private Semaphore _initializationSemaphore = new Semaphore(0);

        private async void Awake() {
            
            var type = GetType();

            _instances[type] = this;
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
