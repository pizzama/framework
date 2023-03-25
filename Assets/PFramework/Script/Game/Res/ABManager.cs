using System.Net.Mime;
using System.Collections.Generic;
using UnityEngine;

namespace PFramework
{
    public class ABManager
    {
        private static ABManager _instance;
        public static ABManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ABManager();
                    _instance.init();
                }
                return _instance;
            }
        }
        private Dictionary<string, AssetBundle> _abCache;
        private void init()
        {
            _abCache = new Dictionary<string, AssetBundle>();
        }

        private AssetBundle mainAB = null;  //main abbundle
        private AssetBundleManifest mainMainfest = null; // main config get refrence

        private string basePath
        {
            get
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                return Application.streamingAssetsPath;
#elif UNITY_IPHONE 
                return Application.dataPath + "/Raw/";
#elif UNOTY_ANDROID
                return Application.dataPath + "!/assets/";
#endif
            }
        }

        private string mainABName
        {
            get
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                return "StandaloneWindows";
#elif UNITY_IPHONE 
                return "IOS";
#elif UNOTY_ANDROID
                return "Android";
#endif
            }
        }

        private AssetBundle loadABPackage(string abName)
        {
            AssetBundle ab = null;
            if(mainAB == null)
            {
                mainAB = AssetBundle.LoadFromFile(basePath + mainABName);
                mainMainfest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }

            string[] dependencies = mainMainfest.GetAllDependencies(abName);

            return ab;
        }


    }

}