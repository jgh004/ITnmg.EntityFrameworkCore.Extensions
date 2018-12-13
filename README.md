# ITnmg.EntityFrameworkCore.Extensions
EntityFrameworkCore 的扩展，通过反射注册实体模型。

# Install

Run the following command in the Package Manager Console.  
在 nuget 包管理器控制台输入以下命令

    PM> Install-Package ITnmg.EntityFrameworkCore.Extensions

# Getting Started

```c#
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
