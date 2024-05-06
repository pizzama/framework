using Cysharp.Threading.Tasks;
using SFramework.Game;
using ProtoGameData;

namespace App.Farm
{
	public class FarmModel : RootModel
	{
		protected override void opening()
		{
			var t = ReadData<ProtoUserData>();
			GetData("").Forget();
		}
	}
}
