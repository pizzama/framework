using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFramework
{
    public interface IManager
    {
        IBundle GetBundle(string name, string alias);
        IBundle AddBundle(IBundle bundle, string alias);
        IBundle DeleteBundle(string name, string alias);
        IBundle DeleteBundle(IBundle bundle);
        void InstallBundle(IBundle bundle, string alias, bool withOpen);
        void UninstallBundle(string name, string alias);
        void UninstallBundle(IBundle bundle);
        void OpenControl(string fullPath, object messageData, bool isSequence, string alias = "", int sort = 0);
        void CloseControl(string fullPath, string alias = "");
        void CloseAllControl(List<IBundle> excludeBundles);
        void SubscribeMessage(string messageId, IBundle bundle);
        void UnSubscribeMessage(string messageId, IBundle bundle);
    }
}