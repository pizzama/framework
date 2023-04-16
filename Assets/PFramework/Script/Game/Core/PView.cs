using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFramework
{
    public class PView : PBundle
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
}