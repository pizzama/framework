using SFramework;
using SFramework.Game;
using SFramework.Statics;

namespace App.Farm
{
	public class FarmControl : RootControl
	{
		public override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
		protected override void opening()
		{
			// Code Here
			BroadcastControl("1111", SFStaticsControl.App_Inventory_InventoryControl, "1111");
			OpenControl(SFStaticsControl.App_WorldTime_WorldTimeControl);
		}

		public void OpenFarmView(FlowerEntity entity)
		{
			OpenControl(SFStaticsControl.App_Farm_FarmMenuControl, entity);
		}
	}
}
