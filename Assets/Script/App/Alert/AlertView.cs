using UnityEngine;
using SFramework;
using SFramework.Game;
using UnityEngine.UI;
using TMPro;

namespace App.Alert
{
	public class AlertView : SUIView
	{
		private TextMeshProUGUI _textMeshGui;
		protected override ViewOpenType GetViewOpenType()
		{
			return ViewOpenType.Single;
		}
		protected override UILayer GetViewLayer()
		{
			return UILayer.Popup;
		}

        protected override void SetViewPrefabPath(out string prefabPath, out string prefabName, out Vector3 position, out Quaternion rotation)
        {
			position = default;
			rotation = default;
			AlertModel model = GetModel<AlertModel>();
			SBundleParams sParams = model.OpenParams;
			AlertParams aParams = (AlertParams)sParams.MessageData;

			prefabName = aParams.PrefabName;
			prefabPath = aParams.PrefabPath;
			
        }

        protected override void opening()
		{
			// Code Here
			AlertModel model = GetModel<AlertModel>();
			AlertParams mData = (AlertParams)model.OpenParams.MessageData;
			_textMeshGui = getExportObject<TextMeshProUGUI>("Content");
			_textMeshGui.text = mData.Content;

			//

		}
		protected override void closing()
		{
			// Code Here
		}
	}
}
