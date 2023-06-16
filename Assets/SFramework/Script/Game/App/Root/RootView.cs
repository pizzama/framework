using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
            const string uiName = "SUIROOT";
            if (!UIROOT)
            {
                var rootPrefab = Resources.Load<GameObject>(uiName);
                if (!rootPrefab)
                {
                    throw new NotFoundException(uiName);
                }
                GameObject uiRoot = Object.Instantiate(rootPrefab);
                Object.DontDestroyOnLoad(uiRoot);
                UIROOT = ComponentTools.GetOrAddComponent<SUIROOT>(uiRoot);
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
                UniversalAdditionalCameraData cameraData = Camera.main.GetUniversalAdditionalCameraData();
                cameraData.cameraStack.Add(UIROOT.UICamera);
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