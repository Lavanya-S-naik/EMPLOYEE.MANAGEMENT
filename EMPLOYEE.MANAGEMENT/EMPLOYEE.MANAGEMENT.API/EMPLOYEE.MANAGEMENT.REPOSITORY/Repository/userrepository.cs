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

        public async Task<bool> AddRoleToUserAsync(string username, string newRole)
        {
            var user = await users.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (user == null) return false;

            // Initialize roles list if it's empty (for old users)
            if (user.Roles == null || !user.Roles.Any())
            {
                user.Roles = new List<string>();
                // If user has old single role, add it to the list
                if (!string.IsNullOrEmpty(user.Role))
                {
                    user.Roles.Add(user.Role);
                }
            }

            if (!user.Roles.Contains(newRole))
            {
                user.Roles.Add(newRole);
                var update = Builders<UserData>.Update.Set(u => u.Roles, user.Roles);
                var result = await users.UpdateOneAsync(u => u.Username == username, update);
                return result.MatchedCount > 0 && result.ModifiedCount > 0;
            }
            return true; // Role already exists
        }

        public async Task<bool> RemoveRoleFromUserAsync(string username, string roleToRemove)
        {
            var user = await users.Find(u => u.Username == username).FirstOrDefaultAsync();
            if (user == null) return false;

            if (user.Roles.Contains(roleToRemove))
            {
                user.Roles.Remove(roleToRemove);
                if (user.Roles.Count == 0)
                {
                    return false; // Cannot remove all roles
                }
                var update = Builders<UserData>.Update.Set(u => u.Roles, user.Roles);
                var result = await users.UpdateOneAsync(u => u.Username == username, update);
                return result.MatchedCount > 0 && result.ModifiedCount > 0;
            }
            return true; // Role doesn't exist
        }




        public async Task<bool> UpdateUserAsync(UserData user)
        {
            var update = Builders<UserData>.Update
                .Set(u => u.Password, user.Password)
                .Set(u => u.Roles, user.Roles);
            // Add other fields as needed

            var result = await users.UpdateOneAsync(
                u => u.Username == user.Username,
                update
            );

            return result.MatchedCount > 0 && result.ModifiedCount > 0;
        }


    }
}

