using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;

namespace game
{
    public class GameMainView : SView
    {
        private GameObject _gameRoot;
        protected override void opening()
        {
            // Debug.Log("gamemain view enter");
            // const string gamename = "Game";
            // _gameRoot = GameObject.Find(gamename);  
            // if(_gameRoot == null)
            // {
            //     Debug.LogError("scene not ready");
            // }

            // // 遍历场景中的GameObject
            // foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            // {
            //     Debug.Log(obj.name);
            // }
        }

        protected override async void openingAsync()
        {
            Debug.Log("gamemain view enterasync");
            await UniTask.Yield();
        }
    }
}