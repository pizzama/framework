using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFramework
{
    public class PMessage
    {
        public PMessage(string mid, object mdata, object msender = null)
        {
            MessageId = mid;
            MessageData = mdata;
            MessageSender = msender;
        }

        public string MessageId;
        public object MessageData;
        public object MessageSender;
    }
}