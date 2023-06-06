using System.Collections.Generic;
using UnityEngine;

namespace SFramework
{
    public class RootView : SView
    {
        public static SUIROOT UIROOT;
        protected static Dictionary<string, GameObject> sceneDict;

        protected AssetsManager assetManager;

        public override void Install()
        {
            assetManager = AssetsManager.Instance;
            //init ui
            initUI();
            initScene();
            base.Install();
        }

        private void initUI()
        {
            const string uiname = "SUIROOT";
            if (!UIROOT)
            {
                var uirootPrefab = Resources.Load<GameObject>(uiname);
                if (!uirootPrefab)
                {
                    throw new NotFoundException(uiname);
                }
                GameObject uiroot = Object.Instantiate(uirootPrefab);
                Object.DontDestroyOnLoad(uiroot);
                UIROOT = ComponentTools.GetOrAddComponent<SUIROOT>(uiroot);
            }
        }

        private void initScene()
        {
            //init current scene
            if (sceneDict == null)
            {
                sceneDict = new Dictionary<string, GameObject>();
                foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
                {
                    //遍历场景中的GameObject 记录需要的object
                    if (obj.name.IndexOf(RootModel.SCENEPREFIX) >= 0)
                    {
                        sceneDict[obj.name] = obj;
                    }
                }
            }
        }

        public GameObject GetDefineSceneObject(string name)
        {
            GameObject go = null;
            sceneDict.TryGetValue(RootModel.SCENEPREFIX + name, out go);
            return go;
        }
    }
}