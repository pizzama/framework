using System.Threading;
using UnityEngine.Networking;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using SFramework.Tools;

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

        public async UniTask<byte[]> GetData()
        {
            return await GetData("", true);
        }

        // public async UniTask<byte[]> GetData(string url, IProgress<float> progress, CancellationTokenSource cancelSource)
        // {
        //     if (url == "")
        //     {
        //         ModelCallback?.Invoke(0);
        //         return null;
        //     }
        //     try
        //     {
        //         UnityWebRequest webRequest = UnityWebRequest.Get(url);
        //         await webRequest.SendWebRequest().WithCancellation(cancelSource.Token);
        //         ModelCallback?.Invoke(0);
        //         return webRequest.downloadHandler.data;
        //     }
        //     catch (OperationCanceledException err)
        //     {
        //         if (cancelSource.IsCancellationRequested)
        //         {
        //             ModelCallback?.Invoke(-1);
        //             UnityEngine.Debug.LogError("Timeout." + err.ToString());
        //         }
        //         else if (cancelSource.IsCancellationRequested)
        //         {
        //             ModelCallback?.Invoke(-2);
        //             UnityEngine.Debug.LogError("Cancel clicked." + err.ToString());
        //         }
        //         else
        //         {
        //             ModelCallback?.Invoke(-3);
        //         }
        //
        //         return null;
        //     }
        // }

        public async UniTask<byte[]> GetData(string url, Dictionary<string, string> getParams,  bool isModelCallback = false)
        {
            string pathurl = url;
            string urlparam = StringTools.BuildQueryString(getParams);
            if (!string.IsNullOrEmpty(urlparam))
            {
                pathurl = url + "?" + urlparam;
            }
            return await GetData(pathurl, isModelCallback);
        }

        public async UniTask<byte[]> GetData(string url, bool isModelCallback)
        {
            if (url == "")
            {
                if(isModelCallback)
                    ModelCallback?.Invoke(0);
                return null;
            }
            try
            {
                UnityWebRequest webRequest = UnityWebRequest.Get(url);
                byte[] bytes = await requestData(webRequest, isModelCallback);
                if(isModelCallback)
                    ModelCallback?.Invoke(0);
                return bytes;
            }
            catch (System.Exception err)
            {
                if(isModelCallback)
                    ModelCallback?.Invoke(-3);
                UnityEngine.Debug.LogError("Timeout." + err.ToString());
                return null;
            }

        }

        public async UniTask<byte[]> PostData(string url, object pars, Dictionary<string, string> headParams = default)
        {
            return await PostData(url, pars, headParams, false);
        }

        public async UniTask<byte[]> PostData(string url, object pars, Dictionary<string, string> headParams = default, bool isModelCallback = true)
        {
            if (url == "")
            {
                if(isModelCallback)
                    ModelCallback?.Invoke(0);
                return null;
            }
            try
            {
                string paramsJson = "";
                if (pars is string)
                {
                    paramsJson = (string)pars;
                }
                else
                {
                    paramsJson = JsonUtility.ToJson(pars);
                }
                
                UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
                // 将 JSON 数据作为请求体
                byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(paramsJson);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                foreach (var head in headParams)
                {
                    webRequest.SetRequestHeader(head.Key,head.Value);
                }
                byte[] bytes = await requestData(webRequest, isModelCallback);
                if(isModelCallback)
                    ModelCallback?.Invoke(0);
                return bytes;
            }
            catch (System.Exception err)
            {
                if(isModelCallback)
                    ModelCallback?.Invoke(-3);
                UnityEngine.Debug.LogError("Timeout." + err.ToString());
                return null;
            }

        }

        private async UniTask<byte[]> requestData(UnityWebRequest webRequest, bool isModelCallback)
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
                    if(isModelCallback)
                        ModelCallback?.Invoke(-1);
                    throw new Exception($"you use Web Request Error.{webRequest.url} Message{webRequest.error}");
                }

                var result = webRequest.downloadHandler.data;
                webRequest.Dispose();
                if(isModelCallback)
                    ModelCallback?.Invoke(0);
                return result;
            }
            catch (Exception err)
            {
                if(isModelCallback)
                    ModelCallback?.Invoke(-3);
                throw new Exception($"you use Web Request Error.{webRequest.url} Message{err.Message}");
            }
            finally
            {
                if (webRequest != null)
                {
                    webRequest.Dispose();
                }
            }
        }
    }
}