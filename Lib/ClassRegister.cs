

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace DifferentMethods.Univents
{
    public static class ClassRegister
    {
        static Dictionary<string, Type> items;

        public static string GetClassName(Type type, MethodInfo mi)
        {
            var niceAssemblyName = string.Join("", from i in type.Assembly.GetName().Name where char.IsLetterOrDigit(i) select i);
            return $"{niceAssemblyName}_{type.Name}_{mi.Name}_" + string.Join("_", (from i in mi.GetParameters() select i.ParameterType.Name));
        }

        public static string GetNiceName(System.Type type, MethodInfo mi)
        {
            var name = mi.Name;
            if (mi.IsSpecialName && mi.Name.StartsWith("set_"))
                name = $"Set {name.Substring(4)} =";
            return $"{type.Name}.{name} ({string.Join(", ", from i in mi.GetParameters() select i.Name)})";
        }

        public static string GetNiceName(MethodInfo mi)
        {
            return GetNiceName(mi.DeclaringType, mi);
        }

        public static string[] GetParameterTypeNames(MethodInfo mi)
        {
            return (from i in mi.GetParameters() select i.ParameterType.AssemblyQualifiedName).ToArray();
        }

        public static T CreateInstance<T>(string className) where T : IEncapsulatedMethodCall
        {
            if (string.IsNullOrEmpty(className)) return default(T);
            System.Type type;
            if (items.TryGetValue(className, out type))
            {
                try
                {
                    if (type.GetInterfaces().Contains(typeof(T)))
                        return (T)System.Activator.CreateInstance(type);
                }
                catch (TypeLoadException)
                {
                    return default(T);
                }
            }
            return default(T);
        }

        static ClassRegister()
        {
            items = new Dictionary<string, Type>();
            foreach (var ass in Thread.GetDomain().GetAssemblies())
            {
                try
                {
                    foreach (var type in ass.GetTypes())
                    {
                        var attr = type.GetCustomAttribute(typeof(UniventGeneratedClassAttribute));
                        if (attr != null)
                        {
                            items[type.Name] = type;
                        }
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                    Debug.LogWarning($"Could not load types from {ass}.");
                }
            }
        }

    }



}