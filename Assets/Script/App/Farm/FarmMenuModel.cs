using SFramework.Game;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Farm
{
	public class FarmMenuModel : RootModel
	{
		protected override void opening()
		{
			GetData("").Forget();
			Debug.Log("opening:" + OpenParams.ToString());
		}
	}
}
