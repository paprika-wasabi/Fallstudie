using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOT.Models
{
    public class Portfolio
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("portfolio_id")]

        public int PortfolioId { get; set; }

        [BsonElement("name")]

        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
