namespace PFrameWork
{
    public sealed class ABInfo:System.IDisposable
    {
        private ABManifest _manifest;
        private AssetBundle _assetBundle;
#if UNITY_EDITOR
        public long TotalSize { get; private set; }
#endif

        public int RefCount { get; set; }

        public ABInfo()
        {
            RefCount = 0;
        }

        public AssetBundle AssetBundle
        {
            get => _assetBundle;
            set
            {
                if (value == null)
                {
                    return;
                }

                _assetBundle = value;
            }
        }

        public void Dispose()
        {
            if (AssetBundle != null)
            {
                AssetBundle.Unload(true);
            }

            RefCount = 0;
#if UNITY_EDITOR
            TotalSize = 0;
#endif
        }

#if UNITY_EDITOR
        public void SetAssetsSize(Dictionary<string, Dictionary<Type,Object>> assets)
        {
            if (assets == null)
            {
                return;
            }
            foreach (var asset in assets.Values)
            {
                foreach (var kPair in asset)
                {
                    TotalSize += UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(kPair.Value);
                }
                
            }
        }
#endif


    }
}

