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
        public string Leader;
        public string Department;
        public string ProjectType;
        public string ProjectStatus;
        public enum MessageType
        {
            Search,
            RefreshUI,
            Error
        }
        public Message()
        {

        }

        public Message(MessageType type, string minBudget, string maxBudget, string name, bool isPflicht, string leader, string department, string projectType, string projectStatus)
        {
            MinBudget = minBudget;
            MaxBudget = maxBudget;
            Type = type;
            Name = name;
            IsPflicht = isPflicht;
            Leader = leader;
            Department = department;
            ProjectType = projectType;
            ProjectStatus = projectStatus;

        }
    }
}
