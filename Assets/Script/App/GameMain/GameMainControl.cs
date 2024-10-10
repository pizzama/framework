using Cysharp.Threading.Tasks;
using SFramework;
using UnityEngine;

namespace Game
{
    public class GameMainControl : SControl
    {
        protected override void opening()
        {
        }

        public override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        public override void HandleMessage(SBundleParams value)
        {
        }
    }
}