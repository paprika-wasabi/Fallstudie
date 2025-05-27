using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOT.Models
{
    public class Department
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("abteilungs_id")]
        public int DepartmentId { get; set; }

        [BsonElement("abteilung")]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
