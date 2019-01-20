namespace Goblinary.Data.SqlServer
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.ModelConfiguration.Conventions;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Goblinary.Model;

	public class SqlContext : DbContext
	{
		public SqlContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			this.Configuration.LazyLoadingEnabled = true;
		}

		public SqlContext()
#if DEBUG
			: base("name=LocalDb")
#else
			: base("name=WikiDb")
#endif
		{
		}

		public virtual DbSet<AdvancementRankFact> AdvancementRankFacts { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

			modelBuilder.Entity<AdvancementRankAbilityBonus>()
				.Property(x => x.BonusScore)
				.HasPrecision(18, 4);
		}
	}
}
