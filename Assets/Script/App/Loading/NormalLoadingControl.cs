using SFramework;
using UnityEngine;

namespace Game
{
    public class NormalLoadingControl : SControl
    {
        protected override void opening()
        {
            GetView<NormalLoadingView>().PlayAnimator(Model.OpenParams.MessageData.ToString());
        }

        public override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        public override void HandleMessage(SBundleParams value)
        {
            //switch(value.MessageId)
            //{

            //}
        }
    }
}