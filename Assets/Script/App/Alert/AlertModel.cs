using SFramework.Game;
using Cysharp.Threading.Tasks;

namespace App.Alert
{
	public class AlertModel : RootModel
	{
		protected override void opening()
		{
			 GetData().Forget();
		}
		protected override void closing()
		{
			// Code Here
		}
	}
}
