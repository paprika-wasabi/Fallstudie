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
        public string MinBudget;
        public string MaxBudget;
        public string Name;
        public bool IsPflicht;
        public enum MessageType
        {
            Search,
            RefreshUI,
            Error
        }
        public Message()
        {

        }

        public Message(MessageType type, string minBudget, string maxBudget, string name, bool isPflicht)
        {
            MinBudget = minBudget;
            MaxBudget = maxBudget;
            Type = type;
            Name = name;
            IsPflicht = isPflicht;
        }
    }
}
