using System;
using System.Collections.Generic;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Definitions.Types
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisTypeRelationshipDefinitionType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisTypeRelationshipDefinitionType : cmisTypeDefinitionType
    {

        public cmisTypeRelationshipDefinitionType() : base(true)
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisTypeRelationshipDefinitionType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisTypeRelationshipDefinitionType, string>> _setter = new Dictionary<string, Action<cmisTypeRelationshipDefinitionType, string>>() { }; // _setter

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
            base.ReadXmlCore(reader, attributeOverrides);
            _allowedSourceTypes = ReadArray<string>(reader, attributeOverrides, "allowedSourceTypes", Constants.Namespaces.cmis);
            _allowedTargetTypes = ReadArray<string>(reader, attributeOverrides, "allowedTargetTypes", Constants.Namespaces.cmis);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            base.WriteXmlCore(writer, attributeOverrides);
            WriteArray(writer, attributeOverrides, "allowedSourceTypes", Constants.Namespaces.cmis, _allowedSourceTypes);
            WriteArray(writer, attributeOverrides, "allowedTargetTypes", Constants.Namespaces.cmis, _allowedTargetTypes);
        }
        #endregion

        protected string[] _allowedSourceTypes;
        public virtual string[] AllowedSourceTypes
        {
            get
            {
                return _allowedSourceTypes;
            }
            set
            {
                if (!ReferenceEquals(value, _allowedSourceTypes))
                {
                    var oldValue = _allowedSourceTypes;
                    _allowedSourceTypes = value;
                    OnPropertyChanged("AllowedSourceTypes", value, oldValue);
                }
            }
        } // AllowedSourceTypes

        protected string[] _allowedTargetTypes;
        public virtual string[] AllowedTargetTypes
        {
            get
            {
                return _allowedTargetTypes;
            }
            set
            {
                if (!ReferenceEquals(value, _allowedTargetTypes))
                {
                    var oldValue = _allowedTargetTypes;
                    _allowedTargetTypes = value;
                    OnPropertyChanged("AllowedTargetTypes", value, oldValue);
                }
            }
        } // AllowedTargetTypes

    }
}