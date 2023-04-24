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
            initPView();
        }

        protected virtual void initPView()
        {

        }

        public override void Update()
        {
            base.Update();
            viewUpdate();

        }

        public override void FixUpdate()
        {
            base.FixUpdate();
            viewFixUpdate();

        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            viewLastUpdate();

        }

        protected virtual void viewUpdate()
        {

        }

        protected virtual void viewFixUpdate()
        {

        }

        protected virtual void viewLastUpdate()
        {

        }
    }

    public enum UILayer
    {
        Tags,
        Pend,
        Hud,
        Popup,
        Dialog,
        Toast,
        Blocker
    }

    public abstract class PUIView : PView
    {
        public abstract UILayer GetViewLayer();
    }
}