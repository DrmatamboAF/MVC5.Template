namespace MvcTemplate.Tests.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Username = c.String(nullable: false, maxLength: 128),
                        Passhash = c.String(nullable: false, maxLength: 512),
                        Email = c.String(nullable: false, maxLength: 256),
                        IsLocked = c.Boolean(nullable: false),
                        RecoveryToken = c.String(maxLength: 128),
                        RecoveryTokenExpirationDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        RoleId = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id, clustered: false)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.Username, unique: true)
                .Index(t => t.Email, unique: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 128),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id, clustered: false)
                .Index(t => t.Title, unique: true);
            
            CreateTable(
                "dbo.RolePrivileges",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        PrivilegeId = c.String(nullable: false, maxLength: 128),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id, clustered: false)
                .ForeignKey("dbo.Privileges", t => t.PrivilegeId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.PrivilegeId);
            
            CreateTable(
                "dbo.Privileges",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Area = c.String(maxLength: 128),
                        Controller = c.String(nullable: false, maxLength: 128),
                        Action = c.String(nullable: false, maxLength: 128),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id, clustered: false);
            
            CreateTable(
                "dbo.AuditLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AccountId = c.String(maxLength: 128),
                        Action = c.String(nullable: false, maxLength: 128),
                        EntityName = c.String(nullable: false, maxLength: 128),
                        EntityId = c.String(nullable: false, maxLength: 128),
                        Changes = c.String(nullable: false),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id, clustered: false);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AccountId = c.String(maxLength: 128),
                        Message = c.String(nullable: false),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id, clustered: false);
            
            CreateTable(
                "dbo.TestModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Text = c.String(maxLength: 512),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id, clustered: false);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePrivileges", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePrivileges", "PrivilegeId", "dbo.Privileges");
            DropIndex("dbo.RolePrivileges", new[] { "PrivilegeId" });
            DropIndex("dbo.RolePrivileges", new[] { "RoleId" });
            DropIndex("dbo.Roles", new[] { "Title" });
            DropIndex("dbo.Accounts", new[] { "RoleId" });
            DropIndex("dbo.Accounts", new[] { "Email" });
            DropIndex("dbo.Accounts", new[] { "Username" });
            DropTable("dbo.TestModels");
            DropTable("dbo.Logs");
            DropTable("dbo.AuditLogs");
            DropTable("dbo.Privileges");
            DropTable("dbo.RolePrivileges");
            DropTable("dbo.Roles");
            DropTable("dbo.Accounts");
        }
    }
}
