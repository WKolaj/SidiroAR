using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Assets.Editor
{
    public class GenerateAllAssetBundles : EditorWindow
    {
        [MenuItem("SidiroAR/Generate Android Asset bundles...")]
        static void CreateSMDLFile()
        {
            BuildPipeline.BuildAssetBundles(@"Assets/AssetBundles", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
        }

        [MenuItem("SidiroAR/Generate IOS Asset bundles...")]
        static void CreateISMDLFile()
        {
            BuildPipeline.BuildAssetBundles(@"Assets/AssetBundles", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
        }
    }
}
