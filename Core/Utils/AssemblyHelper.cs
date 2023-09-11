using System.Reflection;
namespace ZLC.Utils
{
    /// <summary>
    /// 程序集工具类
    /// </summary>
    public sealed class AssemblyHelper
    {
        /// <summary>
        /// 查找所有使用T标记的对象（不包含抽象类和接口）
        /// </summary>
        /// <typeparam name="T">查找的Attribute类型</typeparam>
        /// <param name="assembly">待查找的程序集</param>
        /// <param name="temp">查询结果的集合</param>
        /// <param name="inherit">是否父亲有使用Attribute标记就添加到合集中</param>
        public static void GetAttributedTypes<T>(Assembly assembly, List<Type> temp, bool inherit = false) where T : Attribute
        {
            GetAttributedTypes(assembly, temp, typeof(T), inherit);
        }

        /// <summary>
        /// referencedName是否被assembly程序集引用了
        /// </summary>
        /// <returns>true:引用了 false:为引用</returns>
        public static bool IsAssemblyReferenced(Assembly assembly, string referencedName)
        {
            var referencedAssemblies = assembly.GetReferencedAssemblies();
            foreach (var referencedAssembly in referencedAssemblies) {
                if (referencedAssembly.Name == referencedName) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 查找所有使用ToolDataAttribute/T标记的对象（不包含抽象类和接口）
        /// </summary>
        /// <param name="assemblies">待查找的程序集数组</param>
        /// <param name="target">被引用的程序集，未引用target的程序集将被过滤</param>
        /// <param name="temp">查询结果的集合</param>
        /// <typeparam name="T">查找的Attribute类型</typeparam>
        public static void GetAttributedTypes<T>(Assembly[] assemblies, Assembly target, List<Type> temp) where T : Attribute
        {
            GetAttributedTypes(assemblies, target, temp, typeof(T));
        }
        
        /// <summary>
        /// 获取程序集中的所有T的子类型
        /// </summary>
        /// <param name="assembly">待查找的程序集</param>
        /// <param name="temp">查询结果的集合</param>
        /// <typeparam name="T">查找的类型</typeparam>
        public static void GetAllChildType<T>(Assembly assembly, List<Type> temp)
        {
            GetAllChildType(assembly, temp, typeof(T));
        }

        /// <summary>
        /// 获取程序集中的所有T的子类型
        /// </summary>
        /// <param name="assemblies">待查找的程序集数组</param>
        /// <param name="temp">查询结果的集合</param>
        /// <typeparam name="T">查找的类型</typeparam>
        public static void GetAllChildType<T>(Assembly[] assemblies, List<Type> temp)
        {
            GetAllChildType(assemblies, temp, typeof(T));
        }

        /// <summary>
        /// 查找所有使用ToolDataAttribute/T标记的对象（不包含抽象类和接口）
        /// </summary>
        /// <param name="assembly">待查找的程序集</param>
        /// <param name="temp">查询结果的集合</param>
        /// <param name="attributeType">查找的Attribute类型</param>
        /// <param name="inherit">true:查找父对象有使用Attribute标记的类型</param>
        public static void GetAttributedTypes(Assembly assembly, List<Type> temp, Type attributeType, bool inherit = false)
        {
            if (temp == null) temp = new List<Type>();
            var types = assembly.GetTypes();
            foreach (var type in types) {
                if (type.IsAbstract || type.IsInterface) continue;
                if (type.GetCustomAttribute(attributeType, inherit) == null) continue;
                temp.Add(type);
            }
        }

        /// <summary>
        /// 查找所有使用<paramref name="attributeType"/>标记的对象（不包含抽象类和接口）
        /// </summary>
        /// <param name="assemblies">待查找的程序集</param>
        /// <param name="target">判断<paramref name="assemblies"/>是否引用了<paramref name="target"/></param>
        /// <param name="temp">查询结果的集合</param>
        /// <param name="attributeType">查找的Attribute类型</param>
        public static void GetAttributedTypes(Assembly[] assemblies, Assembly target, List<Type> temp, Type attributeType)
        {
            if(assemblies == null) return;
            var referencedName = target.GetName().Name;
            if (temp == null) temp = new List<Type>();
            foreach (var assembly in assemblies) {
                if (!IsAssemblyReferenced(assembly, referencedName)) continue;
                GetAttributedTypes(assembly, temp, attributeType);
            }
        }

        /// <summary>
        /// 获取程序集中的所有<paramref name="targetType"/>的子类型
        /// </summary>
        /// <param name="assembly">待查找的程序集</param>
        /// <param name="temp">查询结果的集合</param>
        /// <param name="targetType">查找的类型</param>
        public static void GetAllChildType(Assembly assembly, List<Type> temp, Type targetType)
        {
            if (temp == null) temp = new List<Type>();
            var types = assembly.GetTypes();
            foreach (var type in types) {
                if (type.IsAbstract || type.IsInterface) continue;
                if (!TypeHelper.IsChildOf(type, targetType)) continue;
                temp.Add(type);
            }
        }

        /// <summary>
        /// 获取程序集中的所有<paramref name="targetType"/>的子类型
        /// </summary>
        /// <param name="assemblies">待查找的程序集数组</param>
        /// <param name="temp">查询结果的集合</param>
        /// <param name="targetType">查找的类型</param>
        public static void GetAllChildType(Assembly[] assemblies, List<Type> temp, Type targetType)
        {
            if(assemblies == null) return;
            foreach (var assembly in assemblies) {
                GetAllChildType(assembly, temp, targetType);
            }
        }
    }
}