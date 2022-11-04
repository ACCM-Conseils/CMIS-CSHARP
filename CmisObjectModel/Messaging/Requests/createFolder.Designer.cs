using System;
using System.Collections.Generic;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Messaging.Requests
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see createFolder
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    [sxs.XmlRoot("createFolder", Namespace = Constants.Namespaces.cmism)]
    public partial class createFolder : RequestBase
    {

        public createFolder()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected createFolder(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<createFolder, string>> _setter = new Dictionary<string, Action<createFolder, string>>() { }; // _setter

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
            _repositoryId = Read(reader, attributeOverrides, "repositoryId", Constants.Namespaces.cmism, _repositoryId);
            _properties = Read(reader, attributeOverrides, "properties", Constants.Namespaces.cmism, GenericXmlSerializableFactory<Core.Collections.cmisPropertiesType>);
            _folderId = Read(reader, attributeOverrides, "folderId", Constants.Namespaces.cmism, _folderId);
            _policies = ReadArray<string>(reader, attributeOverrides, "policies", Constants.Namespaces.cmism);
            _addACEs = Read(reader, attributeOverrides, "addACEs", Constants.Namespaces.cmism, GenericXmlSerializableFactory<Core.Security.cmisAccessControlListType>);
            _removeACEs = Read(reader, attributeOverrides, "removeACEs", Constants.Namespaces.cmism, GenericXmlSerializableFactory<Core.Security.cmisAccessControlListType>);
            _extension = Read(reader, attributeOverrides, "extension", Constants.Namespaces.cmism, GenericXmlSerializableFactory<cmisExtensionType>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "repositoryId", Constants.Namespaces.cmism, _repositoryId);
            WriteElement(writer, attributeOverrides, "properties", Constants.Namespaces.cmism, _properties);
            WriteElement(writer, attributeOverrides, "folderId", Constants.Namespaces.cmism, _folderId);
            WriteArray(writer, attributeOverrides, "policies", Constants.Namespaces.cmism, _policies);
            WriteElement(writer, attributeOverrides, "addACEs", Constants.Namespaces.cmism, _addACEs);
            WriteElement(writer, attributeOverrides, "removeACEs", Constants.Namespaces.cmism, _removeACEs);
            WriteElement(writer, attributeOverrides, "extension", Constants.Namespaces.cmism, _extension);
        }
        #endregion

        protected Core.Security.cmisAccessControlListType _addACEs;
        public virtual Core.Security.cmisAccessControlListType AddACEs
        {
            get
            {
                return _addACEs;
            }
            set
            {
                if (!ReferenceEquals(value, _addACEs))
                {
                    var oldValue = _addACEs;
                    _addACEs = value;
                    OnPropertyChanged("AddACEs", value, oldValue);
                }
            }
        } // AddACEs

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

        protected string _folderId;
        public virtual string FolderId
        {
            get
            {
                return _folderId;
            }
            set
            {
                if ((_folderId ?? "") != (value ?? ""))
                {
                    string oldValue = _folderId;
                    _folderId = value;
                    OnPropertyChanged("FolderId", value, oldValue);
                }
            }
        } // FolderId

        protected string[] _policies;
        public virtual string[] Policies
        {
            get
            {
                return _policies;
            }
            set
            {
                if (!ReferenceEquals(value, _policies))
                {
                    var oldValue = _policies;
                    _policies = value;
                    OnPropertyChanged("Policies", value, oldValue);
                }
            }
        } // Policies

        protected Core.Collections.cmisPropertiesType _properties;
        public virtual Core.Collections.cmisPropertiesType Properties
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

        protected Core.Security.cmisAccessControlListType _removeACEs;
        public virtual Core.Security.cmisAccessControlListType RemoveACEs
        {
            get
            {
                return _removeACEs;
            }
            set
            {
                if (!ReferenceEquals(value, _removeACEs))
                {
                    var oldValue = _removeACEs;
                    _removeACEs = value;
                    OnPropertyChanged("RemoveACEs", value, oldValue);
                }
            }
        } // RemoveACEs

        protected string _repositoryId;
        public virtual string RepositoryId
        {
            get
            {
                return _repositoryId;
            }
            set
            {
                if ((_repositoryId ?? "") != (value ?? ""))
                {
                    string oldValue = _repositoryId;
                    _repositoryId = value;
                    OnPropertyChanged("RepositoryId", value, oldValue);
                }
            }
        } // RepositoryId

    }
}