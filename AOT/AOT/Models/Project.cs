using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AOT.Models
{
    public class Project
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("projektnummer")]
        public string Projektnummer { get; set; }

        [BsonElement("projecttype")]
        public string Type { get; set; }

        [BsonElement("portfolioName")]
        public string PortfolioName { get; set; }

        [BsonElement("pflicht")]
        public string Pflicht { get; set; }

        [BsonElement("begründung_pflicht")]
        public string BegründungPflicht { get; set; }

        [BsonElement("ausgangslage")]
        public string Ausgangslage { get; set; }

        [BsonElement("projektziele")]
        public string Projektziele { get; set; }

        [BsonElement("abgrenzungen")]
        public string Abgrenzungen { get; set; }

        [BsonElement("meilensteine")]
        public string Meilensteine { get; set; }

        [BsonElement("termine")]
        public string Termine { get; set; }

        [BsonElement("personenaufwand_beschreibung")]
        public string Personenaufwand_Beschreibung { get; set; }

        [BsonElement("personenaufwand")]
        public string Personenaufwand { get; set; }

        [BsonElement("sachmittel_beschreibung")]
        public string Sachmittel_Beschreibung { get; set; }

        [BsonElement("sachmittel")]
        public string Sachmittel { get; set; }

        [BsonElement("budget")]
        public decimal Budget { get; set; }

        [BsonElement("auftraggeber")]
        public string Auftraggeber { get; set; }

        [BsonElement("projectleader")]
        public string Leader { get; set; }

        [BsonElement("department")]
        public string Department { get; set; }

        [BsonElement("member")]
        public string Member { get; set; }

        [BsonElement("stakeholder")]
        public string Stakeholder { get; set; }

        [BsonElement("verteiler")]
        public string Verteiler { get; set; }

        [BsonElement("kpi-score")]
        public float KPI { get; set; }

        [BsonElement("kpi-list")]
        public List<int> KPIList { get; set; }

        [BsonElement("creator")]
        public string Creator { get; set; }

        [BsonElement("date")]
        public string Date { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("pdfObjectId")]
        public ObjectId PdfObjectId { get; set; }
    }
}