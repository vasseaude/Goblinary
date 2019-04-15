namespace Goblinary.WikiData.SqlServer
{
	using System.Data.Entity;
	using System.Data.Entity.ModelConfiguration.Conventions;
	using Model;

	public class WikiDataContext : DbContext
	{
		public WikiDataContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			Configuration.LazyLoadingEnabled = true;
		}

		public WikiDataContext()
#if DEBUG
			: this("name=LocalWikiDb")
#elif PUBLIC_TEST
			: this("name=PublicTestWikiDb")
#else
            : this("name=WikiDb")
#endif
		{
#if DEBUG
			//this.Database.Log = s => Debug.WriteLine(s);
#endif
		}

		public virtual DbSet<Feat> Feats { get; set; }
		public virtual DbSet<Structure> Structures { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Properties<decimal>().Configure(x => x.HasPrecision(18, 3));
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
		}
	}
}