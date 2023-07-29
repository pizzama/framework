using System.Runtime.InteropServices.ComTypes;
using System.IO.Compression;
using System;
using System.Collections.Generic;
using UnityEngine;
using game;
using static UnityEditor.Rendering.FilterWindow;
using Unity.VisualScripting;

namespace SFramework
{
    public class BundleManager : MonoBehaviour, IManager
    {
        private static BundleManager _instance;
        public static BundleManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<BundleManager>();
                if (_instance != null) return _instance;
                var obj = new GameObject();
                obj.name = "BundleManager";
                _instance = obj.AddComponent<BundleManager>();
                _instance.init();
                return _instance;
            }
        }

        private List<BundleParams> _messageParams; //通知的消息队列
        private List<BundleParams> _openSequenceParams; //执行打开操作的消息队列
        private SMemory<string, string, IBundle> _bundleMap; //已经启动了的所有模块的管理器
        private Dictionary<string, List<IBundle>> _bundleObserverMap; //注册消息管理器


        protected virtual void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (_instance == null)
            {
                _instance = this as BundleManager;
                _instance.init();
                DontDestroyOnLoad(transform.gameObject);
            }
            else
            {
                if (this != _instance)
                {
                    Destroy(this.gameObject);
                }
            }
        }

        private void Update()
        {
            if (_bundleMap == null)
                return;
            foreach (KeyValuePair<string, Dictionary<string, IBundle>> result in _bundleMap)
            {
                foreach (KeyValuePair<string, IBundle> bundle in result.Value)
                {
                    bundle.Value.Update();
                }
            }
        }

        private void FixUpdate()
        {
            if (_bundleMap == null)
                return;
            foreach (KeyValuePair<string, Dictionary<string, IBundle>> result in _bundleMap)
            {
                foreach (KeyValuePair<string, IBundle> bundle in result.Value)
                {
                    bundle.Value.FixUpdate();
                }
            }
        }

        private void LateUpdate()
        {
            if (_bundleMap == null)
                return;
            foreach (KeyValuePair<string, Dictionary<string, IBundle>> result in _bundleMap)
            {
                foreach (KeyValuePair<string, IBundle> bundle in result.Value)
                {
                    bundle.Value.LateUpdate();
                }
            }

            handleMessageParams();
        }
        private void init()
        {
            _bundleMap = new SMemory<string, string, IBundle>();
            _messageParams = new List<BundleParams>();
            _openSequenceParams = new List<BundleParams>();
            _bundleObserverMap = new Dictionary<string, List<IBundle>>();
        }

        public IBundle GetBundle(string name, string alias)
        {
            IBundle value = _bundleMap.GetValue(name, alias);
            return value;
        }

        public IBundle AddBundle(IBundle bundle, string alias)
        {
            if (bundle == null)
                throw new NotFoundException("bundle name not be null");
            string fullName;
            string className;
            string nameSpace;
            bundle.GetBundleName(out fullName, out nameSpace, out className);
            if (alias == "")
                alias = className;
            bundle.AliasName = alias;
            _bundleMap.SetValue(fullName, alias, bundle);
            bundle.Manager = this;
            return bundle;
        }

        public IBundle DeleteBundle(string name, string alias)
        {
            return _bundleMap.DeleteValue(name, alias);
        }

        public IBundle DeleteBundle(IBundle bundle)
        {
            string fullName;
            string className;
            string nameSpace;
            bundle.GetBundleName(out fullName, out nameSpace, out className);

            return DeleteBundle(fullName, bundle.AliasName);
        }

        public void InstallBundle(IBundle bundle, string alias = "", bool withOpen = false)
        {
            try
            {
                IBundle value = AddBundle(bundle, alias);
                value.Install();
                if (withOpen)
                    value.Open();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.ToString());
            }
        }

        public void UninstallBundle(string name, string alias)
        {
            IBundle value = DeleteBundle(name, alias);
            if (value != null)
                value.Uninstall();
        }

        public void UninstallBundle(IBundle bundle)
        {
            string fullName;
            string className;
            string nameSpace;
            bundle.GetBundleName(out fullName, out nameSpace, out className);
            UninstallBundle(fullName, bundle.AliasName);
        }

        // classPath is nameSpace + className
        public void OpenControl(string nameSpace, string className, object messageData = null, bool isSequence = false, string alias = "", int sort = 0)
        {
            BundleParams bdParams = new BundleParams()
            {
                MessageId = "$#$", //特殊id表示打开界面的消息
                NameSpace = nameSpace,
                ClassName = className,
                MessageData = messageData,
                Alias = alias,
                MessageSender = this,
                Sort = 0,
            };

            if (isSequence)
            {
                bdParams.OpenType = OpenType.Sequence;
                AddOpenParams(bdParams);
            }
            else
            {
                OpenControl(bdParams);
            }
        }

        public void OpenControl(BundleParams value)
        {
            BundleManager manager = BundleManager.Instance;
            IBundle bd = manager.GetBundle(value.ClassPath, value.Alias);
            if (bd == null)
            {
                SControl ctl = ObjectTools.CreateInstance<SControl>(value.NameSpace, value.ClassName);
                if (ctl == null)
                    throw new NotFoundException($"class {value.NameSpace}.{value.ClassName} is miss!");
                InstallBundle(ctl, value.Alias);
                ctl.Open(value);
            }
            else
            {
                if (!bd.IsOpen)
                {
                    bd.Open(value);
                }
            }
        }
        public T GetControl<T>() where T : IBundle
        {
            T result = _bundleMap.GetValue<T>();
            return result;

        }

        public void AddMessageParams(BundleParams value)
        {
            _messageParams.Add(value);
        }

        public void AddOpenParams(BundleParams value)
        {
            _openSequenceParams.Add(value);
        }

        private void handleMessageParams()
        {
            for (int i = 0; i < _messageParams.Count; i++)
            {
                BundleParams pa = _messageParams[i];
                _messageParams.Remove(pa);
                i--;

                IBundle control = BundleManager.Instance.GetBundle(pa.ClassPath, pa.Alias);
                if (control != null)
                    control.HandleMessage(pa);
                else 
                {
                    //如果不是指向的消息则广播给所有注册的用户
                    if (_bundleObserverMap.ContainsKey(pa.MessageId))
                    {
                        List<IBundle> bundles = _bundleObserverMap[pa.MessageId];
                        for (int j = 0; j < bundles.Count; j++)
                        {
                            control = bundles[j];
                            control.HandleMessage(pa);    
                        }
                    }
                }
            }
        }

        public BundleParams? PopUpOpenParams()
        {
            if (_openSequenceParams.Count > 0)
            {
                BundleParams value = _openSequenceParams[0];
                _openSequenceParams.RemoveAt(0);
                return value;
            }

            return null;
        }

        public void SubscribeMessage(string messageId, IBundle bundle)
        {

            if (!_bundleObserverMap.ContainsKey(messageId))
            {
                _bundleObserverMap[messageId] = new List<IBundle>();
            }

            if (!_bundleObserverMap[messageId].Contains(bundle))
            {
                _bundleObserverMap[messageId].Add(bundle);
            }
        }

        public void UnSubscribeMessage(string messageId, IBundle bundle)
        {
            if (_bundleObserverMap.ContainsKey(messageId))
            {
                _bundleObserverMap[messageId].RemoveAll((value) => {
                    return value == bundle;
                });
            }
        }
    }
}