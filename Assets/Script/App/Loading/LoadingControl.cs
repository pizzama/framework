using UnityEngine;
using SFramework;
using SFramework.Statics;

namespace App.Loading
{
	public class LoadingControl : SControl
	{
		protected override void opening()
		{
			// Code Here
			OpenControl(SFStaticsControl.App_Farm_FarmControl);
		}

		public override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
	}
}
