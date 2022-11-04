using System;
using ssw = System.ServiceModel.Web;
using sx = System.Xml;
using CmisObjectModel.Constants;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Messaging.Requests
{
    public partial class createItem
    {

        #region missing property support (defined in 2.2.4.6 createItem, but not defined in CMIS-Messaging.xsd)
        /// <summary>
      /// Bridge the gap between createItem-definition as function (see 2.2.4.6 createItem) and the createItem-definition
      /// in CMIS-Messaging.xsd
      /// </summary>
      /// <remarks></remarks>
        public class createItemExtensionType : cmisExtensionType
        {

            #region IXmlSerializable
            protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
            {
                base.ReadXmlCore(reader, attributeOverrides);
                _policies = ReadArray<string>(reader, attributeOverrides, "policies", Namespaces.cmism);
            }

            protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
            {
                base.WriteXmlCore(writer, attributeOverrides);
                WriteArray(writer, attributeOverrides, "policies", Namespaces.cmism, _policies);
            }
            #endregion

            protected string[] _policies;
            /// <summary>
         /// Bridge the gap between createItem-definition as function (see 2.2.4.6 createItem) and the createItem-definition
         /// in CMIS-Messaging.xsd
         /// </summary>
         /// <value></value>
         /// <returns></returns>
         /// <remarks>As soon as this property will cause a compiler error the complete region has to be removed
         /// including the methods Read() and WriteElement()</remarks>
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
        }

        /// <summary>
      /// Deserialization of _extension
      /// </summary>
      /// <typeparam name="TResult"></typeparam>
      /// <param name="reader"></param>
      /// <param name="attributeOverrides"></param>
      /// <param name="nodeName"></param>
      /// <param name="namespace"></param>
      /// <param name="factory"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private cmisExtensionType Read<TResult>(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides, string nodeName, string @namespace, Func<sx.XmlReader, cmisExtensionType> factory) where TResult : cmisExtensionType
        {
            return base.Read(reader, attributeOverrides, nodeName, @namespace, GenericXmlSerializableFactory<createItemExtensionType>);
        }

        /// <summary>
      /// Bridge the gap between createItem-definition as function (see 2.2.4.6 createItem) and the createItem-definition
      /// in CMIS-Messaging.xsd
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks>As soon as this property will cause a compiler error the complete region has to be removed
      /// including the methods Read() and WriteElement() and the class createItemExtensionType and the CType-
      /// Operator should use _policies-field instead of Policies-property</remarks>
        public virtual string[] Policies
        {
            get
            {
                return _extension is createItemExtensionType ? ((createItemExtensionType)_extension).Policies : null;
            }
            set
            {
                var oldValue = Policies;

                if (!ReferenceEquals(value, oldValue))
                {
                    if (value is null)
                    {
                        Extension = null;
                    }
                    else
                    {
                        Extension = new createItemExtensionType() { Policies = value };
                    }
                    OnPropertyChanged("Policies", value, oldValue);
                }
            }
        } // Policies
        #endregion

        /// <summary>
      /// Reads transmitted parameters from queryString
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <remarks></remarks>
        public override void ReadQueryString(string repositoryId)
        {
            var requestParams = ssw.WebOperationContext.Current is null ? null : ssw.WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters;

            base.ReadQueryString(repositoryId);

            if (requestParams is not null)
            {
                _repositoryId = Read(repositoryId, _repositoryId);
                _folderId = Read(requestParams["folderId"], _folderId);
            }
        }

        /// <summary>
      /// Wraps the request-parameters of the createPolicy-Service
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static implicit operator AtomPub.AtomEntry(createItem value)
        {
            if (value is null || value._properties is null)
            {
                return null;
            }
            else
            {
                var cmisraObject = new Core.cmisObjectType(value._properties);
                var policies = value.Policies;

                // missing property Policies defined in 2.2.4.6 createItem, but not defined in CMIS-Messaging.xsd
                if (policies is not null && policies.Length > 0)
                {
                    cmisraObject.PolicyIds = new Core.Collections.cmisListOfIdsType() { Ids = policies };
                }

                return new AtomPub.AtomEntry(cmisraObject);
            }
        }

    }
}