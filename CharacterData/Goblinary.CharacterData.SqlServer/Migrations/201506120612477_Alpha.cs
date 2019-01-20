namespace Goblinary.CharacterData.SqlServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alpha : DbMigration
    {
        public override void Up()
        {
			CreateTable(
				"dbo.Characters",
				c => new
					{
						ID = c.Int(nullable: false, identity: true),
						User_ID = c.String(nullable: false),
						Name = c.String(nullable: false),
					})
				.PrimaryKey(t => t.ID);

			CreateTable(
				"dbo.CharacterAchievementRanks",
				c => new
					{
						Character_ID = c.Int(nullable: false),
						Achievement_Name = c.String(nullable: false, maxLength: 128),
						EarnedRank = c.Int(),
						WishListRank = c.Int(),
					})
				.PrimaryKey(t => new { t.Character_ID, t.Achievement_Name })
				.ForeignKey("dbo.Characters", t => t.Character_ID, cascadeDelete: true)
				.Index(t => t.Character_ID);

			CreateTable(
				"dbo.CharacterFeatRanks",
				c => new
					{
						Character_ID = c.Int(nullable: false),
						Feat_Name = c.String(nullable: false, maxLength: 128),
						TrainedRank = c.Int(),
						WishListRank = c.Int(),
					})
				.PrimaryKey(t => new { t.Character_ID, t.Feat_Name })
				.ForeignKey("dbo.Characters", t => t.Character_ID, cascadeDelete: true)
				.Index(t => t.Character_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CharacterFeatRanks", "Character_ID", "dbo.Characters");
            DropForeignKey("dbo.CharacterAchievementRanks", "Character_ID", "dbo.Characters");
            DropIndex("dbo.CharacterFeatRanks", new[] { "Character_ID" });
            DropIndex("dbo.CharacterAchievementRanks", new[] { "Character_ID" });
            DropTable("dbo.CharacterFeatRanks");
            DropTable("dbo.CharacterAchievementRanks");
            DropTable("dbo.Characters");
        }
    }
}
