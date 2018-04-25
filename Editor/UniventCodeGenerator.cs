using UnityEditor;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Scripting;

namespace DifferentMethods.Univents
{

    public class UniventCodeGenerator : ScriptableObject, ISerializationCallbackReceiver
    {
        static Type[] CommonTypes = new[] {
            typeof(Transform),
            typeof(Light),
            typeof(Renderer),
            typeof(Collider),
            typeof(Collider2D),
            typeof(Physics),
            typeof(Physics2D),
            typeof(Rigidbody),
            typeof(Rigidbody2D),
            typeof(Animator),
            typeof(AudioSource),
            typeof(Camera),
            typeof(Animation),
            typeof(ParticleSystem),
            typeof(TrailRenderer)
        };

        static UniventCodeGenerator _instance;
        public static UniventCodeGenerator Instance
        {
            get
            {
                if (!_instance)
                    _instance = Resources.FindObjectsOfTypeAll<UniventCodeGenerator>().FirstOrDefault();
                if (!_instance)
                {
                    var guids = AssetDatabase.FindAssets("t:UniventCodeGenerator");
                    if (guids.Length > 0)
                    {
                        _instance = AssetDatabase.LoadAssetAtPath<UniventCodeGenerator>(AssetDatabase.GUIDToAssetPath(guids[0]));
                        if (_instance == null)
                            Debug.LogError("Could not find Univent Code Generator asset.");
                    }
                }

                return _instance;
            }
        }
        public string assemblyName = "Univent";
        Dictionary<string, MetaMethodInfo> methods = new Dictionary<string, MetaMethodInfo>();
        [HideInInspector] [SerializeField] MetaMethodInfo[] _methods;


        [ContextMenu("Init With Common Classes")]
        public void AddAllComponents()
        {
            methods.Clear();
            foreach (var t in CommonTypes)
                if (t.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length == 0)
                    if (t == typeof(GameObject) || t.IsSubclassOf(typeof(Component)))
                        foreach (var mi in t.GetMethods())
                            if (IsSupportedMethod(mi))
                                AddMethod(t, mi, compile: false);

            Compile();
        }

        public void AddMethod(Type type, MethodInfo mi, bool compile = true)
        {
            var className = ClassRegister.GetClassName(type, mi);
            if (methods.ContainsKey(className)) return;
            var mmi = new MetaMethodInfo();
            mmi.name = mi.Name;
            mmi.className = className;
            mmi.niceName = ClassRegister.GetNiceName(mi);
            mmi.parameterTypeNames = ClassRegister.GetParameterTypeNames(mi);
            mmi.type = mi.DeclaringType.AssemblyQualifiedName;
            mmi.returnType = mi.ReturnType.AssemblyQualifiedName;
            methods.Add(className, mmi);
            EditorUtility.SetDirty(this);
            if (compile) Compile();
        }

