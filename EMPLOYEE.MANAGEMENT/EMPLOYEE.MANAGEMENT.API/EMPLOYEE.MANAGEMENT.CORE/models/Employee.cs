

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace EMPLOYEE.MANAGEMENT.CORE.models;
[BsonIgnoreExtraElements]
public class Employee
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;   // maps to _id in MongoDB

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("department")]
    public string Department { get; set; } = string.Empty;

    [BsonElement("position")]
    public string Position { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("phone")]
    public string Phone { get; set; } = string.Empty;

    [BsonElement("salary")]
    public decimal Salary { get; set; }

    [BsonElement("dateOfJoining")]
    public DateTime DateOfJoining { get; set; }

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;
}
