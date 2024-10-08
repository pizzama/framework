using System.Threading;
using UnityEngine.Networking;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace SFramework
{
    public partial class SModel : SBundle
    {
        public SBundleParams OpenParams;
        private SControl _control;
        public SControl Control
        {
            set { _control = value; }
            get { return _control; }
        }

        public T GetControl<T>() where T : SControl
        {
            return (T)_control;
        }

        public T GetView<T>() where T : SView
        {
            return (T)_control.View;
        }

        public delegate void DelegateModelCallback(int code);
        public DelegateModelCallback ModelCallback;
        public override void Install()
        {
            base.Install();
            initing();
        }

        public override void Open(SBundleParams value)
        {
            OpenParams = value;
            base.Open(value);
        }

        public override void Open()
        {
            base.Open();
        }

        public override void Update()
        {
            base.Update();
            modelUpdate();

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            modelFixedUpdate();

        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            modelLastUpdate();

        }

        protected virtual void modelUpdate()
        {

        }

        protected virtual void modelFixedUpdate()
        {

        }

        protected virtual void modelLastUpdate()
        {

        }

        public async UniTask<byte[]> GetData(string url, IProgress<float> progress, CancellationTokenSource cancelSource)
        {
            if (url == "")
            {
                ModelCallback?.Invoke(0);
                return null;
            }
            try
            {
                UnityWebRequest webRequest = UnityWebRequest.Get(url);
                await webRequest.SendWebRequest().WithCancellation(cancelSource.Token);
                ModelCallback?.Invoke(0);
                return webRequest.downloadHandler.data;
            }
            catch (OperationCanceledException err)
            {
                if (cancelSource.IsCancellationRequested)
                {
                    ModelCallback?.Invoke(-1);
                    UnityEngine.Debug.LogError("Timeout." + err.ToString());
                }
                else if (cancelSource.IsCancellationRequested)
                {
                    ModelCallback?.Invoke(-2);
                    UnityEngine.Debug.LogError("Cancel clicked." + err.ToString());
                }
                else
                {
                    ModelCallback?.Invoke(-3);
                }

                return null;
            }


        }

        public async UniTask<byte[]> GetData(string url)
        {
            if (url == "")
            {
                ModelCallback?.Invoke(0);
                return null;
            }
            try
            {
                UnityWebRequest webRequest = UnityWebRequest.Get(url);
                byte[] bytes = await requestData(webRequest);
                ModelCallback?.Invoke(0);
                return bytes;
            }
            catch (System.Exception err)
            {
                ModelCallback?.Invoke(-3);
                UnityEngine.Debug.LogError("Timeout." + err.ToString());
                return null;
            }

        }

        public async UniTask<byte[]> PostData(string url, object pars)
        {
            if (url == "")
            {
                ModelCallback?.Invoke(0);
                return null;
            }
            try
            {
                UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(url, pars.ToString());
                byte[] bytes = await requestData(webRequest);
                ModelCallback?.Invoke(0);
                return bytes;
            }
            catch (System.Exception err)
            {
                ModelCallback?.Invoke(-3);
                UnityEngine.Debug.LogError("Timeout." + err.ToString());
                return null;
            }

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
                    ModelCallback?.Invoke(-1);
                    throw new Exception($"you use Web Request Error.{webRequest.url} Message{webRequest.error}");
                }

                var result = webRequest.downloadHandler.data;
                webRequest.Dispose();
                ModelCallback?.Invoke(0);
                return result;
            }
            catch (Exception err)
            {
                ModelCallback?.Invoke(-3);
                throw new Exception($"you use Web Request Error.{webRequest.url} Message{err.Message}");
            }
        }
    }
}