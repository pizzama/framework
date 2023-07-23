using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
    public class LoadingView : SUIView
    {
        protected override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        protected override UILayer GetViewLayer()
        {
            return UILayer.Toast;
        }

        protected override void SetViewPrefabPath(out string prefabPath, out string prefabName, out Vector3 position, out Quaternion rotation)
        {
            prefabPath = "Loading/BarLoading";
            prefabName = "BarLoading";
            position = new Vector3(0, 0, 0);
            rotation = Quaternion.Euler(0, 0, 0);
        }

        private Animator animator;

        protected override void opening()
        {
            animator = getAssetFromGoDict<Animator>("BarLoading");
            Debug.Log(animator);
        }

        protected override async UniTaskVoid openingAsync()
        {
            await UniTask.Yield();
        }

        private void closeHandle()
        {
            Close();
        }

        protected override void closing()
        {
            
        }

        public void FinishLoading()
        {
            //Debug.Log("FinishLoading");
            animator.SetTrigger("End");
        }
    }
}