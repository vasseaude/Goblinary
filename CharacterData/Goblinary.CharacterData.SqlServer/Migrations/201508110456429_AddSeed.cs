namespace Goblinary.CharacterData.SqlServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSeed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "ShareStatus", c => c.String(nullable: false));
			Sql("update dbo.Characters set ShareStatus = 'Private' ");
            AddColumn("dbo.Characters", "ShareSeed", c => c.String(nullable: false));
            DropColumn("dbo.Characters", "IsPublic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Characters", "IsPublic", c => c.Boolean(nullable: false));
            DropColumn("dbo.Characters", "ShareSeed");
            DropColumn("dbo.Characters", "ShareStatus");
        }
    }
}
