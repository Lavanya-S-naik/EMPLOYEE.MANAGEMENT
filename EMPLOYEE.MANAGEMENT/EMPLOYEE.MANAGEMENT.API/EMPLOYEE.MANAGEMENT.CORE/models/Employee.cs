

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;


namespace EMPLOYEE.MANAGEMENT.CORE.models;
/// <summary>
/// Represents an employee entity stored in MongoDB.
/// </summary>
[BsonIgnoreExtraElements]
public class Employee
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    /// <summary>
    /// Gets or sets the unique identifier for the employee.
    /// </summary>
    public string Id { get; set; } = string.Empty;   // maps to _id in MongoDB

    [BsonElement("name")]
    /// <summary>
    /// Gets or sets the employee's full name.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [BsonElement("department")]
    /// <summary>
    /// Gets or sets the department in which the employee works.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [BsonElement("position")]
    /// <summary>
    /// Gets or sets the employee's job title or position.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Position { get; set; } = string.Empty;

    [BsonElement("email")]
    /// <summary>
    /// Gets or sets the employee's email address.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [BsonElement("phone")]
    /// <summary>
    /// Gets or sets the employee's phone number.
    /// </summary>
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [BsonElement("salary")]
    /// <summary>
    /// Gets or sets the employee's salary.
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }

    [BsonElement("dateOfJoining")]
    /// <summary>
    /// Gets or sets the date the employee joined the organization.
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime DateOfJoining { get; set; }

    [BsonElement("isActive")]
    /// <summary>
    /// Gets or sets a value indicating whether the employee is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Returns a string that represents the current employee.
    /// </summary>
    /// <returns>A string representation of the employee.</returns>
    public override string ToString()
    {
        return $"Employee: Id={Id}, Name={Name}, Department={Department}, Position={Position}, Email={Email}, Phone={Phone}, Salary={Salary}, DateOfJoining={DateOfJoining}, IsActive={IsActive}";
    }

}
