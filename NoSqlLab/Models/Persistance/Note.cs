using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoSqlLab.Models.Persistance
{
    public class Note
    {
        [BsonId]
        public Guid Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("text")]
        public string Text { get; set;}
        [BsonElement("userId")]
        public Guid UserId { get; set; }
        [BsonElement("updateTime")]
        public DateTime UpdateTime { get; set; }
    }
}
