using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class AssetService : Service<AssetServiceConfig>, IAssetService {
    

    private Dictionary<string, string> _assetToUrlDict;
    private Dictionary<string, Texture2D> _assetNameToTextureDict;

    public AssetService(string config) : base(config) {
    }

    public override async UniTask Initialize() {

        SetAssetToTextureDictionary();
        await SetAssetNameToTextureDictionary();
    }

    private async UniTask SetAssetNameToTextureDictionary() {
        
        _assetNameToTextureDict = new Dictionary<string, Texture2D>();
        foreach (var key in _assetToUrlDict.Keys) {

            if (_assetNameToTextureDict.ContainsKey(key)) {
                
                continue;
            }

            var assetId = _assetToUrlDict[key];
            var assetDirectUrl = $"https://drive.usercontent.google.com/u/0/uc?id={assetId}&export=download";
            var texture = await GetSprite(assetDirectUrl);
            _assetNameToTextureDict[key] = texture;
        }
    }

    private void SetAssetToTextureDictionary() {
        var config = GetConfig();
        var assets = config.Assets;

        _assetToUrlDict = new();
        foreach (var assetConfig in assets) {
            _assetToUrlDict[assetConfig.Name] = assetConfig.Id;
        }
    }

    private async UniTask<Texture2D> GetSprite(string url) {

        var webRequest = UnityWebRequestTexture.GetTexture(url);
        await webRequest.SendWebRequest().WithCancellation(new CancellationToken());

        var texture = ((DownloadHandlerTexture) webRequest.downloadHandler).texture;

        return texture;
    }

    public override void Clean() {
        
        _assetToUrlDict?.Clear();
        _assetNameToTextureDict?.Clear();
    }

    public Texture2D GetAsset(string assetName) {

        return _assetNameToTextureDict[assetName];
    }

}