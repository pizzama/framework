using System;
using System.Collections.Generic;
using SFramework.Tools;
using UnityEngine;
using static SEnum;

namespace SFramework
{
    public class SBundleManager : MonoBehaviour, ISManager
    {
        private static SBundleManager _instance;
        public static SBundleManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                _instance = FindObjectOfType<SBundleManager>();
                if (_instance != null)
                    return _instance;
                var obj = new GameObject();
                obj.name = "BundleManager";
                _instance = obj.AddComponent<SBundleManager>();
                _instance.init();
                return _instance;
            }
        }

        private SBundleManager() { }

        private List<SBundleParams> _messageParams; //通知的消息队列
        private List<SBundleParams> _openSequenceParams; //执行打开操作的消息队列
        private SMemory<string, string, ISBundle> _bundleMap; //已经启动了的所有模块的管理器
        private Dictionary<string, List<ISBundle>> _bundleObserverMap; //注册消息管理器

        [SerializeField]
        private List<string> _bundleInspector;

        [SerializeField]
        private Performance _performance;

        protected virtual void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (_instance == null)
            {
                _instance = this as SBundleManager;
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
            foreach (KeyValuePair<string, Dictionary<string, ISBundle>> result in _bundleMap)
            {
                foreach (KeyValuePair<string, ISBundle> bundle in result.Value)
                {
                    bundle.Value.Update();
                }
            }
        }

        private void FixedUpdate()
        {
            if (_bundleMap == null)
                return;
            foreach (KeyValuePair<string, Dictionary<string, ISBundle>> result in _bundleMap)
            {
                foreach (KeyValuePair<string, ISBundle> bundle in result.Value)
                {
                    bundle.Value.FixedUpdate();
                }
            }
        }

        private void LateUpdate()
        {
            if (_bundleMap == null)
                return;
            //record bundel inspector
            if (_bundleInspector == null)
                _bundleInspector = new List<string>();
            else
                _bundleInspector.Clear();

            foreach (KeyValuePair<string, Dictionary<string, ISBundle>> result in _bundleMap)
            {
                foreach (KeyValuePair<string, ISBundle> bundle in result.Value)
                {
                    bundle.Value.LateUpdate();
                    _bundleInspector.Add(bundle.Value.AliasName + ":" + bundle.Value.IsOpen);
                }
            }

            handleMessageParams();
        }

        private void init()
        {
            _bundleMap = new SMemory<string, string, ISBundle>();
            _messageParams = new List<SBundleParams>();
            _openSequenceParams = new List<SBundleParams>();
            _bundleObserverMap = new Dictionary<string, List<ISBundle>>();
        }

        public ISBundle GetBundle(string name, string alias)
        {
            ISBundle value = _bundleMap.GetValue(name, alias);
            return value;
        }

        public ISBundle AddBundle(ISBundle bundle, string alias)
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

        public ISBundle DeleteBundle(string name, string alias)
        {
            ISBundle bundle = _bundleMap.DeleteValue(name, alias);
            return bundle;
        }

        public ISBundle DeleteBundle(ISBundle bundle)
        {
            string fullName;
            string className;
            string nameSpace;
            bundle.GetBundleName(out fullName, out nameSpace, out className);

            return DeleteBundle(fullName, bundle.AliasName);
        }

        public void InstallBundle(ISBundle bundle, string alias = "", bool withOpen = false)
        {
            try
            {
                ISBundle value = AddBundle(bundle, alias);
                value.Install();
                if (withOpen)
                    value.Open();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
                DeleteBundle(bundle);
            }
        }

        public void UninstallBundle(ISBundle bundle)
        {
            string fullName;
            string className;
            string nameSpace;
            bundle.GetBundleName(out fullName, out nameSpace, out className);
            UninstallBundle(fullName, bundle.AliasName);
        }

        public void UninstallBundle(string name, string alias)
        {
            ISBundle value = DeleteBundle(name, alias);
            if (value != null)
                value.Uninstall();
        }

        public void OpenControl(
            string fullPath,
            object messageData = null,
            bool isSequence = false,
            string alias = "",
            int sort = 0
        )
        {
            SBundleParams bdParams = new SBundleParams()
            {
                MessageId = "$#$", //This's a special id using opening message.
                ClassPath = fullPath,
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

        public void OpenControl(SBundleParams value)
        {
            SBundleManager manager = SBundleManager.Instance;
            ISBundle bd = manager.GetBundle(value.ClassPath, value.Alias);
            if (bd == null)
            {
                SControl ctl = ObjectTools.CreateInstance<SControl>(
                    value.NameSpace,
                    value.ClassName
                );
                if (ctl == null)
                    throw new NotFoundException(
                        $"class {value.NameSpace}.{value.ClassName} is miss!"
                    );
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

        public T GetControl<T>()
            where T : ISBundle
        {
            T result = _bundleMap.GetValue<T>();
            return result;
        }

        public void AddMessageParams(SBundleParams value)
        {
            _messageParams.Add(value);
        }

        public void AddOpenParams(SBundleParams value)
        {
            _openSequenceParams.Add(value);
        }

        private void handleMessageParams()
        {
            for (int i = 0; i < _messageParams.Count; i++)
            {
                SBundleParams pa = _messageParams[i];
                _messageParams.Remove(pa);
                i--;

                ISBundle control = SBundleManager.Instance.GetBundle(pa.ClassPath, pa.Alias);
                if (control != null && control.IsOpen)
                    control.HandleMessage(pa);
                else
                {
                    //如果不是指向的消息则广播给所有注册的用户
                    if (_bundleObserverMap.ContainsKey(pa.MessageId))
                    {
                        List<ISBundle> bundles = _bundleObserverMap[pa.MessageId];
                        for (int j = 0; j < bundles.Count; j++)
                        {
                            control = bundles[j];
                            if(control != null && control.IsOpen)
                            {
                                control.HandleMessage(pa);
                            }
                        }
                    }
                }
            }
        }

        public SBundleParams? PopUpOpenParams()
        {
            if (_openSequenceParams.Count > 0)
            {
                SBundleParams value = _openSequenceParams[0];
                _openSequenceParams.RemoveAt(0);
                return value;
            }

            return null;
        }

        public void SubscribeMessage(string messageId, ISBundle bundle)
        {
            if (!_bundleObserverMap.ContainsKey(messageId))
            {
                _bundleObserverMap[messageId] = new List<ISBundle>();
            }

            if (!_bundleObserverMap[messageId].Contains(bundle))
            {
                _bundleObserverMap[messageId].Add(bundle);
            }
        }

        public void UnSubscribeMessage(string messageId, ISBundle bundle)
        {
            if (_bundleObserverMap.ContainsKey(messageId))
            {
                _bundleObserverMap[messageId]
                    .RemoveAll(
                        (value) =>
                        {
                            return value == bundle;
                        }
                    );
            }
        }

        public void CloseAllControl(List<ISBundle> excludeBundles = default)
        {
            if (_bundleMap == null)
                return;
            foreach (KeyValuePair<string, Dictionary<string, ISBundle>> result in _bundleMap)
            {
                foreach (KeyValuePair<string, ISBundle> bundle in result.Value)
                {
                    if (excludeBundles != null && excludeBundles.Exists(t => bundle.Value == t))
                    {
                        continue;
                    }
                    else
                    {
                        bundle.Value.Close();
                    }
                }
            }
        }

        public void UninstallAllBundle(List<ISBundle> excludeBundles = default)
        {
            if (_bundleMap == null)
                return;
            //first close bundle
            CloseAllControl(excludeBundles);
            foreach (KeyValuePair<string, Dictionary<string, ISBundle>> result in _bundleMap)
            {
                List<ISBundle> bundles = new List<ISBundle>(result.Value.Values);
                for (int i = bundles.Count - 1; i >= 0; i--)
                {
                    ISBundle bundle = bundles[i];
                    if (excludeBundles != null && excludeBundles.Exists(t => bundle == t))
                    {
                        continue;
                    }
                    else
                    {
                        UninstallBundle(bundle);
                    }
                }
            }
        }

        private void OnApplicationQuit()
        {
            Destroy();
        }

        public void Destroy()
        {
            UninstallAllBundle();
            AssetsManager.Instance.Destroy();
            if (_bundleObserverMap != null)
                _bundleObserverMap.Clear();
            if (_bundleMap != null)
                _bundleMap.Clear();
            if (_messageParams != null)
                _messageParams.Clear();
            if (_bundleInspector != null)
                _bundleInspector.Clear();
            if (_openSequenceParams != null)
                _openSequenceParams.Clear();
            _instance = null;
        }

        public void CloseControl(string fullPath, string alias = "")
        {
            string nameSpace;
            string className;
            if (alias == string.Empty)
            {
                StringTools.PrefixClassName(fullPath, out nameSpace, out className);
                alias = className;
            }

            ISBundle bundle = GetBundle(fullPath, alias);
            bundle.Close();
        }

        public Tuple<int, Performance> GetPerformance()
        {
            int frame = GetFrameRate();
            return new Tuple<int, Performance>(frame, _performance);
        }

        private int GetFrameRate()
        {
            if (
                SystemInfo.systemMemorySize >= 4096
                && SystemInfo.processorFrequency >= 2048
                && SystemInfo.processorCount >= 4
            )
            {
                //"performance better";
                _performance = Performance.High;
                return 60;
            }
            else if (SystemInfo.systemMemorySize <= 2048)
            {
                _performance = Performance.Low;
                return 30;
            }
            else
            {
                _performance = Performance.Middle;
                return 30;
            }
        }
    }
}
