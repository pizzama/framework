using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SFramework
{
    public abstract class SSCENEView : RootView
    {
        protected override void init()
        {


        }
        public override void Open()
        {

        }

        public async UniTask<AsyncOperation> LoadSceneAsync(string sceneFullName, LoadSceneMode mode)
        {
            if (SceneManager.GetSceneByName(sceneFullName).isLoaded)
            {
                return null;
            }
            var progress = Progress.Create<float>(p => Debug.LogFormat("p:{0}", p));
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneFullName, mode);
            await operation.ToUniTask(progress);
            await UniTask.Yield();
            return operation;
        }
    }
}

