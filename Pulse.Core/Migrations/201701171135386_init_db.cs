namespace Pulse.Core.Entity
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init_db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.String(),
                        Email = c.String(),
                        Secret = c.String(),
                        SecretKey = c.String(),
                        Name = c.String(),
                        SignalrUrl = c.String(),
                        MongoName = c.String(),
                        MongoConnectionString = c.String(),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        TokenLifeTime = c.Int(nullable: false),
                        AllowedGrant = c.Int(nullable: false),
                        AllowedOrigin = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientsCountries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.String(),
                        CountryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Kiosks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineId = c.String(),
                        ClientId = c.String(),
                        ConnectionId = c.String(),
                        IpAddress = c.String(),
                        Status = c.Int(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        Long = c.String(),
                        Lat = c.String(),
                        DefaultValue = c.String(),
                        GroupId = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ClientId = c.String(),
                        CreatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EncryptionUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserEncrypted = c.String(),
                        UserId = c.String(),
                        PasswordHash = c.String(),
                        SaltKey = c.String(),
                        VIKey = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Histories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProcessType = c.Int(nullable: false),
                        Comment = c.String(),
                        HistoryId = c.Int(nullable: false),
                        HistoryName = c.String(),
                        HistoryType = c.Int(nullable: false),
                        UserId = c.String(),
                        MachineId = c.String(),
                        FullName = c.String(),
                        ClientId = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KioskSecurities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineId = c.String(),
                        ClientId = c.String(),
                        LicenseKey = c.String(),
                        MacAddress = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        ExpiryDate = c.DateTime(),
                        EncryptionUserId = c.Int(nullable: false),
                        CreatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EncryptionUsers", t => t.EncryptionUserId, cascadeDelete: true)
                .Index(t => t.EncryptionUserId);
            
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RefreshTokenId = c.String(),
                        Subject = c.String(),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PulseIdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.PulseUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.PulseIdentityRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.PulseIdentityUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        FullName = c.String(),
                        AvatarPath = c.String(),
                        Gender = c.Int(nullable: false),
                        Email = c.String(),
                        Birthday = c.DateTime(),
                        Address = c.String(),
                        UserId = c.String(),
                        Password = c.String(),
                        CreatedAt = c.DateTime(),
                        CreatedBy = c.String(),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                        ClientId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PulseIdentityUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ClientId = c.String(),
                        SecretKey = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.PulseUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PulseIdentityUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PulseUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.PulseIdentityUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PulseUserRoles", "UserId", "dbo.PulseIdentityUsers");
            DropForeignKey("dbo.PulseUserLogins", "UserId", "dbo.PulseIdentityUsers");
            DropForeignKey("dbo.PulseUserClaims", "UserId", "dbo.PulseIdentityUsers");
            DropForeignKey("dbo.PulseUserRoles", "RoleId", "dbo.PulseIdentityRoles");
            DropForeignKey("dbo.KioskSecurities", "EncryptionUserId", "dbo.EncryptionUsers");
            DropForeignKey("dbo.ClientsCountries", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Kiosks", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Kiosks", "CountryId", "dbo.Countries");
            DropIndex("dbo.PulseUserLogins", new[] { "UserId" });
            DropIndex("dbo.PulseUserClaims", new[] { "UserId" });
            DropIndex("dbo.PulseIdentityUsers", "UserNameIndex");
            DropIndex("dbo.PulseUserRoles", new[] { "RoleId" });
            DropIndex("dbo.PulseUserRoles", new[] { "UserId" });
            DropIndex("dbo.PulseIdentityRoles", "RoleNameIndex");
            DropIndex("dbo.KioskSecurities", new[] { "EncryptionUserId" });
            DropIndex("dbo.Kiosks", new[] { "CountryId" });
            DropIndex("dbo.Kiosks", new[] { "GroupId" });
            DropIndex("dbo.ClientsCountries", new[] { "CountryId" });
            DropTable("dbo.PulseUserLogins");
            DropTable("dbo.PulseUserClaims");
            DropTable("dbo.PulseIdentityUsers");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.PulseUserRoles");
            DropTable("dbo.PulseIdentityRoles");
            DropTable("dbo.RefreshTokens");
            DropTable("dbo.KioskSecurities");
            DropTable("dbo.Histories");
            DropTable("dbo.EncryptionUsers");
            DropTable("dbo.Groups");
            DropTable("dbo.Kiosks");
            DropTable("dbo.Countries");
            DropTable("dbo.ClientsCountries");
            DropTable("dbo.Clients");
        }
    }
}
