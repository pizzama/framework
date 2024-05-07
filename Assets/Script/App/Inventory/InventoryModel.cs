using Cysharp.Threading.Tasks;
using ProtoGameData;
using SFramework.Game;
namespace App.Inventory
{
	public class InventoryModel : RootModel
	{
		private ProtoUserData _userData;
		protected override void opening()
		{
			// read userData
			_userData = ReadData<ProtoUserData>();
			GetData("").Forget();
		}

		public void AddPackage(int cid, int num)
		{
			//
		}

		public void SaveUserData()
		{
			SaveData<ProtoUserData>(_userData);
		}
	}
}
