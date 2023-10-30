using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFramework
{
    public interface ISManager
    {
        ISBundle GetBundle(string name, string alias);
        ISBundle AddBundle(ISBundle bundle, string alias);
        ISBundle DeleteBundle(string name, string alias);
        ISBundle DeleteBundle(ISBundle bundle);
        void InstallBundle(ISBundle bundle, string alias, bool withOpen);
        void UninstallBundle(string name, string alias);
        void UninstallBundle(ISBundle bundle);
        void OpenControl(string fullPath, object messageData, bool isSequence, string alias = "", int sort = 0);
        void CloseControl(string fullPath, string alias = "");
        void CloseAllControl(List<ISBundle> excludeBundles);
        void SubscribeMessage(string messageId, ISBundle bundle);
        void UnSubscribeMessage(string messageId, ISBundle bundle);
    }
}