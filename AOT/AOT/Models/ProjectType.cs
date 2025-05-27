using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AOT.Models
{
    public class ProjectType
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("projektart")]
        public string Type {  get; set; }

        [BsonElement("kategorie")]
        public string Category { get; set; }

        [BsonElement("beschreibung")]
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Type}";
        }

    }
}
