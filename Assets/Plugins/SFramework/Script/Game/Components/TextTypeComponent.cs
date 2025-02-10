using UnityEngine;
using UnityEngine.UI;

namespace SFramework.Components
{
    [RequireComponent(typeof(Text))]
    public class TextTypeComponent : TypeComponent
    {
        private Text _label;
        protected override void Init()
        {
            if (_label == null)
                _label = GetComponent<Text>();
            if(_label != null)
                _label.text = "";
        }

        protected override UnityEngine.Component getCompoent()
        {
            return _label;
        }
        
        protected override float getSize()
        {
            return _label.fontSize;
        }

        protected override void changeText(string value)
        {
            _label.text = value;
        }

        protected override void skipHandle()
        {
            _label.text = finalText;
        }

        public override void Clean()
        {
            base.Clean();
            _label.text = "";
        }
    }
    

}