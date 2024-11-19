using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Presensi360.Models
{
    public class AppLogModel
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        [BsonElement("Source")]
        public string? Source { get; set; }
        [BsonElement("Params")]
        public string? Params { get; set; }
        [BsonElement("CreatedBy")]
        public string? CreatedBy { get; set; }
        [BsonElement("CreatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? CreatedAt { get; set; }
    }
}
