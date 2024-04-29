using UnityEngine;
using SFramework.Game;
using SFramework.Statics;

namespace App.Farm
{
	public class FarmControl : RootControl
	{
		protected override void opening()
		{
			// Code Here
			BroadcastMessage("1111", SFStaticsControl.App_Inventory_InventoryControl, "1111");
		}
	}
}
