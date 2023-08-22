using Cysharp.Threading.Tasks;
using SFramework;
using SFramework.Game;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
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
            openScene().Forget();
        }

        private async UniTaskVoid openScene()
        {
            Control.OpenControl("Game.LoadingControl");
            AsyncOperation operation = await LoadSceneAsync("Scenes/BaseScene", "BaseScene", LoadSceneMode.Single);
            if (operation != null)
            {
                var progress = Progress.Create<float>(p => Control.BroadcastMessage("LoadingUpdate", "Game.LoadingControl", p));
                await operation.ToUniTask(progress);
            }
            Control.BroadcastMessage("LoadingEnd", "Game.LoadingControl");
            await UniTask.Delay(TimeSpan.FromSeconds(2));

            //Scene sc = SceneManager.GetActiveScene();
            //await SceneManager.UnloadSceneAsync(sc);
            //sc = SceneManager.GetActiveScene();
            //rootManager.CollectCamera();
        }
    }
}