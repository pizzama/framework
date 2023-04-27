using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System;

namespace SFramework
{
    public partial class SModel : SBundle
    {
        private SControl _control;
        public SControl Control
        {
            set { _control = value; }
            get { return _control; }
        }

        public delegate void DelegateModelCallback();
        public DelegateModelCallback ModelCallback;
        public override void Install()
        {
            base.Install();
            initPModel();
        }

        protected virtual void initPModel()
        {

        }
        public override void Update()
        {
            base.Update();
            modelUpdate();

        }

        public override void FixUpdate()
        {
            base.FixUpdate();
            modelFixUpdate();

        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            modelLastUpdate();

        }

        protected virtual void modelUpdate()
        {

        }

        protected virtual void modelFixUpdate()
        {

        }

        protected virtual void modelLastUpdate()
        {

        }

        public async UniTask<byte[]> GetData(string url)
        {
            if (url == "")
            {
                ModelCallback?.Invoke();
                return null;
            }
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            byte[] bytes = await requestData(webRequest);
            ModelCallback?.Invoke();
            return bytes;
        }

        public async UniTask<byte[]> PostData(string url, object pars)
        {
            if (url == "")
            {
                ModelCallback?.Invoke();
                return null;
            }
            UnityWebRequest webRequest = UnityWebRequest.Post(url, pars.ToString());
            byte[] bytes = await requestData(webRequest);
            ModelCallback?.Invoke();
            return bytes;
        }

        private async UniTask<byte[]> requestData(UnityWebRequest webRequest)
        {
            try
            {
                await webRequest.SendWebRequest();
                while (!webRequest.isDone)
                {
                    if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                    {
                        break;
                    }

                    await UniTask.Yield();
                }

                if (!string.IsNullOrEmpty(webRequest.error))
                {
                    throw new Exception($"you use Web Request Error.{webRequest.url} Message{webRequest.error}");
                }

                var result = webRequest.downloadHandler.data;
                webRequest.Dispose();
                ModelCallback?.Invoke();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"you use Web Request Error.{webRequest.url} Message{e.Message}");
            }
        }
    }
}