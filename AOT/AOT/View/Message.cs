using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOT.View
{
    public class Message
    {
        public MessageType Type;
        public enum MessageType
        {
            RefreshUI,
            Error
        }
    }
}
