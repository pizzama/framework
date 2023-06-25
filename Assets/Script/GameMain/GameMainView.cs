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
            Debug.Log("gameMain view enterAsync");
            AsyncOperation operation = await LoadSceneAsync("Scenes/BaseScene", LoadSceneMode.Additive);
            var progress = Progress.Create<float>(p => Debug.LogFormat("array p:{0}", p));
            await operation.ToUniTask(progress);
            await UniTask.Yield();
        }
    }
}