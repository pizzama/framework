using SFramework.Game;
using SFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BattleView : SUIView
    {
        protected override ViewOpenType GetViewOpenType()
        {
            return ViewOpenType.Single;
        }

        protected override UILayer GetViewLayer()
        {
            return UILayer.Popup;
        }

        private Button _closeBtn;

        protected override void opening()
        {
            _closeBtn = getAssetFromGoDict<Button>("Button");
            _closeBtn.onClick.AddListener(closeHandle);
        }

        private void closeHandle()
        {
            Close();
        }

        protected override void closing()
        {
            base.closing();
            _closeBtn.onClick.RemoveAllListeners();
        }
    }
}