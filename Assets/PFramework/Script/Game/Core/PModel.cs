using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System;

namespace PFramework
{
    public partial class PModel : PBundle
    {
        private PControl _control;
        public PControl Control
        {
            set { _control = value; }
            get { return _control; }
        }

        public delegate void DelegateModelCallback();
        public DelegateModelCallback ModelCallback;
        public override void Open()
        {

        }

        public override void Install()
        {

        }

        public async UniTask<byte[]> GetData(string url, object pars)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            return await requestData(webRequest);
        }

        public async UniTask<byte[]> PostData(string url, object pars)
        {
            UnityWebRequest webRequest = UnityWebRequest.Post(url, pars.ToString());
            return await requestData(webRequest);
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