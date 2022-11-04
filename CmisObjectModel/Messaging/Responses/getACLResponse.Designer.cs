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
   /// see getACLResponse
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    public partial class getACLResponse : Serialization.XmlSerializable
    {

        public getACLResponse()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected getACLResponse(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<getACLResponse, string>> _setter = new Dictionary<string, Action<getACLResponse, string>>() { }; // _setter

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
            _aCL = Read(reader, attributeOverrides, "ACL", Constants.Namespaces.cmism, GenericXmlSerializableFactory<cmisACLType>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "ACL", Constants.Namespaces.cmism, _aCL);
        }
        #endregion

        protected cmisACLType _aCL;
        public virtual cmisACLType ACL
        {
            get
            {
                return _aCL;
            }
            set
            {
                if (!ReferenceEquals(value, _aCL))
                {
                    var oldValue = _aCL;
                    _aCL = value;
                    OnPropertyChanged("ACL", value, oldValue);
                }
            }
        } // ACL

    }
}