using SFramework;
using Cysharp.Threading.Tasks;

namespace App.Farm
{
	public class FarmModel : SModel
	{
		protected override void opening()
		{
			 GetData("").Forget();
		}
	}
}
