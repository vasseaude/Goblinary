namespace Goblinary.WikiData.SqlServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alpha : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feats",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        BaseType_Name = c.String(nullable: false, maxLength: 128),
                        FeatType_Name = c.String(nullable: false, maxLength: 128),
                        Role_Name = c.String(nullable: false, maxLength: 128),
                        AdvancementFeat_Name = c.String(maxLength: 128),
                        DamageFactor = c.Decimal(precision: 18, scale: 3),
                        AttackSeconds = c.Decimal(precision: 18, scale: 3),
                        StaminaCost = c.Int(),
                        Range = c.String(),
                        WeaponCategory_Name = c.String(maxLength: 128),
                        PowerCost = c.Int(),
                        Level = c.Int(),
                        HasEndOfCombatCooldown = c.Boolean(),
                        AttackBonus_Name = c.String(),
                        CooldownSeconds = c.Decimal(precision: 18, scale: 3),
                        WeaponForm_Name = c.String(),
                        SpecificWeapon_Name = c.String(maxLength: 128),
                        School_Name = c.String(),
                        Channel_Name = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Roles", t => t.Role_Name)
                .ForeignKey("dbo.AdvancementFeats", t => t.AdvancementFeat_Name)
                .ForeignKey("dbo.WeaponTypes", t => t.SpecificWeapon_Name)
                .ForeignKey("dbo.WeaponCategories", t => t.WeaponCategory_Name)
                .ForeignKey("dbo.EntityTypes", t => new { t.BaseType_Name, t.FeatType_Name })
                .Index(t => new { t.BaseType_Name, t.FeatType_Name })
                .Index(t => t.Role_Name)
                .Index(t => t.AdvancementFeat_Name)
                .Index(t => t.WeaponCategory_Name)
                .Index(t => t.SpecificWeapon_Name);
            
            CreateTable(
                "dbo.AchievementRanks",
                c => new
                    {
                        Achievement_Name = c.String(nullable: false, maxLength: 128),
                        Rank = c.Int(nullable: false),
                        DisplayName = c.String(nullable: false),
                        InfluenceGain = c.String(nullable: false),
                        Description = c.String(),
                        Flag_Name = c.String(),
                        Feat_Name = c.String(maxLength: 128),
                        Tier = c.Int(),
                        Rarity_Name = c.String(),
                        Upgrade = c.Int(),
                        Counter_Name = c.String(),
                        Value = c.Int(),
                        InteractionKeyword_Name = c.String(),
                        Race_Name = c.String(),
                        WeaponProficiency_Name = c.String(),
                        Settlement_Name = c.String(),
                        Location_Name = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Achievement_Name, t.Rank })
                .ForeignKey("dbo.Achievements", t => t.Achievement_Name)
                .ForeignKey("dbo.Feats", t => t.Feat_Name)
                .Index(t => t.Achievement_Name)
                .Index(t => t.Feat_Name);
            
            CreateTable(
                "dbo.Achievements",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        BaseType_Name = c.String(nullable: false, maxLength: 128),
                        AchievementType_Name = c.String(nullable: false, maxLength: 128),
                        AchievementGroup_Name = c.String(nullable: false, maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.AchievementGroups", t => t.AchievementGroup_Name)
                .ForeignKey("dbo.EntityTypes", t => new { t.BaseType_Name, t.AchievementType_Name })
                .Index(t => new { t.BaseType_Name, t.AchievementType_Name })
                .Index(t => t.AchievementGroup_Name);
            
            CreateTable(
                "dbo.AchievementGroups",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        BaseType_Name = c.String(nullable: false, maxLength: 128),
                        AchievementType_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.EntityTypes", t => new { t.BaseType_Name, t.AchievementType_Name })
                .Index(t => new { t.BaseType_Name, t.AchievementType_Name });
            
            CreateTable(
                "dbo.EntityTypes",
                c => new
                    {
                        BaseType_Name = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        ParentType_Name = c.String(maxLength: 128),
                        Modifier = c.String(nullable: false),
                        DisplayName = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.BaseType_Name, t.Name })
                .ForeignKey("dbo.EntityTypes", t => new { t.BaseType_Name, t.ParentType_Name })
                .Index(t => new { t.BaseType_Name, t.ParentType_Name });
            
            CreateTable(
                "dbo.EntityTypeMappings",
                c => new
                    {
                        BaseType_Name = c.String(nullable: false, maxLength: 128),
                        ParentType_Name = c.String(nullable: false, maxLength: 128),
                        ChildType_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.BaseType_Name, t.ParentType_Name, t.ChildType_Name })
                .ForeignKey("dbo.EntityTypes", t => new { t.BaseType_Name, t.ParentType_Name })
                .ForeignKey("dbo.EntityTypes", t => new { t.BaseType_Name, t.ChildType_Name })
                .Index(t => new { t.BaseType_Name, t.ChildType_Name })
                .Index(t => t.ParentType_Name, name: "IX_BaseType_Name_ParentType_Name");
            
            CreateTable(
                "dbo.AchievementRankCategoryBonuses",
                c => new
                    {
                        Achievement_Name = c.String(nullable: false, maxLength: 128),
                        Achievement_Rank = c.Int(nullable: false),
                        BonusNo = c.Int(nullable: false),
                        Category_Name = c.String(nullable: false),
                        Bonus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Achievement_Name, t.Achievement_Rank, t.BonusNo })
                .ForeignKey("dbo.AchievementRanks", t => new { t.Achievement_Name, t.Achievement_Rank })
                .Index(t => new { t.Achievement_Name, t.Achievement_Rank });
            
            CreateTable(
                "dbo.AchievementRankFlagRequirements",
                c => new
                    {
                        Achievement_Name = c.String(nullable: false, maxLength: 128),
                        Achievement_Rank = c.Int(nullable: false),
                        RequirementNo = c.Int(nullable: false),
                        OptionNo = c.Int(nullable: false),
                        Flag_Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.Achievement_Name, t.Achievement_Rank, t.RequirementNo, t.OptionNo })
                .ForeignKey("dbo.AchievementRanks", t => new { t.Achievement_Name, t.Achievement_Rank })
                .Index(t => new { t.Achievement_Name, t.Achievement_Rank });
            
            CreateTable(
                "dbo.AchievementRankFeatRequirements",
                c => new
                    {
                        Achievement_Name = c.String(nullable: false, maxLength: 128),
                        Achievement_Rank = c.Int(nullable: false),
                        RequirementNo = c.Int(nullable: false),
                        OptionNo = c.Int(nullable: false),
                        Feat_Name = c.String(nullable: false, maxLength: 128),
                        Feat_Rank = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Achievement_Name, t.Achievement_Rank, t.RequirementNo, t.OptionNo })
                .ForeignKey("dbo.FeatRanks", t => new { t.Feat_Name, t.Feat_Rank })
                .ForeignKey("dbo.AchievementRanks", t => new { t.Achievement_Name, t.Achievement_Rank })
                .Index(t => new { t.Achievement_Name, t.Achievement_Rank })
                .Index(t => new { t.Feat_Name, t.Feat_Rank });
            
            CreateTable(
                "dbo.FeatRanks",
                c => new
                    {
                        Feat_Name = c.String(nullable: false, maxLength: 128),
                        Rank = c.Int(nullable: false),
                        ExpCost = c.Int(nullable: false),
                        CoinCost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Feat_Name, t.Rank })
                .ForeignKey("dbo.Feats", t => t.Feat_Name)
                .Index(t => t.Feat_Name);
            
            CreateTable(
                "dbo.FeatRankAbilityBonuses",
                c => new
                    {
                        Feat_Name = c.String(nullable: false, maxLength: 128),
                        Feat_Rank = c.Int(nullable: false),
                        BonusNo = c.Int(nullable: false),
                        OptionNo = c.Int(nullable: false),
                        Ability_Name = c.String(nullable: false, maxLength: 128),
                        Bonus = c.Decimal(nullable: false, precision: 18, scale: 3),
                    })
                .PrimaryKey(t => new { t.Feat_Name, t.Feat_Rank, t.BonusNo, t.OptionNo })
                .ForeignKey("dbo.Abilities", t => t.Ability_Name)
                .ForeignKey("dbo.FeatRanks", t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => t.Ability_Name);
            
            CreateTable(
                "dbo.Abilities",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.FeatRankAbilityRequirements",
                c => new
                    {
                        Feat_Name = c.String(nullable: false, maxLength: 128),
                        Feat_Rank = c.Int(nullable: false),
                        RequirementNo = c.Int(nullable: false),
                        OptionNo = c.Int(nullable: false),
                        Ability_Name = c.String(nullable: false, maxLength: 128),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Feat_Name, t.Feat_Rank, t.RequirementNo, t.OptionNo })
                .ForeignKey("dbo.Abilities", t => t.Ability_Name)
                .ForeignKey("dbo.FeatRanks", t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => t.Ability_Name);
            
            CreateTable(
                "dbo.FeatRankAchievementRequirements",
                c => new
                    {
                        Feat_Name = c.String(nullable: false, maxLength: 128),
                        Feat_Rank = c.Int(nullable: false),
                        RequirementNo = c.Int(nullable: false),
                        OptionNo = c.Int(nullable: false),
                        Achievement_Name = c.String(nullable: false, maxLength: 128),
                        Achievement_Rank = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Feat_Name, t.Feat_Rank, t.RequirementNo, t.OptionNo })
                .ForeignKey("dbo.AchievementRanks", t => new { t.Achievement_Name, t.Achievement_Rank })
                .ForeignKey("dbo.FeatRanks", t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => new { t.Achievement_Name, t.Achievement_Rank });
            
            CreateTable(
                "dbo.FeatRankCategoryRequirements",
                c => new
                    {
                        Feat_Name = c.String(nullable: false, maxLength: 128),
                        Feat_Rank = c.Int(nullable: false),
                        RequirementNo = c.Int(nullable: false),
                        OptionNo = c.Int(nullable: false),
                        Category_Name = c.String(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Feat_Name, t.Feat_Rank, t.RequirementNo, t.OptionNo })
                .ForeignKey("dbo.FeatRanks", t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => new { t.Feat_Name, t.Feat_Rank });
            
            CreateTable(
                "dbo.FeatRankEffects",
                c => new
                    {
                        Feat_Name = c.String(nullable: false, maxLength: 128),
                        Feat_Rank = c.Int(nullable: false),
                        EffectNo = c.Int(nullable: false),
                        EffectDescription_Text = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Feat_Name, t.Feat_Rank, t.EffectNo })
                .ForeignKey("dbo.EffectDescriptions", t => t.EffectDescription_Text)
                .ForeignKey("dbo.FeatRanks", t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => t.EffectDescription_Text);
            
            CreateTable(
                "dbo.EffectDescriptions",
                c => new
                    {
                        Text = c.String(nullable: false, maxLength: 128),
                        FormattedDescription = c.String(nullable: false),
                        Effect_Name = c.String(nullable: false, maxLength: 128),
                        Magnitude = c.String(),
                        Duration = c.String(),
                        Chance = c.String(),
                        Distance = c.String(),
                        Target = c.String(),
                        Discriminator = c.String(),
                        Condition_Name = c.String(maxLength: 128),
                        ConditionTarget = c.String(),
                    })
                .PrimaryKey(t => t.Text)
                .ForeignKey("dbo.Effects", t => t.Effect_Name)
                .ForeignKey("dbo.Conditions", t => t.Condition_Name)
                .Index(t => t.Effect_Name)
                .Index(t => t.Condition_Name);
            
            CreateTable(
                "dbo.Conditions",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Effect_Name = c.String(maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Effects", t => t.Effect_Name)
                .Index(t => t.Effect_Name);
            
            CreateTable(
                "dbo.Effects",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        EffectTerm_Term = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.EffectTerms", t => t.EffectTerm_Term)
                .Index(t => t.EffectTerm_Term);
            
            CreateTable(
                "dbo.EffectTerms",
                c => new
                    {
                        Term = c.String(nullable: false, maxLength: 128),
                        EffectType_Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        MathSpecifics = c.String(),
                        Channel_Name = c.String(),
                    })
                .PrimaryKey(t => t.Term);
            
            CreateTable(
                "dbo.FeatEffects",
                c => new
                    {
                        Feat_Name = c.String(nullable: false, maxLength: 128),
                        EffectType = c.String(nullable: false, maxLength: 128),
                        EffectNo = c.Int(nullable: false),
                        EffectDescription_Text = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Feat_Name, t.EffectType, t.EffectNo })
                .ForeignKey("dbo.EffectDescriptions", t => t.EffectDescription_Text)
                .ForeignKey("dbo.Feats", t => t.Feat_Name)
                .Index(t => t.Feat_Name)
                .Index(t => t.EffectDescription_Text);
            
            CreateTable(
                "dbo.FeatRankFeatRequirements",
                c => new
                    {
                        Feat_Name = c.String(nullable: false, maxLength: 128),
                        Feat_Rank = c.Int(nullable: false),
                        RequirementNo = c.Int(nullable: false),
                        OptionNo = c.Int(nullable: false),
                        RequiredFeat_Name = c.String(nullable: false, maxLength: 128),
                        RequiredFeat_Rank = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Feat_Name, t.Feat_Rank, t.RequirementNo, t.OptionNo })
                .ForeignKey("dbo.FeatRanks", t => new { t.RequiredFeat_Name, t.RequiredFeat_Rank })
                .ForeignKey("dbo.FeatRanks", t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => new { t.RequiredFeat_Name, t.RequiredFeat_Rank });
            
            CreateTable(
                "dbo.FeatRankKeywords",
                c => new
                    {
                        Feat_Name = c.String(nullable: false, maxLength: 128),
                        Feat_Rank = c.Int(nullable: false),
                        KeywordNo = c.Int(nullable: false),
                        KeywordType_Name = c.String(nullable: false, maxLength: 128),
                        Keyword_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Feat_Name, t.Feat_Rank, t.KeywordNo })
                .ForeignKey("dbo.Keywords", t => new { t.KeywordType_Name, t.Keyword_Name })
                .ForeignKey("dbo.FeatRanks", t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => new { t.KeywordType_Name, t.Keyword_Name });
            
            CreateTable(
                "dbo.Keywords",
                c => new
                    {
                        KeywordType_Name = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Value_Name = c.String(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => new { t.KeywordType_Name, t.Name })
                .ForeignKey("dbo.KeywordTypes", t => t.KeywordType_Name)
                .Index(t => t.KeywordType_Name);
            
            CreateTable(
                "dbo.KeywordTypes",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        SourceFeatType_Name = c.String(nullable: false),
                        MatchingFeatType_Name = c.String(),
                        MatchingItemType_Name = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.RecipeOutputItemUpgradeKeywords",
                c => new
                    {
                        Item_Name = c.String(nullable: false, maxLength: 128),
                        Upgrade = c.Int(nullable: false),
                        KeywordKind_Name = c.String(nullable: false, maxLength: 128),
                        KeywordNo = c.Int(nullable: false),
                        KeywordType_Name = c.String(nullable: false, maxLength: 128),
                        Keyword_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Item_Name, t.Upgrade, t.KeywordKind_Name, t.KeywordNo })
                .ForeignKey("dbo.RecipeOutputItemUpgrades", t => new { t.Item_Name, t.Upgrade })
                .ForeignKey("dbo.Keywords", t => new { t.KeywordType_Name, t.Keyword_Name })
                .Index(t => new { t.Item_Name, t.Upgrade })
                .Index(t => new { t.KeywordType_Name, t.Keyword_Name });
            
            CreateTable(
                "dbo.RecipeOutputItemUpgrades",
                c => new
                    {
                        Item_Name = c.String(nullable: false, maxLength: 128),
                        Upgrade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Item_Name, t.Upgrade })
                .ForeignKey("dbo.RecipeOutputItems", t => t.Item_Name)
                .Index(t => t.Item_Name);
            
            CreateTable(
                "dbo.RecipeOutputItems",
                c => new
                    {
                        Item_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Item_Name)
                .ForeignKey("dbo.Items", t => t.Item_Name)
                .Index(t => t.Item_Name);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        BaseType_Name = c.String(nullable: false, maxLength: 128),
                        ItemType_Name = c.String(nullable: false, maxLength: 128),
                        Tier = c.Int(nullable: false),
                        Encumbrance = c.Decimal(precision: 18, scale: 3),
                        Description = c.String(),
                        ItemCategory_Name = c.String(),
                        AmmoType_Name = c.String(),
                        AmmoContainerType_Name = c.String(),
                        ArmorType_Name = c.String(),
                        MainRole_Name = c.String(maxLength: 128),
                        Camp_Name = c.String(maxLength: 128),
                        Consumable_Name = c.String(maxLength: 128),
                        Outpost_Name = c.String(maxLength: 128),
                        Holding_Name = c.String(maxLength: 128),
                        GearType_Name = c.String(maxLength: 128),
                        WeaponType_Name = c.String(maxLength: 128),
                        ImplementType_Name = c.String(),
                        WeaponCoatingType_Name = c.String(),
                        Variety_Name = c.String(),
                        ResourceType_Name = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.EntityTypes", t => new { t.BaseType_Name, t.ItemType_Name })
                .ForeignKey("dbo.Roles", t => t.MainRole_Name)
                .ForeignKey("dbo.Structures", t => t.Camp_Name)
                .ForeignKey("dbo.Structures", t => t.Outpost_Name)
                .ForeignKey("dbo.Structures", t => t.Holding_Name)
                .ForeignKey("dbo.GearTypes", t => t.GearType_Name)
                .ForeignKey("dbo.WeaponTypes", t => t.WeaponType_Name)
                .ForeignKey("dbo.Feats", t => t.Consumable_Name)
                .Index(t => new { t.BaseType_Name, t.ItemType_Name })
                .Index(t => t.MainRole_Name)
                .Index(t => t.Camp_Name)
                .Index(t => t.Consumable_Name)
                .Index(t => t.Outpost_Name)
                .Index(t => t.Holding_Name)
                .Index(t => t.GearType_Name)
                .Index(t => t.WeaponType_Name);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Structures",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        BaseType_Name = c.String(nullable: false, maxLength: 128),
                        StructureType_Name = c.String(nullable: false, maxLength: 128),
                        ConstructionData = c.String(),
                        Description = c.String(),
                        DisplayName = c.String(),
                        Encumbrance = c.Decimal(precision: 18, scale: 3),
                        Quality = c.Int(),
                        Tier = c.Int(),
                        Category = c.String(),
                        HousingData = c.String(),
                        HouseEntityDefn = c.String(),
                        Cooldown = c.Int(),
                        NoLoot = c.Boolean(),
                        Upgradable = c.Boolean(),
                        AccountRedeem_Name = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.EntityTypes", t => new { t.BaseType_Name, t.StructureType_Name })
                .Index(t => new { t.BaseType_Name, t.StructureType_Name });
            
            CreateTable(
                "dbo.CampUpgrades",
                c => new
                    {
                        Structure_Name = c.String(nullable: false, maxLength: 128),
                        Upgrade = c.Int(nullable: false),
                        PowerChannelDurationSeconds = c.Int(nullable: false),
                        PowerRegenerationSeconds = c.Int(nullable: false),
                        PowerCooldownMinutes = c.Int(nullable: false),
                        BuildingDurationMinutes = c.Int(nullable: false),
                        ConstructionData = c.String(nullable: false),
                        BaseType_Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.Structure_Name, t.Upgrade })
                .ForeignKey("dbo.Structures", t => t.Structure_Name)
                .Index(t => t.Structure_Name);
            
            CreateTable(
                "dbo.AdvancementFeats",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.FeatRankTrainerLevels",
                c => new
                    {
                        Feat_Name = c.String(nullable: false, maxLength: 128),
                        Feat_Rank = c.Int(nullable: false),
                        Trainer_Name = c.String(nullable: false, maxLength: 128),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Feat_Name, t.Feat_Rank, t.Trainer_Name })
                .ForeignKey("dbo.Trainers", t => t.Trainer_Name)
                .ForeignKey("dbo.AdvancementFeats", t => t.Feat_Name)
                .Index(t => t.Feat_Name)
                .Index(t => t.Trainer_Name);
            
            CreateTable(
                "dbo.Trainers",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.HoldingUpgradeTrainerLevels",
                c => new
                    {
                        Structure_Name = c.String(nullable: false, maxLength: 128),
                        Upgrade = c.Int(nullable: false),
                        TrainerNo = c.Int(nullable: false),
                        Trainer_Name = c.String(nullable: false, maxLength: 128),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Structure_Name, t.Upgrade, t.TrainerNo })
                .ForeignKey("dbo.HoldingUpgrades", t => new { t.Structure_Name, t.Upgrade })
                .ForeignKey("dbo.Trainers", t => t.Trainer_Name)
                .Index(t => new { t.Structure_Name, t.Upgrade })
                .Index(t => t.Trainer_Name);
            
            CreateTable(
                "dbo.HoldingUpgrades",
                c => new
                    {
                        Structure_Name = c.String(nullable: false, maxLength: 128),
                        Upgrade = c.Int(nullable: false),
                        InfluenceCost = c.Int(nullable: false),
                        CraftingFacilityFeat_Name = c.String(maxLength: 128),
                        CraftingFacilityQuality = c.Int(),
                        PvPPeakGuards = c.Int(nullable: false),
                        NonPvPPeakGuards = c.Int(nullable: false),
                        GuardSurgeSize = c.Int(nullable: false),
                        GuardRespawnsPerMinute = c.Decimal(nullable: false, precision: 18, scale: 3),
                        PvPGuardRespawns = c.Int(nullable: false),
                        NonPvPGuardRespawns = c.Int(nullable: false),
                        NonPvPRespawnFill = c.Int(nullable: false),
                        MinPvPTime = c.Decimal(nullable: false, precision: 18, scale: 3),
                        GuardEntityNames = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.Structure_Name, t.Upgrade })
                .ForeignKey("dbo.Structures", t => t.Structure_Name)
                .ForeignKey("dbo.Feats", t => t.CraftingFacilityFeat_Name)
                .Index(t => t.Structure_Name)
                .Index(t => t.CraftingFacilityFeat_Name);
            
            CreateTable(
                "dbo.HoldingUpgradeBulkResourceBonuses",
                c => new
                    {
                        Structure_Name = c.String(nullable: false, maxLength: 128),
                        Upgrade = c.Int(nullable: false),
                        BonusNo = c.Int(nullable: false),
                        BulkResource_Name = c.String(nullable: false, maxLength: 128),
                        Bonus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Structure_Name, t.Upgrade, t.BonusNo })
                .ForeignKey("dbo.BulkResources", t => t.BulkResource_Name)
                .ForeignKey("dbo.HoldingUpgrades", t => new { t.Structure_Name, t.Upgrade })
                .Index(t => new { t.Structure_Name, t.Upgrade })
                .Index(t => t.BulkResource_Name);
            
            CreateTable(
                "dbo.BulkResources",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.HoldingUpgradeBulkResourceRequirements",
                c => new
                    {
                        Structure_Name = c.String(nullable: false, maxLength: 128),
                        Upgrade = c.Int(nullable: false),
                        RequirementNo = c.Int(nullable: false),
                        BulkResource_Name = c.String(nullable: false, maxLength: 128),
                        Requirement = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Structure_Name, t.Upgrade, t.RequirementNo })
                .ForeignKey("dbo.BulkResources", t => t.BulkResource_Name)
                .ForeignKey("dbo.HoldingUpgrades", t => new { t.Structure_Name, t.Upgrade })
                .Index(t => new { t.Structure_Name, t.Upgrade })
                .Index(t => t.BulkResource_Name);
            
            CreateTable(
                "dbo.OutpostBulkResources",
                c => new
                    {
                        Structure_Name = c.String(nullable: false, maxLength: 128),
                        BulkResource_Name = c.String(nullable: false, maxLength: 128),
                        BulkRating_Name = c.String(nullable: false, maxLength: 128),
                        Percentage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Structure_Name, t.BulkResource_Name, t.BulkRating_Name })
                .ForeignKey("dbo.BulkRatings", t => t.BulkRating_Name)
                .ForeignKey("dbo.Structures", t => t.Structure_Name)
                .ForeignKey("dbo.BulkResources", t => t.BulkResource_Name)
                .Index(t => t.Structure_Name)
                .Index(t => t.BulkResource_Name)
                .Index(t => t.BulkRating_Name);
            
            CreateTable(
                "dbo.BulkRatings",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.HexBulkRatings",
                c => new
                    {
                        Hex_Longitude = c.Int(nullable: false),
                        Hex_Latitude = c.Int(nullable: false),
                        BulkRating_Name = c.String(nullable: false, maxLength: 128),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Hex_Longitude, t.Hex_Latitude, t.BulkRating_Name })
                .ForeignKey("dbo.Hexes", t => new { t.Hex_Longitude, t.Hex_Latitude })
                .ForeignKey("dbo.BulkRatings", t => t.BulkRating_Name)
                .Index(t => new { t.Hex_Longitude, t.Hex_Latitude })
                .Index(t => t.BulkRating_Name);
            
            CreateTable(
                "dbo.Hexes",
                c => new
                    {
                        Longitude = c.Int(nullable: false),
                        Latitude = c.Int(nullable: false),
                        Region_Name = c.String(nullable: false),
                        TerrainType_Name = c.String(nullable: false),
                        HexType_Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.Longitude, t.Latitude });
            
            CreateTable(
                "dbo.OutpostUpgrades",
                c => new
                    {
                        Structure_Name = c.String(nullable: false, maxLength: 128),
                        Upgrade = c.Int(nullable: false),
                        EffortBonus = c.Int(nullable: false),
                        InfluenceCost = c.Int(nullable: false),
                        PvPPeakGuards = c.Int(nullable: false),
                        GuardSurgeSize = c.Int(nullable: false),
                        GuardRespawnsPerMinute = c.Decimal(nullable: false, precision: 18, scale: 3),
                        PvPGuardRespawns = c.Int(nullable: false),
                        MinPvPTime = c.Decimal(nullable: false, precision: 18, scale: 3),
                        GuardEntityNames = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.Structure_Name, t.Upgrade })
                .ForeignKey("dbo.Structures", t => t.Structure_Name)
                .Index(t => t.Structure_Name);
            
            CreateTable(
                "dbo.OutpostWorkerFeats",
                c => new
                    {
                        Structure_Name = c.String(nullable: false, maxLength: 128),
                        WorkerFeat_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Structure_Name, t.WorkerFeat_Name })
                .ForeignKey("dbo.Structures", t => t.Structure_Name)
                .ForeignKey("dbo.Feats", t => t.WorkerFeat_Name)
                .Index(t => t.Structure_Name)
                .Index(t => t.WorkerFeat_Name);
            
            CreateTable(
                "dbo.WeaponCategories",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.WeaponTypes",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        WeaponCategory_Name = c.String(nullable: false, maxLength: 128),
                        AttackBonus_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.AttackBonuses", t => t.AttackBonus_Name)
                .ForeignKey("dbo.WeaponCategories", t => t.WeaponCategory_Name)
                .Index(t => t.WeaponCategory_Name)
                .Index(t => t.AttackBonus_Name);
            
            CreateTable(
                "dbo.AttackBonuses",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.GearTypes",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        WeaponCategory_Name = c.String(maxLength: 128),
                        AttackBonus_Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.AttackBonuses", t => t.AttackBonus_Name)
                .ForeignKey("dbo.WeaponCategories", t => t.WeaponCategory_Name)
                .Index(t => t.WeaponCategory_Name)
                .Index(t => t.AttackBonus_Name);
            
            CreateTable(
                "dbo.RecipeIngredients",
                c => new
                    {
                        Recipe_Name = c.String(nullable: false, maxLength: 128),
                        IngredientNo = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Component_Name = c.String(maxLength: 128),
                        Stock_Name = c.String(maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Recipe_Name, t.IngredientNo })
                .ForeignKey("dbo.Stocks", t => t.Stock_Name)
                .ForeignKey("dbo.Recipes", t => t.Recipe_Name)
                .ForeignKey("dbo.Items", t => t.Component_Name)
                .Index(t => t.Recipe_Name)
                .Index(t => t.Component_Name)
                .Index(t => t.Stock_Name);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        BaseType_Name = c.String(nullable: false, maxLength: 128),
                        RecipeType_Name = c.String(nullable: false, maxLength: 128),
                        Feat_Name = c.String(nullable: false, maxLength: 128),
                        Feat_Rank = c.Int(nullable: false),
                        Tier = c.Int(nullable: false),
                        OutputItem_Name = c.String(nullable: false, maxLength: 128),
                        QtyProduced = c.Int(nullable: false),
                        BaseCraftingSeconds = c.Int(nullable: false),
                        Quality = c.Int(nullable: false),
                        AchievementType_Name = c.String(nullable: false),
                        Upgrade = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.FeatRanks", t => new { t.Feat_Name, t.Feat_Rank })
                .ForeignKey("dbo.EntityTypes", t => new { t.BaseType_Name, t.RecipeType_Name })
                .ForeignKey("dbo.RecipeOutputItems", t => t.OutputItem_Name)
                .Index(t => new { t.BaseType_Name, t.RecipeType_Name })
                .Index(t => new { t.Feat_Name, t.Feat_Rank })
                .Index(t => t.OutputItem_Name);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.StockItemStocks",
                c => new
                    {
                        StockItem_Name = c.String(nullable: false, maxLength: 128),
                        Stock_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.StockItem_Name, t.Stock_Name })
                .ForeignKey("dbo.Items", t => t.StockItem_Name)
                .ForeignKey("dbo.Stocks", t => t.Stock_Name)
                .Index(t => t.StockItem_Name)
                .Index(t => t.Stock_Name);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Structures", new[] { "BaseType_Name", "StructureType_Name" }, "dbo.EntityTypes");
            DropForeignKey("dbo.FeatRanks", "Feat_Name", "dbo.Feats");
            DropForeignKey("dbo.OutpostWorkerFeats", "WorkerFeat_Name", "dbo.Feats");
            DropForeignKey("dbo.HoldingUpgrades", "CraftingFacilityFeat_Name", "dbo.Feats");
            DropForeignKey("dbo.Feats", new[] { "BaseType_Name", "FeatType_Name" }, "dbo.EntityTypes");
            DropForeignKey("dbo.FeatEffects", "Feat_Name", "dbo.Feats");
            DropForeignKey("dbo.AchievementRanks", "Feat_Name", "dbo.Feats");
            DropForeignKey("dbo.AchievementRanks", "Achievement_Name", "dbo.Achievements");
            DropForeignKey("dbo.AchievementRankFeatRequirements", new[] { "Achievement_Name", "Achievement_Rank" }, "dbo.AchievementRanks");
            DropForeignKey("dbo.AchievementRankFeatRequirements", new[] { "Feat_Name", "Feat_Rank" }, "dbo.FeatRanks");
            DropForeignKey("dbo.FeatRankKeywords", new[] { "Feat_Name", "Feat_Rank" }, "dbo.FeatRanks");
            DropForeignKey("dbo.RecipeOutputItemUpgradeKeywords", new[] { "KeywordType_Name", "Keyword_Name" }, "dbo.Keywords");
            DropForeignKey("dbo.RecipeOutputItemUpgradeKeywords", new[] { "Item_Name", "Upgrade" }, "dbo.RecipeOutputItemUpgrades");
            DropForeignKey("dbo.Recipes", "OutputItem_Name", "dbo.RecipeOutputItems");
            DropForeignKey("dbo.RecipeOutputItemUpgrades", "Item_Name", "dbo.RecipeOutputItems");
            DropForeignKey("dbo.RecipeIngredients", "Component_Name", "dbo.Items");
            DropForeignKey("dbo.Recipes", new[] { "BaseType_Name", "RecipeType_Name" }, "dbo.EntityTypes");
            DropForeignKey("dbo.RecipeIngredients", "Recipe_Name", "dbo.Recipes");
            DropForeignKey("dbo.StockItemStocks", "Stock_Name", "dbo.Stocks");
            DropForeignKey("dbo.StockItemStocks", "StockItem_Name", "dbo.Items");
            DropForeignKey("dbo.RecipeIngredients", "Stock_Name", "dbo.Stocks");
            DropForeignKey("dbo.Recipes", new[] { "Feat_Name", "Feat_Rank" }, "dbo.FeatRanks");
            DropForeignKey("dbo.Items", "Consumable_Name", "dbo.Feats");
            DropForeignKey("dbo.WeaponTypes", "WeaponCategory_Name", "dbo.WeaponCategories");
            DropForeignKey("dbo.GearTypes", "WeaponCategory_Name", "dbo.WeaponCategories");
            DropForeignKey("dbo.Feats", "WeaponCategory_Name", "dbo.WeaponCategories");
            DropForeignKey("dbo.Feats", "SpecificWeapon_Name", "dbo.WeaponTypes");
            DropForeignKey("dbo.Items", "WeaponType_Name", "dbo.WeaponTypes");
            DropForeignKey("dbo.WeaponTypes", "AttackBonus_Name", "dbo.AttackBonuses");
            DropForeignKey("dbo.GearTypes", "AttackBonus_Name", "dbo.AttackBonuses");
            DropForeignKey("dbo.Items", "GearType_Name", "dbo.GearTypes");
            DropForeignKey("dbo.Feats", "AdvancementFeat_Name", "dbo.AdvancementFeats");
            DropForeignKey("dbo.FeatRankTrainerLevels", "Feat_Name", "dbo.AdvancementFeats");
            DropForeignKey("dbo.HoldingUpgradeTrainerLevels", "Trainer_Name", "dbo.Trainers");
            DropForeignKey("dbo.HoldingUpgradeTrainerLevels", new[] { "Structure_Name", "Upgrade" }, "dbo.HoldingUpgrades");
            DropForeignKey("dbo.HoldingUpgrades", "Structure_Name", "dbo.Structures");
            DropForeignKey("dbo.Items", "Holding_Name", "dbo.Structures");
            DropForeignKey("dbo.HoldingUpgradeBulkResourceRequirements", new[] { "Structure_Name", "Upgrade" }, "dbo.HoldingUpgrades");
            DropForeignKey("dbo.HoldingUpgradeBulkResourceBonuses", new[] { "Structure_Name", "Upgrade" }, "dbo.HoldingUpgrades");
            DropForeignKey("dbo.OutpostBulkResources", "BulkResource_Name", "dbo.BulkResources");
            DropForeignKey("dbo.OutpostWorkerFeats", "Structure_Name", "dbo.Structures");
            DropForeignKey("dbo.OutpostUpgrades", "Structure_Name", "dbo.Structures");
            DropForeignKey("dbo.Items", "Outpost_Name", "dbo.Structures");
            DropForeignKey("dbo.OutpostBulkResources", "Structure_Name", "dbo.Structures");
            DropForeignKey("dbo.OutpostBulkResources", "BulkRating_Name", "dbo.BulkRatings");
            DropForeignKey("dbo.HexBulkRatings", "BulkRating_Name", "dbo.BulkRatings");
            DropForeignKey("dbo.HexBulkRatings", new[] { "Hex_Longitude", "Hex_Latitude" }, "dbo.Hexes");
            DropForeignKey("dbo.HoldingUpgradeBulkResourceRequirements", "BulkResource_Name", "dbo.BulkResources");
            DropForeignKey("dbo.HoldingUpgradeBulkResourceBonuses", "BulkResource_Name", "dbo.BulkResources");
            DropForeignKey("dbo.FeatRankTrainerLevels", "Trainer_Name", "dbo.Trainers");
            DropForeignKey("dbo.CampUpgrades", "Structure_Name", "dbo.Structures");
            DropForeignKey("dbo.Items", "Camp_Name", "dbo.Structures");
            DropForeignKey("dbo.Feats", "Role_Name", "dbo.Roles");
            DropForeignKey("dbo.Items", "MainRole_Name", "dbo.Roles");
            DropForeignKey("dbo.RecipeOutputItems", "Item_Name", "dbo.Items");
            DropForeignKey("dbo.Items", new[] { "BaseType_Name", "ItemType_Name" }, "dbo.EntityTypes");
            DropForeignKey("dbo.Keywords", "KeywordType_Name", "dbo.KeywordTypes");
            DropForeignKey("dbo.FeatRankKeywords", new[] { "KeywordType_Name", "Keyword_Name" }, "dbo.Keywords");
            DropForeignKey("dbo.FeatRankFeatRequirements", new[] { "Feat_Name", "Feat_Rank" }, "dbo.FeatRanks");
            DropForeignKey("dbo.FeatRankFeatRequirements", new[] { "RequiredFeat_Name", "RequiredFeat_Rank" }, "dbo.FeatRanks");
            DropForeignKey("dbo.FeatRankEffects", new[] { "Feat_Name", "Feat_Rank" }, "dbo.FeatRanks");
            DropForeignKey("dbo.FeatRankEffects", "EffectDescription_Text", "dbo.EffectDescriptions");
            DropForeignKey("dbo.FeatEffects", "EffectDescription_Text", "dbo.EffectDescriptions");
            DropForeignKey("dbo.EffectDescriptions", "Condition_Name", "dbo.Conditions");
            DropForeignKey("dbo.Effects", "EffectTerm_Term", "dbo.EffectTerms");
            DropForeignKey("dbo.EffectDescriptions", "Effect_Name", "dbo.Effects");
            DropForeignKey("dbo.Conditions", "Effect_Name", "dbo.Effects");
            DropForeignKey("dbo.FeatRankCategoryRequirements", new[] { "Feat_Name", "Feat_Rank" }, "dbo.FeatRanks");
            DropForeignKey("dbo.FeatRankAchievementRequirements", new[] { "Feat_Name", "Feat_Rank" }, "dbo.FeatRanks");
            DropForeignKey("dbo.FeatRankAchievementRequirements", new[] { "Achievement_Name", "Achievement_Rank" }, "dbo.AchievementRanks");
            DropForeignKey("dbo.FeatRankAbilityRequirements", new[] { "Feat_Name", "Feat_Rank" }, "dbo.FeatRanks");
            DropForeignKey("dbo.FeatRankAbilityBonuses", new[] { "Feat_Name", "Feat_Rank" }, "dbo.FeatRanks");
            DropForeignKey("dbo.FeatRankAbilityRequirements", "Ability_Name", "dbo.Abilities");
            DropForeignKey("dbo.FeatRankAbilityBonuses", "Ability_Name", "dbo.Abilities");
            DropForeignKey("dbo.AchievementRankFlagRequirements", new[] { "Achievement_Name", "Achievement_Rank" }, "dbo.AchievementRanks");
            DropForeignKey("dbo.AchievementRankCategoryBonuses", new[] { "Achievement_Name", "Achievement_Rank" }, "dbo.AchievementRanks");
            DropForeignKey("dbo.Achievements", new[] { "BaseType_Name", "AchievementType_Name" }, "dbo.EntityTypes");
            DropForeignKey("dbo.AchievementGroups", new[] { "BaseType_Name", "AchievementType_Name" }, "dbo.EntityTypes");
            DropForeignKey("dbo.EntityTypeMappings", new[] { "BaseType_Name", "ChildType_Name" }, "dbo.EntityTypes");
            DropForeignKey("dbo.EntityTypes", new[] { "BaseType_Name", "ParentType_Name" }, "dbo.EntityTypes");
            DropForeignKey("dbo.EntityTypeMappings", new[] { "BaseType_Name", "ParentType_Name" }, "dbo.EntityTypes");
            DropForeignKey("dbo.Achievements", "AchievementGroup_Name", "dbo.AchievementGroups");
            DropIndex("dbo.StockItemStocks", new[] { "Stock_Name" });
            DropIndex("dbo.StockItemStocks", new[] { "StockItem_Name" });
            DropIndex("dbo.Recipes", new[] { "OutputItem_Name" });
            DropIndex("dbo.Recipes", new[] { "Feat_Name", "Feat_Rank" });
            DropIndex("dbo.Recipes", new[] { "BaseType_Name", "RecipeType_Name" });
            DropIndex("dbo.RecipeIngredients", new[] { "Stock_Name" });
            DropIndex("dbo.RecipeIngredients", new[] { "Component_Name" });
            DropIndex("dbo.RecipeIngredients", new[] { "Recipe_Name" });
            DropIndex("dbo.GearTypes", new[] { "AttackBonus_Name" });
            DropIndex("dbo.GearTypes", new[] { "WeaponCategory_Name" });
            DropIndex("dbo.WeaponTypes", new[] { "AttackBonus_Name" });
            DropIndex("dbo.WeaponTypes", new[] { "WeaponCategory_Name" });
            DropIndex("dbo.OutpostWorkerFeats", new[] { "WorkerFeat_Name" });
            DropIndex("dbo.OutpostWorkerFeats", new[] { "Structure_Name" });
            DropIndex("dbo.OutpostUpgrades", new[] { "Structure_Name" });
            DropIndex("dbo.HexBulkRatings", new[] { "BulkRating_Name" });
            DropIndex("dbo.HexBulkRatings", new[] { "Hex_Longitude", "Hex_Latitude" });
            DropIndex("dbo.OutpostBulkResources", new[] { "BulkRating_Name" });
            DropIndex("dbo.OutpostBulkResources", new[] { "BulkResource_Name" });
            DropIndex("dbo.OutpostBulkResources", new[] { "Structure_Name" });
            DropIndex("dbo.HoldingUpgradeBulkResourceRequirements", new[] { "BulkResource_Name" });
            DropIndex("dbo.HoldingUpgradeBulkResourceRequirements", new[] { "Structure_Name", "Upgrade" });
            DropIndex("dbo.HoldingUpgradeBulkResourceBonuses", new[] { "BulkResource_Name" });
            DropIndex("dbo.HoldingUpgradeBulkResourceBonuses", new[] { "Structure_Name", "Upgrade" });
            DropIndex("dbo.HoldingUpgrades", new[] { "CraftingFacilityFeat_Name" });
            DropIndex("dbo.HoldingUpgrades", new[] { "Structure_Name" });
            DropIndex("dbo.HoldingUpgradeTrainerLevels", new[] { "Trainer_Name" });
            DropIndex("dbo.HoldingUpgradeTrainerLevels", new[] { "Structure_Name", "Upgrade" });
            DropIndex("dbo.FeatRankTrainerLevels", new[] { "Trainer_Name" });
            DropIndex("dbo.FeatRankTrainerLevels", new[] { "Feat_Name" });
            DropIndex("dbo.CampUpgrades", new[] { "Structure_Name" });
            DropIndex("dbo.Structures", new[] { "BaseType_Name", "StructureType_Name" });
            DropIndex("dbo.Items", new[] { "WeaponType_Name" });
            DropIndex("dbo.Items", new[] { "GearType_Name" });
            DropIndex("dbo.Items", new[] { "Holding_Name" });
            DropIndex("dbo.Items", new[] { "Outpost_Name" });
            DropIndex("dbo.Items", new[] { "Consumable_Name" });
            DropIndex("dbo.Items", new[] { "Camp_Name" });
            DropIndex("dbo.Items", new[] { "MainRole_Name" });
            DropIndex("dbo.Items", new[] { "BaseType_Name", "ItemType_Name" });
            DropIndex("dbo.RecipeOutputItems", new[] { "Item_Name" });
            DropIndex("dbo.RecipeOutputItemUpgrades", new[] { "Item_Name" });
            DropIndex("dbo.RecipeOutputItemUpgradeKeywords", new[] { "KeywordType_Name", "Keyword_Name" });
            DropIndex("dbo.RecipeOutputItemUpgradeKeywords", new[] { "Item_Name", "Upgrade" });
            DropIndex("dbo.Keywords", new[] { "KeywordType_Name" });
            DropIndex("dbo.FeatRankKeywords", new[] { "KeywordType_Name", "Keyword_Name" });
            DropIndex("dbo.FeatRankKeywords", new[] { "Feat_Name", "Feat_Rank" });
            DropIndex("dbo.FeatRankFeatRequirements", new[] { "RequiredFeat_Name", "RequiredFeat_Rank" });
            DropIndex("dbo.FeatRankFeatRequirements", new[] { "Feat_Name", "Feat_Rank" });
            DropIndex("dbo.FeatEffects", new[] { "EffectDescription_Text" });
            DropIndex("dbo.FeatEffects", new[] { "Feat_Name" });
            DropIndex("dbo.Effects", new[] { "EffectTerm_Term" });
            DropIndex("dbo.Conditions", new[] { "Effect_Name" });
            DropIndex("dbo.EffectDescriptions", new[] { "Condition_Name" });
            DropIndex("dbo.EffectDescriptions", new[] { "Effect_Name" });
            DropIndex("dbo.FeatRankEffects", new[] { "EffectDescription_Text" });
            DropIndex("dbo.FeatRankEffects", new[] { "Feat_Name", "Feat_Rank" });
            DropIndex("dbo.FeatRankCategoryRequirements", new[] { "Feat_Name", "Feat_Rank" });
            DropIndex("dbo.FeatRankAchievementRequirements", new[] { "Achievement_Name", "Achievement_Rank" });
            DropIndex("dbo.FeatRankAchievementRequirements", new[] { "Feat_Name", "Feat_Rank" });
            DropIndex("dbo.FeatRankAbilityRequirements", new[] { "Ability_Name" });
            DropIndex("dbo.FeatRankAbilityRequirements", new[] { "Feat_Name", "Feat_Rank" });
            DropIndex("dbo.FeatRankAbilityBonuses", new[] { "Ability_Name" });
            DropIndex("dbo.FeatRankAbilityBonuses", new[] { "Feat_Name", "Feat_Rank" });
            DropIndex("dbo.FeatRanks", new[] { "Feat_Name" });
            DropIndex("dbo.AchievementRankFeatRequirements", new[] { "Feat_Name", "Feat_Rank" });
            DropIndex("dbo.AchievementRankFeatRequirements", new[] { "Achievement_Name", "Achievement_Rank" });
            DropIndex("dbo.AchievementRankFlagRequirements", new[] { "Achievement_Name", "Achievement_Rank" });
            DropIndex("dbo.AchievementRankCategoryBonuses", new[] { "Achievement_Name", "Achievement_Rank" });
            DropIndex("dbo.EntityTypeMappings", "IX_BaseType_Name_ParentType_Name");
            DropIndex("dbo.EntityTypeMappings", new[] { "BaseType_Name", "ChildType_Name" });
            DropIndex("dbo.EntityTypes", new[] { "BaseType_Name", "ParentType_Name" });
            DropIndex("dbo.AchievementGroups", new[] { "BaseType_Name", "AchievementType_Name" });
            DropIndex("dbo.Achievements", new[] { "AchievementGroup_Name" });
            DropIndex("dbo.Achievements", new[] { "BaseType_Name", "AchievementType_Name" });
            DropIndex("dbo.AchievementRanks", new[] { "Feat_Name" });
            DropIndex("dbo.AchievementRanks", new[] { "Achievement_Name" });
            DropIndex("dbo.Feats", new[] { "SpecificWeapon_Name" });
            DropIndex("dbo.Feats", new[] { "WeaponCategory_Name" });
            DropIndex("dbo.Feats", new[] { "AdvancementFeat_Name" });
            DropIndex("dbo.Feats", new[] { "Role_Name" });
            DropIndex("dbo.Feats", new[] { "BaseType_Name", "FeatType_Name" });
            DropTable("dbo.StockItemStocks");
            DropTable("dbo.Stocks");
            DropTable("dbo.Recipes");
            DropTable("dbo.RecipeIngredients");
            DropTable("dbo.GearTypes");
            DropTable("dbo.AttackBonuses");
            DropTable("dbo.WeaponTypes");
            DropTable("dbo.WeaponCategories");
            DropTable("dbo.OutpostWorkerFeats");
            DropTable("dbo.OutpostUpgrades");
            DropTable("dbo.Hexes");
            DropTable("dbo.HexBulkRatings");
            DropTable("dbo.BulkRatings");
            DropTable("dbo.OutpostBulkResources");
            DropTable("dbo.HoldingUpgradeBulkResourceRequirements");
            DropTable("dbo.BulkResources");
            DropTable("dbo.HoldingUpgradeBulkResourceBonuses");
            DropTable("dbo.HoldingUpgrades");
            DropTable("dbo.HoldingUpgradeTrainerLevels");
            DropTable("dbo.Trainers");
            DropTable("dbo.FeatRankTrainerLevels");
            DropTable("dbo.AdvancementFeats");
            DropTable("dbo.CampUpgrades");
            DropTable("dbo.Structures");
            DropTable("dbo.Roles");
            DropTable("dbo.Items");
            DropTable("dbo.RecipeOutputItems");
            DropTable("dbo.RecipeOutputItemUpgrades");
            DropTable("dbo.RecipeOutputItemUpgradeKeywords");
            DropTable("dbo.KeywordTypes");
            DropTable("dbo.Keywords");
            DropTable("dbo.FeatRankKeywords");
            DropTable("dbo.FeatRankFeatRequirements");
            DropTable("dbo.FeatEffects");
            DropTable("dbo.EffectTerms");
            DropTable("dbo.Effects");
            DropTable("dbo.Conditions");
            DropTable("dbo.EffectDescriptions");
            DropTable("dbo.FeatRankEffects");
            DropTable("dbo.FeatRankCategoryRequirements");
            DropTable("dbo.FeatRankAchievementRequirements");
            DropTable("dbo.FeatRankAbilityRequirements");
            DropTable("dbo.Abilities");
            DropTable("dbo.FeatRankAbilityBonuses");
            DropTable("dbo.FeatRanks");
            DropTable("dbo.AchievementRankFeatRequirements");
            DropTable("dbo.AchievementRankFlagRequirements");
            DropTable("dbo.AchievementRankCategoryBonuses");
            DropTable("dbo.EntityTypeMappings");
            DropTable("dbo.EntityTypes");
            DropTable("dbo.AchievementGroups");
            DropTable("dbo.Achievements");
            DropTable("dbo.AchievementRanks");
            DropTable("dbo.Feats");
        }
    }
}
