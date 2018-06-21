using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
			var config = new ConfigurationBuilder()
				.SetBasePath( Directory.GetCurrentDirectory())
				.AddJsonFile( "appsettings.json" ).Build();

			var build = new DbContextOptionsBuilder()
				.UseMySql( config.GetConnectionString( "DefaultConnection" ) )
				.UseLazyLoadingProxies();
			var db = new EFContext( build.Options );

			if ( db.Database.GetPendingMigrations().Count() > 0 )
			{
				db.Database.Migrate();
			}

			db.Set<Order>().Add( new Order() { Name = DateTime.Now.ToString(), Items = new List<OrderItem>() { new OrderItem() { Name = DateTime.Now.ToString() } } } );

			Console.WriteLine( db.SaveChanges() );

			var o = db.Set<Order>().FirstOrDefault();
			Console.WriteLine( o?.Id + " " + o?.Items.Count );

			Console.ReadKey();
        }
    }
}
