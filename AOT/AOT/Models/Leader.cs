using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOT.Models
{
    public class Leader
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("vorname")]
        public string Name { get; set; }

        [BsonElement("nachname")]
        public string LastName { get; set; }

        [BsonElement("abteilungs_id")]
        public int DepartmentId { get; set; }

        public override string ToString()
        {
            return $"{Name} {LastName}";
        }
    }
}
