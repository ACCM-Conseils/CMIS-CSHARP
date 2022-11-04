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
   /// see deleteContentStream
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    [sxs.XmlRoot("deleteContentStream", Namespace = Constants.Namespaces.cmism)]
    public partial class deleteContentStream : RequestBase
    {

        public deleteContentStream()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected deleteContentStream(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<deleteContentStream, string>> _setter = new Dictionary<string, Action<deleteContentStream, string>>() { }; // _setter

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
            _objectId = Read(reader, attributeOverrides, "objectId", Constants.Namespaces.cmism, _objectId);
            _changeToken = Read(reader, attributeOverrides, "changeToken", Constants.Namespaces.cmism, _changeToken);
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
            WriteElement(writer, attributeOverrides, "objectId", Constants.Namespaces.cmism, _objectId);
            if (!string.IsNullOrEmpty(_changeToken))
                WriteElement(writer, attributeOverrides, "changeToken", Constants.Namespaces.cmism, _changeToken);
            WriteElement(writer, attributeOverrides, "extension", Constants.Namespaces.cmism, _extension);
        }
        #endregion

        protected string _changeToken;
        public virtual string ChangeToken
        {
            get
            {
                return _changeToken;
            }
            set
            {
                if ((_changeToken ?? "") != (value ?? ""))
                {
                    string oldValue = _changeToken;
                    _changeToken = value;
                    OnPropertyChanged("ChangeToken", value, oldValue);
                }
            }
        } // ChangeToken

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

        protected string _objectId;
        public virtual string ObjectId
        {
            get
            {
                return _objectId;
            }
            set
            {
                if ((_objectId ?? "") != (value ?? ""))
                {
                    string oldValue = _objectId;
                    _objectId = value;
                    OnPropertyChanged("ObjectId", value, oldValue);
                }
            }
        } // ObjectId

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