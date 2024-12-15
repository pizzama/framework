using SFramework;
using Cysharp.Threading.Tasks;

namespace App.Loading
{
	public class LoadingModel : SModel
	{
		protected override void opening()
		{
			 GetData().Forget();
		}
	}
}
