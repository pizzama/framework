using SFramework.Game;
using Cysharp.Threading.Tasks;

namespace App.Farm
{
	public class FarmMenuModel : RootModel
	{
		protected override void opening()
		{
			 GetData("").Forget();
		}
	}
}
