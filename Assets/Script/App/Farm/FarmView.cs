using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;
using SFramework.Game.Map;
using SFramework.Tools;

namespace App.Farm
{
	public class FarmView : SSCENEView
	{
		private SMGrid _grid;
		protected override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
		protected override void opening()
		{
			// Code Here
			_grid = new SMGrid(10, 10, 3, new Vector3(0,0,0));
			// create farm
		}

		protected override void closing()
		{
			// Code Here
		}

		protected override void viewUpdate()
		{
			if(Input.GetMouseButtonDown(0))
			{
				Vector3 vec = MapTools.GetMouseWorldPosition();
				Debug.Log(vec);
				_grid.SetValue(vec, 100);
			}
		}
	}
}
