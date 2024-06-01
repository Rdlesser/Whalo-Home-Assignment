using Cysharp.Threading.Tasks;

public class PopupService : Service<PopupServiceConfig>, IPopupService {

    private string[] _popupNames;
    
    public PopupService(string config) : base(config) {
    }

    public override UniTask Initialize() {

        var config = GetConfig();
        _popupNames = config.Popups;
        return UniTask.CompletedTask;
    }

    public override void Clean() {
        
    }

    public string[] GetPopupNames() {

        return _popupNames;
    }

}