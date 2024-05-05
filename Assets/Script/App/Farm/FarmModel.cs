using Cysharp.Threading.Tasks;
using SFramework.Game;
using ProtoGameData;

namespace App.Farm
{
	public class FarmModel : RootModel
	{
		protected override void opening()
		{
			ProtoUserData userData = ReadData<ProtoUserData>();
			userData = new ProtoUserData();
			userData.Uid = 1;
			userData.Exp = 1;
			userData.Name = "test";
			SaveData<ProtoUserData>(userData);
			GetData("").Forget();
		}
	}
}
