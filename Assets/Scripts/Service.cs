using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

public abstract class Service<ConfigT>: IService {

    private ConfigT _config;
    public Service(string config) {
        _config = JsonUtility.FromJson<ConfigT>(config);
    }
    public abstract UniTask Initialize();
    public abstract void Clean();
    
    protected ConfigT GetConfig() {
        return _config;
    }

}