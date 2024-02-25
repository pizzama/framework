using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;
using SFramework.Statics;

namespace App.TurfWorld
{
	public class TurfWorldView : SSCENEView
	{
		protected override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
		protected override void SetViewPrefabPath(out string prefabPath, out string prefabName)
		{
			prefabPath = Game_turpworld_sfs.BundleName;
			prefabName = Game_turpworld_sfs.RES_TurpWorld_unity;
		}
		protected override void opening()
		{
			// Code Here
		}
		protected override void closing()
		{
			// Code Here
		}
	}
}
