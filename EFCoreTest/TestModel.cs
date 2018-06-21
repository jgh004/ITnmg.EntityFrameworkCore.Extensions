using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;

namespace EFCoreTest
{
	public class BaseModel
	{
		public string Id { get; set; }
	}

	public class Order : BaseModel
	{
		public string Name { get; set; }

		public virtual List<OrderItem> Items { get; set; } = new List<OrderItem>();

	}

	public class OrderItem : BaseModel, IEntityTypeConfiguration<OrderItem>
    {
		public string Name { get; set; }

		public virtual Order OrderInfo { get; set; }

        public void Configure( EntityTypeBuilder<OrderItem> builder )
        {
        }
    }

	public class ModelConfig : IEntityTypeConfiguration<Order>
	{
		public void Configure( EntityTypeBuilder<Order> builder )
		{
		}
    }
}
