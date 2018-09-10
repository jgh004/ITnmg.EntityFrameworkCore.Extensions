using System;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// ModelBuild 类的扩展方法
    /// </summary>
	public static class ModelBuildExtension
    {
        /// <summary>
        /// 注册指定程序集中的实体类，实体类必须继承自 IEntityTypeConfiguration&lt;TEntity&gt; 接口
        /// </summary>
        /// <param name="modelBuild"></param>
        /// <param name="assembly">程序集实例,程序集中必须有继承自 IEntityTypeConfiguration&lt;TEntity&gt; 接口的类型, 且每个类只能继承一个此接口.</param>
        /// <returns></returns>
        /// <exception cref="Microsoft.CSharp.RuntimeBinder.RuntimeBinderException">当程序集中有某个类继承了多个 IEntityTypeConfiguration&lt;TEntity&gt; 接口时引发</exception>
        public static ModelBuilder ApplyConfigurationFromAssembly( this ModelBuilder modelBuild, Assembly assembly )
        {
            //找到所有实现 IEntityTypeConfiguration<T> 接口的类，排除抽象类和泛型类。
            var configs = assembly.GetExportedTypes()
                .SelectMany( f =>
                {
                    return f.GetInterfaces().Where( i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof( IEntityTypeConfiguration<> ) );
                }
                , ( t, c ) => !t.IsAbstract && !t.IsGenericType ? new { ConfigType = t, Arguments = c.GetGenericArguments() } : null )
                .Where( f => f != null )
                .GroupBy( f => f.ConfigType )
                .Select( f => new { ConfigType = f.Key, Count = f.Count() } );

            foreach ( var t in configs )
            {
                if ( t.Count > 1 )
                {
                    throw new RuntimeBinderException( $"Class \"{t.ConfigType.FullName}\" inherits multiple \"IEntityTypeConfiguration<TEntity>\" interfaces." );
                }

                modelBuild.ApplyConfiguration( (dynamic)Activator.CreateInstance( t.ConfigType ) );
            }

            return modelBuild;
        }
    }
}