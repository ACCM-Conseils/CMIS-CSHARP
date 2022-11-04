using System;
using System.Collections.Generic;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisExtensionFeatureKeyValuePair
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisExtensionFeatureKeyValuePair : Serialization.XmlSerializable
    {

        public cmisExtensionFeatureKeyValuePair()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisExtensionFeatureKeyValuePair(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisExtensionFeatureKeyValuePair, string>> _setter = new Dictionary<string, Action<cmisExtensionFeatureKeyValuePair, string>>() { }; // _setter

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
            _key = Read(reader, attributeOverrides, "key", Constants.Namespaces.cmis, _key);
            _value = Read(reader, attributeOverrides, "value", Constants.Namespaces.cmis, _value);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "key", Constants.Namespaces.cmis, _key);
            WriteElement(writer, attributeOverrides, "value", Constants.Namespaces.cmis, _value);
        }
        #endregion

        protected string _key;
        public virtual string Key
        {
            get
            {
                return _key;
            }
            set
            {
                if ((_key ?? "") != (value ?? ""))
                {
                    string oldValue = _key;
                    _key = value;
                    OnPropertyChanged("Key", value, oldValue);
                }
            }
        } // Key

        protected string _value;
        public virtual string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if ((_value ?? "") != (value ?? ""))
                {
                    string oldValue = _value;
                    _value = value;
                    OnPropertyChanged("Value", value, oldValue);
                }
            }
        } // Value

    }
}