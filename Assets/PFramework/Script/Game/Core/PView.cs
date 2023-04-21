using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFramework
{
    public abstract class PView : PBundle
    {
        private PControl _control;
        public PControl Control
        {
            set { _control = value; }
            get { return _control; }
        }
        protected ABManager abManager;

        public override void Install()
        {
            abManager = ABManager.Instance;
        }
    }

    public enum UILayer
    {
        Tags,
        GuidePend,
        Pend,
        Hud,
        Dialog,
        Popup,
        Toast,
        Blocker
    }

    public abstract class PUIView : PView
    {
        public abstract UILayer GetViewLayer();
    }
}