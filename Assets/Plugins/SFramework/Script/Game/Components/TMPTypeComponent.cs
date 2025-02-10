using TMPro;
using UnityEngine;

namespace SFramework.Components
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMPTypeComponent : TypeComponent
    {
        private TextMeshProUGUI _label;
        protected override void Init()
        {
            if (_label == null)
                _label = GetComponent<TextMeshProUGUI>();
            if(_label != null)
                _label.text = "";
        }

        protected override UnityEngine.Component getCompoent()
        {
            return _label;
        }

        protected override void skipHandle()
        {
            _label.text = finalText;
        }

        protected override float getSize()
        {
            return _label.fontSize;
        }

        protected override void changeText(string value)
        {
            _label.text = value;
        }

        public override void Clean()
        {
            base.Clean();
            _label.text = "";
        }
    }
}