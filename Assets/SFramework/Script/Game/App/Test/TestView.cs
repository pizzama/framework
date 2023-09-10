using SFramework;
using SFramework.Game;
using UnityEngine.UI;

namespace Game
{
    public class TestView : SUIView
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
            _closeBtn = getUIObject<Button>("Button");
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