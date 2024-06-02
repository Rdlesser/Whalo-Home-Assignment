using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Service<TConfigT>: IService {

    private TConfigT _config;
    public Service(string config) {
        _config = JsonUtility.FromJson<TConfigT>(config);
    }
    public abstract UniTask Initialize();
    public abstract void Clean();
    
    protected TConfigT GetConfig() {
        return _config;
    }

}