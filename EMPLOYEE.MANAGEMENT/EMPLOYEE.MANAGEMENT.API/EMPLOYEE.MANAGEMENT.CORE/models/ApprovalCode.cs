using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EMPLOYEE.MANAGEMENT.CORE.models
{
    public class ApprovalCode
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("code")]
        public string Code { get; set; } = string.Empty; // simple numeric/string code

        [BsonElement("role")]
        public string Role { get; set; } = "Moderator";

        [BsonElement("expiresAt")]
        public DateTime ExpiresAt { get; set; }

        [BsonElement("usedAt")]
        public DateTime? UsedAt { get; set; }

        [BsonElement("issuedBy")]
        public string IssuedBy { get; set; } = string.Empty;
    }
}


