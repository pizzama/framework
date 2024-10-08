namespace NativeHelper
{
    public class IOSNativeHelper : INativeHelper
    {
        public void Alert(string content)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
            Debug.Log("alert");
#endif
        }

        public void Save(byte[] bytes, string fileName)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
            string fileFullPath = GetApplicationPersistentDataPath() + "/" + fileName;

            FileStream stream = null;

            if (File.Exists(fileFullPath))
            {
                stream = new FileStream(fileFullPath, FileMode.Open);
            }
            else
            {
                stream = new FileStream(fileFullPath, FileMode.Create);
            }

            stream.Write(bytes);
            stream.Close();
#endif 
        }

        public byte[] Read(string fileName)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
            string fileFullPath = GetApplicationPersistentDataPath() + "/" + fileName;
            if (File.Exists(fileFullPath))
            {
                FileStream stream = new FileStream(fileFullPath, FileMode.Open);
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                stream.Close();
                return bytes;
            }
#endif    
            return null;
        }
        
        public void Delete(string fileName)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
            string fileFullPath = GetApplicationPersistentDataPath() + "/" + fileName;
            if (File.Exists(fileFullPath))
            {
                try
                {
                    File.Delete(fileFullPath);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
#endif   
        }

        public string GetApplicationPersistentDataPath()
        {
            throw new System.NotImplementedException();
        }
    }
}