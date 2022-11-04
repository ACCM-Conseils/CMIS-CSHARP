using System;
using System.Collections.Generic;

namespace CmisObjectModel.Attributes
{
    /// <summary>
   /// Class contains the CmisTypeName of a class, the TargetTypeName that is
   /// the type this class is designed for, and the DefaultElementName that is
   /// the node name within a xml-file, in which this class is presented
   /// </summary>
   /// <remarks></remarks>
    public class CmisTypeInfoAttribute : Attribute
    {

        public CmisTypeInfoAttribute(string cmisTypeName, string targetTypeName, string defaultElementName)
        {
            CmisTypeName = cmisTypeName is null ? "" : cmisTypeName;
            TargetTypeName = targetTypeName is null ? "" : targetTypeName;
            DefaultElementName = defaultElementName is null ? "" : defaultElementName;
        }

        public readonly string CmisTypeName;
        public readonly string DefaultElementName;
        public readonly string TargetTypeName;

        #region Explore environment
        /// <summary>
      /// Searches for types derived from TBase, constructible by default constructor and
      /// supporting the CmisTypeInfoAttribute
      /// </summary>
      /// <typeparam name="TBase"></typeparam>
      /// <param name="factories"></param>
      /// <param name="genericTypeDefinition"></param>
      /// <param name="types"></param>
      /// <remarks></remarks>
        public static void ExploreTypes<TBase>(Dictionary<string, CmisObjectModel.Common.Generic.Factory<TBase>> factories, Type genericTypeDefinition, params Type[] types) where TBase : CmisObjectModel.Serialization.XmlSerializable
        {
            var baseType = typeof(TBase);

            lock (factories)
            {
                if (types is not null)
                {
                    foreach (Type type in types)
                    {
                        var attrs = type.GetCustomAttributes(typeof(CmisTypeInfoAttribute), false);

                        if (attrs is not null && attrs.Length > 0)
                        {
                            AppendTypeFactory<TBase>(factories, genericTypeDefinition, type, baseType, (CmisTypeInfoAttribute)attrs[0]);
                        }
                    }
                }
            }
        }

        public static void AppendTypeFactory<TBase>(Dictionary<string, CmisObjectModel.Common.Generic.Factory<TBase>> factories, Type genericTypeDefinition, Type type, CmisTypeInfoAttribute attr) where TBase : CmisObjectModel.Serialization.XmlSerializable
        {
            if (attr is not null)
                AppendTypeFactory<TBase>(factories, genericTypeDefinition, type, typeof(TBase), attr);
        }

        protected static void AppendTypeFactory<TBase>(Dictionary<string, CmisObjectModel.Common.Generic.Factory<TBase>> factories, Type genericTypeDefinition, Type type, Type baseType, CmisTypeInfoAttribute attr) where TBase : CmisObjectModel.Serialization.XmlSerializable
        {
            try
            {
                if (baseType.IsAssignableFrom(type) && !type.IsAbstract && type.GetConstructor(new Type[] { }) is not null)
                {
                    // create factory for this valid type
                    CmisObjectModel.Common.Generic.Factory<TBase> factory = (CmisObjectModel.Common.Generic.Factory<TBase>)Activator.CreateInstance(genericTypeDefinition.MakeGenericType(baseType, type));
                    string cmisFullTypeName;
                    string cmisTypeName;
                    string targetTypeName;
                    string defaultElementName;

                    cmisFullTypeName = attr.CmisTypeName.ToLowerInvariant();
                    cmisTypeName = !string.IsNullOrEmpty(cmisFullTypeName) ? cmisFullTypeName.Substring(cmisFullTypeName.IndexOf(':') + 1) : "";
                    targetTypeName = attr.TargetTypeName.ToLowerInvariant();
                    defaultElementName = attr.DefaultElementName.ToLowerInvariant();

                    // provide type construction by cmisFullTypeName, cmisTypeName, targetTypeName and defaultElementName
                    foreach (string key in new string[] { cmisFullTypeName, cmisTypeName, targetTypeName, defaultElementName })
                    {
                        if (factories.ContainsKey(key))
                        {
                            factories[key] = factory;
                        }
                        else if (!string.IsNullOrEmpty(key))
                        {
                            factories.Add(key, factory);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

    }
}