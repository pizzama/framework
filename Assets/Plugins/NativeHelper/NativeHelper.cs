using UnityEngine;

namespace NativeHelper
{
    public interface INativeHelper
    {
        void Alert(string content);

        void SyncDB();
    }

    public class NativeHelper: INativeHelper
    {
        public virtual void Alert(string content)
        {
            Debug.Log("Alert: " + content);
        }

        public virtual void SyncDB()
        {
            Debug.Log("SyncDB data!");
        }
    }
}
