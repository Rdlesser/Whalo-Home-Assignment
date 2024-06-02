using Cysharp.Threading.Tasks;

public interface IService {
    UniTask Initialize();
    void Clean();
}