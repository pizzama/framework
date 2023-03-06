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
            initUI();
        }

        private void initUI()
        {
            //Init root UI
            const string uiname = "PUIROOT";
            bool hasUI = GameObject.Find(uiname);
            if (!hasUI)
            {
                var uirootPrefab = Resources.Load<GameObject>(uiname);
                if(!uirootPrefab)
                {
                    throw new NotFoundException(uiname);
                }
                var uiroot = Object.Instantiate(uirootPrefab);
            }
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
