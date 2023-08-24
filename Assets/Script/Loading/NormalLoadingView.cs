using Cysharp.Threading.Tasks;
using SFramework.Game;
using SFramework;
using UnityEngine;

namespace Game
{
    public class NormalLoadingView : SUIView
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
            if(animator)
            {
                animator.SetTrigger(Control.Model.OpenParams.MessageData.ToString());
            }
        }

        private void closeHandle()
        {
            Close();
        }

        protected override void closing()
        {
            
        }
    }
}