using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EFCoreTest
{
	public class DesignTimeCotextFactory : IDesignTimeDbContextFactory<EFContext>
	{
		public EFContext CreateDbContext( string[] args )
		{
			var config = new ConfigurationBuilder()
				.SetBasePath( Directory.GetCurrentDirectory() )
				.AddJsonFile( "appsettings.json" ).Build();

			return new EFContext( new DbContextOptionsBuilder().UseMySql( config.GetConnectionString( "DefaultConnection" ) ).Options );
		}
	}
}
