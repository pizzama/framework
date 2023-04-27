using UnityEngine;

namespace SFramework
{
    public class UIManager : MonoBehaviour
    {
        private static BundleManager _instance;
        public static BundleManager Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}