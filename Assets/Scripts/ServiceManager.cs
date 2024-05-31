using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Scripts;
using UnityEngine;
using Semaphore = Scripts.Semaphore;

namespace DefaultNamespace {

    public static class ServiceManager {

        private static Dictionary<IService, Semaphore> _servicesBeingInitialized = new();
        private static Dictionary<Type, IService> _services = new();
        private static ServiceAwaiters _awaiters = new();

        private static Type _serviceReceiverType = typeof(IServiceReceiver);

        private static CancellationTokenSource _quitToken = new();

        static ServiceManager() {
            UnityEngine.Application.quitting += AppQuit;
        }

        private static void AppQuit() {
            foreach(var key in _services.Keys) {
                _services[key].Clean();
            }
            _quitToken.Cancel();
        }

        public static T GetServiceOfType<T>() where T : IService {
            var type = typeof(T);
            T service;
            if(FindService(_services.Values, out service)) {
                return service;
            }
            if(FindService(_servicesBeingInitialized.Keys, out service)) {
                return service;
            }
            return default;
        }
        private static bool FindService<T>(IEnumerable<IService> services, out T retVal) {
            var type = typeof(T);
            foreach(var service in services) {
                if(type.IsAssignableFrom(service.GetType())) {
                    retVal = (T)service;
                    return true;
                }
            }
            retVal = default;
            return false;
        }

        public static async Task<T> GetService<T>(CancellationToken c = default) {
            var type = typeof(T);
            if(_services.ContainsKey(type)) {
                return (T)_services[type];
            } else {
                var service = (T)await _awaiters.WaitForService(type, _quitToken.Token);
                if(c.IsCancellationRequested) {
                    throw new TaskCanceledException();
                }
                return service;
            }
        }
        public static async Task RegisterService<T, U>(U service) where U : IService, T {

            if(_servicesBeingInitialized.ContainsKey(service)) {
                await _servicesBeingInitialized[service].WaitAsync();
            } else {
                var sem = new Semaphore(0);
                _servicesBeingInitialized.Add(service, sem);

                Inject(service);
                try {
                    // Debug.Log("Initializing service: " + service.GetType().Name + " as " + typeof(T).Name);
                    await service.Initialize();
                    // Debug.Log("Service initialized: " + service.GetType().Name + " as " + typeof(T).Name);
                } catch(Exception e) {
                    Debug.LogException(e);
                }

                _servicesBeingInitialized.Remove(service);
                sem.ReleaseAll();
            }

            var type = typeof(T);
            if(_services.ContainsKey(type)) {
                if(_services[type].Equals(service)) {
                    // Double register detected
                    return;
                }
                // Service swap detected
                _services[type] = service;
                // Swap injections? how?
                // For swaps, i need to remember all the the injection requesters, then reinject
            } else {
                _services.Add(type, service);
            }
            _awaiters.ServiceAvailable<T>(service);
        }
        public static void DeregisterService<T>(T service) {
            var type = typeof(T);
            if(_services.ContainsKey(type)) {
                if(_services[type].Equals(service)) {
                    _services.Remove(type);
                }
            }
        }
        public static void Inject(object obj) {

            var type = obj.GetType();
            var allFields = type.GetFields(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic);

            for(int i = 0; i < allFields.Length; ++i) {

                var field = allFields[i];
                if(!_serviceReceiverType.IsAssignableFrom(field.FieldType)) {
                    continue;
                }

                var receiver = field.GetValue(obj) as IServiceReceiver;
                if(receiver == null) {
                    Debug.LogError("Object of type " + type.Name + " has a service receiver of type " + field.FieldType + " but does not have an instance ready for injection");
                    continue;
                }
                var serviceType = receiver.InjectedType;

                if(_services.ContainsKey(serviceType)) {
                    try {
                        receiver.Inject(_services[serviceType]);
                    } catch(Exception e) {
                        Debug.LogException(e);
                    }
                } else {
                    // Delayed
                    InjectWhenAvailable(receiver);
                }
            }
        }
        public static IList<IService> GetInjectionList(MethodBase method, int startFrom = 0) {
            var retVal = new List<IService>();
            var parameters = method.GetParameters();
            for(int i = startFrom; i < parameters.Length; ++i) {
                if(_services.TryGetValue(parameters[i].ParameterType, out var service)) {
                    retVal.Add(service);
                }
            }
            return retVal;
        }
        private static async void InjectWhenAvailable(IServiceReceiver receiver) {
            var service = await _awaiters.WaitForService(receiver.InjectedType, _quitToken.Token);
            receiver.Inject(service);
        }
        private class ServiceAwaiters {
            private Dictionary<Type, ServiceAwaiter> _awaiters = new Dictionary<Type, ServiceAwaiter>();
            public async Task<object> WaitForService(Type type, CancellationToken c = default) {
                ServiceAwaiter awaiter;
                if(_awaiters.ContainsKey(type)) {
                    awaiter = _awaiters[type];
                } else {
                    awaiter = new ServiceAwaiter();
                    _awaiters.Add(type, awaiter);
                }
                await awaiter.Sem.WaitAsync(c);
                return awaiter.Service;
            }
            public void ServiceAvailable<T>(object service) {
                var type = typeof(T);
                if(_awaiters.ContainsKey(type)) {
                    var awaiter = _awaiters[type];
                    _awaiters.Remove(type);
                    awaiter.Service = service;
                    awaiter.Sem.ReleaseAll();
                }
            }
            private class ServiceAwaiter {
                public Semaphore Sem = new Semaphore(0);
                public object Service;
            }
        }
    }
}
