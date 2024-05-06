using Cysharp.Threading.Tasks;
using ProtoGameData;
using SFramework.Game;
namespace App.Inventory
{
	public class InventoryModel : RootModel
	{
		protected override void opening()
		{
			// read userData
			GetData("").Forget();
		}
	}
}
