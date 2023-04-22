using UnityEngine;

namespace PFramework
{
    public class RootUIControl : PControl
    {
        public override void Install()
        {
            base.Install();
            initUI();
        }

        private void initUI()
        {
            const string uiname = "PUIROOT";
            bool hasUI = GameObject.Find(uiname);
            if (!hasUI)
            {
                var uirootPrefab = Resources.Load<GameObject>(uiname);
                if(!uirootPrefab)
                {
                    throw new NotFoundException(uiname);
                }
                var uiroot = Object.Instantiate(uirootPrefab);
                Object.DontDestroyOnLoad(uiroot);
            }
        }
    }
}