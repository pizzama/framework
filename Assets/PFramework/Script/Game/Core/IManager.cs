using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFramework
{
    public interface IManager
    {
        IBundle GetBundle(string name, string alias);
        IBundle AddBundle(IBundle bundle, string alias);
        IBundle DeleteBundle(string name, string alias);
        IBundle DeleteBundle(IBundle bundle, string alias);
        void InstallBundle(IBundle bundle, string alias);
        void UninstallBundle(string name, string alias);
        void UninstallBundle(IBundle bundle, string alias);
        void OpenControl(string classpath, string alias = "", params object[] paramss);
    }
}