namespace ContactManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);

            CreateIndex("dbo.Contacts", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropTable("dbo.Contacts");
        }
    }
}
