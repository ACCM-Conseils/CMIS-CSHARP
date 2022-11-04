using System;
using System.Collections.Generic;
using sx = System.Xml;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Security
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisPermissionDefinition
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    public partial class cmisPermissionDefinition : Serialization.XmlSerializable
    {

        public cmisPermissionDefinition()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisPermissionDefinition(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisPermissionDefinition, string>> _setter = new Dictionary<string, Action<cmisPermissionDefinition, string>>() { }; // _setter

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
            _permission = Read(reader, attributeOverrides, "permission", Constants.Namespaces.cmis, _permission);
            _description = Read(reader, attributeOverrides, "description", Constants.Namespaces.cmis, _description);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "permission", Constants.Namespaces.cmis, _permission);
            if (!string.IsNullOrEmpty(_description))
                WriteElement(writer, attributeOverrides, "description", Constants.Namespaces.cmis, _description);
        }
        #endregion

        protected string _description;
        public virtual string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if ((_description ?? "") != (value ?? ""))
                {
                    string oldValue = _description;
                    _description = value;
                    OnPropertyChanged("Description", value, oldValue);
                }
            }
        } // Description

        protected string _permission;
        public virtual string Permission
        {
            get
            {
                return _permission;
            }
            set
            {
                if ((_permission ?? "") != (value ?? ""))
                {
                    string oldValue = _permission;
                    _permission = value;
                    OnPropertyChanged("Permission", value, oldValue);
                }
            }
        } // Permission

    }
}