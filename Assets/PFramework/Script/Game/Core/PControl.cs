using System;
using UnityEngine;

namespace PFramework
{
    public class PControl : IBundle
    {
        private PModel _model;
        private PView _view;
        public void Close()
        {
            throw new NotImplementedException();
        }

        public string GetBundleName()
        {
            Type classtype = this.GetType();
            return classtype.FullName;
        }

        public void Install()
        {

        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void Uninstall()
        {
            throw new NotImplementedException();
        }
    }
}