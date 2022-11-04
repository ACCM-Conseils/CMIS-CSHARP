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
   /// see cmisUriTemplateType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-RestAtom.xsd
   /// </remarks>
    public partial class cmisUriTemplateType : Serialization.XmlSerializable
    {

        public cmisUriTemplateType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisUriTemplateType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisUriTemplateType, string>> _setter = new Dictionary<string, Action<cmisUriTemplateType, string>>() { }; // _setter

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
            _template = Read(reader, attributeOverrides, "template", Constants.Namespaces.cmisra, _template);
            _type = Read(reader, attributeOverrides, "type", Constants.Namespaces.cmisra, _type);
            _mediatype = Read(reader, attributeOverrides, "mediatype", Constants.Namespaces.cmisra, _mediatype);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "template", Constants.Namespaces.cmisra, _template);
            WriteElement(writer, attributeOverrides, "type", Constants.Namespaces.cmisra, _type);
            WriteElement(writer, attributeOverrides, "mediatype", Constants.Namespaces.cmisra, _mediatype);
        }
        #endregion

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

        protected string _template;
        public virtual string Template
        {
            get
            {
                return _template;
            }
            set
            {
                if ((_template ?? "") != (value ?? ""))
                {
                    string oldValue = _template;
                    _template = value;
                    OnPropertyChanged("Template", value, oldValue);
                }
            }
        } // Template

        protected string _type;
        public virtual string Type
        {
            get
            {
                return _type;
            }
            set
            {
                if ((_type ?? "") != (value ?? ""))
                {
                    string oldValue = _type;
                    _type = value;
                    OnPropertyChanged("Type", value, oldValue);
                }
            }
        } // Type

    }
}