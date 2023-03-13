namespace PFramework
{
    public class BundleManager
    {
        private static BundleManager _instance;

        public static BundleManager Instance 
        {
            get 
            {
                if(_instance == null)
                {
                    _instance = new BundleManager();
                }
                return _instance;
            }
        }
    }
}