using UnityEngine.Networking;

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
            // //request data
            // Callback?.Invoke();
        }

        public override void Install()
        {
            
        }

        public void GetData(string url, object pars)
        {
            UnityWebRequest req = UnityWebRequest.Get(url);
        }

        public void PostData(string url, object pars)
        {
            UnityWebRequest req = UnityWebRequest.Post(url, pars.ToString());
        }
    }
}