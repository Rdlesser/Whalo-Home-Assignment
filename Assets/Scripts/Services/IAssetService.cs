using UnityEngine;

namespace Services {

    public interface IAssetService {


        Texture2D GetAsset(string assetName);
    }

}