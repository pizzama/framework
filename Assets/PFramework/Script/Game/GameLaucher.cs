using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PFramework
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
            var ui = new RootUIControl();
            BundleManager.Instance.InstallBundle(ui);
            ui.Open();
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
