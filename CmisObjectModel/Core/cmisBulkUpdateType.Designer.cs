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
   /// see cmisBulkUpdateType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisBulkUpdateType : Serialization.XmlSerializable
    {

        public cmisBulkUpdateType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisBulkUpdateType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisBulkUpdateType, string>> _setter = new Dictionary<string, Action<cmisBulkUpdateType, string>>() { }; // _setter

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
            _objectIdAndChangeTokens = ReadArray(reader, attributeOverrides, "objectIdAndChangeToken", Constants.Namespaces.cmis, GenericXmlSerializableFactory<cmisObjectIdAndChangeTokenType>);
            _properties = Read(reader, attributeOverrides, "properties", Constants.Namespaces.cmis, GenericXmlSerializableFactory<Collections.cmisPropertiesType>);
            _addSecondaryTypeIds = ReadArray<string>(reader, attributeOverrides, "addSecondaryTypeIds", Constants.Namespaces.cmis);
            _removeSecondaryTypeIds = ReadArray<string>(reader, attributeOverrides, "removeSecondaryTypeIds", Constants.Namespaces.cmis);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteArray(writer, attributeOverrides, "objectIdAndChangeToken", Constants.Namespaces.cmis, _objectIdAndChangeTokens);
            WriteElement(writer, attributeOverrides, "properties", Constants.Namespaces.cmis, _properties);
            WriteArray(writer, attributeOverrides, "addSecondaryTypeIds", Constants.Namespaces.cmis, _addSecondaryTypeIds);
            WriteArray(writer, attributeOverrides, "removeSecondaryTypeIds", Constants.Namespaces.cmis, _removeSecondaryTypeIds);
        }
        #endregion

        protected string[] _addSecondaryTypeIds;
        public virtual string[] AddSecondaryTypeIds
        {
            get
            {
                return _addSecondaryTypeIds;
            }
            set
            {
                if (!ReferenceEquals(value, _addSecondaryTypeIds))
                {
                    var oldValue = _addSecondaryTypeIds;
                    _addSecondaryTypeIds = value;
                    OnPropertyChanged("AddSecondaryTypeIds", value, oldValue);
                }
            }
        } // AddSecondaryTypeIds

        protected cmisObjectIdAndChangeTokenType[] _objectIdAndChangeTokens;
        public virtual cmisObjectIdAndChangeTokenType[] ObjectIdAndChangeTokens
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

        protected Collections.cmisPropertiesType _properties;
        public virtual Collections.cmisPropertiesType Properties
        {
            get
            {
                return _properties;
            }
            set
            {
                if (!ReferenceEquals(value, _properties))
                {
                    var oldValue = _properties;
                    _properties = value;
                    OnPropertyChanged("Properties", value, oldValue);
                }
            }
        } // Properties

        protected string[] _removeSecondaryTypeIds;
        public virtual string[] RemoveSecondaryTypeIds
        {
            get
            {
                return _removeSecondaryTypeIds;
            }
            set
            {
                if (!ReferenceEquals(value, _removeSecondaryTypeIds))
                {
                    var oldValue = _removeSecondaryTypeIds;
                    _removeSecondaryTypeIds = value;
                    OnPropertyChanged("RemoveSecondaryTypeIds", value, oldValue);
                }
            }
        } // RemoveSecondaryTypeIds

    }
}