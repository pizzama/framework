using System;
using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public class GameMainView : SSCENEView
    {
        private GameObject _gameRoot;

        protected override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        protected override void opening()
        {
            Scene sc = SceneManager.GetActiveScene();
            Debug.Log("GameMainView:" + sc.name);
        }

        protected override async void openingAsync()
        {
            Debug.Log("gameMain view enterAsync");
            AsyncOperation operation = await LoadSceneAsync("Scenes/BaseScene", "BaseScene", LoadSceneMode.Single);
            if (operation != null)
            {
                var progress = Progress.Create<float>(p => Debug.LogFormat("array p:{0}", p));
                await operation.ToUniTask(progress);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(2));

            Scene sc = SceneManager.GetActiveScene();
            await SceneManager.UnloadSceneAsync(sc);
            sc = SceneManager.GetActiveScene();
            rootManager.CollectCamera();
        }
    }
}