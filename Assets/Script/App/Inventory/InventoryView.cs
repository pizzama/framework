using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;

namespace App.Inventory
{
	public class InventoryView : SUIView
	{
		protected override void opening()
		{
			// Code Here
		}
		protected override void closing()
		{
			// Code Here
		}

        protected override UILayer GetViewLayer()
        {
            return UILayer.None;
        }
    }
}
