using System;
using System.Collections.Generic;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.RestAtom
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisContentType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-RestAtom.xsd
   /// </remarks>
    [System.CodeDom.Compiler.GeneratedCode("CmisXsdConverter", "1.0.0.0")]
    public partial class cmisContentType : Serialization.XmlSerializable
    {

        public cmisContentType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisContentType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisContentType, string>> _setter = new Dictionary<string, Action<cmisContentType, string>>() { }; // _setter

        /// <summary>
      /// Deserialization of all properties stored in attributes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadAttributes(sx.XmlReader reader)
        {
            // at least one property is serialized in an attribute-value
            if (_setter.Count > 0)
            {
                for (int attributeIndex = 0, loopTo = reader.AttributeCount - 1; attributeIndex <= loopTo; attributeIndex++)
                {
                    reader.MoveToAttribute(attributeIndex);
                    // attribute name
                    string key = reader.Name.ToLowerInvariant();
                    if (_setter.ContainsKey(key))
                        _setter[key].Invoke(this, reader.GetAttribute(attributeIndex));
                }
            }
        }

        /// <summary>
      /// Deserialization of all properties stored in subnodes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            string lastNamespaceURI = null;
            string lastLocalName = null;

            reader.MoveToContent();
            while (reader.IsStartElement())
            {
                string localName = reader.LocalName;
                string namespaceURI = reader.NamespaceURI;

                if (string.Equals(lastLocalName, localName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(lastNamespaceURI, namespaceURI, StringComparison.InvariantCultureIgnoreCase))
                {
                    // unknown node detected
                    reader.ReadOuterXml();
                }
                else
                {
                    lastLocalName = localName;
                    lastNamespaceURI = namespaceURI;
                    ReadXmlCoreFuzzy(reader, attributeOverrides, true);
                }
                reader.MoveToContent();
            }
        }
        protected override bool ReadXmlCoreFuzzy(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides, bool callReadXmlCoreFuzzy2)
        {
            if (base.ReadXmlCoreFuzzy(reader, attributeOverrides, false))
                return true;

            switch ((reader.NamespaceURI ?? string.Empty).ToLowerInvariant() ?? "")
            {
                case var @case when @case == (Constants.NamespacesLowerInvariant.cmisra ?? ""):
                    {
                        switch (reader.LocalName.ToLowerInvariant() ?? "")
                        {
                            case "mediatype":
                                {
                                    _mediatype = Read(reader, attributeOverrides, "mediatype", Constants.Namespaces.cmisra, _mediatype);
                                    break;
                                }
                            case "base64":
                                {
                                    _base64 = Read(reader, attributeOverrides, "base64", Constants.Namespaces.cmisra, _base64);
                                    break;
                                }

                            default:
                                {
                                    // try to find node in the namespace-independent section
                                    return callReadXmlCoreFuzzy2 && ReadXmlCoreFuzzy2(reader, attributeOverrides, true);
                                }
                        }

                        break;
                    }

                default:
                    {
                        // try to find node in the namespace-independent section
                        return callReadXmlCoreFuzzy2 && ReadXmlCoreFuzzy2(reader, attributeOverrides, true);
                    }
            }

            return true;
        }
        protected override bool ReadXmlCoreFuzzy2(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides, bool callReadOuterXml)
        {
            if (base.ReadXmlCoreFuzzy2(reader, attributeOverrides, false))
                return true;

            // ignore node
            if (callReadOuterXml)
                reader.ReadOuterXml();
            return callReadOuterXml;
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "mediatype", Constants.Namespaces.cmisra, _mediatype);
            WriteElement(writer, attributeOverrides, "base64", Constants.Namespaces.cmisra, _base64);
        }
        #endregion

        protected string _base64;
        public virtual string Base64
        {
            get
            {
                return _base64;
            }
            set
            {
                if ((_base64 ?? "") != (value ?? ""))
                {
                    string oldValue = _base64;
                    _base64 = value;
                    OnPropertyChanged("Base64", value, oldValue);
                }
            }
        } // Base64

        protected string _mediatype;
        public virtual string Mediatype
        {
            get
            {
                return _mediatype;
            }
            set
            {
                if ((_mediatype ?? "") != (value ?? ""))
                {
                    string oldValue = _mediatype;
                    _mediatype = value;
                    OnPropertyChanged("Mediatype", value, oldValue);
                }
            }
        } // Mediatype

    }
}