using System;
using Cysharp.Threading.Tasks;

public class ServiceReceiver<T> : IServiceReceiver {

    public Type InjectedType => typeof(T);

    public bool HasArrived => _sem == null;

    private T _service;
    private Semaphore _sem = new Semaphore(0);

    public void Inject(object service) {
        _service = (T)service;
        if(_sem != null) {
            _sem.ReleaseAll();
            _sem = null;
        }
    }

    public T Get() {
        return _service;
    }
    public async UniTask ServiceArrived() {
        if(_sem != null) {
            await _sem.WaitAsync();
        }
    }
}

public class ServiceReceiver<T, U> : ServiceReceiver<T> where U : class, T where T : class {
    new public U Get() {
        return base.Get() as U;
    }
}