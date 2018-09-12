using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
	/// <summary>
	/// ModelBuild 类的扩展方法
	/// </summary>
	public static class ModelBuildExtension
	{
		/// <summary>
		/// 封闭泛型 ApplyConfiguration 方法的签名
		/// </summary>
		public static MethodInfo ApplyConfigurationMethod { get; set; }

		static ModelBuildExtension()
		{
			//modelBuilder的类型
			var modelBuilderType = typeof( ModelBuilder );
			//指定封闭泛型 ApplyConfiguration 方法的签名
			ApplyConfigurationMethod = modelBuilderType.GetMethods()
				.Where( m =>
				{
					return m.IsGenericMethod && m.Name == nameof( ModelBuilder.ApplyConfiguration )
						&& m.GetParameters().Any( s => s.ParameterType.GetGenericTypeDefinition() == typeof( IEntityTypeConfiguration<> ) )
						&& m.GetParameters().Count() == 1;
				} )
				.First();
		}

		/// <summary>
		/// 注册指定程序集中的实体类，实体类必须继承自 IEntityTypeConfiguration&lt;TEntity&gt; 接口
		/// </summary>
		/// <param name="modelBuilder"></param>
		/// <param name="assembly">程序集实例,程序集中必须有继承自 IEntityTypeConfiguration&lt;TEntity&gt; 接口的类型.</param>
		/// <returns></returns>
		/// <exception cref="Microsoft.CSharp.RuntimeBinder.RuntimeBinderException">当程序集中有某个类继承了多个 IEntityTypeConfiguration&lt;TEntity&gt; 接口时引发</exception>
		public static ModelBuilder ApplyConfigurationFromAssembly( this ModelBuilder modelBuilder, Assembly assembly )
		{
			//找到所有实现 IEntityTypeConfiguration<T> 接口的类，排除抽象类和泛型类。
			var configs = assembly.GetTypes()
				.Where( f =>
				{
					return f.IsClass && !f.IsAbstract && !f.IsGenericType
						&& f.GetInterfaces().Where( i => i.GetGenericTypeDefinition() == typeof( IEntityTypeConfiguration<> )
						&& i.GetGenericArguments().Any( t => t == f ) ).Count() > 0;
				} )
				.Select( t => new
				{
					//IEntityTypeConfiguration 接口的泛型参数，即实体类型
					EntityType = t.GetInterfaces().First( i => i.GetGenericTypeDefinition() == typeof( IEntityTypeConfiguration<> )
						&& i.GetGenericArguments().Any( k => k == t ) ).GenericTypeArguments[0],
					//实现 IEntityTypeConfiguration 接口的配置类型
					ConfigType = t
				} );

			foreach ( var c in configs )
			{
				//通过 MakeGenericMethod 转换为泛型方法
				var method = ApplyConfigurationMethod.MakeGenericMethod( c.EntityType );
				method.Invoke( modelBuilder, new[] { Activator.CreateInstance( c.ConfigType ) } );
			}

			return modelBuilder;
		}
	}
}
