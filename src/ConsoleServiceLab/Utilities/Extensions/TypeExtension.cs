using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServiceLab
{
    public static class TypeExtension
    {
        // Methods
        public static ConstructorInfo FindConstructor(this Type instanceType)
        {
            #region Contracts

            if (instanceType == null) throw new ArgumentException(nameof(instanceType));

            #endregion

            // Require
            if (instanceType.IsAbstract == true) throw new InvalidOperationException($"The '{instanceType.FullName}' is abstract.");

            // ConstructorInfoList
            var constructorInfoList = instanceType.GetConstructors().ToList();
            if (constructorInfoList.Count == 0) throw new InvalidOperationException($"Not having constructor in the type '{instanceType.FullName}' is not supported.");
            if (constructorInfoList.Count >= 2) throw new InvalidOperationException($"Having multiple constructor in the type '{instanceType.FullName}' is not supported.");

            // ConstructorInfo
            var constructorInfo = constructorInfoList.First();
            if (constructorInfo == null) throw new InvalidOperationException($"{nameof(constructorInfo)}=null");

            // Return
            return constructorInfo;
        }

        public static MethodInfo FindMethod(this Type instanceType, string methodName)
        {
            #region Contracts

            if (instanceType == null) throw new ArgumentException(nameof(instanceType));
            if (string.IsNullOrEmpty(methodName) == true) throw new ArgumentException(nameof(methodName));

            #endregion

            // MethodInfoList
            var methodInfoList = instanceType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(method => method.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (methodInfoList.Count == 0) return null;
            if (methodInfoList.Count >= 2) throw new InvalidOperationException($"Having multiple overloads of method '{methodName}' in the type '{instanceType.FullName}' is not supported.");

            // MethodInfo
            var methodInfo = methodInfoList.First();
            if (methodInfo == null) throw new InvalidOperationException($"{nameof(methodInfo)}=null");
           
            // Return
            return methodInfo;
        }
    }
}
