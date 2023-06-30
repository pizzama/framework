using UnityEngine;

namespace SFramework
{
    public abstract class GameLauncher : MonoBehaviour
    {
        private void Awake()
        {
            //Hold GameLauncher all the time
            DontDestroyOnLoad(transform.gameObject);
            initFrameworkBundle();
            installBundle();
        }

        private void initFrameworkBundle()
        {
            // init scene and ui
            // BundleManager.Instance.InstallBundle(new RootControl(), "", true);
        }

        protected abstract void installBundle();

        private void Update()
        {

        }

        private void OnApplicationPause(bool pause)
        {
            Debug.Log("pause");
        }

        private void OnApplicationQuit()
        {
            Debug.Log("quit");
        }

        private void OnApplicationFocus(bool focus)
        {
            Debug.Log("focus");
        }

    }
}
