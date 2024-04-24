using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;
using SFramework.Game.Map;

namespace App.Farm
{
	public class FarmView : SSCENEView
	{
		protected override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
		protected override void opening()
		{
			// Code Here
			SMGrid grid = new SMGrid(10, 10, 3, new Vector3(0,0,0));
		}
		protected override void closing()
		{
			// Code Here
		}
	}
}
