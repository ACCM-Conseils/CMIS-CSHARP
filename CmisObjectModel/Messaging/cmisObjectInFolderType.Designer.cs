using System;
using System.Collections.Generic;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Messaging
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisObjectInFolderType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    public partial class cmisObjectInFolderType : Serialization.XmlSerializable
    {

        public cmisObjectInFolderType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisObjectInFolderType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisObjectInFolderType, string>> _setter = new Dictionary<string, Action<cmisObjectInFolderType, string>>() { }; // _setter

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
            _object = Read(reader, attributeOverrides, "object", Constants.Namespaces.cmism, GenericXmlSerializableFactory<Core.cmisObjectType>);
            _pathSegment = Read(reader, attributeOverrides, "pathSegment", Constants.Namespaces.cmism, _pathSegment);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "object", Constants.Namespaces.cmism, _object);
            if (!string.IsNullOrEmpty(_pathSegment))
                WriteElement(writer, attributeOverrides, "pathSegment", Constants.Namespaces.cmism, _pathSegment);
        }
        #endregion

        protected Core.cmisObjectType _object;
        public virtual Core.cmisObjectType Object
        {
            get
            {
                return _object;
            }
            set
            {
                if (!ReferenceEquals(value, _object))
                {
                    var oldValue = _object;
                    _object = value;
                    OnPropertyChanged("Object", value, oldValue);
                }
            }
        } // Object

        protected string _pathSegment;
        public virtual string PathSegment
        {
            get
            {
                return _pathSegment;
            }
            set
            {
                if ((_pathSegment ?? "") != (value ?? ""))
                {
                    string oldValue = _pathSegment;
                    _pathSegment = value;
                    OnPropertyChanged("PathSegment", value, oldValue);
                }
            }
        } // PathSegment

    }
}