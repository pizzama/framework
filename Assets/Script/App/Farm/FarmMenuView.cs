using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;

namespace App.Farm
{
	public class FarmMenuView : SUIView
	{
		protected override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
		protected override UILayer GetViewLayer()
		{
			return UILayer.Popup;
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
