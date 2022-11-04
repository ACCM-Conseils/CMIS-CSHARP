using CmisObjectModel.Common;
using System;
using System.Collections.Generic;
using sx = System.Xml;
using sxs = System.Xml.Serialization;

namespace CmisObjectModel.Serialization
{

    public abstract class SerializationHelper
    {
        internal const string csElementName = "eb99185431584dcfb65c838e36421f09";

        public static System.IO.Stream ToStream<T>(T instance) where T : sxs.IXmlSerializable, new()
        {
            return new Generic.SerializationHelper<T>(instance);
        }

        public static System.IO.Stream ToStream<T>(T instance, sxs.XmlAttributeOverrides attributeOverrides) where T : sxs.IXmlSerializable, new()
        {
            return new Generic.SerializationHelper<T>(instance, attributeOverrides);
        }

        public static sx.XmlDocument ToXmlDocument<T>(T instance) where T : sxs.IXmlSerializable, new()
        {
            return new Generic.SerializationHelper<T>(instance);
        }

        public static sx.XmlDocument ToXmlDocument<T>(T instance, sxs.XmlAttributeOverrides attributeOverrides) where T : sxs.IXmlSerializable, new()
        {
            return new Generic.SerializationHelper<T>(instance, attributeOverrides);
        }
    }

    namespace Generic
    {
        /// <summary>
        /// Creates Xml-results using cmis-specified prefixes in the root
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <remarks>
        /// Classes that implement the System.Xml.Serialization.IXmlSerializable interface will not
        /// be serialized with a prefix for the root node. To achieve that a prefix specified by cmis
        /// is used for the root node, this helper class is created.
        /// </remarks>
        [Serializable()]
        [sxs.XmlRoot(ElementName = csElementName)]
        public class SerializationHelper<T> : SerializationHelper where T : sxs.IXmlSerializable
        {

            #region Constructors
            /// <summary>
            /// Scans T for specified elementname and namespace
            /// </summary>
            /// <remarks></remarks>
            static SerializationHelper()
            {
                {
                    var withBlock = typeof(T).GetXmlRootAttribute(exactNonNullResult: true);
                    _elementName = withBlock.ElementName;
                    _namespace = withBlock.Namespace;
                }

                if (string.IsNullOrEmpty(_elementName))
                    _elementName = typeof(T).Name;
            }

            public SerializationHelper()
            {
            }
            public SerializationHelper(T item)
            {
                Item = item;
            }
            public SerializationHelper(T item, sxs.XmlAttributeOverrides attributeOverrides)
            {
                Item = item;
                _attributeOverrides = attributeOverrides;
            }
            #endregion

            private readonly sxs.XmlAttributeOverrides _attributeOverrides;
            private static readonly sxs.XmlAttributeOverrides _defaultAttributeOverrides = new sxs.XmlAttributeOverrides();

            private static string _elementName;
            /// <summary>
            /// Returns the default elementname for T-Type
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            private string ElementName
            {
                get
                {
                    var attrs = _attributeOverrides is null ? null : _attributeOverrides[typeof(T)];
                    return attrs is null || attrs.XmlRoot is null ? _elementName : attrs.XmlRoot.ElementName ?? _elementName;
                }
            }

            public T Item { get; set; }

            private static string _namespace;
            /// <summary>
            /// Returns the default namespace for T-Type
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            private string Namespace
            {
                get
                {
                    var attrs = _attributeOverrides is null ? null : _attributeOverrides[typeof(T)];
                    return attrs is null || attrs.XmlRoot is null ? _namespace : attrs.XmlRoot.Namespace ?? _namespace;
                }
            }

            private static Dictionary<sxs.XmlAttributeOverrides, sxs.XmlSerializer> _serializers = new Dictionary<sxs.XmlAttributeOverrides, sxs.XmlSerializer>();
            private sxs.XmlSerializer Serializer
            {
                get
                {
                    var attributeOverrides = _attributeOverrides ?? _defaultAttributeOverrides;

                    lock (_serializers)
                    {
                        if (_serializers.ContainsKey(attributeOverrides))
                        {
                            return _serializers[attributeOverrides];
                        }
                        else
                        {
                            try
                            {
                                // define namespace and elementname for property 'Item'
                                if (attributeOverrides[typeof(SerializationHelper<T>), "Item"] is null)
                                {
                                    var attr = new sxs.XmlElementAttribute(ElementName) { Namespace = Namespace };
                                    var attrs = new sxs.XmlAttributes();

                                    attrs.XmlElements.Add(attr);
                                    attributeOverrides.Add(typeof(SerializationHelper<T>), "Item", attrs);
                                }
                            }
                            catch
                            {
                            }

                            var retVal = new sxs.XmlSerializer(typeof(SerializationHelper<T>), attributeOverrides);

                            _serializers.Add(attributeOverrides, retVal);
                            return retVal;
                        }
                    }
                }
            }

            public static implicit operator System.IO.Stream(SerializationHelper<T> value)
            {
                sx.XmlDocument xmlDoc = value;
                return new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlDoc.OuterXml)) { Position = 0L };
            }

            public static implicit operator sx.XmlDocument(SerializationHelper<T> value)
            {
                var retVal = new sx.XmlDocument();

                if (value is not null)
                {
                    // specified namespaces
                    var namespaces = new sxs.XmlSerializerNamespaces();
                    foreach (KeyValuePair<sx.XmlQualifiedName, string> de in CmisObjectModel.Common.CommonFunctions.get_CmisNamespaces(new string[] { }))
                        namespaces.Add(de.Key.Name, de.Value);

                    using (var ms = new System.IO.MemoryStream())
                    {
                        using (var sw = new System.IO.StreamWriter(ms))
                        {
                            using (var writer = sx.XmlWriter.Create(sw))
                            {
                                if (value._attributeOverrides is null)
                                {
                                    value.Serializer.Serialize(writer, value, namespaces);
                                }
                                else
                                {
                                    // the serialization will use the attributeOverrides-instance
                                    using (var attributeOverrides = new Serialization.XmlAttributeOverrides(writer, value._attributeOverrides))
                                    {
                                        value.Serializer.Serialize(writer, value, namespaces);
                                    }
                                }
                                ms.Position = 0L;
                                retVal.Load(ms);
                            }
                        }

                        // replace root with 'Item'-node
                        var node = retVal.SelectSingleNode(csElementName);
                        retVal.RemoveChild(node);
                        retVal.AppendChild(node.FirstChild);
                    }
                }

                return retVal;
            }
        }
    }
}