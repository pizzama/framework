using SFramework;
using UnityEngine;

namespace Game
{
    public class TestControl: SControl
    {
        protected override void opening()
        {
            Debug.Log("test control enter");
        }

        public bool TestFunc()
        {
            return true;
        }
    }
}