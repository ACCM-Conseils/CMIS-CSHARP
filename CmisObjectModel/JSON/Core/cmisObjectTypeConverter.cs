using System;
using System.Collections;
using System.Collections.Generic;
using swss = System.Web.Script.Serialization;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core
{
    public partial class cmisObjectType
    {

        #region Java-Serialization support
        /// <summary>
      /// Pass-through to extensions of _properties-instance
      /// </summary>
      /// <remarks></remarks>
        [Attributes.JavaScriptConverter(typeof(JSON.Core.cmisObjectTypeConverter.PropertiesExtensionsConverter))]
        public class PropertiesExtensions : Serialization.XmlSerializable
        {

            #region Constructors
            public PropertiesExtensions()
            {
                _extensions = new CmisObjectModel.Collections.Generic.ArrayContainer<Extensions.Extension>("Extensions");
            }
            public PropertiesExtensions(cmisObjectType owner)
            {
                Func<Extensions.Extension[]> getter = () => { if (owner is null || owner._properties is null) { return null; } else { return owner._properties.Extensions; } };
                Action<Extensions.Extension[]> setter = value => { if (owner is not null) { if (value is null) { if (owner._properties is not null) owner._properties.Extensions = null; } else if (owner._properties is null) { owner.Properties = new Collections.cmisPropertiesType() { Extensions = value }; } else { owner._properties.Extensions = value; } } };
                _extensions = new Common.Generic.DynamicProperty<Extensions.Extension[]>(getter, setter, "Extensions");
            }
            public PropertiesExtensions(Collections.cmisPropertiesType owner)
            {
                Func<Extensions.Extension[]> getter = () => { if (owner is null) { return null; } else { return owner.Extensions; } };
                Action<Extensions.Extension[]> setter = value => { if (owner is not null) { if (value is null) { owner.Extensions = null; } else { owner.Extensions = value; } } };
                _extensions = new Common.Generic.DynamicProperty<Extensions.Extension[]>(getter, setter, "Extensions");
            }
            #endregion

            #region IXmlSerialization
            protected override void ReadAttributes(System.Xml.XmlReader reader)
            {
            }

            protected override void ReadXmlCore(System.Xml.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
            {
                Extensions = ReadArray(reader, attributeOverrides, null, CmisObjectModel.Extensions.Extension.CreateInstance);
            }

            protected override void WriteXmlCore(System.Xml.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
            {
                WriteArray(writer, attributeOverrides, null, Constants.Namespaces.cmis, Extensions);
            }
            #endregion

            protected Common.Generic.DynamicProperty<Extensions.Extension[]> _extensions;
            public virtual Extensions.Extension[] Extensions
            {
                get
                {
                    return _extensions.Value;
                }
                set
                {
                    var oldValue = Extensions;

                    if (!ReferenceEquals(value, Extensions))
                    {
                        _extensions.Value = value;
                        OnPropertyChanged("Extensions", value, oldValue);
                    }
                }
            } // Extensions
        }

        /// <summary>
      /// pass-through for extensions of properties
      /// </summary>
        public PropertiesExtensions PropertiesExtension
        {
            get
            {
                return new PropertiesExtensions(this);
            }
            set
            {
                var extensions = value is null ? null : value.Extensions;

                if (extensions is null)
                {
                    if (_properties is not null)
                        _properties.Extensions = null;
                }
                else if (_properties is null)
                {
                    Properties = new Collections.cmisPropertiesType() { Extensions = extensions };
                }
                else
                {
                    _properties.Extensions = extensions;
                }
            }
        } // PropertiesExtension

        #endregion

    }
}

namespace CmisObjectModel.JSON.Core
{
    /// <summary>
   /// Converter for propertiesExtension of a cmisObjectType-Instance
   /// </summary>
   /// <remarks></remarks>
    public partial class cmisObjectTypeConverter
    {

        /// <summary>
      /// Converter for PropertiesExtension-property
      /// </summary>
      /// <remarks>Serializes the extensions of the cmisObjectType.Properties-property into a IDictionary(Of String, Object)
      /// with one item. Key: 'extensions', value: array of extensions.</remarks>
        public class PropertiesExtensionsConverter : Serialization.Generic.JavaScriptConverter<CmisObjectModel.Core.cmisObjectType.PropertiesExtensions>
        {

            public PropertiesExtensionsConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Core.cmisObjectType.PropertiesExtensions> objectResolver) : base(objectResolver)
            {
            }

            /// <summary>
         /// Deserializes the extensions of the cmisPropertiesType-class
         /// </summary>
         /// <param name="context"></param>
         /// <remarks>Supported formats:
         /// 1. Key: "extensions", Value: array of IDictionary(Of String, Object)
         /// 2. Keys: names of known ExtensionTypeNames, Values: IDictionary(Of String, Object) or IDictionary(Of String, Object)()</remarks>
            protected override void Deserialize(SerializationContext context)
            {
                if (context.Dictionary.ContainsKey("extensions"))
                {
                    context.Object.Extensions = ReadArray(context, "extensions", context.Object.Extensions);
                }
                else
                {
                    CmisObjectModel.Extensions.Extension extension;
                    var extensions = new List<CmisObjectModel.Extensions.Extension>();

                    foreach (KeyValuePair<string, object> de in context.Dictionary)
                    {
                        if (de.Value is IDictionary<string, object>)
                        {
                            extension = DeserializeExtension(context, de.Key, de.Value);
                            if (extension is not null)
                                extensions.Add(extension);
                        }
                        else if (de.Value is ICollection)
                        {
                            foreach (object value in (ICollection)de.Value)
                            {
                                extension = DeserializeExtension(context, de.Key, value);
                                if (extension is not null)
                                    extensions.Add(extension);
                            }
                        }
                    }
                    context.Object.Extensions = extensions.ToArray();
                }
            }

            /// <summary>
         /// Deserializes a single extension
         /// </summary>
            private CmisObjectModel.Extensions.Extension DeserializeExtension(SerializationContext context, string extensionTypeName, object value)
            {
                if (value is IDictionary<string, object>)
                {
                    var extensionType = CmisObjectModel.Extensions.Extension.GetExtensionType(extensionTypeName);

                    if (extensionType is not null)
                    {
                        swss.JavaScriptConverter javaScriptConverter = context.Serializer.GetJavaScriptConverter(extensionType);
                        if (javaScriptConverter is not null)
                        {
                            return (CmisObjectModel.Extensions.Extension)javaScriptConverter.Deserialize((IDictionary<string, object>)value, extensionType, context.Serializer);
                        }
                    }
                }

                return null;
            }

            protected override void Serialize(SerializationContext context)
            {
                WriteArray(context, context.Object.Extensions, "extensions");
            }
        }

        /// <summary>
      /// Extended version of PropertiesExtensionsConverter (see Serialize())
      /// </summary>
      /// <remarks>Serializes the extensions of the cmisObjectType.Properties-property into a IDictionary(Of String, Object).
      /// Keys: the extensionTypeNames found in cmisObjectType.Properties.Extensions,
      /// values: if more than one extension-instance of the same type exist cmisObjectType.Properties.Extensions, they are
      ///         collected in an array of extensions, otherwise it is a single extension</remarks>
        public class PropertiesExtensionsExConverter : PropertiesExtensionsConverter
        {

            #region Constructors
            public PropertiesExtensionsExConverter() : base(new Serialization.Generic.DefaultObjectResolver<CmisObjectModel.Core.cmisObjectType.PropertiesExtensions>())
            {
            }
            public PropertiesExtensionsExConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Core.cmisObjectType.PropertiesExtensions> objectResolver) : base(objectResolver)
            {
            }
            #endregion

            /// <summary>
         /// Collects extensions of the same type and serializes them in single items within the context.Dictionary
         /// </summary>
         /// <param name="context"></param>
         /// <remarks></remarks>
            protected override void Serialize(SerializationContext context)
            {
                var extensions = new Dictionary<string, List<IDictionary<string, object>>>();

                // collect same types
                foreach (CmisObjectModel.Extensions.Extension extension in context.Object.Extensions)
                {
                    string key = extension is null ? null : extension.ExtensionTypeName;
                    if (!string.IsNullOrEmpty(key))
                    {
                        swss.JavaScriptConverter javaScriptConverter = context.Serializer.GetJavaScriptConverter(extension.GetType());
                        if (javaScriptConverter is not null)
                        {
                            var dictionary = javaScriptConverter.Serialize(extension, context.Serializer);

                            if (dictionary is not null)
                            {
                                if (!extensions.ContainsKey(key))
                                {
                                    extensions.Add(key, new List<IDictionary<string, object>>());
                                }
                                extensions[key].Add(dictionary);
                            }
                        }
                    }
                }

                // transfer to output
                foreach (KeyValuePair<string, List<IDictionary<string, object>>> de in extensions)
                {
                    if (de.Value.Count == 1)
                    {
                        context.Dictionary.Add(de.Key, de.Value[0]);
                    }
                    else
                    {
                        context.Dictionary.Add(de.Key, de.Value.ToArray());
                    }
                }
            }
        }

        /// <summary>
      /// Specialized Read() overload to preserve the properties-extensions
      /// </summary>
        protected CmisObjectModel.Core.Collections.cmisPropertiesType Read(SerializationContext context, string propertyName, CmisObjectModel.Core.Collections.cmisPropertiesType defaultValue)
        {
            var retVal = base.Read(context, propertyName, defaultValue);

            // preserve extensions (doing so the code is independent from the order of deserialization of the properties)
            if (!(defaultValue is null || retVal is null))
                retVal.Extensions = defaultValue.Extensions;

            return retVal;
        }

    }
}