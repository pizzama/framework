using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework
{
    public sealed class ABInfo : System.IDisposable
    {
        private string _hashName;
        public string HashName
        {
            get { return _hashName; }
            set { _hashName = value; }
        }
        private AssetBundleCreateRequest _request;
        private bool _isLoad;

        private AssetBundle _assetBundle;

        private int _refCount;

        public int RefCount { get { return _refCount; } set { _refCount = value; } }

        public ABInfo()
        {
            RefCount = 0;
            _depends = new List<ABInfo>();
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

        public bool Release()
        {
            if (RefCount <= 0)
            {
                if (AssetBundle != null)
                {
                    AssetBundle.Unload(true);
                }

                Dispose();

                return true;
            }

            return false;
        }

        private List<ABInfo> _depends;

        public List<ABInfo> Depends
        {
            get { return _depends; }
            set { _depends = value; }
        }

        public void AddDepends(ABInfo info)
        {
            if (!_depends.Contains(info))
            {
                _depends.Add(info);
            }
        }

        public void Unload(bool value = false)
        {
            DelRef();
            AssetBundle.Unload(value);
        }

        public void AddRef()
        {
            RefCount++;
        }

        public void DelRef()
        {
            RefCount--;
        }

        public void Dispose()
        {
        }
    }
}

