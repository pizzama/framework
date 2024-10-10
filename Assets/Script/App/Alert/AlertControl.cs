using SFramework;
using UnityEngine;
using SFramework.Game;

namespace App.Alert
{
	public class AlertControl : RootControl
	{
		protected override void opening()
		{
			// Code Here
		}
		protected override void alreadyOpened()
		{
			// Code Here
		}

		public override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}

		protected override void closing()
		{
			// Code Here
		}
	}
}
