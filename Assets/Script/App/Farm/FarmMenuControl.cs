using SFramework;
using UnityEngine;
using SFramework.Game;

namespace App.Farm
{
	public class FarmMenuControl : RootControl
	{
		protected override void opening()
		{
			// Code Here
		}

		public override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
	}
}
