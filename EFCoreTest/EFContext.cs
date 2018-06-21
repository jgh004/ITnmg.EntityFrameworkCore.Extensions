using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTest
{
	public class EFContext : DbContext
	{
		public EFContext( DbContextOptions options ) : base( options )
		{
		}


		protected override void OnModelCreating( ModelBuilder modelBuilder )
		{
			base.OnModelCreating( modelBuilder );
			//modelBuilder.ApplyConfiguration<Order>( new ModelConfig() );
			//modelBuilder.ApplyConfiguration( new OrderItem() );

			modelBuilder.ApplyConfigurationFromAssembly( Assembly.GetExecutingAssembly() );
		}
	}
}
