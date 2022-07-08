using Dogstagram.WebApi.Data;
using Dogstagram.WebApi.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Dogstagram.WebApi.IntegrationTests
{
    public static class Utilities
    {
        public static void InitializeDbForTests(this DogstagramDbContext db)
        {
            var users = GetSeedingUsers();
            users[1].DeletedOn = DateTime.UtcNow;
            users[1].IsDeleted = true;

            db.Users.AddRange(users);
            var hashedPassword = new PasswordHasher<User>().HashPassword(users[0], "123456");
            users[0].SecurityStamp = Guid.NewGuid().ToString();
            users[0].PasswordHash = hashedPassword;

            hashedPassword = new PasswordHasher<User>().HashPassword(users[1], "n1k0lay");
            users[1].SecurityStamp = Guid.NewGuid().ToString();
            users[1].PasswordHash = hashedPassword;
            db.SaveChanges();

        }

        public static void ReinitializeDbForTests(this DogstagramDbContext db)
        {
            db.Users.RemoveRange(db.Users);
            InitializeDbForTests(db);
        }

        public static List<User> GetSeedingUsers()
        {
            return new List<User>()
            {
                new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "nikolay",
                    NormalizedUserName = "NIKOLAY",
                    Email = "nikolay@ndn.com",
                    NormalizedEmail = "NIKOLAY@NDN.COM",
                },
                new User
                {
                    Id= Guid.NewGuid().ToString(),
                    UserName="nikolay1",
                    NormalizedUserName="NIKOLAY!",
                    Email="nikolay1@ndn.com",
                    NormalizedEmail="NIKOLAY!@NDN.COM"
                }
            };
        }
    }
}
