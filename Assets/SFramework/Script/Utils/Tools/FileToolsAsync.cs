using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace PFramework
{
    public partial class FileTools
    {
        public static async UniTask<byte[]> LoadFileAsync(string path)
        {
            try
            {
                UnityWebRequest webRequest = UnityWebRequest.Get(path);
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
                    throw new Exception($"you use Web Request Error.{path} Message{webRequest.error}");
                }

                var result = webRequest.downloadHandler.data;
                webRequest.Dispose();

                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"you use Web Request Error.{path} Message{e.Message}");
            }
        }
    }
}