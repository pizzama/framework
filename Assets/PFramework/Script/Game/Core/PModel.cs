using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFramework
{
    public class PModel : PBundle
    {
        private PControl _control;
        public PControl Control {
            set {_control = value;}
            get {return _control;}
        }

        public delegate void DelegateModelCallback();
        public DelegateModelCallback Callback;
        public override void Open()
        {
            //request data
            Callback?.Invoke();
        }
    }
}