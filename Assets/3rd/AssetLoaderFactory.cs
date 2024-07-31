public class AssetLoaderFactory
{
    public static IAssetLoader CreateLoader(AssetLoadType loadType)
    {
        if (loadType == AssetLoadType.AssetBundle)
        {
            return new AssetBundleLoader();
        }
        else if (loadType == AssetLoadType.Resources)
        {
            return new ResourcesLoader();
        }
        else
        {
            throw new System.Exception("Unsupported load type:" + loadType);
        }
    }
}