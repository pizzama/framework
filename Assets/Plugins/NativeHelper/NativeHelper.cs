using UnityEngine;

namespace NativeHelper
{
    public interface INativeHelper
    {
        void Alert(string content);
    }

    public class NativeHelper: INativeHelper
    {
        public virtual void Alert(string content)
        {
            Debug.Log("Alert: " + content);
        }
    }
}
