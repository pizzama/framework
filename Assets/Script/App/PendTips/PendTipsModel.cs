using SFramework;
using Cysharp.Threading.Tasks;

namespace App.PendTips
{
	public class PendTipsModel : SModel
	{
		protected override void opening()
		{
			 GetData("").Forget();
		}
	}
}
