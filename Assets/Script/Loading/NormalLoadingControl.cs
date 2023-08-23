using SFramework;
using UnityEngine;

namespace Game
{
    public class NormalLoadingControl : SControl
    {
        protected override void opening()
        {
        }

        public override void HandleMessage(BundleParams value)
        {
            switch(value.MessageId)
            {
                case "LoadingUpdate":
                    Debug.Log("loading value:" + value.MessageData);
                break;
                case "LoadingEnd":
                    (View as NormalLoadingView).FinishLoading();
                    Debug.Log("loading end:" + value.MessageData);
                break;
            }

        }
    }
}