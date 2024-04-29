using Cysharp.Threading.Tasks;
using SFramework.Game;

namespace App.Farm
{
	public class FarmModel : RootModel
	{
		protected override void opening()
		{
			 GetData("").Forget();
		}
	}
}
