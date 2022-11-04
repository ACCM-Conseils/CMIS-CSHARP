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
   /// see getRepositoryInfoResponse
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Messaging.xsd
   /// </remarks>
    public partial class getRepositoryInfoResponse : Serialization.XmlSerializable
    {

        public getRepositoryInfoResponse()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected getRepositoryInfoResponse(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<getRepositoryInfoResponse, string>> _setter = new Dictionary<string, Action<getRepositoryInfoResponse, string>>() { }; // _setter

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
            _repositoryInfo = Read(reader, attributeOverrides, "repositoryInfo", Constants.Namespaces.cmism, GenericXmlSerializableFactory<Core.cmisRepositoryInfoType>);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "repositoryInfo", Constants.Namespaces.cmism, _repositoryInfo);
        }
        #endregion

        protected Core.cmisRepositoryInfoType _repositoryInfo;
        public virtual Core.cmisRepositoryInfoType RepositoryInfo
        {
            get
            {
                return _repositoryInfo;
            }
            set
            {
                if (!ReferenceEquals(value, _repositoryInfo))
                {
                    var oldValue = _repositoryInfo;
                    _repositoryInfo = value;
                    OnPropertyChanged("RepositoryInfo", value, oldValue);
                }
            }
        } // RepositoryInfo

    }
}