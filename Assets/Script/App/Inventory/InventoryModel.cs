using Cysharp.Threading.Tasks;
using SFramework.Game;

namespace App.Inventory
{
	public class InventoryModel : RootModel
	{
		protected override void opening()
		{
			GetData("").Forget();
		}
	}
}
