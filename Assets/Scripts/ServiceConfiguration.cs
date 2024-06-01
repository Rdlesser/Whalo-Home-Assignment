using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Scripts;
using UnityEngine;

namespace DefaultNamespace {

    public class ServiceConfiguration {

        private const string CONFIGURATION_FOLDER_NAME = "ServiceConfigs";
        Dictionary<string, string> _servicesConfiguration;
        
        public ServiceConfiguration() {
            var allConfigs = Resources.LoadAll<TextAsset>(CONFIGURATION_FOLDER_NAME);
            _servicesConfiguration = new Dictionary<string, string>();
            foreach(var config in allConfigs) {
                _servicesConfiguration.Add(config.name, config.text);
            }
        }

        public async UniTask Initialize() {
            
            await UniTask.WhenAll( new[]

            {
                Register<ISceneService, SceneService>(),
                Register<IAssetService, AssetService>(),
                Register<IPopupService, PopupService>()
            });
            
        }
        
        private async UniTask Register<T, U>() where U : T, IService {
            var service = ServiceManager.GetServiceOfType<U>();
            if(service == null) {
                service = (U)Activator.CreateInstance(typeof(U), GetConfig<U>());
            }
            await ServiceManager.RegisterService<T, U>(service);
        }
        
        private string GetConfig<T>() {
            var type = typeof(T);
            if((_servicesConfiguration != null) && _servicesConfiguration.ContainsKey(type.Name)) {
                return _servicesConfiguration[type.Name];
            } else {
                Debug.LogWarning("Did not find config for: " + type.Name);
                return "{}";
            }
        }

    }

}
