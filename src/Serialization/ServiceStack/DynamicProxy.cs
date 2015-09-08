using System;
using System.Linq;
using System.Reflection;
#if !SILVERLIGHT && !MONOTOUCH
using System.Reflection.Emit;

namespace ServiceStack.Text {
    public static class DynamicProxy {
        public static T GetInstanceFor<T> () {
            return (T)GetInstanceFor(typeof(T));
        }

        static readonly ModuleBuilder ModuleBuilder;
        static readonly AssemblyBuilder DynamicAssembly;

        public static object GetInstanceFor (Type targetType) {
            lock (DynamicAssembly)
            {
                var constructedType = DynamicAssembly.GetType(ProxyName(targetType)) ?? GetConstructedType(targetType);
                var instance = Activator.CreateInstance(constructedType);
                return instance;
            }
        }

        static string ProxyName(Type targetType)
        {
            return targetType.Name + "Proxy";
        }

        static DynamicProxy () {
            var assemblyName = new AssemblyName("DynImpl");
            DynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder = DynamicAssembly.DefineDynamicModule("DynImplModule");
        }

        static Type GetConstructedType (Type targetType) {
            var typeBuilder = ModuleBuilder.DefineType(targetType.Name + "Proxy", TypeAttributes.Public);

            var ctorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                new Type[] { });
            var ilGenerator = ctorBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ret);

            IncludeType(targetType, typeBuilder);

            foreach (var face in targetType.GetInterfaces())
                IncludeType(face, typeBuilder);

            return typeBuilder.CreateType();
        }

        static void IncludeType (Type typeOfT, TypeBuilder typeBuilder) {
            var methodInfos = typeOfT.GetMethods();
            foreach (var methodInfo in methodInfos) {
                if (methodInfo.Name.StartsWith("set_", StringComparison.Ordinal)) continue; // we always add a set for a get.

                if (methodInfo.Name.StartsWith("get_", StringComparison.Ordinal))
                {
                    BindProperty(typeBuilder, methodInfo);
                } else {
                    BindMethod(typeBuilder, methodInfo);
                }
            }

            typeBuilder.AddInterfaceImplementation(typeOfT);
        }

        static void BindMethod (TypeBuilder typeBuilder, MethodInfo methodInfo) {
            var methodBuilder = typeBuilder.DefineMethod(
                methodInfo.Name,
                MethodAttributes.Public | MethodAttributes.Virtual,
                methodInfo.ReturnType,
                methodInfo.GetParameters().Select(p => p.GetType()).ToArray()
                );
            var methodILGen = methodBuilder.GetILGenerator();
            if (methodInfo.ReturnType == typeof(void)) {
                methodILGen.Emit(OpCodes.Ret);
            } else {
                if (methodInfo.ReturnType.IsValueType || methodInfo.ReturnType.IsEnum) {
                    MethodInfo getMethod = typeof(Activator).GetMethod("CreateInstance",
                                                                       new[] { typeof(Type) });
                    LocalBuilder lb = methodILGen.DeclareLocal(methodInfo.ReturnType);
                    methodILGen.Emit(OpCodes.Ldtoken, lb.LocalType);
                    methodILGen.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
                    methodILGen.Emit(OpCodes.Callvirt, getMethod);
                    methodILGen.Emit(OpCodes.Unbox_Any, lb.LocalType);
                } else {
                    methodILGen.Emit(OpCodes.Ldnull);
                }
                methodILGen.Emit(OpCodes.Ret);
            }
            typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
        }

        public static void BindProperty (TypeBuilder typeBuilder, MethodInfo methodInfo) {
            // Backing Field
            string propertyName = methodInfo.Name.Replace("get_", "");
            Type propertyType = methodInfo.ReturnType;
            FieldBuilder backingField = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            //Getter
            MethodBuilder backingGet = typeBuilder.DefineMethod("get_" + propertyName, MethodAttributes.Public |
                MethodAttributes.SpecialName | MethodAttributes.Virtual |
                MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = backingGet.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, backingField);
            getIl.Emit(OpCodes.Ret);


            //Setter
            MethodBuilder backingSet = typeBuilder.DefineMethod("set_" + propertyName, MethodAttributes.Public |
                MethodAttributes.SpecialName | MethodAttributes.Virtual |
                MethodAttributes.HideBySig, null, new[] { propertyType });

            ILGenerator setIl = backingSet.GetILGenerator();

            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, backingField);
            setIl.Emit(OpCodes.Ret);

            // Property
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, null);
            propertyBuilder.SetGetMethod(backingGet);
            propertyBuilder.SetSetMethod(backingSet);
        }
    }
}
#endif
