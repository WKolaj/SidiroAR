using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class AssetBundleLoader 
{
    /// <summary>
    /// Method for loading game object build in asset bundle
    /// Asset bundle should contain only one asset - throws otherwise
    /// </summary>
    /// <param name="bundlePath">
    /// Path to bundle
    /// </param>
    /// <param name="boundle">
    /// Asset bundle object
    /// </param>
    /// <returns>
    /// Game object loaded from bundle
    /// </returns>
    public static GameObject LoadBundle(string bundlePath, out AssetBundle bundle)
    {
        bundle = AssetBundle.LoadFromFile(bundlePath);

        var allAssets = bundle.LoadAllAssets<GameObject>();
        
        if (allAssets.Length <= 0)
            throw new InvalidDataException(string.Format("Bundle contains no assets! {0}", bundlePath));


        if (allAssets.Length > 1)
            throw new InvalidDataException(string.Format("Bundle contains more than one asset! {0}", bundlePath));
       
        return allAssets[0];
    }

    /// <summary>
    /// Method for unloading bundle from app
    /// </summary>
    /// <param name="bundle">
    /// bundle to unload
    /// </param>
    public static void Unload(AssetBundle bundle)
    {
        bundle.Unload(true);
    }

}
