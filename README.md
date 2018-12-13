# ITnmg.EntityFrameworkCore.Extensions
EntityFrameworkCore 的扩展，通过反射注册实体模型。

# Install

Run the following command in the Package Manager Console.  
在 nuget 包管理器控制台输入以下命令

    PM> Install-Package ITnmg.EntityFrameworkCore.Extensions

# Getting Started

```c#
public class Order : IEntityTypeConfiguration<Order>
{
    public string Id { get; set; }

    public void Configure( EntityTypeBuilder<Order> builder )
    {
        base.Configure( builder );
        builder.Property( m => m.Id ).HasMaxLength( 255 ).IsRequired();
    }
}

public class EFContext : DbContext
{
    public EFContext( DbContextOptions options ) : base( options )
    {
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating( modelBuilder );
        modelBuilder.ApplyConfigurationFromAssembly( Assembly.GetExecutingAssembly() );
    }
}
```
