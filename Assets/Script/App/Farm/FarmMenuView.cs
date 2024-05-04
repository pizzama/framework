using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;

namespace App.Farm
{
	public class FarmMenuView : SUIView
	{
		private Button _closeBtn;
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
			_closeBtn = getExportObject<Button>("CloseBtn");
			_closeBtn.onClick.AddListener(CloseHandle);
		}
		protected override void closing()
		{
			// Code Here
			_closeBtn.onClick.RemoveListener(CloseHandle);
		}

		private void CloseHandle()
		{
			GetControl<FarmMenuControl>().Close();
		}
	}
}
