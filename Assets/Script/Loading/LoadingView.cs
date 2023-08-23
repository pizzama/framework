using SFramework.Game;
using SFramework;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace Game
{
    public class LoadingView : SSCENEView
    {
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
                var progress = Progress.Create<float>(p => Control.BroadcastMessage("LoadingUpdate", "", p));
                await operation.ToUniTask(progress);
            }
            Control.BroadcastMessage("LoadingEnd", "Game.LoadingControl");
            await UniTask.Delay(TimeSpan.FromSeconds(2));
        }
    }
}