using System;
using System.Collections.Generic;
using swss = System.Web.Script.Serialization;
using ccg1 = CmisObjectModel.Collections.Generic;
using ccg = CmisObjectModel.Common.Generic;
using cs = CmisObjectModel.Serialization;
using Microsoft.VisualBasic.CompilerServices;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON.Serialization
{
    /// <summary>
   /// JavaScriptSerializer to handle instances derived from CmisObjectModelLibrary.Serialization.XmlSerializable-class
   /// </summary>
   /// <remarks></remarks>
    public class JavaScriptSerializer : swss.JavaScriptSerializer
    {

        #region Constructors
        public JavaScriptSerializer() : base()
        {
            MaxJsonLength = int.MaxValue;
        }
        /// <summary>
      /// Succinct support
      /// </summary>
      /// <param name="succinct"></param>
      /// <remarks></remarks>
        public JavaScriptSerializer(bool succinct) : this()
        {
            if (succinct)
            {
                AttributesOverrides.set_ElementConverter(typeof(CmisObjectModel.Core.cmisObjectType), "properties", "succinctProperties", new Collections.SuccinctPropertiesConverter());
            }
        }
        #endregion

        public readonly JSONAttributeOverrides AttributesOverrides = new JSONAttributeOverrides();
        private static Dictionary<Type, JavaScriptConverter> _converters = new Dictionary<Type, JavaScriptConverter>();

        /// <summary>
      /// Creates an instance of type using reflection
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="type"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private static T CreateInstance<T>(Type type, params object[] args)
        {
            if (typeof(T).IsAssignableFrom(type))
            {
                int argsLength = args is null ? 0 : args.Length;

                if (argsLength == 0)
                {
                    // default constructor
                    var ci = type.GetConstructor(new Type[] { });

                    return ci is null ? default : Conversions.ToGenericParameter<T>(ci.Invoke(new object[] { }));
                }
                else
                {
                    // use constructor with parameters
                    foreach (System.Reflection.ConstructorInfo ci in type.GetConstructors())
                    {
                        var parameters = ci.GetParameters();
                        if (argsLength == (parameters is null ? 0 : parameters.Length))
                        {
                            int argIndex = 0;
                            while (argIndex < argsLength)
                            {
                                var arg = args[argIndex];
                                // constructor does not match the arguments
                                if (!(arg is null || parameters[argIndex].ParameterType.IsAssignableFrom(arg.GetType())))
                                {
                                    continue;
                                }
                                else
                                {
                                    argIndex += 1;
                                }
                            }
                            // all arguments accepted
                            return Conversions.ToGenericParameter<T>(ci.Invoke(args));
                        }
                    }

                    // unable to find suitable constructor then try default constructor
                    return CreateInstance<T>(type);
                }
            }

            return default;
        }

        /// <summary>
      /// Before deserialization of input Deserialize() auto-registers the best fitting javaConverter-instance for type TSerializable
      /// </summary>
        public new TSerializable Deserialize<TSerializable>(string input) where TSerializable : cs.XmlSerializable
        {
            RegisterConverter<TSerializable>();
            return base.Deserialize<TSerializable>(input);
        }
        /// <summary>
      /// Deserializes an array
      /// </summary>
        public TSerializable[] DeserializeArray<TSerializable>(string input) where TSerializable : cs.XmlSerializable
        {
            RegisterConverter<TSerializable>();
            return base.Deserialize<TSerializable[]>(input);
        }
        /// <summary>
      /// Deserializes an array represented as a map
      /// </summary>
        public new TSerializable[] DeserializeMap<TSerializable, TKey>(string input, ccg.DynamicProperty<TSerializable, TKey> keyProperty) where TSerializable : cs.XmlSerializable
        {
            IDictionary<string, object> dictionary = base.Deserialize<Dictionary<string, object>>(input);
            if (dictionary is null)
            {
                return null;
            }
            else
            {
                var arrayProperty = new ccg1.ArrayContainer<TSerializable>("Values");
                var mapper = new ccg1.ArrayMapper<cs.EmptyXmlSerializable, TSerializable, TKey>(cs.EmptyXmlSerializable.Singleton, arrayProperty, keyProperty);
                mapper.JavaImport(dictionary, this);
                return arrayProperty.Value;
            }
        }

        /// <summary>
      /// Returns a JavaScriptConverter-instance designed for an object of type or null, if no JavaScriptConverter could be found
      /// </summary>
        public JavaScriptConverter GetJavaScriptConverter(Type type)
        {
            var retVal = AttributesOverrides.get_TypeConverter(type);

            if (retVal is null)
            {
                lock (_converters)
                {
                    if (_converters.ContainsKey(type))
                    {
                        return _converters[type];
                    }
                    else if (typeof(cs.XmlSerializable).IsAssignableFrom(type))
                    {
                        // first call with given type parameter, so the system has to search for JavaScriptConverterAttribute
                        // to create a JavaScriptConverter-factory for it
                        try
                        {
                            var javaScriptConverterAttr = GetCustomAttribute<Attributes.JavaScriptConverterAttribute>(type);
                            // this attribute will always be found, because there is a fallback mechanism defined for XmlSerializable-classes
                            var javaScriptObjectResolverAttr = GetCustomAttribute<Attributes.JavaScriptObjectResolverAttribute>(type);
                            if (javaScriptConverterAttr is not null)
                            {
                                // if type is a mustinherit type then a specific objectResolver MUST be declared for it, otherwise - only using the fallback
                                // mechanism - an exception is thrown while trying to get the ObjectResolverType
                                retVal = CreateInstance<JavaScriptConverter>(javaScriptConverterAttr.get_JavaScriptConverterType(type), CreateInstance<ObjectResolver>(javaScriptObjectResolverAttr.get_ObjectResolverType(type)));
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    _converters.Add(type, retVal);
                }
            }

            return retVal;
        }

        /// <summary>
      /// Searches in the custom type attributes for specified attribute type
      /// </summary>
        public static TAttribute GetCustomAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            var currentType = type;

            while (currentType is not null)
            {
                var attrs = currentType.GetCustomAttributes(typeof(TAttribute), false);

                if (attrs is not null && attrs.Length > 0)
                {
                    return (TAttribute)attrs[0];
                }
                else
                {
                    currentType = currentType.BaseType;
                }
            }

            return null;
        }

        /// <summary>
      /// Registers the best fitting javaScriptConverter for type TSerializable
      /// </summary>
      /// <typeparam name="TSerializable"></typeparam>
      /// <remarks></remarks>
        private void RegisterConverter<TSerializable>() where TSerializable : cs.XmlSerializable
        {
            // auto-select javaScriptConverter to start the serialization
            base.RegisterConverters(new swss.JavaScriptConverter[] { GetJavaScriptConverter(typeof(TSerializable)) });
        }

        /// <summary>
      /// Marks RegisterConverters() as obsolete
      /// </summary>
      /// <remarks></remarks>
        [Obsolete("The JavaScriptSerializer automatically selects the best fitting JavaScriptConverter for (de-)serialization.", false)]
        public new void RegisterConverters(IEnumerable<swss.JavaScriptConverter> converters)
        {
            base.RegisterConverters(converters);
        }

        /// <summary>
      /// Before serialization of obj Serialize() auto-registers the best fitting javaConverter-instance for type TSerializable
      /// </summary>
        public new string Serialize<TSerializable>(TSerializable obj) where TSerializable : cs.XmlSerializable
        {
            RegisterConverter<TSerializable>();
            return base.Serialize(obj);
        }
        /// <summary>
      /// Before serialization of objects Serialize() auto-registers the best fitting javaConverter-instance for type TSerializable
      /// </summary>
        public new string SerializeArray<TSerializable>(TSerializable[] objects) where TSerializable : cs.XmlSerializable
        {
            RegisterConverter<TSerializable>();
            return base.Serialize(objects);
        }
        /// <summary>
      /// Serializes an array as a map
      /// </summary>
        public new string SerializeMap<TSerializable, TKey>(TSerializable[] obj, ccg.DynamicProperty<TSerializable, TKey> keyProperty) where TSerializable : cs.XmlSerializable
        {
            if (obj is null)
            {
                return null;
            }
            else
            {
                var arrayProperty = new ccg1.ArrayContainer<TSerializable>("Values", obj);
                var mapper = new ccg1.ArrayMapper<cs.EmptyXmlSerializable, TSerializable, TKey>(cs.EmptyXmlSerializable.Singleton, arrayProperty, keyProperty);
                return base.Serialize(mapper.JavaExport(null, this));
            }
        }
    }
}