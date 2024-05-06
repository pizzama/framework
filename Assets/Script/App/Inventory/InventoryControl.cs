using UnityEngine;
using SFramework.Game;
using SFramework;

namespace App.Inventory
{
	public class InventoryControl : RootControl
	{
		protected override void opening()
		{
			// Code Here
		}

        public override void HandleMessage(SBundleParams value)
        {
			if(IsOpen)
			{
				Debug.Log(value.ToString());
			}
			 
        }
    }
}
