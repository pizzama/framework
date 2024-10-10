using UnityEngine;
using SFramework;

namespace App.PendTips
{
	public class PendTipsControl : SControl
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
