﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOT.Models
{
    public class PdfFile
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("filename")]
        public string FileName { get; set; }

        [BsonElement("filedata")]
        public byte[] Data { get; set; }
    }
}
