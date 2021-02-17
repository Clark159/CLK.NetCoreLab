using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleServiceLab
{
    public class ServiceActivator
    {
        // Methods
        public static void ExecuteMethod<TInstanceType>(IServiceProvider serviceProvider, string methodName, Dictionary<string, object> parameters = null) where TInstanceType : class
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException(nameof(serviceProvider));
            if (string.IsNullOrEmpty(methodName) == true) throw new ArgumentException(nameof(methodName));

            #endregion

            // ExecuteMethod
            ExecuteMethod<TInstanceType, object>(serviceProvider, methodName, parameters);
        }

        public static TResultType ExecuteMethod<TInstanceType, TResultType>(IServiceProvider serviceProvider, string methodName, Dictionary<string, object> parameters = null) where TInstanceType : class
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException(nameof(serviceProvider));
            if (string.IsNullOrEmpty(methodName) == true) throw new ArgumentException(nameof(methodName));

            #endregion

            // Parameters
            if (parameters == null) parameters = new Dictionary<string, object>();

            // Instance
            var instance = CreateInstance<TInstanceType>(serviceProvider);
            if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

            // MethodInfo
            var methodInfo = typeof(TInstanceType).FindMethod(methodName);
            if (methodInfo == null) throw new InvalidOperationException($"{nameof(methodInfo)}=null");

            // ParameterValueArray
            var parameterValueArray = CreateParameterValueArray(serviceProvider, methodInfo.GetParameters(), parameters);
            if (parameterValueArray == null) throw new InvalidOperationException($"{nameof(parameterValueArray)}=null");

            // ExecuteMethod
            return (TResultType)methodInfo.Invoke
            (
                obj: instance,
                parameters: parameterValueArray,
                invokeAttr: BindingFlags.DoNotWrapExceptions,
                binder: null,
                culture: null
            );
        }

        public static TInstanceType CreateInstance<TInstanceType>(IServiceProvider serviceProvider, Dictionary<string, object> parameters = null) where TInstanceType : class
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException(nameof(serviceProvider));

            #endregion

            // Parameters
            if (parameters == null) parameters = new Dictionary<string, object>();

            // ConstructorInfo
            var constructorInfo = typeof(TInstanceType).FindConstructor();
            if (constructorInfo == null) throw new InvalidOperationException($"{nameof(constructorInfo)}=null");

            // ParameterValueArray
            var parameterValueArray = CreateParameterValueArray(serviceProvider, constructorInfo.GetParameters(), parameters);
            if (parameterValueArray == null) throw new InvalidOperationException($"{nameof(parameterValueArray)}=null");

            // Create
            return (TInstanceType)constructorInfo.Invoke
            (
               parameters: parameterValueArray,
               invokeAttr: BindingFlags.DoNotWrapExceptions,
               culture: null,
               binder: null
            );
        }

        private static object[] CreateParameterValueArray(IServiceProvider serviceProvider, ParameterInfo[] parameterInfoArray, Dictionary<string, object> parameters)
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException(nameof(serviceProvider));
            if (parameterInfoArray == null) throw new ArgumentException(nameof(parameterInfoArray));
            if (parameters == null) throw new ArgumentException(nameof(parameterInfoArray));

            #endregion

            // ParameterValueArray
            var parameterValueArray = new object[parameterInfoArray.Length];
            for (var i = 0; i < parameterInfoArray.Length; i++)
            {
                // ParameterInfo
                var parameterInfo = parameterInfoArray[i];
                if (parameterInfo == null) throw new InvalidOperationException($"{nameof(parameterInfo)}=null");

                // ParameterValue
                {
                    // IServiceProvider
                    if (parameterInfo.ParameterType == typeof(IServiceProvider)) { parameterValueArray[i] = serviceProvider; continue; }

                    // Parameters
                    if (parameters.ContainsKey(parameterInfo.Name) == true) { parameterValueArray[i] = parameters[parameterInfo.Name]; continue; }

                    // Injection
                    parameterValueArray[i] = serviceProvider.GetService(parameterInfo.ParameterType); continue;
                }
            }

            // Return
            return parameterValueArray;
        }
    }
}
