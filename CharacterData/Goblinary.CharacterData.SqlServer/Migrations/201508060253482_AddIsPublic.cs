namespace Goblinary.CharacterData.SqlServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsPublic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "IsPublic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Characters", "IsPublic");
        }
    }
}
