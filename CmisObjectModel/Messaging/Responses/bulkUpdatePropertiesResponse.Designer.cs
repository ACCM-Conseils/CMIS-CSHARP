using System;
using System.Collections.Generic;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Messaging.Responses
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see bulkUpdatePropertiesResponse
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    public partial class bulkUpdatePropertiesResponse : Serialization.XmlSerializable
    {

        public bulkUpdatePropertiesResponse()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected bulkUpdatePropertiesResponse(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<bulkUpdatePropertiesResponse, string>> _setter = new Dictionary<string, Action<bulkUpdatePropertiesResponse, string>>() { }; // _setter

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
            _objectIdAndChangeTokens = ReadArray(reader, attributeOverrides, "objectIdAndChangeToken", Constants.Namespaces.cmism, GenericXmlSerializableFactory<Core.cmisObjectIdAndChangeTokenType>);
            _extension = Read(reader, attributeOverrides, "extension", Constants.Namespaces.cmism, GenericXmlSerializableFactory<cmisExtensionType>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteArray(writer, attributeOverrides, "objectIdAndChangeToken", Constants.Namespaces.cmism, _objectIdAndChangeTokens);
            WriteElement(writer, attributeOverrides, "extension", Constants.Namespaces.cmism, _extension);
        }
        #endregion

        protected cmisExtensionType _extension;
        public virtual cmisExtensionType Extension
        {
            get
            {
                return _extension;
            }
            set
            {
                if (!ReferenceEquals(value, _extension))
                {
                    var oldValue = _extension;
                    _extension = value;
                    OnPropertyChanged("Extension", value, oldValue);
                }
            }
        } // Extension

        protected Core.cmisObjectIdAndChangeTokenType[] _objectIdAndChangeTokens;
        public virtual Core.cmisObjectIdAndChangeTokenType[] ObjectIdAndChangeTokens
        {
            get
            {
                return _objectIdAndChangeTokens;
            }
            set
            {
                if (!ReferenceEquals(value, _objectIdAndChangeTokens))
                {
                    var oldValue = _objectIdAndChangeTokens;
                    _objectIdAndChangeTokens = value;
                    OnPropertyChanged("ObjectIdAndChangeTokens", value, oldValue);
                }
            }
        } // ObjectIdAndChangeTokens

    }
}