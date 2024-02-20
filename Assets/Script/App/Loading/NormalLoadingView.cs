using SFramework.Game;
using SFramework;
using UnityEngine;
using SFramework.Statics;

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
            prefabPath = Loading_sfp.BundleName; //"Loading/BaseLoading";
            prefabName = Loading_sfp.RES_BaseLoading_prefab;
            position = new Vector3(0, 0, 0);
            rotation = Quaternion.Euler(0, 0, 0);
        }

        private Animator animator;

        protected override void opening()
        {
            animator = getUIObject<Animator>("BaseLoading");
        }

        public void PlayAnimator(string name)
        {
            if (animator)
            {
                animator.SetTrigger(name);
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