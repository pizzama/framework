using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
/*
    File Tools
    //test
    //    StartCoroutine(FileManager.LoadXML(FilePathDef.configPath, test));
    //void test()
    //{
    //    XmlDocument configXml = FileManager.getXmlDocument(FilePathDef.configPath);
    //    string sever_ip = XmlManager.readXmlValueBykey(configXml, "sever_ip");
    //    int post = int.Parse(XmlManager.readXmlValueBykey(configXml, "post"));
    //    Singleton<TcpClient>.getSingleton().addListener(this);
    //    Singleton<TcpClient>.getSingleton().connect(sever_ip, post);
    //    m_path = sever_ip + post;
    //}
*/

namespace PFramework
{
    public class FileTools
    {

        public delegate void CommonPopDelegate();
        //pc
        private static string preSavePath = Application.persistentDataPath + "/";

        public static Dictionary<string, XmlDocument> xmlDic = new Dictionary<string, XmlDocument>();

        /// <summary>
        /// 把文件写到文档目录存起来
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="info">Info.</param>
        /// <param name="length">Length.</param>
        public static void saveFile(string path, byte[] info)
        {
            path = preSavePath + path;
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
            path = preSavePath + path;
            //路径、文件名、写入内容
            StreamWriter sw;
            FileInfo fi = new FileInfo(path);
            sw = fi.CreateText();        //直接重新写入，如果要在原文件后面追加内容，应用fi.AppendText()
            sw.WriteLine(info);
            sw.Close();
            sw.Dispose();
        }


        public static string LoadFileAndIntialSpeed(string path)
        {
            string retStr = "";
            path = preSavePath + path;

            if (!File.Exists(path)) return retStr;

            StreamReader sr = null;
            sr = File.OpenText(path);

            string t_Line;

            if ((t_Line = sr.ReadLine()) != null)
            {
                retStr += t_Line;
                //do some thing with t_sLine

            }
            else
            {

            }

            sr.Close();
            sr.Dispose();
            return retStr;
        }


        /// <summary>
        /// 把文件写到文档目录存起来
        /// </summary>
        /// <param name="path">Path.</param>
        /// <param name="info">Info.</param>
        /// <param name="length">Length.</param>
        public static void LoadFile(string path, byte[] info)
        {
            path = preSavePath + path;
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
        public static void saveFileByString(string path, string info)
        {
            byte[] arr = Encoding.UTF8.GetBytes(info);
            saveFile(path, arr);
        }
        /// <summary>
        /// 从io本地文件读取数据,可读取xml,txt文件
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="path">Path.</param>
        public static string getFile(string path)
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
        /// <summary>
        /// 保存 xml.
        /// </summary>
        /// <param name="path">Path.保存路径</param>
        /// <param name="doc">Document.</param>
        public static void saveXml(string path, XmlDocument doc)
        {
            doc.Save(path);
        }
        /// <summary>
        /// 获得xml
        /// </summary>
        /// <returns>The xml.</returns>
        /// <param name="path">Path.</param>
        public static XmlDocument getXml(string path)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(new XmlTextReader(path));
            return xml;
        }

        /// <summary>
        /// www加载xml文件
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static IEnumerator LoadXML(string localPath)
        {
            localPath = localPath.Trim();
            var str = System.Text.Encoding.UTF8.GetString(File.ReadAllBytes(localPath));
            saveXmlDocument(localPath, str);
            yield break;
            string newPath = "";

            if (Application.platform == RuntimePlatform.Android)
            {
                newPath = localPath;
            }
            else
            {
                newPath = "file://" + localPath;
            }

            WWW www = new WWW(newPath);
            /**
            while (!www.isDone)
            {
        **/
            yield return www;

            if (www.error != null)
            {
                Debug.LogError(www.url + "===============" + www.error);
            }
            saveXmlDocument(localPath, www.text);
            /**
        }
    **/
        }

        /// <summary>
        /// 获得xml
        /// </summary>
        /// <returns>The xml.</returns>
        /// <param name="path">Path.</param>
        private static XmlDocument saveXmlDocument(string localPath, string wwwtext)
        {
            XmlDocument xml = new XmlDocument();

            xml.LoadXml(wwwtext);

            xmlDic[localPath] = xml;

            return xml;
        }

        public static XmlDocument getXmlDocumentInDic(string localPath)
        {
            if (xmlDic.ContainsKey(localPath) && xmlDic[localPath] != null)
            {
                return xmlDic[localPath];
            }
            return null;
        }
    }
}