        [ContextMenu("Compile")]
        void Compile()
        {
            var domain = Thread.GetDomain();
            var asmName = new AssemblyName();
            asmName.Name = assemblyName;
            string fileName = $"{assemblyName}.dll";
            var outputPath = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(this)) + "/../Generated/";
            var asmBuilder = domain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave, outputPath);
            var classCtorInfo = typeof(PreserveAttribute).GetConstructor(Type.EmptyTypes);
            var caBuilder = new CustomAttributeBuilder(classCtorInfo, new object[] { });
            asmBuilder.SetCustomAttribute(caBuilder);
            var modBuilder = asmBuilder.DefineDynamicModule(assemblyName, fileName, false);
            foreach (var i in methods.Values)
                CreateType(modBuilder, i);
            asmBuilder.Save(fileName);
            AssetDatabase.Refresh();
        }

        Type CreateType(ModuleBuilder modBuilder, MetaMethodInfo mmi)
        {
            var mi = mmi.GetMethodInfo();
            if (mi == null)
            {
                // Debug.LogWarning($"Skipping {mmi.niceName}, no longer exists.");
                return null;
            }
            var className = mmi.className;
            // Debug.Log("Creating Class: " + className);
            var typeBuilder = modBuilder.DefineType(className, TypeAttributes.Public);
            var classCtorInfo = typeof(UniventGeneratedClassAttribute).GetConstructor(Type.EmptyTypes);
            var caBuilder = new CustomAttributeBuilder(classCtorInfo, new object[] { });
            typeBuilder.SetCustomAttribute(caBuilder);
            if (mi.ReturnType == typeof(void))
            {
                typeBuilder.AddInterfaceImplementation(typeof(IEncapsulatedAction));
                CreateMethod(mi, typeBuilder);
            }
            else
            {
                var itype = typeof(IEncapsulatedFunction<>).MakeGenericType(mi.ReturnType);
                typeBuilder.AddInterfaceImplementation(itype);
                CreateFunction(mi, typeBuilder, itype);
            }
            return typeBuilder.CreateType();
        }

        static void CreateMethod(MethodInfo mi, TypeBuilder typeBuilder)
        {
            var mb = typeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Virtual, CallingConventions.Standard, typeof(void), Type.EmptyTypes);
            var il = mb.GetILGenerator();
            var componentField = typeBuilder.DefineField("__component", mi.DeclaringType, FieldAttributes.Public);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, componentField);
            foreach (var p in mi.GetParameters())
            {
                var field = typeBuilder.DefineField(p.Name, p.ParameterType, FieldAttributes.Public);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, field);
            }
            il.Emit(OpCodes.Call, mi);
            il.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(mb, typeof(IEncapsulatedAction).GetMethod("Invoke"));
        }

        static void CreateFunction(MethodInfo mi, TypeBuilder typeBuilder, Type itype)
        {
            var mb = typeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Virtual, CallingConventions.Standard, mi.ReturnType, Type.EmptyTypes);
            var il = mb.GetILGenerator();
            var componentField = typeBuilder.DefineField("__component", mi.DeclaringType, FieldAttributes.Public);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, componentField);
            foreach (var p in mi.GetParameters())
            {
                var field = typeBuilder.DefineField(p.Name, p.ParameterType, FieldAttributes.Public);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, field);
            }
            il.Emit(OpCodes.Call, mi);
            il.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(mb, itype.GetMethod("Invoke"));
        }

        public void OnBeforeSerialize()
        {
            _methods = methods.Values.ToArray();
        }

        public void OnAfterDeserialize()
        {
            if (_methods != null)
            {
                methods = new Dictionary<string, MetaMethodInfo>();
                foreach (var i in _methods)
                    methods.Add(i.className, i);
            }
        }

        public static bool IsSupportedMethod(MethodInfo m)
        {
            if (m.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length > 0)
                return false;
            if (m.ContainsGenericParameters)
                return false;
            // if (m.IsSpecialName && m.Name.StartsWith("get_"))
            //     return false;
            if (!m.IsPublic)
                return false;
            if (m.IsStatic || m.IsAbstract || m.IsConstructor || m.IsGenericMethod)
                return false;
            if (m.ReturnType != typeof(void))
                return false;
            foreach (var p in m.GetParameters())
            {
                if (!IsSupportedParameter(p.ParameterType))
                    return false;
            }
            return true;
        }

        public static bool IsSupportedFunction(MethodInfo m, Type returnType)
        {
            if (m.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length > 0)
                return false;
            if (m.ContainsGenericParameters)
                return false;
            if (!m.IsPublic)
                return false;
            if (m.IsStatic || m.IsAbstract || m.IsConstructor || m.IsGenericMethod)
                return false;
            if (m.ReturnType != returnType)
                return false;
            foreach (var p in m.GetParameters())
            {
                if (!IsSupportedParameter(p.ParameterType))
                    return false;
            }
            return true;
        }

        public static bool IsSupportedParameter(Type t)
        {
            if (t.IsArray || t.IsByRef || t.IsNotPublic || t.IsInterface || t.IsPointer)
                return false;
            if (t.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length > 0)
                return false;
            if (t == typeof(string))
                return true;
            if (t == typeof(int))
                return true;
            if (t == typeof(bool))
                return true;
            if (t == typeof(float))
                return true;
            if (t == typeof(Vector3))
                return true;
            if (t == typeof(Vector2))
                return true;
            if (t == typeof(Vector4))
                return true;
            if (t == typeof(Color))
                return true;
            if (t.IsEnum)
                return true;
            if (t == typeof(LayerMask))
                return true;
            if (t.IsSubclassOf(typeof(UnityEngine.Object)))
                return true;
            return false;
        }
    }
}