using System;
using System.Collections.Generic;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Properties
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisProperty
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public abstract partial class cmisProperty : Serialization.XmlSerializable
    {

        protected cmisProperty()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisProperty(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisProperty, string>> _setter = new Dictionary<string, Action<cmisProperty, string>>() { { "displayname", SetDisplayName }, { "localname", SetLocalName }, { "propertydefinitionid", SetPropertyDefinitionId }, { "queryname", SetQueryName } }; // _setter

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
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            if (!string.IsNullOrEmpty(_propertyDefinitionId))
                WriteAttribute(writer, attributeOverrides, "propertyDefinitionId", null, _propertyDefinitionId);
            if (!string.IsNullOrEmpty(_localName))
                WriteAttribute(writer, attributeOverrides, "localName", null, _localName);
            if (!string.IsNullOrEmpty(_displayName))
                WriteAttribute(writer, attributeOverrides, "displayName", null, _displayName);
            if (!string.IsNullOrEmpty(_queryName))
                WriteAttribute(writer, attributeOverrides, "queryName", null, _queryName);
        }
        #endregion

        protected string _displayName;
        public virtual string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                if ((_displayName ?? "") != (value ?? ""))
                {
                    string oldValue = _displayName;
                    _displayName = value;
                    OnPropertyChanged("DisplayName", value, oldValue);
                }
            }
        } // DisplayName
        private static void SetDisplayName(cmisProperty instance, string value)
        {
            instance.DisplayName = value;
        }

        protected string _localName;
        public virtual string LocalName
        {
            get
            {
                return _localName;
            }
            set
            {
                if ((_localName ?? "") != (value ?? ""))
                {
                    string oldValue = _localName;
                    _localName = value;
                    OnPropertyChanged("LocalName", value, oldValue);
                }
            }
        } // LocalName
        private static void SetLocalName(cmisProperty instance, string value)
        {
            instance.LocalName = value;
        }

        protected string _propertyDefinitionId;
        public virtual string PropertyDefinitionId
        {
            get
            {
                return _propertyDefinitionId;
            }
            set
            {
                if ((_propertyDefinitionId ?? "") != (value ?? ""))
                {
                    string oldValue = _propertyDefinitionId;
                    _propertyDefinitionId = value;
                    OnPropertyChanged("PropertyDefinitionId", value, oldValue);
                }
            }
        } // PropertyDefinitionId
        private static void SetPropertyDefinitionId(cmisProperty instance, string value)
        {
            instance.PropertyDefinitionId = value;
        }

        protected string _queryName;
        public virtual string QueryName
        {
            get
            {
                return _queryName;
            }
            set
            {
                if ((_queryName ?? "") != (value ?? ""))
                {
                    string oldValue = _queryName;
                    _queryName = value;
                    OnPropertyChanged("QueryName", value, oldValue);
                }
            }
        } // QueryName
        private static void SetQueryName(cmisProperty instance, string value)
        {
            instance.QueryName = value;
        }

    }
}