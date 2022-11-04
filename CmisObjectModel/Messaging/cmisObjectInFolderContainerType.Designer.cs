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
   /// see cmisObjectInFolderContainerType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    public partial class cmisObjectInFolderContainerType : Serialization.XmlSerializable
    {

        public cmisObjectInFolderContainerType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisObjectInFolderContainerType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisObjectInFolderContainerType, string>> _setter = new Dictionary<string, Action<cmisObjectInFolderContainerType, string>>() { }; // _setter

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
            _objectInFolder = Read(reader, attributeOverrides, "objectInFolder", Constants.Namespaces.cmism, GenericXmlSerializableFactory<cmisObjectInFolderType>);
            _children = ReadArray(reader, attributeOverrides, "children", Constants.Namespaces.cmism, GenericXmlSerializableFactory<cmisObjectInFolderContainerType>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "objectInFolder", Constants.Namespaces.cmism, _objectInFolder);
            WriteArray(writer, attributeOverrides, "children", Constants.Namespaces.cmism, _children);
        }
        #endregion

        protected cmisObjectInFolderContainerType[] _children;
        public virtual cmisObjectInFolderContainerType[] Children
        {
            get
            {
                return _children;
            }
            set
            {
                if (!ReferenceEquals(value, _children))
                {
                    var oldValue = _children;
                    _children = value;
                    OnPropertyChanged("Children", value, oldValue);
                }
            }
        } // Children

        protected cmisObjectInFolderType _objectInFolder;
        public virtual cmisObjectInFolderType ObjectInFolder
        {
            get
            {
                return _objectInFolder;
            }
            set
            {
                if (!ReferenceEquals(value, _objectInFolder))
                {
                    var oldValue = _objectInFolder;
                    _objectInFolder = value;
                    OnPropertyChanged("ObjectInFolder", value, oldValue);
                }
            }
        } // ObjectInFolder

    }
}