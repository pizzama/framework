using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public class GameMainView : SSCENEView
    {
        private GameObject _gameRoot;
        protected override void opening()
        {
            Scene sc = SceneManager.GetActiveScene();
            Debug.Log("GameMainView:" + sc.name);
        }

        protected override async void openingAsync()
        {
            Debug.Log("gamemain view enterasync");
            await UniTask.Yield();
        }
    }
}