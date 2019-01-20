namespace Goblinary.CharacterData.SqlServer
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.Data.Entity;
	using System.Data.Entity.Core.Metadata.Edm;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.Migrations;
	using System.Data.Entity.ModelConfiguration.Conventions;
	using System.Data.SqlClient;
	using System.Linq;

	using Goblinary.CharacterData.Model;

	public class CharacterDataContext : DbContext
	{
		public CharacterDataContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			this.Configuration.LazyLoadingEnabled = true;
		}

		public CharacterDataContext()
#if DEBUG
			: this("name=LocalCharacterDb")
#elif PUBLIC_TEST
			: this("name=PublicTestCharacterDb")
#else
            : this("name=CharacterDb")
#endif
		{
		}

		public virtual DbSet<Character> Characters { get; set; }
	}
}