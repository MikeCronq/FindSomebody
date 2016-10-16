namespace FindSomebody.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Photo = c.String(),
                        Name = c.String(nullable: false, maxLength: 60),
                        Email = c.String(nullable: false),
                        Address = c.String(maxLength: 120),
                        Age = c.Int(),
                        Interests = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.People");
        }
    }
}
