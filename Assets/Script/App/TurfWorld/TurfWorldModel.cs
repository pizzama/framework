using SFramework;
using Cysharp.Threading.Tasks;

namespace App.TurfWorld
{
	public class TurfWorldModel : SModel
	{
		protected override void opening()
		{
			 GetData().Forget();
		}
	}
}
