using SFramework.Game;
using UnityEngine;

namespace Game
{
    public class InputModel : RootModel
    {
        protected override void opening()
        {
            Debug.Log("test model enter");
        }

        public void Refresh()
        {
            Debug.Log("test refresh model");
        }
    }
}
