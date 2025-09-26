using EMPLOYEE.MANAGEMENT.CORE.models;
using MongoDB.Driver;

namespace EMPLOYEE.MANAGEMENT.REPOSITORY.Repository
{
    public class ApprovalCodeRepository
    {
        private readonly IMongoCollection<ApprovalCode> codes;

        public ApprovalCodeRepository(IMongoClient mongoClient, IEmployeeStoreDB settings)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            codes = database.GetCollection<ApprovalCode>("approval_codes");
        }

        public async Task<string> CreateAsync(string role, DateTime expiresAt, string issuedBy, string code)
        {
            var doc = new ApprovalCode
            {
                Code = code,
                Role = role,
                ExpiresAt = expiresAt,
                IssuedBy = issuedBy,
                UsedAt = null
            };
            await codes.InsertOneAsync(doc);
            return doc.Id;
        }

        public async Task<bool> ValidateAndConsumeAsync(string code, string role)
        {
            var now = DateTime.UtcNow;
            var filter = Builders<ApprovalCode>.Filter.Where(c => c.Code == code && c.Role == role && c.UsedAt == null && c.ExpiresAt > now);
            var update = Builders<ApprovalCode>.Update.Set(c => c.UsedAt, now);
            var result = await codes.UpdateOneAsync(filter, update);
            return result.MatchedCount > 0 && result.ModifiedCount > 0;
        }
    }
}


