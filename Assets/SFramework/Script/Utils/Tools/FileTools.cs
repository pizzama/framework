using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace SFramework.Tools
{
    public class FileTools
    {
        /// <summary>
        /// 把文件写到文档目录存起来
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="info">Info.</param>
        /// <param name="length">Length.</param>
        public static void SaveFile(string path, byte[] info)
        {
            int length = info.Length;
            Stream sw;
            FileInfo file = new FileInfo(path);
            try
            {
                if (file.Exists)
                {
                    file.Delete();
                }
                sw = file.Create();
                sw.Write(info, 0, length);
                sw.Close();//关闭流
                sw.Dispose();//销毁流
            }
            catch (Exception e)
            {
                Debug.LogError("saveFile failed:" + e.ToString());
            }
        }

        public static void CreateOrOPenFile(string path, string info)
        {
            //路径、文件名、写入内容
            StreamWriter sw;
            FileInfo fi = new FileInfo(path);
            sw = fi.CreateText();        //直接重新写入，如果要在原文件后面追加内容，应用fi.AppendText()
            sw.WriteLine(info);
            sw.Close();
            sw.Dispose();
        }

        public static byte[] ReadFile(string path)
        {
            byte[] bytes;
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                fs.Close();
                fs.Dispose();
            }

            return bytes;
        }

        public static async UniTask<byte[]> ReadFileAsync(string path)
        {
            try
            {
                UnityWebRequest webRequest = UnityWebRequest.Get(path);
                await webRequest.SendWebRequest();
                while (!webRequest.isDone)
                {
                    if (webRequest.result == UnityWebRequest.Result.ProtocolError)
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

        /// <summary>
        /// md5加密方法
        /// </summary>
        /// <param name="pwd">传入需要加密的字符串</param>
        /// <returns>返回加密后的md5值</returns>    
        public static string GetMD5String(byte[] file)
        {
            MD5 md5 = MD5.Create();
            StringBuilder sb = new StringBuilder();
            byte[] buffer = md5.ComputeHash(file);
            foreach (byte b in buffer)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static long GetLength(string path)
        {
            System.IO.FileInfo _fileInfo = new System.IO.FileInfo(path);
            return _fileInfo.Length;
        }
        /// <summary>
        /// 通过字符串来存储文件
        /// </summary>
        /// <param name="path">Path.存储路径</param>
        /// <param name="info">Info.存储内容</param>
        public static void SaveFileByString(string path, string info)
        {
            byte[] arr = Encoding.UTF8.GetBytes(info);
            SaveFile(path, arr);
        }
        /// <summary>
        /// 从io本地文件读取数据,可读取xml,txt文件
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="path">Path.</param>
        public static string ReadFileToString(string path)
        {
            string result = "";
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(path);
            }
            catch (Exception e)
            {
                Debug.LogError("getFile 找不到 path=" + path + ",异常是" + e.ToString());
                return null;
            }
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                result += line;
            }

            sr.Close();//关闭流
            sr.Dispose();//销毁流，

            return result;
        }
        public static void deleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}