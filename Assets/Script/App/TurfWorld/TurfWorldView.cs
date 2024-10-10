using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;
using SFramework.Statics;

namespace App.TurfWorld
{
	public class TurfWorldView : SSCENEView
	{
		private WorldGen _worldGen;
		protected override void SetViewPrefabPath(out string prefabPath, out string prefabName)
		{
			prefabPath = Game_turpworld_sfs.BundleName;
			prefabName = Game_turpworld_sfs.RES_TurpWorld_unity;
		}
		protected override void opening()
		{
			// Code Here
			_worldGen = getExportObject<WorldGen>("WorldGen");
			_worldGen.GenerateWorld();
		}
		protected override void closing()
		{
			// Code Here
		}
	}
}
