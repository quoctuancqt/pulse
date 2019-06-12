namespace Pulse.Core.Entity
{
    using Common.Helpers;
    using Domain;
    using Domain.Entity;
    using Domain.Enum;
    using Microsoft.AspNet.Identity;
    using Security.Identity.IdentityModels;
    using Settings;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Connection.Entity.PulseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Connection.Entity.PulseContext context)
        {
            base.Seed(context);

            PasswordHasher passwordHasher = new PasswordHasher();
            //Fake data
            var roleAdmin = context.Roles.Where(r => r.Name.Equals(PulseIdentityRole.Administrator)).FirstOrDefault();

            if (roleAdmin == null)
            {
                context.Roles.Add(new PulseIdentityRole { Name = PulseIdentityRole.Administrator });
                context.SaveChanges();
            }

            var clientAdmin = context.Roles.Where(r => r.Name.Equals(PulseIdentityRole.ClientAdmin)).FirstOrDefault();

            if (clientAdmin == null)
            {
                context.Roles.Add(new PulseIdentityRole { Name = PulseIdentityRole.ClientAdmin });
            }

            var roleKiosk = context.Roles.Where(r => r.Name.Equals(PulseIdentityRole.Kiosk)).FirstOrDefault();

            if (roleKiosk == null)
            {
                context.Roles.Add(new PulseIdentityRole { Name = PulseIdentityRole.Kiosk });
            }

            var roleUser = context.Roles.Where(r => r.Name.Equals(PulseIdentityRole.User)).FirstOrDefault();

            if (roleUser == null)
            {
                context.Roles.Add(new PulseIdentityRole { Name = PulseIdentityRole.User });
            }

            var user = context.Users.Where(u => u.UserName.Equals("admin")).FirstOrDefault();

            var clientId = UnitHelper.GenerateNewGuid();

            string secretKey = UnitHelper.GenerateNewGuid();

            if (user == null)
            {
                user = new PulseIdentityUser
                {
                    UserName = "admin",
                    Email = "pinga.lau@tekcent.com",
                    PasswordHash = passwordHasher.HashPassword("tekcent"),
                    SecurityStamp = passwordHasher.HashPassword("tekcent"),
                    ClientId = clientId,
                    SecretKey = secretKey
                };

                context.Users.Add(user);

                context.SaveChanges();

                roleAdmin = context.Roles.Where(r => r.Name.Equals(PulseIdentityRole.Administrator)).FirstOrDefault();

                context.Users.Where(u => u.UserName.Equals("admin")).FirstOrDefault()
                    .Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole
                    {
                        UserId = user.Id,
                        RoleId = roleAdmin.Id
                    });

                context.SaveChanges();

                var userProfile = new UserProfile
                {
                    FirstName = "Tekcent",
                    LastName = "Vietnam",
                    Gender = Gender.Male,
                    Email = user.Email,
                    UserId = user.Id,
                    Password = "tekcent",
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin",
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = "Admin"
                };

                context.UserProfiles.Add(userProfile);
            }

            var client = context.Clients.Where(u => u.Name.Equals("Tekcent")).FirstOrDefault();

            if (client == null)
            {
                context.Clients.Add(new Client
                {
                    Name = "TekCent",
                    Secret = passwordHasher.HashPassword(secretKey),
                    SecretKey = secretKey,
                    AllowedGrant = OAuthGrant.SystemAdmin,
                    Active = true,
                    RefreshTokenLifeTime = 7,
                    TokenLifeTime = 20,
                    ClientId = clientId,
                    SignalrUrl = "http://localhost:9090",
                    MongoName = "PulseTekcent",
                    MongoConnectionString = "mongodb://localhost:27017/PulseTekcent"
                });
            }

            var groups = context.Groups.Where(g => g.Name.Equals("Unknow")).FirstOrDefault();

            if (groups == null)
            {
                context.Groups.Add(new Group
                {
                    Name = "Unknow",
                });
            }

            context.SaveChanges();


        }
    }
}
