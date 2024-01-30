using SFramework;
using UnityEngine;

namespace Game
{
    public class InputControl : SControl
    {
        protected override void opening()
        {
        //    TestControl test = GetControl<TestControl>();
        //    Debug.Log(test.TestFunc());
        }

        public void Refresh()
        {
            Debug.Log("test refresh");
        }
    }
}