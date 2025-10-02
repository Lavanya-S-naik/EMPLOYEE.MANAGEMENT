using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EMPLOYEE.MANAGEMENT.CORE.models
{
    public class UserData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("username")]
        public string Username { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        
        // Support both old 'role' field and new 'roles' field
        [BsonElement("role")]
        public string? Role { get; set; }
        
        [BsonElement("roles")]
        public List<string> Roles { get; set; } = new List<string>();
    }
}

