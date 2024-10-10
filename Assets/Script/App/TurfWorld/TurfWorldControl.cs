using UnityEngine;
using SFramework;

namespace App.TurfWorld
{
	public class TurfWorldControl : SControl
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
