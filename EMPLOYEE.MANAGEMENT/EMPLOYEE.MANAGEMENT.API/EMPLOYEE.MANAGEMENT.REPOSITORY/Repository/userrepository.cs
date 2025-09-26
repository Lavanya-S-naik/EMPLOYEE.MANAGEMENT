using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EMPLOYEE.MANAGEMENT.CORE.models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace EMPLOYEE.MANAGEMENT.REPOSITORY.Repository
{
    public class UserRepository
    {
        private readonly IMongoCollection<UserData> users;
        private readonly UserRepository _userRepository;


        public UserRepository(IMongoClient mongoClient, IEmployeeStoreDB settings)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            users = database.GetCollection<UserData>("user_data");
        }
        public async Task<UserData> GetByUsernameAsync(string username)
        {
            return await users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }


        public async Task<bool> RegisterUserAsync(UserData newUser)
        {
            var existing = await users.Find(u => u.Username == newUser.Username).FirstOrDefaultAsync();
            if (existing != null) return false; // Username already exists
            await users.InsertOneAsync(newUser);
            return true;
        }

        public async Task<bool> UpdateUserRoleAsync(string username, string newRole)
        {
            var update = Builders<UserData>.Update.Set(u => u.Role, newRole);
            var result = await users.UpdateOneAsync(u => u.Username == username, update);
            return result.MatchedCount > 0 && result.ModifiedCount > 0;
        }

    }
}

