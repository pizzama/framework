using SFramework;
using UnityEngine;

namespace Game.App.Battle
{
    public class BattleUIControl : SControl
    {
        protected override void opening()
        {
            Debug.Log("test control enter");
        }

        public override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }
    }
}