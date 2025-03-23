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

        public T BytesToObject<T>(byte[] bytes) where T: new()
        {
            if (bytes == null || bytes.Length == 0)
            {
                return default(T);
            }
            // 方法1：如果是JSON格式数据
            string jsonString = System.Text.Encoding.UTF8.GetString(bytes);
            return JsonUtility.FromJson<T>(jsonString);
        }

        public async UniTask<byte[]> GetData()
        {
            return await GetRemoteData("", true);
        }

        public async UniTask<byte[]> GetRemoteData(string url, bool isModelCallback)
        {
            return await GetRemoteData(url, null, null, null, 5, isModelCallback);
        }

        public async UniTask<byte[]> GetRemoteData(string url, Dictionary<string, string> getParams,  bool isModelCallback)
        {
            string pathurl = url;
            string urlparam = StringTools.BuildQueryString(getParams);
            if (!string.IsNullOrEmpty(urlparam))
            {
                pathurl = url + "?" + urlparam;
            }
            return await GetRemoteData(pathurl, isModelCallback);
        }
        
        public async UniTask<byte[]> GetRemoteData(string url, Dictionary<string, string> getParams, Dictionary<string, string> headParams, IProgress<float> progress, int timeout, bool isModelCallback)
        {
            if (url == "")
            {
                if(isModelCallback)
                    ModelCallback?.Invoke(0);
                return null;
            }

            string pathurl = url;
            if (getParams != null)
            {
                string urlparam = StringTools.BuildQueryString(getParams);
                if (!string.IsNullOrEmpty(urlparam))
                {
                    pathurl = url + "?" + urlparam;
                }
            }

            try
            {
                UnityWebRequest webRequest = UnityWebRequest.Get(pathurl);
                webRequest.timeout = timeout;
                if (webRequest != null)
                {
                    foreach (var head in headParams)
                    {
                        webRequest.SetRequestHeader(head.Key,head.Value);
                    }
                }

                byte[] bytes = await requestData(webRequest, isModelCallback, progress);
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
        
        public async UniTask<byte[]> PostRemoteData(string url, object pars, Dictionary<string, string> getParams, Dictionary<string, string> headParams, IProgress<float> progress, int timeout, bool isModelCallback)
        {
            if (url == "")
            {
                if(isModelCallback)
                    ModelCallback?.Invoke(0);
                return null;
            }

            string pathurl = url;
            if (getParams != null)
            {
                string urlparam = StringTools.BuildQueryString(getParams);
                if (!string.IsNullOrEmpty(urlparam))
                {
                    pathurl = url + "?" + urlparam;
                }
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
                
                UnityWebRequest webRequest = new UnityWebRequest(pathurl, "POST");
                // 将 JSON 数据作为请求体
                byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(paramsJson);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                foreach (var head in headParams)
                {
                    webRequest.SetRequestHeader(head.Key,head.Value);
                }
                byte[] bytes = await requestData(webRequest, isModelCallback, progress);
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

        private async UniTask<byte[]> requestData(UnityWebRequest webRequest, bool isModelCallback, IProgress<float> progress = null)
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
                    
                    // Report progress if progress callback is provided
                    if (progress != null)
                    {
                        progress.Report(webRequest.downloadProgress);
                    }

                    await UniTask.Yield();
                }

                if (!string.IsNullOrEmpty(webRequest.error))
                {
                    if(isModelCallback)
                        ModelCallback?.Invoke(-1);
                    throw new Exception($"you use Web Request Error.{webRequest.url} Message{webRequest.error}");
                }

                // Report 100% completion if progress callback is provided
                if (progress != null)
                {
                    progress.Report(1.0f);
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
                throw new Exception($"Remote Request Error.{webRequest.url} Message{err.Message}");
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