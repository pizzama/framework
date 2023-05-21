using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game;

namespace SFramework
{
    public class GameLaucher : MonoBehaviour
    {
        private void Start()
        {

        }

        private void Awake()
        {
            //Hold GameLauncher all the time
            DontDestroyOnLoad(transform.gameObject);
            initFrameworkBundle();
        }

        private void initFrameworkBundle()
        {
            BundleManager.Instance.InstallBundle(new RootControl(), "", true);
            BundleManager.Instance.InstallBundle(new GameMainControl(), "", true);      
        }

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
