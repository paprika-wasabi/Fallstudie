using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AOT.Models
{
    public class Project
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("budget")]
        public string Budget { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("projectleader")]
        public string Leader { get; set; }

        [BsonElement("department")]
        public string Department { get; set; }

        [BsonElement("projecttype")]
        public string Type { get; set; }

        [BsonElement("member")]
        public string Member { get; set; }
    }
}
