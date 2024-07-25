using UnityEngine;

namespace NativeHelper
{
    public interface INativeHelper
    {
        void Alert(string content);

        void SyncDB();//special WebGl platform need data refresh 

        string GetApplicationPersistentDataPath();
    }

    public class NativeHelper : INativeHelper
    {
        public virtual void Alert(string content)
        {
            Debug.Log("Alert: " + content);
        }

        public virtual void SyncDB()
        {
            Debug.Log("SyncDB data!");
        }

        public virtual string GetApplicationPersistentDataPath()
        {
            return Application.persistentDataPath;
        }
    }
}
